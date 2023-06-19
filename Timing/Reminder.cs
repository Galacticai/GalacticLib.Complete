namespace GalacticLib.Timing;

[Obsolete("Not working as intended")]
/// <summary> Timer with support for triggering reminders </summary>
public class Reminder : IDisposable {
    #region this object

    private readonly Dictionary<DateTime, BetterTimer> _Timers;

    public Reminder(params DateTime[] times) {
        TimerDeadline += OnTimerDeadline;
        _Timers = new();
        AddTimers(times);
    }

    #endregion

    #region Events

    public delegate void ReminderHandler(Reminder reminder, DateTime time);

    /// <summary> A timer has reached its deadline </summary>
    public event ReminderHandler? TimerDeadline;

    /// <summary> Added a <see cref="DateTime"/> (along with a timer waiting to be started) </summary>
    public event ReminderHandler? TimerAdded;

    /// <summary> Removed a <see cref="DateTime"/> (alone with its timer) </summary>
    public event ReminderHandler? TimerRemoved;

    /// <summary> Cleared all <see cref="DateTime"/>s (alone with their timers) </summary>
    public event ReminderHandler? TimersCleared;

    /// <summary> Timer of a <see cref="DateTime"/> has started </summary>
    public event ReminderHandler? TimerStarted;

    /// <summary> Timer of a <see cref="DateTime"/> was paused </summary>
    public event ReminderHandler? TimerPaused;

    #endregion

    #region Shortcuts

    public int TimersCount => _Timers.Count;
    public bool HasTimers => TimersCount > 0;
    public DateTime? NextTime => !HasTimers ? null : _Timers.Keys.Min();
    public TimeSpan? NextTimeSpan => NextTime == null ? null : NextTime - DateTime.Now;

    #endregion

    #region Methods

    protected virtual void OnTimerDeadline(Reminder reminder, DateTime time) {
        reminder.RemoveTimer(time);
    }

    /// <summary> Add multiple <see cref="DateTime"/>s </summary>
    /// <returns> Whether everything succeeded </returns>
    public bool AddTimers(params DateTime[] times) {
        bool result = true;
        foreach (var time in times) {
            if (!AddTimer(time)) //? Shouldn't be silent
                result = false;
        }

        return result;
    }

    /// <summary> Remove multiple <see cref="DateTime"/>s </summary>
    /// <returns> Whether everything succeeded </returns>
    public bool RemoveTimers(params DateTime[] times) {
        bool result = true;
        foreach (var timer in times) {
            if (!RemoveTimer(timer)) //? Shouldn't be silent
                result = false;
        }

        return result;
    }

    /// <summary> Remove all <see cref="DateTime"/>s </summary>
    /// <returns> Whether everything succeeded </returns>
    public bool ClearTimers() {
        bool result = true;
        foreach (var time in _Timers.Keys) {
            if (!_RemoveTimer_Silent(time)) //? Should be silent
                result = false;
            TimersCleared?.Invoke(this, time);
        }

        return result;
    }

    /// <summary> Add a <see cref="DateTime"/> </summary>
    /// <returns> Whether the <see cref="DateTime"/> was added </returns>
    public bool AddTimer(DateTime time) {
        if (_AddTimer_Silent(time)) {
            TimerAdded?.Invoke(this, time);
            return true;
        }

        return false;
    }

    /// <summary> Add a <see cref="DateTime"/> without raising any events </summary>
    private bool _AddTimer_Silent(DateTime time) {
        if (!TimeCanBeAdded(time)) return false;
        if (_Timers.ContainsKey(time)) return false;
        _Timers.Add(time, new(time, _ => TimerDeadline?.Invoke(this, time)));
        return true;
    }

    /// <summary> Remove a <see cref="DateTime"/> </summary>
    /// <returns> Whether the <see cref="DateTime"/> was removed </returns>
    public bool RemoveTimer(DateTime time) {
        if (_RemoveTimer_Silent(time)) {
            TimerRemoved?.Invoke(this, time);
            return true;
        }

        return false;
    }

    /// <summary> Remove a <see cref="DateTime"/> without raising any events </summary>
    private bool _RemoveTimer_Silent(DateTime timer) {
        if (!_Timers.ContainsKey(timer)) return false;
        _Timers[timer].Dispose();
        _Timers.Remove(timer);
        return true;
    }

    public void StartTimer(DateTime time) {
        if (!_Timers.ContainsKey(time)) return;
        _Timers[time].Start();
        TimerStarted?.Invoke(this, time);
    }

    public void PauseTimer(DateTime time) {
        if (!_Timers.ContainsKey(time)) return;
        _Timers[time].Pause();
        TimerPaused?.Invoke(this, time);
    }

    public void StartAllTimers() {
        foreach (var time in _Timers.Keys) StartTimer(time);
    }

    public void PauseAllTimers() {
        foreach (var time in _Timers.Keys) PauseTimer(time);
    }

    /// <summary> Validate if a <see cref="DateTime"/> can be used in <see cref="Reminder"/>s </summary>
    public static bool TimeCanBeAdded(DateTime time) => time > DateTime.Now;

    #endregion

    #region Overrides

    #endregion

    #region Inheritence

    public void Dispose() {
        ClearTimers();

        TimerDeadline = null;
        TimerAdded = null;
        TimerRemoved = null;
        TimersCleared = null;
        TimerStarted = null;
        TimerPaused = null;

        GC.SuppressFinalize(this);
    }

    #endregion
}