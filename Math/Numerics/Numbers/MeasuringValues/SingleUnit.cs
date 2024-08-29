namespace GalacticLib.Math.Numerics.Numbers.Units;


public class SingleUnit(NameShortLong name, double baseMultiplier, BaseUnit? baseUnit = null)
        : Unit {
    public override NameShortLong Name { get; } = name;
    public override double BaseMultiplier { get; } = baseMultiplier;
    public BaseUnit? BaseUnit { get; } = baseUnit;

    public bool IsExponent => this is Exponent;
    public bool IsBase => BaseUnit is null && !IsExponent;
}
