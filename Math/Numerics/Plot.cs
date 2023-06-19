// —————————————————————————————————————————————
//?
//!? 📜 Plot.cs
//!? 🖋️ Galacticai 📅 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) GalacticLib/Math/Numerics/Numbers/Number.T.cs
//x      + (Nathan P Jones) doubleMath/doubleEx.cs
//?
// —————————————————————————————————————————————

using GalacticLib.Math.Numerics.Numbers;
using System.Text;
using static System.Math;

namespace GalacticLib.Math.Numerics;

public class Plot {
    #region object

    /// <summary> Cell width in x-axis where a value is recorded </summary>
    public double CellWidth { get; }
    /// <summary> Range where values are recorded </summary>
    public Range<double> Range => new(_Range.Start, _Range.End);
    private readonly Range<double> _Range;
    public double PropegationPoint { get; }
    /// <summary> Function converting x to y axis </summary>
    public Func<double, double> Fx { get; }
    /// <summary> Recorded values in <see cref="Range"/> for every <see cref="CellWidth"/> </summary>
    public SortedDictionary<double, double> Values => new(_Values);
    private readonly SortedDictionary<double, double> _Values;

    public Plot(double cellWidth, Range<double> range, Func<double, double> fx, double? propegationPoint = null) {
        if (range.Start == range.End)
            throw new ArgumentOutOfRangeException($"Range ({range}) can't be zero-width or less.");
        if (cellWidth > range.Width.Double)
            throw new ArgumentOutOfRangeException($"Cell width ({cellWidth}) must fit inside the range ({range})");
        if (propegationPoint != null && !range.Contains((Number<double>)propegationPoint))
            throw new ArgumentOutOfRangeException($"Propegation point (${propegationPoint}) is out of range ({range})");

        CellWidth = cellWidth;
        _Range = new(range.Min, range.Max);
        PropegationPoint = propegationPoint ?? _Range.Start;
        Fx = fx;

        _Values = new();

        void Record(double x) => _Values[x] = Fx(x);
        for (double x = PropegationPoint; x <= _Range.End; x += CellWidth)
            Record(x);
        if (propegationPoint != null) {
            for (double x = PropegationPoint; x >= _Range.Start; x -= CellWidth)
                Record(x);
        }
        if (!_Values.ContainsKey(_Range.Start))
            _Values[_Range.Start] = Fx(_Range.Start);
        if (!_Values.ContainsKey(_Range.End))
            _Values[_Range.End] = Fx(_Range.End);
    }

    #endregion
    #region Shortcuts

    /// <summary> Amount of <see cref="Values"/> recorded </summary>
    public int Stops => _Values.Count;

    #endregion
    #region

    /// <summary> f(x) =  </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="standardDeviation"/></exception>
    public static Plot NormalDistribution(double cellWidth, double mean, double standardDeviation) {
        if (standardDeviation == 0)
            throw new ArgumentOutOfRangeException(nameof(standardDeviation), "Standard deciation can't be 0.");

        double boundary = 3 * standardDeviation;
        Range<double> range = new(mean - boundary, mean + boundary);
        double fx(double x)
            => 1 / (standardDeviation * Sqrt(2 * PI))
                * Pow(E, -.5 * Pow((x - mean) / standardDeviation, 2));

        return new(cellWidth, range, fx, mean);
    }


    #endregion
    #region Overrides

    public string ToString(char separator) {
        StringBuilder sb = new();
        foreach ((double x, double y) in _Values) {
            sb.Append(y);
            if (x < Range.Max) {
                sb.Append(separator);
                sb.Append(' ');
            }
        }
        return sb.ToString();
    }
    public override string ToString() => ToString(',');
    public override int GetHashCode()
        => HashCode.Combine(CellWidth, Range, Fx);
    public override bool Equals(object? obj)
        => obj is Plot distrobution
        && CellWidth.Equals(distrobution.CellWidth)
        && Range.Equals(distrobution.Range)
        && Fx.Equals(distrobution.Fx);

    #endregion
    #region Operators

    public static bool operator ==(Plot left, Plot right) => left.Equals(right);
    public static bool operator !=(Plot left, Plot right) => !(left == right);

    #endregion
}
