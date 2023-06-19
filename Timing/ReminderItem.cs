namespace GalacticLib.Timing;

[Obsolete("Not working as intended")]
/// <summary> Reminder info item used in <see cref="Reminder"/> </summary>
public struct ReminderItem : IDisposable {
    #region this object
    /// <summary> Due <see cref="System.DateTime"/> </summary>
    public DateTime DateTime { get; set; }
    /// <summary> any <see cref="object"/> attached to this <see cref="ReminderItem"/> </summary>
    public object? Attachement { get; set; }

    public ReminderItem(DateTime dateTime, object? attachement = null) {
        DateTime = dateTime;
        Attachement = attachement;
    }

    #endregion
    #region Shortcuts

    public bool IsDue => DateTime <= DateTime.Now;
    public bool HasAttachement => Attachement != null;

    #endregion
    #region Methods

    /// <summary> Create a <see cref="ReminderItem"/> that is due (<see cref="from"/> + <see cref="difference"/>) </summary>
    /// <param name="from"> Anchor </param>
    /// <param name="difference"> Difference </param>
    public static ReminderItem DeltaFrom(DateTime from, TimeSpan difference, object? attachement = null)
        => new(from + difference, attachement);

    #endregion
    #region Overrides

    public override int GetHashCode()
        => HashCode.Combine(DateTime, Attachement);
    public override bool Equals(object? other)
        => other is ReminderItem otherItem
        && DateTime.Equals(otherItem.DateTime)
        && ((Attachement == null)
            ? otherItem.Attachement == null
            : Attachement.Equals(otherItem.Attachement)
        );

    #endregion
    #region Operators

    public static bool operator ==(ReminderItem left, ReminderItem right) => left.Equals(right);
    public static bool operator !=(ReminderItem left, ReminderItem right) => !(left == right);

    public static explicit operator ReminderItem(DateTime dateTime) => new(dateTime);
    public static explicit operator DateTime(ReminderItem reminderItem) => reminderItem.DateTime;

    #endregion
    #region Inheritence

    public void Dispose() {
        if (Attachement is IDisposable disposableAttachment)
            disposableAttachment.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
