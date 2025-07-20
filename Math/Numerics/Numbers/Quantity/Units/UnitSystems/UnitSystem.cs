namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.UnitSystems;

public abstract class UnitSystem(
    double factor,
    AffixWrap affixWrap
) : SuffixUnit(affixWrap) {
    protected double Factor { get; } = factor;
    public abstract double SystemFactor { get; }

    public override double ToUnit(double previous)
        => previous / SystemFactor;
    public override double ToBase(double previous)
        => previous * SystemFactor;
}
