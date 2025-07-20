namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;


public class PerUnit : Unit {
    public IReadOnlyList<Unit> Denominators { get; }

    public AffixWrap DenominatorsAffix { get; }


    public override AffixWrap ApplyAffixWrap(AffixWrap previous)
        => new(
            previous.Prefix,
            new(
                previous.Suffix.Shorter + "/" + DenominatorsAffix.Suffix.Shorter,
                previous.Suffix.Longer + " per " + DenominatorsAffix.Suffix.Longer
            )
        );

    //? "per" doesn't affect the value. just a label
    public override double ToUnit(double previous) => previous;
    public override double ToBase(double previous) => previous;

    public PerUnit(Unit[] denominators) : base(
        new(Affix.Empty, new("/", " per "))
    ) {
        Denominators = denominators;
        DenominatorsAffix = Denominators.Aggregate(
            AffixWrap.Empty, (v, u) => u.ApplyAffixWrap(v)
        );
    }
}
//public class PerUnit : Unit {
//    public IReadOnlyList<Unit> Denominators { get; }

//    public double DenominatorsValue { get; }
//    public AffixWrap DenominatorsAffix { get; }


//    public override AffixWrap ApplyAffixWrap(AffixWrap previous)
//        => new(
//            previous.Prefix,
//            new(
//                previous.Suffix.Shorter + "/" + DenominatorsAffix.Suffix.Shorter,
//                previous.Suffix.Longer + " per " + DenominatorsAffix.Suffix.Longer
//            )
//        );

//    public override double ToUnit(double previous)
//        => previous / DenominatorsValue;

//    public override double ToBase(double previous)
//        => previous * DenominatorsValue;

//    public PerUnit(Unit[] denominators) : base(
//        new(Affix.Empty, new("/", " per "))
//    ) {
//        Denominators = denominators;
//        DenominatorsAffix = Denominators.Aggregate(
//            AffixWrap.Empty, (v, u) => u.ApplyAffixWrap(v)
//        );
//        DenominatorsValue = Denominators.Aggregate(
//            1.0, (v, u) => u.ToUnit(v)
//        );
//    }
//}