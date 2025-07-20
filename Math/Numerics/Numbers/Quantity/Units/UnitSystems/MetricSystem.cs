namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units.UnitSystems;

/// <summary> <code> 1e<see cref="MetricFactor"/> </code> </summary>
public enum MetricFactor {
    Quecto = -30,
    Ronto = -27,
    Yocto = -24,
    Zepto = -21,
    Atto = -18,
    Femto = -15,
    Pico = -12,
    Nano = -9,
    Micro = -6,
    Milli = -3,
    Centi = -2,
    Deci = -1,
    Deca = 1,
    Hecto = 2,
    Kilo = 3,
    Mega = 6,
    Giga = 9,
    Tera = 12,
    Peta = 15,
    Exa = 18,
    Zetta = 21,
    Yotta = 24,
    Ronna = 27,
    Quetta = 30,
}

public class MetricSystem : UnitSystem {
    public override double SystemFactor => System.Math.Pow(10, Factor);
    public override double ToUnit(double previous)
        => previous / SystemFactor;
    public override double ToBase(double previous)
        => previous * SystemFactor;

    private MetricSystem(MetricFactor factor, AffixWrap affixWrap)
        : base((double)factor, affixWrap) { }
    public MetricSystem(MetricFactor factor, string shortSuffix)
        : this(
              factor,
              new AffixWrap(Affix.Empty, new(shortSuffix, factor.ToString()))
        ) { }



    public static readonly MetricSystem Quecto = new(MetricFactor.Quecto, "q");
    public static readonly MetricSystem Ronto = new(MetricFactor.Ronto, "r");
    public static readonly MetricSystem Yocto = new(MetricFactor.Yocto, "y");
    public static readonly MetricSystem Zepto = new(MetricFactor.Zepto, "z");
    public static readonly MetricSystem Atto = new(MetricFactor.Atto, "a");
    public static readonly MetricSystem Femto = new(MetricFactor.Femto, "f");
    public static readonly MetricSystem Pico = new(MetricFactor.Pico, "p");
    public static readonly MetricSystem Nano = new(MetricFactor.Nano, "n");
    public static readonly MetricSystem Micro = new(MetricFactor.Micro, "μ");
    public static readonly MetricSystem Milli = new(MetricFactor.Milli, "m");
    public static readonly MetricSystem Centi = new(MetricFactor.Centi, "c");
    public static readonly MetricSystem Deci = new(MetricFactor.Deci, "d");
    public static readonly MetricSystem Deca = new(MetricFactor.Deca, "da");
    public static readonly MetricSystem Hecto = new(MetricFactor.Hecto, "h");
    public static readonly MetricSystem Kilo = new(MetricFactor.Kilo, "k");
    public static readonly MetricSystem Mega = new(MetricFactor.Mega, "M");
    public static readonly MetricSystem Giga = new(MetricFactor.Giga, "G");
    public static readonly MetricSystem Tera = new(MetricFactor.Tera, "T");
    public static readonly MetricSystem Peta = new(MetricFactor.Peta, "P");
    public static readonly MetricSystem Exa = new(MetricFactor.Exa, "E");
    public static readonly MetricSystem Zetta = new(MetricFactor.Zetta, "Z");
    public static readonly MetricSystem Yotta = new(MetricFactor.Yotta, "Y");
    public static readonly MetricSystem Ronna = new(MetricFactor.Ronna, "R");
    public static readonly MetricSystem Quetta = new(MetricFactor.Quetta, "Q");
}
