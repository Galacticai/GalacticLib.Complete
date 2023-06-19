namespace GalacticLib.Timing;

/// <summary> Wrapper for <see cref="Timer"/> with actually useful methods and properties + intuitive constructors/events </summary>
public class BetterTimer : IDisposable {
    #region this object

    private readonly Timer _Timer;

    public DateTime Created { get; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? PausedAt { get; private set; }
    public bool IsRunning { get; private set; }


    private DateTime _Deadline;
    public DateTime Deadline {
        get {
            if (PausedAt != null) {
                return _Deadline + (PausedAt.Value - (DateTime)StartedAt!);
            }
            return _Deadline;
        }
        set {
            _Deadline = value >= DateTime.Now ? value : DateTime.Now;
            DeadlineChanged?.Invoke(this);
        }
    }

    /// <summary> Wrapper for <see cref="Timer"/> with actually useful methods and properties + intuitive constructors/events </summary>
    /// <param name="deadline"> The time when <see cref="DeadlineReached"/> is triggered </param>
    /// <param name="action"> <see cref="DeadlineReached"/> action </param>
    /// <param name="startNow"> <see cref="Start"/> now </param>
    public BetterTimer(DateTime deadline, BetterTimerHandler action, bool startNow = false)
            : this(deadline) {
        DeadlineReached += action;
        if (startNow) Start();
    }
    /// <summary> Wrapper for <see cref="Timer"/> with actually useful methods and properties + intuitive constructors/events </summary>
    /// <param name="deadline"> The time when <see cref="DeadlineReached"/> is triggered </param>
    public BetterTimer(DateTime deadline) {
        Created = DateTime.Now;
        Deadline = deadline;
        _Timer = new(_ => DeadlineReached?.Invoke(this), null, Timeout.Infinite, Timeout.Infinite);
        DeadlineReached += _ => _Timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    #endregion
    #region Events

    public delegate void BetterTimerHandler(BetterTimer timer);
    public event BetterTimerHandler? DeadlineReached, DeadlineChanged, Started, Paused, Skipped;

    #endregion
    #region Shortcuts

    public TimeSpan Remaining => Deadline - DateTime.Now;

    #endregion
    #region Methods

    /// <summary> Create <see cref="BetterTimer"/> where the <see cref="Deadline"/> is after a certain <paramref name="timeSpan"/> </summary>
    public static BetterTimer After(TimeSpan timeSpan)
        => new(DateTime.Now + timeSpan);
    /// <summary> Create <see cref="BetterTimer"/> where the <see cref="Deadline"/> is after a certain <paramref name="timeSpan"/> </summary>
    public static BetterTimer After(TimeSpan span, BetterTimerHandler action, bool startNow = false)
        => new(DateTime.Now + span, action, startNow);

    private void _ChangeTimerDelay(long newDelay) {
        _Timer.Change(newDelay, Timeout.Infinite);
    }

    private void _StopTimer() => _ChangeTimerDelay(Timeout.Infinite);

    /// <summary> Start the timer </summary>
    public void Start() {
        if (IsRunning) return;

        long remaining = (long)Remaining.TotalMilliseconds;

        if (remaining <= 0) {
            DeadlineReached?.Invoke(this);
        } else {
            _ChangeTimerDelay(remaining);
        }

        StartedAt = DateTime.Now;
        PausedAt = null;
        IsRunning = true;
        Started?.Invoke(this);
    }

    /// <summary> Pause without triggering <see cref="Paused"/> event </summary>
    private void _PauseSilently() {
        if (!IsRunning) return;
        _StopTimer();
        PausedAt = DateTime.Now;
        IsRunning = false;
    }
    /// <summary> Pause the timer </summary>
    public void Pause() {
        _PauseSilently();
        Paused?.Invoke(this);
    }

    #endregion
    #region Overrides

    public override string ToString() => $"({nameof(BetterTimer)}) Due at {Deadline:g}";

    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj)
        || (obj is BetterTimer otherTimer
            && Created == otherTimer.Created
            && StartedAt == otherTimer.StartedAt
            && PausedAt == otherTimer.PausedAt
            && IsRunning == otherTimer.IsRunning
            && Deadline == otherTimer.Deadline
            && DeadlineReached == otherTimer.DeadlineReached
            && DeadlineChanged == otherTimer.DeadlineChanged
            && Started == otherTimer.Started
            && Paused == otherTimer.Paused
            && Skipped == otherTimer.Skipped
        );

    #endregion
    #region Inheritence

    public void Dispose() {
        _PauseSilently();
        _Timer?.Dispose();
        IsRunning = false;
        StartedAt = null;
        PausedAt = null;

        DeadlineReached = null;
        DeadlineChanged = null;
        Started = null;
        Paused = null;
        Skipped = null;

        GC.SuppressFinalize(this);
    }

    #endregion
}
