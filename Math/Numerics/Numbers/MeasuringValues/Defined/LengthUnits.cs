using GalacticLib.Math.Numerics.Numbers.Units;

namespace GalacticLib.Math.Numerics.Numbers.MeasuringValues.Defined;

public static class LengthUnits {
    public static BaseUnit Meter => new(new("m", nameof(Meter)));

    public static CompoundUnit YoctoMeter => new(Exponents.Metric.Yocto, Meter);
    public static CompoundUnit ZeptoMeter => new(Exponents.Metric.Zepto, Meter);
    public static CompoundUnit AttoMeter => new(Exponents.Metric.Atto, Meter);
    public static CompoundUnit FemtoMeter => new(Exponents.Metric.Femto, Meter);
    public static CompoundUnit PicoMeter => new(Exponents.Metric.Pico, Meter);
    public static CompoundUnit NanoMeter => new(Exponents.Metric.Nano, Meter);
    public static CompoundUnit MicroMeter => new(Exponents.Metric.Micro, Meter);
    public static CompoundUnit MilliMeter => new(Exponents.Metric.Milli, Meter);
    public static CompoundUnit CentiMeter => new(Exponents.Metric.Centi, Meter);
    public static CompoundUnit DeciMeter => new(Exponents.Metric.Deci, Meter);
    public static CompoundUnit DecaMeter => new(Exponents.Metric.Deca, Meter);
    public static CompoundUnit KiloMeter => new(Exponents.Metric.Kilo, Meter);
    public static CompoundUnit MegaMeter => new(Exponents.Metric.Mega, Meter);
    public static CompoundUnit GigaMeter => new(Exponents.Metric.Giga, Meter);
    public static CompoundUnit TeraMeter => new(Exponents.Metric.Tera, Meter);
    public static CompoundUnit PetaMeter => new(Exponents.Metric.Peta, Meter);
    public static CompoundUnit ExaMeter => new(Exponents.Metric.Exa, Meter);
    public static CompoundUnit ZettaMeter => new(Exponents.Metric.Zetta, Meter);
    public static CompoundUnit YottaMeter => new(Exponents.Metric.Yotta, Meter);

    public const double LightyearInKiloMeters = 9_460_730_472_580.8;
    public static SingleUnit Lightyear
        => new(new("ly", nameof(Lightyear)), LightyearInKiloMeters * Multipliers.Metric.Kilo, Meter);

    public const double ParsecInMeters = 3.0857E16;
    public static SingleUnit Parsec
        => new(new("pc", nameof(Parsec)), ParsecInMeters, Meter);
    public static CompoundUnit MegaParsec => new(Exponents.Metric.Mega, Parsec);
    public static CompoundUnit GigaParsec => new(Exponents.Metric.Giga, Parsec);

    /// <summary> ⚠️ Margin of error = ± 0.030 km </summary>
    public const double AstronomicalUnitInKiloMeters = 149_597_870.691;
    /// <summary> ⚠️ Margin of error = ± 0.030 km </summary>
    public static SingleUnit AstronomicalUnit
        => new(new("AU", nameof(AstronomicalUnit)), AstronomicalUnitInKiloMeters * Multipliers.Metric.Kilo, Meter);

    /// <summary> ⚠️ Margin of error: 5.291 772 109 03 (80) ×10^−11 m</summary>
    public const double BohrRadiusInMeters = 5.29177210903E-11;
    /// <summary> ⚠️ Margin of error: 5.291 772 109 03 (80) ×10^−11 m</summary>
    public static SingleUnit BohrRadius
        => new(new("a₀", nameof(BohrRadius)), BohrRadiusInMeters, Meter);
}

