// —————————————————————————————————————————————
//? 
//!? 📜 Point.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//? 
// —————————————————————————————————————————————

using sMath = System.Math;
// —————————————————————————————————————————————
//? 
//!? 📜 Point.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//? 
// —————————————————————————————————————————————

using ScreenFIRE.Lib.Math.Space.Space3D;

namespace ScreenFIRE.Lib.Math.Space.Space3D
{
    /// <summary> A representation of a point in 3D space </summary>
    public class Point {
        /// <summary> Origin point (0,0,0) </summary>
        public static readonly Point ORIGIN = new(0, 0, 0);

        /// <summary> Get the distance between this <see cref="Point"/> and another one </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> Distance from this <see cref="Point"/> to the other <paramref name="point"/> </returns>
        public double Distance(Point point)
            => sMath.Sqrt(
                sMath.Pow(X - point.X, 2)
                + sMath.Pow(Y - point.Y, 2)
                + sMath.Pow(Z - point.Z, 2));

        /// <summary> Get the distance between this <see cref="Point"/> and the <see cref="ORIGIN"/> (0,0,0) </summary>
        /// <returns> Distance from this <see cref="Point"/> to the <see cref="ORIGIN"/> (0,0,0) </returns>
        public double DistanceToOrigin
            => Distance(Point.ORIGIN);

        /// <summary> Position of this <see cref="Point"/> on the x-axis </summary>
        public double X { get; set; }
        /// <summary> Position of this <see cref="Point"/> on the y-axis </summary>
        public double Y { get; set; }
        /// <summary> Position of this <see cref="Point"/> on the z-axis </summary>
        public double Z { get; set; }
        /// <summary> A representation of a point in 3D space 
        /// <br/> Generated with <see cref="X"/>, <see cref="Y"/>, and <see cref="Z"/> positions </summary>
        /// <param name="x"> Position on the x-axis </param>
        /// <param name="y"> Position on the y-axis </param>
        /// <param name="z"> Position on the z-axis </param>
        public Point(double x, double y, double z) {
            X = x; Y = y; Z = z;
        }
    }
}
