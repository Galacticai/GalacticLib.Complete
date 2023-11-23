// —————————————————————————————————————————————
//?
//!? 📜 Plot.cs
//!? 🖋️ Galacticai 📅 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) GalacticLib/Math/Numerics/Numbers/Number.cs
//      + (Galacticai) GalacticLib/Math/Numerics/Numbers/Number.T.cs
//?
// —————————————————————————————————————————————

using S = System;

namespace GalacticLib.Math.Numerics.Numbers;

public static class NumberMath {
    public static bool IsNumericType(this Type type)
        => Type.GetTypeCode(type) switch {
            TypeCode.Byte or TypeCode.SByte
            or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64
            or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64
            or TypeCode.Single or TypeCode.Double or TypeCode.Decimal
                => true,
            _ => false,
        };
    public static bool CanUseAsNumericValue(this Type type)
        => Type.GetTypeCode(type) switch {
            TypeCode.Byte
            or TypeCode.SByte
            or TypeCode.UInt16
            or TypeCode.UInt32
            or TypeCode.UInt64
            or TypeCode.Int16
            or TypeCode.Int32
            or TypeCode.Int64
            or TypeCode.Decimal
            or TypeCode.Double
            or TypeCode.Single
                => true,
            _ => false
        };

    public static Type? GetWiderNumericType<T1, T2>()
                where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
                where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
            => GetWiderNumericType(typeof(T1), typeof(T2));
    public static Type? GetWiderNumericType(Type type1, Type type2) {
        if (!CanUseAsNumericValue(type1) || !CanUseAsNumericValue(type2)) return null;

        if (type1 == type2) return type1;
        if (type1 == typeof(decimal) || type2 == typeof(decimal))
            return typeof(decimal);
        else if (type1 == typeof(double) || type2 == typeof(double))
            return typeof(double);
        else if (type1 == typeof(float) || type2 == typeof(float))
            return typeof(float);
        else if (type1 == typeof(long) || type2 == typeof(long))
            return typeof(long);
        else if (type1 == typeof(ulong) || type2 == typeof(ulong))
            return typeof(ulong);
        else if (type1 == typeof(int) || type2 == typeof(int))
            return typeof(int);
        else if (type1 == typeof(uint) || type2 == typeof(uint))
            return typeof(uint);
        else if (type1 == typeof(short) || type2 == typeof(short))
            return typeof(short);
        else if (type1 == typeof(ushort) || type2 == typeof(ushort))
            return typeof(ushort);
        else if (type1 == typeof(byte) || type2 == typeof(byte))
            return typeof(byte);
        else if (type1 == typeof(sbyte) || type2 == typeof(sbyte))
            return typeof(sbyte);
        else return null;
    }
    public static bool IsFloatingPoint<T>() => typeof(T).IsFloatingPoint();
    public static bool IsFloatingPoint(this Type type)
        => Type.GetTypeCode(type) switch {
            TypeCode.Decimal or TypeCode.Double or TypeCode.Single => true,
            _ => false
        };

    /// <summary> Get the floating point type equivelant to <paramref name="type"/> </summary>
    /// <returns>
    /// <list type="bullet">
    /// <item> <paramref name="type"/> if already a floating point type  </item>
    /// <item> <see cref="float"/> if <paramref name="type"/> size &lt;= 32 bits </item>
    /// <item> <see cref="double"/> if <paramref name="type"/> size == 64 bits </item>
    /// <item> <see langword="null"/> if <paramref name="type"/> is not a numeric type </item>
    /// </list>
    /// </returns>
    public static Type? ToFloatingPoint(this Type type) {
        if (!type.IsNumericType()) return null;
        if (type.IsFloatingPoint()) return type;
        return S.Runtime.InteropServices.Marshal.SizeOf(type) <= 32
            ? typeof(float) : typeof(double);
    }
    /// <summary> Get the integer type equivelant to <paramref name="type"/> </summary>
    /// <returns>
    /// <list type="bullet">
    /// <item> <paramref name="type"/> if already an integer type  </item>
    /// <item> <see cref="long"/> if <paramref name="type"/> size >= 64 bits </item>
    /// <item> <see cref="int"/> if <paramref name="type"/> size == 32 bits </item>
    /// <item> <see langword="null"/> if <paramref name="type"/> is not a numeric type </item>
    /// </list>
    /// </returns>
    public static Type? ToInteger(this Type type) {
        if (!type.IsNumericType()) return null;
        if (!type.IsFloatingPoint()) return type;
        return S.Runtime.InteropServices.Marshal.SizeOf(type) >= 64
            ? typeof(long) : typeof(int);
    }
}

