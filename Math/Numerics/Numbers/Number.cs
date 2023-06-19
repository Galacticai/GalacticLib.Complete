// —————————————————————————————————————————————
//?
//!? 📜 Number.cs
//!? 🖋️ Galacticai 📅 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


namespace GalacticLib.Math.Numerics.Numbers {
    /// <summary> Number that could be any type. (Similar to Javascript)
    /// <br/> ⚠️ <see cref="Value"/> type depends on the input initial value type
    /// <br/><br/>
    /// > Allowed types:
    /// <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
    ///  </summary>
    public readonly struct Number {
        #region this object

        private readonly dynamic _Value;
        /// <summary> The Value of this <see cref="Number"/> (The <see cref="System.Type"/> depends on the initial value type from the constructor
        /// <br/><br/>
        /// > Possible types: <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>
        /// <br/>
        /// > Actual type: Use <see cref="Type"/>
        /// </summary>
        public dynamic Value => _Value ?? 0;
        public Number() { _Value = 0; }
        public Number(sbyte value) { _Value = value; }
        public Number(byte value) { _Value = value; }
        public Number(short value) { _Value = value; }
        public Number(ushort value) { _Value = value; }
        public Number(int value) { _Value = value; }
        public Number(uint value) { _Value = value; }
        public Number(long value) { _Value = value; }
        public Number(ulong value) { _Value = value; }
        public Number(float value) { _Value = value; }
        public Number(double value) { _Value = value; }
        public Number(decimal value) { _Value = value; }

        #endregion
        #region Shortcuts

        public static Number FromGeneric<T>(Number<T> number)
                where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, ISpanFormattable
            => new((dynamic)number.Value);

        /// <summary> <see cref="Type"/> of <see cref="Value"/> </summary>
        public Type ValueType => Value.GetType();

        public int CompareTo(object other) => Value.CompareTo(other);

        /// <summary> Convert this <see cref="Number"/> to a new one
        /// <br/> with the specified <typeparamref name="TOther"/> as the <see cref="ValueType"/> </summary>
        public Number ConvertTo<TOther>()
                where TOther : struct, IComparable, IComparable<TOther>, IConvertible, IEquatable<TOther>, IFormattable
            => (Number)Convert.ChangeType(Value, typeof(TOther));
        /// <summary> Convert this <see cref="Number"/> to a new one
        /// <br/> with the wider data type as the <see cref="ValueType"/>
        /// <br/> (To avoid data loss) </summary>
        public Number ConvertToWider<TOther>()
                where TOther : struct, IComparable, IComparable<TOther>, IConvertible, IEquatable<TOther>, IFormattable
            => (Number)Convert.ChangeType(Value, NumberMath.GetWiderNumericType(ValueType, typeof(TOther)));

        #endregion
        #region Operators

        #region Math

        public static bool operator ==(Number number1, Number number2)
            => number1.Equals(number2.Value);
        public static bool operator !=(Number number1, Number number2)
            => !number1.Equals(number2.Value);
        public static bool operator >(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) > Convert.ChangeType(number2.Value, widerType));
        }
        public static bool operator <(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) < Convert.ChangeType(number2.Value, widerType));
        }
        public static bool operator >=(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) >= Convert.ChangeType(number2.Value, widerType));
        }
        public static bool operator <=(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) <= Convert.ChangeType(number2.Value, widerType));
        }

        public static Number operator +(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) + Convert.ChangeType(number2.Value, widerType));
        }
        public static Number operator -(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) - Convert.ChangeType(number2.Value, widerType));
        }
        public static Number operator *(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) * Convert.ChangeType(number2.Value, widerType));
        }
        public static Number operator /(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) / Convert.ChangeType(number2.Value, widerType));
        }

        public static Number operator %(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) % Convert.ChangeType(number2.Value, widerType));
        }

        public static Number operator +(Number number) => new(+number.Value);
        public static Number operator -(Number number) => new(-number.Value);

        public static Number operator ++(Number number) => number + 1;
        public static Number operator --(Number number) => number - 1;

        #region Bitwise

        public static Number operator ~(Number number)
            => new(~number.Value);
        //public static Number operator <<(Number number1, Number number2)
        //    => new(number1.Value << number2.Value);
        //public static Number operator >>(Number number1, Number number2)
        //    => new(number1.Value >> number2.Value);
        //public static Number operator >>>(Number number1, Number number2)
        //    => new(number1.Value >>> number2.Value);

        #endregion

        #endregion
        #region Logic

        /// <summary> AND </summary>
        /// <returns> New <see cref="Number"/> which is the AND result </returns>
        public static Number operator &(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) % Convert.ChangeType(number2.Value, widerType));
        }

        /// <summary> OR </summary>
        /// <returns> New <see cref="Number"/> which is the OR result </returns>
        public static Number operator |(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) | Convert.ChangeType(number2.Value, widerType));
        }

        /// <summary> XOR </summary>
        /// <returns> New <see cref="Number"/> which is the XOR result </returns>
        public static Number operator ^(Number number1, Number number2) {
            Type? widerType = NumberMath.GetWiderNumericType(number1.ValueType, number2.ValueType);
            return (Number)(Convert.ChangeType(number1.Value, widerType) ^ Convert.ChangeType(number2.Value, widerType));
        }

        /// <summary> NOT </summary>
        /// <returns> true if <paramref name="number"/> is equivelant to 0 </returns>
        public static bool operator !(Number number) => !Convert.ToBoolean(number.Value);
        /// <summary> is false </summary>
        /// <returns> true if <paramref name="number"/> is equal to 0
        /// <br/> (!<see cref="Convert.ToBoolean(dynamic)"/>) </returns>
        public static bool operator false(Number number) => !Convert.ToBoolean(number.Value);
        /// <summary> is true </summary>
        /// <returns> true if <paramref name="number"/> is different from 0
        /// <br/> (<see cref="Convert.ToBoolean(dynamic)"/>) </returns>
        public static bool operator true(Number number) => Convert.ToBoolean(number.Value);

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
        public static implicit operator bool(Number number) => number.Bool;
        public static implicit operator sbyte(Number number) => number.SByte;
        public static implicit operator byte(Number number) => number.Byte;
        public static implicit operator short(Number number) => number.Short;
        public static implicit operator ushort(Number number) => number.UShort;
        public static implicit operator int(Number number) => number.Int32;
        public static implicit operator uint(Number number) => number.UInt32;
        public static implicit operator long(Number number) => number.Int64;
        public static implicit operator ulong(Number number) => number.UInt64;
        public static implicit operator nint(Number number) => number.IntPtr;
        public static implicit operator nuint(Number number) => number.UIntPtr;
        public static implicit operator float(Number number) => number.Single;
        public static implicit operator double(Number number) => number.Double;
        public static implicit operator decimal(Number number) => number.Decimal;

        public static implicit operator Number(sbyte value) => new(value);
        public static implicit operator Number(byte value) => new(value);
        public static implicit operator Number(short value) => new(value);
        public static implicit operator Number(ushort value) => new(value);
        public static implicit operator Number(int value) => new(value);
        public static implicit operator Number(uint value) => new(value);
        public static implicit operator Number(long value) => new(value);
        public static implicit operator Number(ulong value) => new(value);
        public static implicit operator Number(float value) => new(value);
        public static implicit operator Number(double value) => new(value);
        public static implicit operator Number(decimal value) => new(value);

        #endregion

        #endregion
        #region Overrides

#nullable enable
        /// <summary> Check whether an <paramref name="obj"/> is equal to this <see cref="Number"/> </summary>
        /// <returns> <see cref="true"/> if <paramref name="obj"/> is a <see cref="Number"/> that's is equal to this <see cref="Number"/></returns>
        public override bool Equals(object? obj)
            => obj != null && Value.Equals(obj);
#nullable restore
        /// <summary> HashCode of <see cref="Value"/> </summary>
        public override int GetHashCode()
            => Value.GetHashCode();
        /// <summary> Convert <see cref="Value"/> to <see cref="string"/> </summary>
        public override string ToString()
            => Value.ToString();

        #endregion
    }
}
