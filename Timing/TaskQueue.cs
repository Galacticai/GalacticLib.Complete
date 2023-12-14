using GalacticLib.Objects;

namespace GalacticLib.Timing;

/// <summary>
/// <para>
/// Finish tasks asyncronously, but mark them sequencially as done/timeout/error...
/// </para>
/// <para>
/// In other words: <br/>
/// </para>
/// Queue tasks with key/value of <typeparamref name="TKey"/>/<typeparamref name="TValue"/> and run them as soon as they are added, <br/>
/// then trigger TaskDone when the top task is done. <br/>
/// Once the top is done, it will check the next until it reaches a Pending one so it stops and waits for it to finish
/// </summary>
/// <typeparam name="TKey"> Task key type (must be a <see cref="IComparable{TKey}"/>) </typeparam>
/// <typeparam name="TValue"> Task return type </typeparam>
public class TaskQueue<TKey, TValue> where TKey : IComparable<TKey> {

    private TaskQueue(
            TaskHandler task,
            int maxTaskDuration,
            SortedDictionary<TKey, FutureValue<TValue>> dictionary) {
        Task = task;
        MaxTaskDuration = maxTaskDuration;
        Queue = dictionary;
        RunningTasks = [];
    }

    public TaskQueue(TaskHandler task, int maxTaskDuration)
            : this(task, maxTaskDuration, []) { }
    public TaskQueue(TaskHandler task, int maxTaskDuration, IComparer<TKey> comparer)
            : this(
                task,
                maxTaskDuration,
                new SortedDictionary<TKey, FutureValue<TValue>>(comparer)
            ) { }

    /// <summary> Get queue item using <paramref name="key"/> </summary>
    /// <param name="key"> Target item key </param>
    /// <returns> 
    /// <see cref="FutureValue{TValue}"/> representation of the value <br/>
    /// or <see langword="null"/> if <paramref name="key"/> doesn't exist
    /// </returns>
    /// <exception cref="KeyNotFoundException" />
    public FutureValue<TValue>? this[TKey key] {
        get => Queue.TryGetValue(key, out var value)
            ? value
            : null;
        private set => Queue[key]
            = value
            ?? throw new KeyNotFoundException($"Key not found: {key}");
    }

    public TaskHandler Task { get; }
    /// <summary> 
    /// Max duration in milliseconds for a tasks. <br/>
    /// Once this elapses, the task gets cancelled and marked as <see cref="FutureValue{TValue}.TimedOut"/>
    /// </summary>
    public int MaxTaskDuration { get; }
    private SortedDictionary<TKey, FutureValue<TValue>> Queue { get; }
    private Dictionary<TKey, (CancellationTokenSource CancelToken, Task<TValue?> Task)> RunningTasks { get; }

    /// <summary> Count of tasks currently queued </summary>
    public int Count => Queue.Count;
    /// <summary> Queue is empty </summary>
    public bool IsEmpty => Count == 0;
    /// <summary> Queue contains tasks </summary>
    public bool IsNotEmpty => !IsEmpty;


    /// <summary>
    /// Add a task as <see cref="FutureValue{TValue}.Pending"/> without calling <see cref="RunTask"/> <br/>
    /// ⚠️ Warning: you must remember to start it eventually otherwise the queue will keep waiting for it
    /// </summary>
    /// <param name="key"> Task key </param>
    /// <param name="force"> force Replace existing in case it exists </param>
    /// <returns> true if added </returns>
    public bool AddWithoutRun(TKey key, bool force = false) {
        if (!force && ContainsKey(key)) return false;
        this[key] = new FutureValue<TValue>.Pending();
        TaskAdded?.Invoke(key);
        return true;
    }

    /// <summary> 
    /// Add a task as <see cref="FutureValue{TValue}.Pending"/> and run it. <br/>
    /// Then update the value once done or something else happened (time out, error...) 
    /// </summary> 
    /// <param name="key"> Task key </param>
    /// <param name="force"> force Replace existing in case it exists </param>
    /// <returns> true if added </returns>
    public bool AddRun(TKey key, bool force = false) {
        bool added = AddWithoutRun(key, force);
        if (!added) return false;
        //! Intentionally not awaited
        _ = RunTask(key).ConfigureAwait(false);
        return true;
    }

