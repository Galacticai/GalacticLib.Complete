using GalacticLib.Math.Numerics.Numbers;

namespace GalacticLib.Math.Numerics {
    /// <summary> <see cref="Number{T}"/> value that exists in a <see cref="Range{T}"/> </summary>
    public class Amount<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                ISpanFormattable {
        /// <summary> The ratio of <see cref="Value"/>,  relative to the <see cref="Range"/>'s min and max </summary>
        public Number<T> Ratio_InRange
            => Range.GetRatio(Value);

        private Number<T> _Value;
        /// <summary> Value of this <see cref="Amount{T}"/> </summary>
        public Number<T> Value {
            get => _Value;
            set => _Value = Range.Clamp(value);
        }
        /// <summary> Boundary (<see cref="Numerics.Range"/>) of this <see cref="Amount{T}"/> </summary>
        public Range<T> Range { get; set; }

        /// <summary> <see cref="Number{T}"/> value that is bounded by the <see cref="Range"/>
        /// <br/> Bounded from 0 to <paramref name="value"/> </summary>
        public Amount(Number<T> value)
                : this(value, new((T)(object)0, value)) { }
        /// <summary> <see cref="Number{T}"/> value that is bounded by the <see cref="Range"/>
        /// <br/> Bounded by <paramref name="range"/> </summary>
        public Amount(Number<T> value, Range<T> range) {
            Range = range;
            Value = value;
        }

        /// <summary> (implicit) <see cref="Value"/> of <paramref name="amount"/> </summary>
        /// <param name="amount"> Target <see cref="Amount{T}"/> </param>
        public static implicit operator Number<T>(Amount<T> amount) => amount.Value;

        //!? Replacing the instance causes the Range to be reset which means old range data gets lost unexpectedly!
        //!?    => Better create a new instance manually while knowing that a new range is made
        //public static implicit operator Amount<T>(Number<T> value) => new(value);

        /// <summary> Convert this <see cref="Amount{T}"/> to <see cref="string"/> </summary>
        /// <returns> "<see cref="Value"/>" </returns>
        public override string ToString()
            => Value.ToString();
        /// <summary> Get the hash code for this <see cref="Amount{T}"/>  </summary>
        /// <returns> Combined hash code of <see cref="Value"/> and <see cref="Range"/> </returns>
        public override int GetHashCode()
            => HashCode.Combine(Value, Range);
        /// <summary> Determines whether this <see cref="Amount{T}"/> is equal to an <see cref="object"/> </summary>
        /// <param name="obj"> Target <see cref="object"/> </param>
        /// <returns> true if:
        /// <list type="number">
        ///     <item> <paramref name="obj"/> is an <see cref="Amount{T}"/> </item>
        ///     <item> <see cref="Value"/>s and <see cref="Range"/>s are equal </item>
        /// </list></returns>
#nullable enable
        public override bool Equals(object? obj)
            => ReferenceEquals(this, obj)
            || (obj is Amount<T> other
                && Value == other.Value
                && Range.Equals(other.Range));
#nullable restore
        /// <summary> Determines if 2 <see cref="Amount{T}"/>s are equal </summary>
        /// <returns> true if <paramref name="amount1"/> and <paramref name="amount2"/> are equal </returns>
        public static bool operator ==(Amount<T> amount1, Amount<T> amount2)
            => amount1.Equals(amount2);
        /// <summary> Determines if 2 <see cref="Amount{T}"/>s are different </summary>
        /// <returns> true if <paramref name="amount1"/> and <paramref name="amount2"/> are different </returns>
        public static bool operator !=(Amount<T> amount1, Amount<T> amount2)
            => !amount1.Equals(amount2);
        /// <summary> Determines if an <see cref="Amount{T}"/> is greater than another </summary>
        /// <returns> true if the <see cref="Value"/> of <paramref name="amount1"/> is greater than the that of <paramref name="amount2"/> </returns>
        public static bool operator >(Amount<T> amount1, Amount<T> amount2)
            => amount1.Value > amount2.Value;
        /// <summary> Determines if an <see cref="Amount{T}"/> is smaller than another </summary>
        /// <returns> true if the <see cref="Value"/> of <paramref name="amount1"/> is smaller than the that of <paramref name="amount2"/> </returns>
        public static bool operator <(Amount<T> amount1, Amount<T> amount2)
            => amount1.Value < amount2.Value;
        /// <summary> Determines if an <see cref="Amount{T}"/> is greater than another </summary>
        /// <returns> true if the <see cref="Value"/> of <paramref name="amount1"/> is greater or equal to the that of <paramref name="amount2"/> </returns>
        public static bool operator >=(Amount<T> amount1, Amount<T> amount2)
            => amount1.Value >= amount2.Value;
        /// <summary> Determines if an <see cref="Amount{T}"/> is smaller than another </summary>
        /// <returns> true if the <see cref="Value"/> of <paramref name="amount1"/> is smaller or equal to the that of <paramref name="amount2"/> </returns>
        public static bool operator <=(Amount<T> amount1, Amount<T> amount2)
            => amount1.Value <= amount2.Value;
    }
}
