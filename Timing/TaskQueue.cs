using System.Runtime.CompilerServices;
using GalacticLib.Objects;

namespace GalacticLib.Timing;

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


    public FutureValue<TValue> this[TKey key] {
        get => Dictionary.TryGetValue(key, out var value)
            ? value
            : new FutureValue<TValue>.NotFound();
        private set => Dictionary[key] = value;
    }

    public int MaxTaskDuration { get; }
    private SortedDictionary<TKey, FutureValue<TValue>> Dictionary { get; }
    private Dictionary<string, (CancellationTokenSource CancelToken, Task<TValue> Task)> RunningTasks { get; }

    public int Count => Dictionary.Count;
    public bool IsEmpty => Count == 0;
    public bool IsNotEmpty => !IsEmpty;


    /// <summary> Add a task and run it </summary> 
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

    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

    /// <summary> Remove a task </summary> 
    /// <returns> true if removed </returns>
    public bool Remove(TKey key) => Dictionary.Remove(key);

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


    public delegate TValue TaskQueuedHandler(TKey key);
    public delegate void TaskTimedOutHandler(TKey key, int duration);
    public delegate void TaskErrorHandler(TKey key, Exception exception);
    public delegate void TaskDoneHandler(TKey key, TValue value);

    public event TaskQueuedHandler? TaskQueued;
    public event TaskTimedOutHandler? TaskTimedOut;
    public event TaskErrorHandler? TaskError;
    public event TaskDoneHandler? TaskDone;

}