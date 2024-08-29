using GalacticLib.Math.Numerics.Numbers.MeasuringValues.Defined;
using GalacticLib.Math.Numerics.Numbers.Units;

namespace GalacticLib.Math.Numerics.Numbers.MeasuringValues.DefinedUnits;

public static class DataUnits {
    public static BaseUnit Bit => new(new("b", nameof(Bit)));
    public static SingleUnit Byte => new(new("B", nameof(Byte)), 8, Bit);

    public static CompoundUnit KiloBit => new(Exponents.Metric.Kilo, Bit);
    public static CompoundUnit MegaBit => new(Exponents.Metric.Mega, Bit);
    public static CompoundUnit GigaBit => new(Exponents.Metric.Giga, Bit);
    public static CompoundUnit TeraBit => new(Exponents.Metric.Tera, Bit);
    public static CompoundUnit PetaBit => new(Exponents.Metric.Peta, Bit);
    public static CompoundUnit ExaBit => new(Exponents.Metric.Exa, Bit);
    public static CompoundUnit ZettaBit => new(Exponents.Metric.Zetta, Bit);
    public static CompoundUnit YottaBit => new(Exponents.Metric.Yotta, Bit);

    public static CompoundUnit KiloByte => new(Exponents.Metric.Kilo, Byte);
    public static CompoundUnit MegaByte => new(Exponents.Metric.Mega, Byte);
    public static CompoundUnit GigaByte => new(Exponents.Metric.Giga, Byte);
    public static CompoundUnit TeraByte => new(Exponents.Metric.Tera, Byte);
    public static CompoundUnit PetaByte => new(Exponents.Metric.Peta, Byte);
    public static CompoundUnit ExaByte => new(Exponents.Metric.Exa, Byte);
    public static CompoundUnit ZettaByte => new(Exponents.Metric.Zetta, Byte);
    public static CompoundUnit YottaByte => new(Exponents.Metric.Yotta, Byte);


    public static CompoundUnit KibiBit => new(Exponents.Binary.Kibi, Bit);
    public static CompoundUnit MebiBit => new(Exponents.Binary.Mebi, Bit);
    public static CompoundUnit GibiBit => new(Exponents.Binary.Gibi, Bit);
    public static CompoundUnit TebiBit => new(Exponents.Binary.Tebi, Bit);
    public static CompoundUnit PebiBit => new(Exponents.Binary.Pebi, Bit);
    public static CompoundUnit ExbiBit => new(Exponents.Binary.Exbi, Bit);
    public static CompoundUnit ZebiBit => new(Exponents.Binary.Zebi, Bit);
    public static CompoundUnit YobiBit => new(Exponents.Binary.Yobi, Bit);

    public static CompoundUnit KibiByte => new(Exponents.Binary.Kibi, Byte);
    public static CompoundUnit MebiByte => new(Exponents.Binary.Mebi, Byte);
    public static CompoundUnit GibiByte => new(Exponents.Binary.Gibi, Byte);
    public static CompoundUnit TebiByte => new(Exponents.Binary.Tebi, Byte);
    public static CompoundUnit PebiByte => new(Exponents.Binary.Pebi, Byte);
    public static CompoundUnit ExbiByte => new(Exponents.Binary.Exbi, Byte);
    public static CompoundUnit ZebiByte => new(Exponents.Binary.Zebi, Byte);
    public static CompoundUnit YobiByte => new(Exponents.Binary.Yobi, Byte);
}

