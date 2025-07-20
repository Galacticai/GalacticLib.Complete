namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.UnitSystems;

/// <summary> <code>1L &lt;&lt; <see cref="BinaryFactor"/></code> </summary>
public enum BinaryFactor {
    Kibi = 10,
    Mebi = 20,
    Gibi = 30,
    Tebi = 40,
    Pebi = 50,
    Exbi = 60,
    Zebi = 70,
    Yobi = 80,
}

public class BinarySystem : UnitSystem {
    public override double SystemFactor => 1L << (int)Factor;

    private BinarySystem(BinaryFactor factor, AffixWrap affixWrap)
        : base((double)factor, affixWrap) { }
    public BinarySystem(BinaryFactor factor, string shortSuffix)
        : this(
              factor,
              new AffixWrap(Affix.Empty, new(shortSuffix, factor.ToString()))
        ) { }

    public static readonly BinarySystem Kibi = new(BinaryFactor.Kibi, "Ki");
    public static readonly BinarySystem Mebi = new(BinaryFactor.Mebi, "Mi");
    public static readonly BinarySystem Gibi = new(BinaryFactor.Gibi, "Gi");
    public static readonly BinarySystem Tebi = new(BinaryFactor.Tebi, "Ti");
    public static readonly BinarySystem Pebi = new(BinaryFactor.Pebi, "Pi");
    public static readonly BinarySystem Exbi = new(BinaryFactor.Exbi, "Ei");
    public static readonly BinarySystem Zebi = new(BinaryFactor.Zebi, "Zi");
    public static readonly BinarySystem Yobi = new(BinaryFactor.Yobi, "Yi");
}
