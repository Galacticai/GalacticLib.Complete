namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;
public class MinuteUnit() : SuffixUnit(
    new(Affix.Empty, new("m", "minute"))
) {
    public const double Factor = 60;
    public override double ToUnit(double previous)
        => previous / Factor;
    public override double ToBase(double previous)
        => previous * Factor;
}
