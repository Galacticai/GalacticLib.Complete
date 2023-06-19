namespace GalacticLib.Math.Numerics.Numbers {
    internal abstract class Arithmatic<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                ISpanFormattable {
        public abstract Number<T> Value { get; }

        public static bool operator ==(Arithmatic<T> left, Arithmatic<T> right)
            => left.Equals(right);
        public static bool operator !=(Arithmatic<T> left, Arithmatic<T> right)
            => !left.Equals(right);
        public static bool operator >(Arithmatic<T> left, Arithmatic<T> right)
            => left.Value > right.Value;
        public static bool operator >=(Arithmatic<T> left, Arithmatic<T> right)
            => left.Value >= right.Value;
        public static bool operator <(Arithmatic<T> left, Arithmatic<T> right)
            => left.Value < right.Value;
        public static bool operator <=(Arithmatic<T> left, Arithmatic<T> right)
            => left.Value <= right.Value;

        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object? obj)
            => ReferenceEquals(this, obj)
            || (obj is Arithmatic<T> arithmatic && Value == arithmatic.Value);

    }
}
