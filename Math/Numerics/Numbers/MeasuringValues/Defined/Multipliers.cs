namespace GalacticLib.Math.Numerics.Numbers.MeasuringValues.Defined;

/// <summary> Multiply a value having a unit corresponding to one of these in order to get the value but in the base unit </summary>
public static class Multipliers {
    /// <summary> <see href="https://en.wikipedia.org/wiki/Metric_prefix"/> </summary>
    public static class Metric {
        public const double Quecto = 1E-30;
        public const double Ronto = 1E-27;
        public const double Yocto = 1E-24;
        public const double Zepto = 1E-21;
        public const double Atto = 1E-18;
        public const double Femto = 1E-15;
        public const double Pico = 1E-12;
        public const double Nano = 1E-9;
        public const double Micro = 1E-6;
        public const double Milli = 1E-3;
        public const double Centi = 1E-2;
        public const double Deci = 1E-1;

        public const double Deca = 1E1;
        public const double Hecto = 1E2;
        public const double Kilo = 1E3;
        public const double Mega = 1E6;
        public const double Giga = 1E9;
        public const double Tera = 1E12;
        public const double Peta = 1E15;
        public const double Exa = 1E18;
        public const double Zetta = 1E21;
        public const double Yotta = 1E24;
        public const double Ronna = 1E27;
        public const double Quetta = 1E30;
    }
    /// <summary> <see href="https://en.wikipedia.org/wiki/Binary_prefix"/>
    /// <br/> <see href="https://en.wikipedia.org/wiki/ISO/IEC_80000"/> </summary>
    public static class Binary {
        public const long Kibi = 1L << 10;
        public const long Mebi = 1L << 20;
        public const long Gibi = 1L << 30;
        public const long Tebi = 1L << 40;
        public const long Pebi = 1L << 50;
        public const long Exbi = 1L << 60;
        public const long Zebi = 1L << 70;
        public const long Yobi = 1L << 80;
    }

    /// <summary> Using seconds as the base </summary>
    public static class Time {
        public const int Minute = 60;
        public const int Hour = Minute * 60;
        public const int Day = Hour * 24;
        public const int Month = Day * 30;
        public const int Year = Month * 12;
    }
}