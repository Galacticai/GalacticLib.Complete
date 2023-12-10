using System.Runtime.CompilerServices;
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
            int maxTaskDuration,
            SortedDictionary<TKey, FutureValue<TValue>> dictionary) {
        MaxTaskDuration = maxTaskDuration;
        Dictionary = dictionary;
        RunningTasks = [];
    }

    public TaskQueue(int maxTaskDuration)
            : this(maxTaskDuration, []) { }
    public TaskQueue(int maxTaskDuration, Comparer<TKey> comparer)
            : this(
                maxTaskDuration,
                new SortedDictionary<TKey, FutureValue<TValue>>(comparer)
            ) { }

    /// <summary> Get queue item using <paramref name="key"/> </summary>
    /// <param name="key"> Target item key </param>
    /// <returns> 
    /// <see cref="FutureValue{TValue}"/> representation of the value <br/>
    /// or <see cref="FutureValue{TValue}.NotFound"/> if <paramref name="key"/> doesn't exist
    /// </returns>
    public FutureValue<TValue> this[TKey key] {
        get => Dictionary.TryGetValue(key, out var value)
            ? value
            : new FutureValue<TValue>.NotFound();
        private set => Dictionary[key] = value;
    }

    /// <summary> 
    /// Max duration in milliseconds for a tasks. <br/>
    /// Once this elapses, the task gets cancelled and marked as <see cref="FutureValue{TValue}.TimedOut"/>
    /// </summary>
    public int MaxTaskDuration { get; }
    private SortedDictionary<TKey, FutureValue<TValue>> Dictionary { get; }
    private Dictionary<string, (CancellationTokenSource CancelToken, Task<TValue> Task)> RunningTasks { get; }

    /// <summary> Count of tasks currently queued </summary>
    /// <summary> Queue is empty </summary>
    public bool IsEmpty => Count == 0;
    /// <summary> Queue contains tasks </summary>
    public bool IsNotEmpty => !IsEmpty;


    /// <summary> 
    /// Add a task as <see cref="FutureValue{TValue}.Pending"/> and run it. <br/>
    /// Then update the value once done or something else happened (time out, error...) 
    /// </summary> 
    /// <param name="force"> force Replace existing in case it exists </param>
    /// <returns> true if added </returns>
    public bool AddRun(TKey key, bool force = false) {
        if (!force && ContainsKey(key)) return false;
        this[key] = new FutureValue<TValue>.Pending();
        //! Intentionally not awaited
        _ = RunTask(key).ConfigureAwait(false);
        return true;
    }
    private async Task RunTask(TKey key) {
        if (TaskQueued is null) return;

        TValue value;
        try {
            var id = Guid.NewGuid().ToString();
            using var cancelToken = new CancellationTokenSource(MaxTaskDuration);
            var task = new Task<TValue>(
                () => TaskQueued(key),
                cancelToken.Token
            );

            lock (RunningTasks) RunningTasks.Add(id, (cancelToken, task));
            value = await task;
            lock (RunningTasks) RunningTasks.Remove(id);

            this[key] = new FutureValue<TValue>.Finished(value);

        } catch (OperationCanceledException) {
            this[key] = new FutureValue<TValue>.TimedOut(MaxTaskDuration);
            //? no return because TimedOut is treated as Finished (skip) 

        } catch (Exception exception) {
            if (TaskError is null) throw;
            TaskError(key, exception);
            return;
        }

        DoNext();
    }
    private void DoNext() {

        while (IsNotEmpty) {
            TKey key = PeekKey()!;

            FutureValue<TValue> value = this[key];

            if (value is FutureValue<TValue>.Finished finished) {
                TaskDone?.Invoke(key, finished.Value);

            } else if (value is FutureValue<TValue>.TimedOut timedOut) {
                TaskTimedOut?.Invoke(key, timedOut.Duration);

            } else break;

            Remove(key);
        }
    }

    /// <summary> Check out the 1st key in queue </summary> 
    /// <returns> First key or null if queue is empty </returns>
    public TKey? PeekKey() => Dictionary.FirstOrDefault().Key;

    /// <summary> Check if <paramref name="key"/> exists in queue </summary>
    /// <param name="key"> Target <typeparamref name="TKey"/> </param>
    /// <returns> true if queu contains the <paramref name="key"/> </returns>

    /// <summary> Remove a task </summary> 
    /// <returns> true if removed </returns>
    public bool Remove(TKey key) => Dictionary.Remove(key);

    /// <summary> Cancel all running tasks and clear the queue </summary>
    public void Clear() {
        lock (RunningTasks) {
            foreach (var task in RunningTasks.ToList()) {
                task.Value.CancelToken.Cancel();
                task.Value.CancelToken.Dispose();
                RunningTasks.Remove(task.Key);
            }
        }
        lock (Dictionary) Dictionary.Clear();
    }


    /// <summary> Task has been queued </summary>
    /// <param name="key"> Task key </param>
    /// <returns> <typeparamref name="TValue"/> to be stored then used later in <see cref="TaskDone"/> event </returns>
    public delegate TValue TaskQueuedHandler(TKey key);
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
    public delegate void TaskDoneHandler(TKey key, TValue value);

    /// <summary> Task has been queued </summary>
    public event TaskQueuedHandler? TaskQueued;
    /// <summary> Task ran for too long and got timed out </summary>
    public event TaskTimedOutHandler? TaskTimedOut;
    /// <summary> Task has encountered an <paramref name="Exception"/> </summary>
    public event TaskErrorHandler? TaskError;
    /// <summary> Task finished successfully </summary>
    public event TaskDoneHandler? TaskDone;

}