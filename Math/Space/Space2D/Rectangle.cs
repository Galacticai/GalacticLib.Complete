// —————————————————————————————————————————————
//?
//!? 📜 Rectangle.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space2D/Point.cs
//      + (Galacticai) Math/Space2D/RectanglePoints.cs
//?
// —————————————————————————————————————————————


using ScreenFIRE.Lib.Math.Numerics.Numbers;
using sMath = System.Math;

namespace ScreenFIRE.Lib.Math.Space.Space2D {
    //TODO: TEST Rectangle.{xyz}Length calculation
    /// <summary> A representation of a rectangle in 2D space </summary>
    public class Rectangle<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                IFormattable {
        #region Helper Methods
        /// <summary> Map a <see cref="Point"/> from current <see cref="Rectangle"/> to another </summary>
        /// <param name="point"> Source <see cref="Point"/> </param>
        /// <param name="rectangle"> Target <see cref="Rectangle"/> </param>
        public Point<T> MapPointIntoBox(Point<T> point, Rectangle<T> rectangle)
            => MapPointIntoBox(point, rectangle, false);
        /// <summary> Map a <see cref="Point{T}"/> from current <see cref="Rectangle"/> to another </summary>
        /// <param name="point"> Source <see cref="Point{T}"/> </param>
        /// <param name="rectangle"> Target <see cref="Rectangle"/> </param>
        /// <param name="clamp"> Force the output to be inside the target <paramref name="rectangle"/> </param>
        public Point<T> MapPointIntoBox(Point<T> point, Rectangle<T> rectangle, bool clamp) {
            Point<T> newPoint = new(
                rectangle.X + (rectangle.XLength * (point.X - X) / XLength),
                rectangle.Y + (rectangle.YLength * (point.Y + Y) / YLength)
            );
            if (clamp) {
                newPoint.X = NumberMath<T>.Clamp(newPoint.X, rectangle.X, rectangle.X + rectangle.XLength);
                newPoint.Y = NumberMath<T>.Clamp(newPoint.Y, rectangle.Y, rectangle.Y + rectangle.YLength);
            }
            return newPoint;
        }
        #endregion

        /// <summary> Position of the <see cref="Rectangle"/> on the x-axis </summary>
        /// <value></value>
        public double X { get; set; }
        /// <summary> Position of the <see cref="Rectangle"/> on the y-axis </summary>
        public double Y { get; set; }
        /// <summary> Length of the <see cref="Rectangle"/> on the x-axis </summary>
        public double XLength { get; set; }
        /// <summary> Length of the <see cref="Rectangle"/> on the y-axis </summary>
        public double YLength { get; set; }

        /// <summary> Area of the <see cref="Rectangle"/> </summary>
        public double Area
            => XLength * YLength;
        /// <summary> All the points of the <see cref="Rectangle"/> </summary>
        public RectangleCorners Points => new(this);

        /// <summary> A representation of a rectangle in 2D space
        /// <br/> Generated with position and length data </summary>
        /// <param name="x"> Position of the <see cref="Rectangle"/> on the y-axis </param>
        /// <param name="y"> Position of the <see cref="Rectangle"/> on the y-axis </param>
        /// <param name="xLength"> Length of the <see cref="Rectangle"/> on the x-axis </param>
        /// <param name="yLength"> Length of the <see cref="Rectangle"/> on the y-axis </param>
        public Rectangle(double x, double y, double xLength, double yLength) {
            X = x;
            Y = y;
            XLength = sMath.Abs(xLength);
            YLength = sMath.Abs(yLength);
        }
        /// <summary> A representation of a rectangle in 2D space
        /// <br/> Generated with position and length data </summary>
        /// <param name="position"> Position of the <see cref="Rectangle"/> </param>
        /// <param name="xLength"> Length of the <see cref="Rectangle"/> on the x-axis </param>
        /// <param name="yLength"> Length of the <see cref="Rectangle"/> on the y-axis </param>
        public Rectangle(Point position, double xLength, double yLength)
                    : this(position.X, position.Y, xLength, yLength) { }
        /// <summary> A representation of a rectangle in 2D space
        /// <br/> Generated with <see cref="Point"/>s... </summary>
        /// <param name="point1"> Top left <see cref="Point"/> </param>
        /// <param name="point2"> Bottom right <see cref="Point"/> </param>
        public Rectangle(Point point1, Point point2)
                    : this(point1.X, point1.Y,
                        sMath.Max(point1.X, point2.X) - sMath.Min(point1.X, point2.X),
                        sMath.Max(point1.Y, point2.Y) - sMath.Min(point1.Y, point2.Y)) { }
    }
}
