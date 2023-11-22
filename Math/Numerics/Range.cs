// —————————————————————————————————————————————
//?
//!? 📜 Range.cs
//!? 🖋️ Galacticai 📅 2022 - 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) GalacticLib/Math/Numerics/Numbers/Number.T.cs
//?
// —————————————————————————————————————————————

using GalacticLib.Math.Numerics.Numbers;

namespace GalacticLib.Math.Numerics;

/// <summary> Numerical boundary (<see cref="Number{T}"/> type) </summary>
public class Range<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                ISpanFormattable {
    #region this object

    private Number<T> _Start;
    private Number<T> _End;
    /// <summary> Srarting boundary </summary>
    public Number<T> Start {
        get => Reverse ? _End : _Start;
        set => _Start = value;
    }
    /// <summary> Ending boundary </summary>
    public Number<T> End {
        get => Reverse ? _Start : _End;
        set => _End = value;
    }
    /// <summary> Reverses the <see cref="Start"/> and <see cref="End"/> </summary>
    public bool Reverse { get; set; }
    public Range(Number<T> start, Number<T> end, bool fromEnd) {
        Start = start;
        End = end;
        Reverse = fromEnd;
    }
    public Range(Number<T> start, Number<T> end) : this(start, end, false) { }

    #endregion
    #region Shortcuts

    /// <summary> Minimum boundary of this <see cref="Range{T}"/> </summary>
    public Number<T> Min => NumberMath<T>.Min(_Start, _End);
    /// <summary> Maximum boundary of this <see cref="Range{T}"/> </summary>
    public Number<T> Max => NumberMath<T>.Max(_Start, _End);
    /// <summary> Width of this <see cref="Range{T}"/> </summary>
    public Number<T> Width => Max - Min;
    public Number<T> Center => (Min + Max) / new Number<T>((T)Convert.ChangeType(2, typeof(T)));

    #endregion
    #region Methods
    /// <summary> Clamp <paramref name="x"/> into this <see cref="Range{T}"/> </summary>
    /// <returns> <see cref="Min"/> &lt;= <paramref name="x"/> &lt;= <see cref="Max"/> </returns>
    public Number<T> Clamp(Number<T> x) => NumberMath<T>.Clamp(x, Min, Max);

    public Range<T> Intersect(Range<T> range) => new(NumberMath<T>.Max(Min, range.Min), NumberMath<T>.Min(Max, range.Max));
    public Range<T> Add(Range<T> range) => new(NumberMath<T>.Min(Min, range.Min), NumberMath<T>.Max(Max, range.Max));
    public Range<T>? Subtract(Range<T> range) {
        if (range.Contains(this)) return null;
        else if (!Overlaps(range))
            return new(Min, Max);
        if (range.Min <= Min && range.Max >= Max)
            return null;
        if (range.Min <= Min)
            return new(range.Max, Max);
        else return new(Min, range.Min);
    }

    public Range<T> Complement()
        => new(
            Min > NumberMath<T>.NegativeInfinity
                ? NumberMath<T>.Max(NumberMath<T>.MinusOne, Min + NumberMath<T>.MinusOne)
                : NumberMath<T>.NegativeInfinity,
            Max < NumberMath<T>.PositiveInfinity
                ? NumberMath<T>.Min(NumberMath<T>.Zero, Max + NumberMath<T>.One)
                : NumberMath<T>.PositiveInfinity
        );
    public Range<T>? Difference(Range<T> range) {
        if (Min >= range.Max || range.Min >= Max)
            return this;
        else if (Min >= range.Min && range.Max >= Max)
            return null;
        else if (range.Min <= Min)
            return new(range.Max, Max);
        else return new(Min, range.Min);
    }
    //public Range<T>? SymmetricDifference(Range<T> range) {
    //    Range<T>? range1 = Difference(range);
    //    Range<T>? range2 = range.Difference(this);
    //    if (range1 == null) return range2;
    //    else if (range2 == null) return range1;
    //    else return range1.Add(range2);
    //}

    public bool Overlaps(Range<T> range) => (Min <= range.Max) && (range.Min <= Max);
    public bool Contains(Number<T> number) => NumberMath<T>.IsClamped(number, Min, Max);
    public bool Contains(Range<T> range) => (Min <= range.Min) && (range.Max <= Max);
    public bool IsAdjacentTo(Range<T> range) => (Max == range.Min) || (range.Max == Min);

