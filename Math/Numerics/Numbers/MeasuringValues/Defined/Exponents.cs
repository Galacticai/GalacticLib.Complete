using GalacticLib.Math.Numerics.Numbers.Units;

namespace GalacticLib.Math.Numerics.Numbers.MeasuringValues.Defined;

public static class Exponents {
    /// <summary> Used for array operations as the base for all exponents (just an empty one) </summary>
    public static Exponent None => new(NameShortLong.Empty, 1);

    /// <summary> Using <see cref="Multipliers.Metric"/> </summary>
    public static class Metric {
        public static Exponent Yocto => new(new("y", nameof(Yocto)), Multipliers.Metric.Yocto);
        public static Exponent Zepto => new(new("z", nameof(Zepto)), Multipliers.Metric.Zepto);
        public static Exponent Atto => new(new("a", nameof(Atto)), Multipliers.Metric.Atto);
        public static Exponent Femto => new(new("f", nameof(Femto)), Multipliers.Metric.Femto);
        public static Exponent Pico => new(new("p", nameof(Pico)), Multipliers.Metric.Pico);
        public static Exponent Nano => new(new("n", nameof(Nano)), Multipliers.Metric.Nano);
        public static Exponent Micro => new(new("µ", nameof(Micro)), Multipliers.Metric.Micro);
        public static Exponent Milli => new(new("m", nameof(Milli)), Multipliers.Metric.Milli);
        public static Exponent Centi => new(new("c", nameof(Centi)), Multipliers.Metric.Centi);
        public static Exponent Deci => new(new("d", nameof(Deci)), Multipliers.Metric.Deci);

        public static Exponent Deca => new(new("da", nameof(Deca)), Multipliers.Metric.Deca);
        public static Exponent Kilo => new(new("K", nameof(Kilo)), Multipliers.Metric.Kilo);
        public static Exponent Mega => new(new("M", nameof(Mega)), Multipliers.Metric.Mega);
        public static Exponent Giga => new(new("G", nameof(Giga)), Multipliers.Metric.Giga);
        public static Exponent Tera => new(new("T", nameof(Tera)), Multipliers.Metric.Tera);
        public static Exponent Peta => new(new("P", nameof(Peta)), Multipliers.Metric.Peta);
        public static Exponent Exa => new(new("E", nameof(Exa)), Multipliers.Metric.Exa);
        public static Exponent Zetta => new(new("Z", nameof(Zetta)), Multipliers.Metric.Zetta);
        public static Exponent Yotta => new(new("Y", nameof(Yotta)), Multipliers.Metric.Yotta);
    }

    /// <summary> Using <see cref="Multipliers.Binary"/> </summary>
    public static class Binary {
        public static Exponent Kibi => new(new("Ki", nameof(Kibi)), Multipliers.Binary.Kibi);
        public static Exponent Mebi => new(new("Mi", nameof(Mebi)), Multipliers.Binary.Mebi);
        public static Exponent Gibi => new(new("Gi", nameof(Gibi)), Multipliers.Binary.Gibi);
        public static Exponent Tebi => new(new("Ti", nameof(Tebi)), Multipliers.Binary.Tebi);
        public static Exponent Pebi => new(new("Pi", nameof(Pebi)), Multipliers.Binary.Pebi);
        public static Exponent Exbi => new(new("Ei", nameof(Exbi)), Multipliers.Binary.Exbi);
        public static Exponent Zebi => new(new("Zi", nameof(Zebi)), Multipliers.Binary.Zebi);
        public static Exponent Yobi => new(new("Yi", nameof(Yobi)), Multipliers.Binary.Yobi);
    }
}
