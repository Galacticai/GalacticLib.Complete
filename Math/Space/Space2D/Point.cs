// —————————————————————————————————————————————
//?
//!? 📜 Point.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


using ScreenFIRE.Lib.Math.Numerics.Numbers;
using sMath = System.Math;
// —————————————————————————————————————————————
//?
//!? 📜 Point.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————



namespace ScreenFIRE.Lib.Math.Space.Space2D {
    /// <summary> A representation of a point in 2D space </summary>
    public class Point<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                IFormattable {
        /// <summary> Origin point (0,0) </summary>
        public static Point<T> ORIGIN => new(new((T)(object)0), new((T)(object)0));

        /// <summary> Get the distance between this <see cref="Point"/> and another one </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> Distance from this <see cref="Point"/> to the other <paramref name="point"/> </returns>
        public Number<double> Distance(Point<T> point)
            => sMath.Sqrt(
                sMath.Pow((X - point.X).Double, 2)
                + sMath.Pow((Y - point.Y).Double, 2)
            );

        /// <summary> Get the distance between this <see cref="Point"/> and the <see cref="ORIGIN"/> (0,0) </summary>
        /// <returns> Distance from this <see cref="Point"/> to the <see cref="ORIGIN"/> (0,0) </returns>
        public Number<double> DistanceToOrigin => Distance(ORIGIN);

        /// <summary> Position of this <see cref="Point"/> on the x-axis </summary>
        public Number<T> X { get; set; }
        /// <summary> Position of this <see cref="Point"/> on the y-axis </summary>
        public Number<T> Y { get; set; }
        /// <summary> A representation of a point in 2D space
        /// <br/> Generated with <see cref="X"/> and <see cref="Y"/> positions </summary>
        public Point(Number<T> x, Number<T> y) {
            X = x; Y = y;
        }
    }
}