    /// <summary> Run a task having the provided <paramref name="key"/> </summary>
    /// <param name="key"> Task key </param>
    /// <returns></returns>
    public async Task<TValue?> RunTask(TKey key) {
        if (!ContainsKey(key))
            return default;

        TValue? value;
        try {
            using var cancelToken = new CancellationTokenSource(MaxTaskDuration);
            Task<TValue?> task = new(
                () => Task(key),
                cancelToken.Token
            );

            lock (RunningTasks)
                RunningTasks.Add(key, (cancelToken, task));

            this[key] = new FutureValue<TValue>.Running();
            task.Start();
            TaskStarted?.Invoke(key);
            value = await task;
            this[key] = new FutureValue<TValue>.Finished(value);
            TaskDone?.Invoke(key, value);

            lock (RunningTasks)
                RunningTasks.Remove(key);
            lock (Queue)
                Remove(key, forceStop: false);

        } catch (OperationCanceledException) {
            this[key] = new FutureValue<TValue>.Failed.TimedOut(MaxTaskDuration);

        } catch (Exception exception) {
            this[key] = new FutureValue<TValue>.Failed.Error(exception);
        }

        TValue? current
            = this[key] is FutureValue<TValue>.Finished finished
            ? finished.Value : default;

        DoNext();

        return current;
    }
    private void DoNext() {
        while (IsNotEmpty) {
            TKey key = PeekKey()!;

            FutureValue<TValue> value = this[key]!;

            if (value is FutureValue<TValue>.Failed failed) {

                if (failed is FutureValue<TValue>.Failed.TimedOut timedOut) {
                    TaskTimedOut?.Invoke(key, timedOut.Duration);

                } else if (failed is FutureValue<TValue>.Failed.Error error) {
                    if (TaskError is null) throw error.Exception;
                    TaskError?.Invoke(key, error.Exception);
                }

            } else if (value is not FutureValue<TValue>.Finished)
                break;

            Remove(key, forceStop: false);
        }
    }

    /// <summary> Check out the 1st item in queue </summary> 
    /// <returns> First item or null if queue is empty </returns>
    public KeyValuePair<TKey, FutureValue<TValue>>? Peek() => Queue.FirstOrDefault();
    /// <summary> Check out the 1st key in queue </summary> 
    /// <returns> First key or null if queue is empty </returns>
    public TKey? PeekKey() => Queue.Keys.FirstOrDefault();
    /// <summary> Check out the 1st result in queue </summary> 
    /// <returns> First result or null if queue is empty </returns>
    public FutureValue<TValue>? PeekValue() => Queue.Values.FirstOrDefault();

    /// <summary> Check if <paramref name="key"/> exists in queue </summary>
    /// <param name="key"> Target <typeparamref name="TKey"/> </param>
    /// <returns> true if queu contains the <paramref name="key"/> </returns>
    public bool ContainsKey(TKey key) => Queue.ContainsKey(key);

    // public enum RemoveType {
    //     /// <summary> Remove the item and ignore the value it has even if it was finished </summary>
    //     DiscardStatusAndRemove,
    //     /// <summary> Raise the event corresponding to the item status then remove </summary>
    //     RaiseStatusThenRemove,
    // }
    /// <summary> Remove a task </summary> 
    /// <returns> true if removed </returns>
    public bool Remove(TKey key, bool forceStop = true) {
        if (forceStop) StopTask(key);
        lock (Queue) return Queue.Remove(key);
    }
    public bool StopTask(TKey key) {
        lock (RunningTasks) {
            if (!RunningTasks.TryGetValue(key, out var value))
                return false;
            value.CancelToken.Cancel();
            value.CancelToken.Dispose();
            return RunningTasks.Remove(key);
        }
    }


    /// <summary> Stop all running tasks and clear the queue <br/>
    /// The results of unfinished tasks will be <see cref="FutureValue{TValue}.TimedOut" /> </summary>
    public void Clear() {
        lock (RunningTasks) lock (Queue) {
                foreach (var task in RunningTasks.ToList())
                    StopTask(task.Key);
                Queue.Clear();
            }
    }


    /// <summary> Definition of a task</summary>
    /// <param name="key"> Task key </param>
    /// <returns> <typeparamref name="TValue"/> to be stored then used later in <see cref="TaskDone"/> event </returns>
    public delegate TValue? TaskHandler(TKey key);
    /// <summary> Task has been queued </summary>
    /// <param name="key"> Task key </param>
    public delegate void TaskAddedHandler(TKey key);
    /// <summary> Task has been queued </summary>
    /// <param name="key"> Task key </param>
    public delegate void TaskStartedHandler(TKey key);
    /// <summary> Task ran for too long and got timed out </summary>
    /// <param name="key"> Task key </param>
    /// <param name="duration"> The running duration in milliseconds </param>
    public delegate void TaskTimedOutHandler(TKey key, int duration);
    /// <summary> Task has encountered the given <paramref name="exception"/> </summary>
    /// <param name="key"> Task key </param>
    /// <param name="exception"> <see cref="Exception"/> that the task has encountered </param>
    public delegate void TaskErrorHandler(TKey key, Exception exception);
    /// <summary> Task finished successfully </summary>
    /// <param name="key"> Task key </param>
    /// <param name="value"> value of the task as <typeparamref name="TValue"/> </param>
    public delegate void TaskDoneHandler(TKey key, TValue? value);

    /// <summary> Task has been queued </summary>
    public event TaskAddedHandler? TaskAdded;
    /// <summary> Task has started </summary>
    public event TaskStartedHandler? TaskStarted;
    /// <summary> Task ran for too long and got timed out </summary>
    public event TaskTimedOutHandler? TaskTimedOut;
    /// <summary> Task has encountered an <paramref name="Exception"/> </summary>
    public event TaskErrorHandler? TaskError;
    /// <summary> Task finished successfully </summary>
    public event TaskDoneHandler? TaskDone;
}
