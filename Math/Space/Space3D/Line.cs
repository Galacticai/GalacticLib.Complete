// —————————————————————————————————————————————
//? 
//!? 📜 Line.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space3D/Point.cs
//? 
// —————————————————————————————————————————————

using ScreenFIRE.Lib.Math.Space.Space3D;

// —————————————————————————————————————————————
//? 
//!? 📜 Line.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space3D/Point.cs
//? 
// —————————————————————————————————————————————

namespace ScreenFIRE.Lib.Math.Space.Space3D
{
    /// <summary> A 3D line defined by 2 <see cref="Point"/>s </summary>
    public class Line {
        /// <summary> Base axis as <see cref="Line"/>s </summary>
        public static class AXIS {
            /// <summary> X-axis as <see cref="Line"/> </summary>
            public static readonly Line X = new(Point.ORIGIN, new(1, 0, 0));
            /// <summary> Y-axis as <see cref="Line"/> </summary>
            public static readonly Line Y = new(Point.ORIGIN, new(0, 1, 0));
            /// <summary> Z-axis as <see cref="Line"/> </summary>
            public static readonly Line Z = new(Point.ORIGIN, new(0, 0, 1));
        }

        /// <summary> First <see cref="Point"/> of this <see cref="Line"/> </summary>
        public Point Point1 { get; set; }
        /// <summary> Last <see cref="Point"/> of this <see cref="Line"/> </summary>
        public Point Point2 { get; set; }
        /// <summary> A 3D line defined by <paramref name="point1"/> and <paramref name="point2"/> </summary>
        /// <param name="point1"> First <see cref="Point"/> </param>
        /// <param name="point2"> Last <see cref="Point"/> </param>
        public Line(Point point1, Point point2) {
            Point1 = point1;
            Point2 = point2;
        }
    }
}
