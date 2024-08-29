using GalacticLib.Math.Numerics.Numbers.MeasuringValues.Defined;
using GalacticLib.Math.Numerics.Numbers.Units;

namespace GalacticLib.Math.Numerics.Numbers.MeasuringValues.DefinedUnits;

public static class TimeUnits {
    public static CompoundUnit Microsecond => new(Exponents.Metric.Micro, Second);
    public static CompoundUnit Millisecond => new(Exponents.Metric.Milli, Second);
    public static CompoundUnit Nanosecond => new(Exponents.Metric.Nano, Second);
    public static BaseUnit Second => new(new("s", nameof(Second)));
    public static SingleUnit Minute
        => new(new("m", nameof(Minute)), Multipliers.Time.Minute, Second);
    static SingleUnit Hour
            => new(new("h", nameof(Hour)), Multipliers.Time.Hour, Second);
    static SingleUnit Day
            => new(new("d", nameof(Day)), Multipliers.Time.Day, Second);
}
