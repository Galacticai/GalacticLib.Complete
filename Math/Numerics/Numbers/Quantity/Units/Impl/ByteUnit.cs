namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;
public class ByteUnit() : SuffixUnit(
    new(Affix.Empty, new("B", "Byte"))
) {
    public const double Factor = 8;
    public override double ToUnit(double previous)
        => previous;
    public override double ToBase(double previous)
        => previous;
    public double ToBit(double previous)
        => previous * Factor;
}
