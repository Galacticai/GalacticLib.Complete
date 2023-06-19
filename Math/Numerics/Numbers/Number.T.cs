using System.Linq.Expressions;

namespace GalacticLib.Math.Numerics.Numbers;
/// <summary>
/// <br/> + More features
/// <br/><br/> ⚠️ Warning: This type cannot defend against <see cref="InvalidOperationException"/>!
/// <br/> Although <see cref="Number{T}"/> tries to preserve data and avoid errors by converting to the common wider data type before operations,
/// <br/> but it's impossible to hide certain functions when <typeparamref name="T"/> does not support them </summary>
/// <exception cref="InvalidOperationException"/>
public readonly struct Number<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                ISpanFormattable {
    #region this object

    public T Value { get; }

    public Number() { Value = (T)(object)0; }
    public Number(T value) { Value = value; }

    #endregion
    #region Shortcuts

    public static Number<T> FromDynamic(Number number) => new(number.Value);

    /// <summary> <see cref="Type"/> of <see cref="Value"/> </summary>
    public Type ValueType => typeof(T);
    public int CompareTo(object other) => Value.CompareTo(other);

    public Number<TOther> ConvertTo<TOther>()
        where TOther : struct, IComparable, IComparable<TOther>, IConvertible, IEquatable<TOther>, ISpanFormattable
        => new((TOther)Convert.ChangeType(Value, typeof(TOther)));

    #endregion
    #region Methods

    public void Deconstruct(out T value) {
        value = Value;
    }

    #endregion
    #region Operators

    #region Comparison

    public static bool operator ==(Number<T> number1, Number<T> number2)
        => number1.Value.Equals(number2.Value);
    public static bool operator !=(Number<T> number1, Number<T> number2)
        => !(number1 == number2);

    public static bool operator >(Number<T> number1, Number<T> number2)
        => number1.Value.CompareTo(number2.Value) > 0;
    public static bool operator <(Number<T> number1, Number<T> number2)
        => number1.Value.CompareTo(number2.Value) < 0;

    public static bool operator >=(Number<T> number1, Number<T> number2)
        => number1.Value.CompareTo(number2.Value) >= 0;
    public static bool operator <=(Number<T> number1, Number<T> number2)
        => number1.Value.CompareTo(number2.Value) <= 0;

    #endregion
    #region Math

    private static Number<T> _RunBinaryExpression<T1, T2>(Func<Expression, Expression, BinaryExpression> expressionMethod, T1 value1, T2 value2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable {
        Type widerType = NumberMath.GetWiderNumericType<T1, T2>() ?? typeof(int);
        return Expression.Lambda<Func<T>>(expressionMethod(
            Expression.Constant((T)Convert.ChangeType(value1, widerType)),
            Expression.Constant((T)Convert.ChangeType(value2, widerType))
        )).Compile().Invoke();
    }
    private static Number<T> _RunUnaryExpression(Func<Expression, UnaryExpression> expressionMethod, T value)
        => Expression.Lambda<Func<T>>(expressionMethod(
            Expression.Constant(value)
        )).Compile().Invoke();

    #region x + y

    public static Number<T> Add<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Add, number1.Value, number2.Value);
    public static Number<T> operator +(Number<T> number1, Number<T> number2) => Add(number1, number2);

    #endregion
    #region x - y

    public static Number<T> Subtract<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Subtract, number1.Value, number2.Value);
    public static Number<T> operator -(Number<T> number1, Number<T> number2) => Subtract(number1, number2);

    #endregion
    #region x * y

    public static Number<T> Multiply<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Multiply, number1.Value, number2.Value);
    public static Number<T> operator *(Number<T> number1, Number<T> number2) => Multiply(number1, number2);

    #endregion
    #region x / y

    public static Number<T> Divide<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Divide, number1.Value, number2.Value);
    public static Number<T> operator /(Number<T> number1, Number<T> number2) => Divide(number1, number2);

    #endregion
    #region x % y

    public static Number<T> Modulo<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Modulo, number1.Value, number2.Value);
    public static Number<T> operator %(Number<T> number1, Number<T> number2) => Modulo(number1, number2);

    #endregion

    #region +x  -x  ++x  --x  x++  x--  ~x

    public static Number<T> operator +(Number<T> number) => _RunUnaryExpression(Expression.UnaryPlus, number.Value);
    public static Number<T> operator -(Number<T> number) => _RunUnaryExpression(Expression.Negate, number.Value);
    public static Number<T> operator ++(Number<T> number) => _RunUnaryExpression(Expression.Increment, number.Value);
    public static Number<T> operator --(Number<T> number) => _RunUnaryExpression(Expression.Decrement, number.Value);
    public static Number<T> operator ~(Number<T> number) => _RunUnaryExpression(Expression.OnesComplement, number.Value);
    //!? Overriding shift operators ( << >> >>> ) are not supported in C# 9

    #endregion

    #endregion
    #region Logic

    #region x & y

    public static Number<T> And<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.And, number1.Value, number2.Value);
    public static Number<T> operator &(Number<T> number1, Number<T> number2) => And(number1, number2);

    #endregion
    #region x | y

    public static Number<T> Or<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.Or, number1.Value, number2.Value);
    public static Number<T> operator |(Number<T> number1, Number<T> number2) => Or(number1, number2);

    #endregion
    #region x ^ y

    public static Number<T> ExclusiveOr<T1, T2>(Number<T1> number1, Number<T2> number2)
            where T1 : struct, IComparable, IComparable<T1>, IConvertible, IEquatable<T1>, ISpanFormattable
            where T2 : struct, IComparable, IComparable<T2>, IConvertible, IEquatable<T2>, ISpanFormattable
        => _RunBinaryExpression(Expression.ExclusiveOr, number1.Value, number2.Value);
    public static Number<T> operator ^(Number<T> number1, Number<T> number2) => ExclusiveOr(number1, number2);

    #endregion


    #region Logic

    #region !x
    public static bool operator !(Number<T> number)
        => EqualityComparer<T>.Default.Equals(number.Value, default);
    #endregion
    #region false
    /// <summary> is false </summary>
    /// <returns> true if <paramref name="number"/> is equal to 0
    /// <br/> (!<see cref="Convert.ToBoolean(dynamic)"/>) </returns>
    public static bool operator false(Number<T> number)
        => !Convert.ToBoolean(number.Value);
    #endregion
    #region true
    /// <summary> is true </summary>
    /// <returns> true if <paramref name="number"/> is different from 0
    /// <br/> (<see cref="Convert.ToBoolean(dynamic)"/>) </returns>
    public static bool operator true(Number<T> number)
        => Convert.ToBoolean(number.Value);
    #endregion

    #endregion


    #endregion
    #region Conversion

    public bool Bool => Convert.ToBoolean(Value);
    public sbyte SByte => Convert.ToSByte(Value);
    public byte Byte => Convert.ToByte(Value);
    public short Int16 => Convert.ToInt16(Value); public short Short => Int16;
    public ushort UInt16 => Convert.ToUInt16(Value); public ushort UShort => UInt16;
    public int Int32 => Convert.ToInt32(Value); public int Int => Int32;
    public uint UInt32 => Convert.ToUInt32(Value); public uint UInt => UInt32;
    public long Int64 => Convert.ToInt64(Value); public long Long => Int64;
    public ulong UInt64 => Convert.ToUInt64(Value); public ulong ULong => UInt64;
    public nint IntPtr => (nint)Convert.ChangeType(Value, typeof(nint)); public nint NInt => IntPtr;
    public nuint UIntPtr => (nuint)Convert.ChangeType(Value, typeof(nuint)); public nuint NUInt => UIntPtr;
    public float Single => Convert.ToSingle(Value); public float Float => Single;
    public double Double => Convert.ToDouble(Value);
    public decimal Decimal => Convert.ToDecimal(Value);

    public static T One => throw new NotImplementedException();

    public static int Radix => throw new NotImplementedException();

    public static T Zero => throw new NotImplementedException();

    public static T AdditiveIdentity => throw new NotImplementedException();

    public static T MultiplicativeIdentity => throw new NotImplementedException();

    public static implicit operator bool(Number<T> number) => number.Bool;
    //public static implicit operator sbyte(Number<T> number) => number.SByte;
    //public static implicit operator byte(Number<T> number) => number.Byte;
    //public static implicit operator short(Number<T> number) => number.Short;
    //public static implicit operator ushort(Number<T> number) => number.UShort;
    //public static implicit operator int(Number<T> number) => number.Int32;
    //public static implicit operator uint(Number<T> number) => number.UInt32;
    //public static implicit operator long(Number<T> number) => number.Int64;
    //public static implicit operator ulong(Number<T> number) => number.UInt64;
    //public static implicit operator nint(Number<T> number) => number.IntPtr;
    //public static implicit operator nuint(Number<T> number) => number.UIntPtr;
    //public static implicit operator float(Number<T> number) => number.Single;
    //public static implicit operator double(Number<T> number) => number.Double;
    //public static implicit operator decimal(Number<T> number) => number.Decimal;
    public static implicit operator T(Number<T> number) => number.Value;

    //public static implicit operator Number<T>(sbyte Sbyte) => new((T)Convert.ChangeType(Sbyte, typeof(T)));
    //public static implicit operator Number<T>(byte Byte) => new((T)Convert.ChangeType(Byte, typeof(T)));
    //public static implicit operator Number<T>(short Short) => new((T)Convert.ChangeType(Short, typeof(T)));
    //public static implicit operator Number<T>(ushort Ushort) => new((T)Convert.ChangeType(Ushort, typeof(T)));
    //public static implicit operator Number<T>(int Int) => new((T)Convert.ChangeType(Int, typeof(T)));
    //public static implicit operator Number<T>(uint Uint) => new((T)Convert.ChangeType(Uint, typeof(T)));
    //public static implicit operator Number<T>(long Long) => new((T)Convert.ChangeType(Long, typeof(T)));
    //public static implicit operator Number<T>(ulong Ulong) => new((T)Convert.ChangeType(Ulong, typeof(T)));
    //public static implicit operator Number<T>(float Float) => new((T)Convert.ChangeType(Float, typeof(T)));
    //public static implicit operator Number<T>(double Double) => new((T)Convert.ChangeType(Double, typeof(T)));
    //public static implicit operator Number<T>(decimal Decimal) => new((T)Convert.ChangeType(Decimal, typeof(T)));
    public static implicit operator Number<T>(Number number) => Number<T>.FromDynamic(number);
    public static implicit operator Number(Number<T> number) => Number.FromGeneric(number);
    public static implicit operator Number<T>(T value) => new(value);

    #endregion

    #endregion
    #region Overrides

#nullable enable
    /// <summary> Check whether an <paramref name="obj"/> is equal to this <see cref="Number{T}"/> </summary>
    /// <returns> <see cref="true"/> if <paramref name="obj"/> is a <see cref="Number{T}"/> that's is equal to this <see cref="Number{T}"/></returns>
    public override bool Equals(object? obj)
        => obj != null
        && obj.GetType().CanUseAsNumericValue()
        && Value.Equals(obj);
#nullable restore
    /// <summary> HashCode of <see cref="Value"/> </summary>
    public override int GetHashCode()
        => Value.GetHashCode();
    /// <summary> Convert <see cref="Value"/> to <see cref="string"/> </summary>
    public override string? ToString()
        => Value.ToString();

    #endregion
}

