using System.Text;
using static System.Math;

namespace GalacticLib.Math.Numerics;

/// <summary> Grid of values representing a 2D symmetrical normal distribution curve.
/// <br/> (<see href="https://en.wikipedia.org/wiki/Normal_distribution"/>)
/// <br/> Optimized for performance such that the values are mirrored using half a quadrant as the original measurement
/// <br/><br/> Example: (O=original, M=mirror)
/// <br/> O O O O M M M M
/// <br/> M O O O M M M M
/// <br/> M M O O M M M M
/// <br/> M M M O M M M M
/// <br/> M M M M M M M M
/// <br/> M M M M M M M M
/// <br/> M M M M M M M M
/// <br/> M M M M M M M M
/// </summary>
public class SymmetricNormalGrid {
    #region this object
    /// <summary> Grid size </summary>
    public int Size { get; }
    public float Mean => (Size - 1) / 2f; //no need for double precision, it is either x.0 or x.5
    /// <summary> Standard deviation </summary>
    public double Spread { get; }

    /// <summary> Get a value at <paramref name="x"/>,<paramref name="y"/> coordinates </summary>
    public double this[int x, int y] => Value(x, y);

    /// <summary> Get a value at <paramref name="x"/>,<paramref name="y"/> coordinates </summary>
    public double Value(int x, int y) => _Values[x, y];
    private double[,] _Values { get; }
    public SymmetricNormalGrid(int size, double spread) {
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
        Size = size;
        Spread = spread;

        _Values = new double[Size, Size];

        int halfSize = Size / 2;
        double spreadSquareTimes2 = 2 * Pow(Spread, 2); //? calc once outside the loops

        for (int x = 0; x <= halfSize; x++) {
            int xMirror = Size - 1 - x;
            double xDistanceP2 = Pow(x - Mean, 2);
            for (int y = x; y <= halfSize; y++) {
                int yMirror = Size - 1 - y;
                double yDistanceP2 = Pow(y - Mean, 2);
                double value = Exp(-(xDistanceP2 + yDistanceP2) / spreadSquareTimes2);
                _Values[x, y] = value; // Half quadrant #1
                _Values[xMirror, y] = value; // HQ3
                _Values[x, yMirror] = value; // HQ5
                _Values[xMirror, yMirror] = value; // HQ7
                if (x != y) {
                    _Values[y, x] = value; // HQ2
                    _Values[yMirror, x] = value; // HQ4
                    _Values[y, xMirror] = value; // HQ6
                    _Values[yMirror, xMirror] = value; // HQ8
                }
            }
        }
    }
    #endregion
    #region overrides

    public override string ToString() => ToString(asGrid: false);
    public string ToString(bool asGrid)
        => asGrid
        ? ToString("0.00", ' ')
        : $"{nameof(SymmetricNormalGrid)} ({nameof(Size)}={Size}, {nameof(Spread)}={Spread})";
    public string ToString(string valueFormat, char separator) {
        StringBuilder sb = new();
        for (int x = 0; x < Size; x++) {
            for (int y = 0; y < Size; y++) {
                sb.Append(Value(x, y).ToString(valueFormat));
                sb.Append(separator);
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public override int GetHashCode()
        => HashCode.Combine(Size, Spread);
    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj)
        || (obj is SymmetricNormalGrid gaussianGrid
        && Size.Equals(gaussianGrid.Size)
        && Spread.Equals(gaussianGrid.Spread));

    #endregion
    #region Operators

    #region Comparison

    public static bool operator ==(SymmetricNormalGrid left, SymmetricNormalGrid right)
        => left.Equals(right);
    public static bool operator !=(SymmetricNormalGrid left, SymmetricNormalGrid right)
        => !(left == right);

    #endregion
    #region Convertion

    public static implicit operator double[,](SymmetricNormalGrid gaussianGrid)
        => (double[,])gaussianGrid._Values.Clone();

    #endregion

    #endregion
}
