namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units;


/// <summary> Unit that can transform values and be part of a <see cref="Unit"/> chain </summary>
public interface IUnit {
    AffixWrap AffixWrap { get; }

    /// <summary> Text around the value </summary>
    AffixWrap ApplyAffixWrap(AffixWrap previous);

    /// <summary> Transform the <paramref name="previous"/> value to this <see cref="Unit"/> </summary>
    double ToUnit(double previous);
    /// <summary> Transform the <paramref name="previous"/> value to the base <see cref="Unit"/> </summary>
    double ToBase(double previous);
}

public abstract class Unit(AffixWrap affixWrap) : IUnit {
    public AffixWrap AffixWrap { get; } = affixWrap;
    public abstract AffixWrap ApplyAffixWrap(AffixWrap previous);
    public abstract double ToUnit(double previous);
    public abstract double ToBase(double previous);
}

public abstract class BaseUnit(AffixWrap affixWrap) : Unit(affixWrap) {
    public override double ToUnit(double previous) => previous;
    public override double ToBase(double previous) => previous;
}

public abstract class SuffixUnit(AffixWrap affixWrap) : Unit(affixWrap) {
    public override AffixWrap ApplyAffixWrap(AffixWrap previous)
        => new(
            previous.Prefix,
            new(
                previous.Suffix.Shorter + AffixWrap.Suffix.Shorter,
                previous.Suffix.Longer + AffixWrap.Suffix.Longer
            )
        );
}

public abstract class SuffixBaseUnit(AffixWrap affixWrap) : SuffixUnit(affixWrap) {
    public override double ToUnit(double previous) => previous;
    public override double ToBase(double previous) => previous;
}