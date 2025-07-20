namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;
public class HourUnit() : SuffixUnit(
    new(Affix.Empty, new("h", "hour"))
) {
    public const double Factor = MinuteUnit.Factor * 60;
    public override double ToUnit(double previous)
        => previous / Factor;
    public override double ToBase(double previous)
        => previous * Factor;
}