    /// <summary> Get the ratio of <paramref name="input"/> relative to this <see cref="Range{T}"/> </summary>
    /// <param name="input"> Input </param>
    /// <returns> Ratio of <paramref name="input"/> relative to this <see cref="Range{T}"/></returns>
    public Number<T> GetRatio(Number<T> input) {
        //? Try catch because double==double is never precise
        //? (can't detect division by zero beforehand)
        try {
            return (input - Min) / (Max - Min);
        } catch { //(System.DivideByZeroException divideByZeroExceptiion) {
            return input > End ? (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
        }
    }

    /// <summary> Get the percentage of <paramref name="input"/> relative to this <see cref="Range{T}"/> </summary>
    /// <param name="input"> Input </param>
    /// <returns> <see cref="GetRatio(Number{T})"/> * 100 </returns>
    public Number<T> GetPercent(Number<T> input)
        => GetRatio(input) * (T)Convert.ChangeType(100, typeof(T));

    public void Deconstruct(out Number<T> start, out Number<T> end) {
        start = Start;
        end = End;
    }

    #endregion

    /// <summary> Converts this <see cref="Range{T}"/> to a <see cref="string"/> </summary>
    /// <returns> "<see cref="Min"/>~<see cref="Max"/>" </returns>
    public override string ToString() => $"{Min}~{Max}";
    /// <summary> Get the hash code for this <see cref="Range{T}"/>  </summary>
    /// <returns> Combined hash code of <see cref="Min"/> and <see cref="Max"/> </returns>
    public override int GetHashCode() => HashCode.Combine(Min, Max);
    /// <summary> Determines whether this <see cref="Range{T}"/> is equal to an <see cref="object"/> </summary>
    /// <param name="obj"> Target <see cref="object"/> </param>
    /// <returns> true if:
    /// <list type="number">
    ///     <item> <paramref name="obj"/> is a <see cref="Range{T}"/> </item>
    ///     <item> <see cref="Min"/> and <see cref="Max"/> values of both <see cref="Range{T}"/>s are equal </item>
    /// </list></returns>
#nullable enable
    public override bool Equals(object? other)
        => ReferenceEquals(this, other)
        || (other is Range<T> otherRange
            && Min == otherRange.Min
            && Max == otherRange.Max);
#nullable restore

    /// <summary> Determines if 2 <see cref="Range{T}"/>s are equal </summary>
    /// <returns> true if <paramref name="range1"/> and <paramref name="range2"/> are equal </returns>
    public static bool operator ==(Range<T> range1, Range<T> range2)
        => range1.Equals(range2);
    /// <summary> Determines if 2 <see cref="Range{T}"/>s are different </summary>
    /// <returns> true if <paramref name="range1"/> and <paramref name="range2"/> are different </returns>
    public static bool operator !=(Range<T> range1, Range<T> range2)
        => !range1.Equals(range2);
    /// <summary> Determines if an <see cref="Range{T}"/> is greater than another </summary>
    /// <returns> true if the <see cref="Max"/> value of <paramref name="range1"/> is greater than that of <paramref name="range2"/> </returns>
    public static bool operator >(Range<T> range1, Range<T> range2)
        => range1.Max > range2.Max;
    /// <summary> Determines if an <see cref="Range{T}"/> is greater than another </summary>
    /// <returns> true if the <see cref="Max"/> value of <paramref name="range1"/> is greater than or equal to that of <paramref name="range2"/> </returns>
    public static bool operator >=(Range<T> range1, Range<T> range2)
        => range1.Max >= range2.Max;
    /// <summary> Determines if an <see cref="Range{T}"/> is smaller than another </summary>
    /// <returns> true if the <see cref="Min"/> value of <paramref name="range1"/> is smaller than that of <paramref name="range2"/> </returns>
    public static bool operator <(Range<T> range1, Range<T> range2)
        => range1.Min < range2.Min;
    /// <summary> Determines if an <see cref="Range{T}"/> is smaller than another </summary>
    /// <returns> true if the <see cref="Min"/> value of <paramref name="range1"/> is smaller than or equal to that of <paramref name="range2"/> </returns>
    public static bool operator <=(Range<T> range1, Range<T> range2)
        => range1.Min <= range2.Min;
}