/// <summary> A special clone of <see cref="S.Math"/> that supports <see cref="Number{T}"/> which respects <typeparamref name="T"/>
/// <br/> instead of using the 1st function override found in <see cref="S.Math"/> (usually <see cref="sbyte"/>)
/// <br/> + More features </summary>
/// <typeparam name="T"></typeparam>
public static class NumberMath<T>
        where T :
            struct,
            IComparable, IComparable<T>,
            IConvertible, IEquatable<T>,
            ISpanFormattable {

    public static Number<T> One => new((T)Convert.ChangeType(1, typeof(T)));
    public static Number<T> Zero => new((T)Convert.ChangeType(0, typeof(T)));
    public static Number<T> MinusOne => new((T)Convert.ChangeType(-1, typeof(T)));
    /// <summary> Allowed types: <br/>
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// <br/> otherwise, returns 0 </summary>
    public static Number<T> NegativeInfinity
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Single => (T)(object)float.NegativeInfinity,
            TypeCode.Double => (T)(object)double.NegativeInfinity,
            TypeCode.Decimal => (T)(object)(-1m / 0.000000000000000000000000001m),
            _ => (T)(object)0
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// <br/> otherwise, returns 0 </summary>
    public static Number<T> PositiveInfinity
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Single => (T)(object)float.PositiveInfinity,
            TypeCode.Double => (T)(object)double.PositiveInfinity,
            TypeCode.Decimal => (T)(object)(1m / 0.000000000000000000000000001m),
            _ => (T)(object)0
        };

    public static bool CanUseAsNumericValue() => NumberMath.CanUseAsNumericValue(typeof(T));
    public static Type? ToFloatingPoint() => NumberMath.ToFloatingPoint(typeof(T));
    public static Type? ToInteger() => NumberMath.ToInteger(typeof(T));


    /// <summary> Allowed types: <br/>
    /// <see cref="sbyte"/>, <see cref="short"/>,
    /// <see cref="int"/>, <see cref="long"/>,
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// </summary>
    public static Number<T> Abs(Number<T> x)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.SByte => (T)(object)S.Math.Abs(x.SByte),
            TypeCode.Int16 => (T)(object)S.Math.Abs(x.Int16),
            TypeCode.Int32 => (T)(object)S.Math.Abs(x.Int32),
            TypeCode.Int64 => (T)(object)S.Math.Abs(x.Int64),
            TypeCode.Single => (T)(object)S.Math.Abs(x.Single),
            TypeCode.Double => (T)(object)S.Math.Abs(x.Double),
            _ => (T)(object)S.Math.Abs(x.Decimal)
        };
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Acos(Number<T> x) => s.Math.Acos(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Acosh(Number<T> x) => s.Math.Acosh(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Asin(Number<T> x) => s.Math.Asin(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Asinh(Number<T> x) => s.Math.Asinh(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Atan(Number<T> x) => s.Math.Atan(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Atan2(Number<T> y, Number<T> x) => s.Math.Atan2(y.Double, x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Atanh(Number<T> x) => s.Math.Atanh(x.Double);
    /////// <summary> Allowed types: <br/>
    /////// <see cref="long"/> </summary>
    ////public static Number<T> BigMul(int a, int b) => s.Math.BigMul(a, b);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Cbrt(Number<T> x) => s.Math.Cbrt(x.Double);
    /// <summary> Allowed types: <br/>
    /// <see cref="decimal"/> <see cref="double"/>
    public static Number<T> Ceiling(Number<T> x)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Ceiling(x.Double),
            _ => (T)(object)S.Math.Ceiling(x.Decimal)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="sbyte"/>, <see cref="byte"/>,
    /// <see cref="short"/>, <see cref="ushort"/>,
    /// <see cref="int"/>, <see cref="uint"/>,
    /// <see cref="long"/>, <see cref="ulong"/>,
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// </summary>
    public static Number<T> Clamp(Number<T> x, Number<T> min, Number<T> max)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.SByte => (T)(object)S.Math.Clamp(x.SByte, min.SByte, max.SByte),
            TypeCode.Byte => (T)(object)S.Math.Clamp(x.Byte, min.Byte, max.Byte),
            TypeCode.Int16 => (T)(object)S.Math.Clamp(x.Int16, min.Int16, max.Int16),
            TypeCode.UInt16 => (T)(object)S.Math.Clamp(x.UInt16, min.UInt16, max.UInt16),
            TypeCode.Int32 => (T)(object)S.Math.Clamp(x.Int32, min.Int32, max.Int32),
            TypeCode.UInt32 => (T)(object)S.Math.Clamp(x.UInt32, min.UInt32, max.UInt32),
            TypeCode.Int64 => (T)(object)S.Math.Clamp(x.Int64, min.Int64, max.Int64),
            TypeCode.UInt64 => (T)(object)S.Math.Clamp(x.UInt64, min.UInt64, max.UInt64),
            TypeCode.Single => (T)(object)S.Math.Clamp(x.Single, min.Single, max.Single),
            TypeCode.Double => (T)(object)S.Math.Clamp(x.Double, min.Double, max.Double),
            _ => (T)(object)S.Math.Clamp(x.Decimal, min.Decimal, max.Decimal)
        };

    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Cos(Number<T> x) => s.Math.Cos(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Cosh(Number<T> x) => s.Math.Cosh(x.Double);
    /// <summary> Allowed types: <br/>
    /// <see cref="int"/>, <see cref="long"/> </summary>
    public static Number<T> DivRem(Number<T> x, Number<T> y, out Number<T> result) {
        Number<T> returnValue;
        switch (Type.GetTypeCode(typeof(T))) {
            case TypeCode.Int32:
                int outValue_int;
                int value_int = S.Math.DivRem(x.Int32, y.Int32, out outValue_int);
                result = (T)(object)outValue_int;
                returnValue = (T)(object)value_int;
                break;
            default:
                long outValue_long;
                long value_long = S.Math.DivRem(x.Int64, y.Int64, out outValue_long);
                result = (T)(object)outValue_long;
                returnValue = (T)(object)value_long;
                break;
        }
        return returnValue;
    }
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Exp(Number<T> x) => s.Math.Exp(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/>, <see cref="decimal"/> </summary>
    //public static Number<T> Floor(Number<T> x) => s.Math.Floor(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> IEEERemainder(Number<T> x, Number<T> y) => s.Math.IEEERemainder(x.Double, y.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Log(Number<T> x) => s.Math.Log(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Log(Number<T> x, Number<T> newBase) => s.Math.Log(x, newBase);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Log10(Number<T> x) => s.Math.Log10(x.Double);
    /// <summary> Allowed types: <br/>
    /// <see cref="sbyte"/>, <see cref="byte"/>,
    /// <see cref="short"/>, <see cref="ushort"/>,
    /// <see cref="int"/>, <see cref="uint"/>,
    /// <see cref="long"/>, <see cref="ulong"/>,
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// </summary>
    public static Number<T> Max(Number<T> x, Number<T> y)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.SByte => (T)(object)S.Math.Max(x.SByte, y.SByte),
            TypeCode.Byte => (T)(object)S.Math.Max(x.Byte, y.Byte),
            TypeCode.Int16 => (T)(object)S.Math.Max(x.Int16, y.Int16),
            TypeCode.UInt16 => (T)(object)S.Math.Max(x.UInt16, y.UInt16),
            TypeCode.Int32 => (T)(object)S.Math.Max(x.Int32, y.Int32),
            TypeCode.UInt32 => (T)(object)S.Math.Max(x.UInt32, y.UInt32),
            TypeCode.Int64 => (T)(object)S.Math.Max(x.Int64, y.Int64),
            TypeCode.UInt64 => (T)(object)S.Math.Max(x.UInt64, y.UInt64),
            TypeCode.Single => (T)(object)S.Math.Max(x.Single, y.Single),
            TypeCode.Double => (T)(object)S.Math.Max(x.Double, y.Double),
            _ => (T)(object)S.Math.Max(x.Decimal, y.Decimal)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="sbyte"/>, <see cref="byte"/>,
    /// <see cref="short"/>, <see cref="ushort"/>,
    /// <see cref="int"/>, <see cref="uint"/>,
    /// <see cref="long"/>, <see cref="ulong"/>,
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// </summary>
    public static Number<T> Min(Number<T> x, Number<T> y)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.SByte => (T)(object)S.Math.Min(x.SByte, y.SByte),
            TypeCode.Byte => (T)(object)S.Math.Min(x.Byte, y.Byte),
            TypeCode.Int16 => (T)(object)S.Math.Min(x.Int16, y.Int16),
            TypeCode.UInt16 => (T)(object)S.Math.Min(x.UInt16, y.UInt16),
            TypeCode.Int32 => (T)(object)S.Math.Min(x.Int32, y.Int32),
            TypeCode.UInt32 => (T)(object)S.Math.Min(x.UInt32, y.UInt32),
            TypeCode.Int64 => (T)(object)S.Math.Min(x.Int64, y.Int64),
            TypeCode.UInt64 => (T)(object)S.Math.Min(x.UInt64, y.UInt64),
            TypeCode.Single => (T)(object)S.Math.Min(x.Single, y.Single),
            TypeCode.Double => (T)(object)S.Math.Min(x.Double, y.Double),
            _ => (T)(object)S.Math.Min(x.Decimal, y.Decimal)
        };
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Pow(Number<T> x, Number<T> y) => s.Math.Pow(x.Double, y.Double);
    /// <summary> Allowed types: <br/>
    /// <see cref="double"/>, <see cref="decimal"/> </summary>
    public static Number<T> Round(Number<T> x, MidpointRounding mode)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Round(x.Double, mode),
            _ => (T)(object)S.Math.Round(x.Decimal, mode)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="double"/>, <see cref="decimal"/> </summary>
    public static Number<T> Round(Number<T> x, int digits, MidpointRounding mode)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Round(x.Double, digits, mode),
            _ => (T)(object)S.Math.Round(x.Decimal, digits, mode)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="double"/>, <see cref="decimal"/> </summary>
    public static Number<T> Round(Number<T> x, int digits)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Round(x.Double, digits),
            _ => (T)(object)S.Math.Round(x.Decimal, digits)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="double"/>, <see cref="decimal"/> </summary>
    public static Number<T> Round(Number<T> x)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Round(x.Double),
            _ => (T)(object)S.Math.Round(x.Decimal)
        };
    /// <summary> Allowed types: <br/>
    /// <see cref="sbyte"/>, <see cref="short"/>,
    /// <see cref="int"/>, <see cref="long"/>,
    /// <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    /// </summary>
    public static int Sign(Number<T> x)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.SByte => S.Math.Sign(x.SByte),
            TypeCode.Int16 => S.Math.Sign(x.Int16),
            TypeCode.Int32 => S.Math.Sign(x.Int32),
            TypeCode.Int64 => S.Math.Sign(x.Int64),
            TypeCode.Single => S.Math.Sign(x.Single),
            TypeCode.Double => S.Math.Sign(x.Double),
            _ => S.Math.Sign(x.Decimal)
        };
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Sin(Number<T> x) => s.Math.Sin(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Sinh(Number<T> x) => s.Math.Sinh(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Sqrt(Number<T> x) => s.Math.Sqrt(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Tan(Number<T> x) => s.Math.Tan(x.Double);
    ///// <summary> Allowed types: <br/>
    ///// <see cref="double"/> </summary>
    //public static Number<T> Tanh(Number<T> x) => s.Math.Tanh(x.Double);
    /// <summary> Allowed types: <br/>
    /// <see cref="double"/>, <see cref="decimal"/> </summary>
    public static Number<T> Truncate(Number<T> x)
        => Type.GetTypeCode(typeof(T)) switch {
            TypeCode.Double => (T)(object)S.Math.Truncate(x.Double),
            _ => (T)(object)S.Math.Truncate(x.Decimal)
        };


    /// <summary> Clamps the <paramref name="x"/> at or above a minimum value </summary>
    /// <param name="x"> Input </param>
    /// <param name="min"> Minimum boundary </param>
    /// <returns> <paramref name="x"/> forced equal or above <paramref name="min"/> </returns>
    public static Number<T> ClampAbove(Number<T> x, Number<T> min)
        => x < min ? min : x;
    /// <summary> Clamps the <paramref name="x"/> at or below a maximum value </summary>
    /// <param name="x"> Input </param>
    /// <param name="max"> Maximum boundary </param>
    /// <returns> <paramref name="x"/> forced equal or below <paramref name="max"/> </returns>
    public static Number<T> ClampBelow(Number<T> x, Number<T> max)
        => x > max ? max : x;
    /// <summary> Clamps the <paramref name="x"/> at or above 0 </summary>
    /// <param name="x"> Input </param>
    /// <returns> <paramref name="x"/> if it's greater than 0; Otherwise, 0.  </returns>
    public static Number<T> Positive(Number<T> x)
        => ClampAbove(x, new Number<T>((T)(object)0));
    /// <summary> Clamps the <paramref name="x"/> at or below 0 </summary>
    /// <param name="x"> Input </param>
    /// <returns> <paramref name="x"/> if it's greater than 0; Otherwise, 0.  </returns>
    public static Number<T> Negative(Number<T> x)
        => ClampBelow(x, new Number<T>((T)(object)0));
    /// <summary> Determines whether <paramref name="x"/> is within a range </summary>
    /// <param name="x"> Input </param>
    /// <param name="min"> Minimum boundary </param>
    /// <param name="max"> Maximum boundary </param>
    /// <returns> true if <paramref name="x"/> is within <paramref name="min"/> and <paramref name="max"/> </returns>
    public static bool IsClamped(Number<T> x, Number<T> min, Number<T> max)
        => x >= min && x <= max;
}
