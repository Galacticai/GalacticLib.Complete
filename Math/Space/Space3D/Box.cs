// —————————————————————————————————————————————
//?
//!? 📜 Box.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Common.cs
//?
// —————————————————————————————————————————————


using sMath = System.Math;

namespace ScreenFIRE.Lib.Math.Space.Space3D {
    //TODO: TEST Box.{XYZ}Length calculation
    /// <summary> A representation of a box in 3D space </summary>
    public class Box {
        #region Helper Methods
        /// <summary> Map a <see cref="Point"/> from this <see cref="Box"/> to another </summary>
        /// <param name="point"> Source <see cref="Point"/> </param>
        /// <param name="box"> Target <see cref="Box"/> </param>
        public Point MapPointIntoBox(Point point, Box box)
            => MapPointIntoBox(point, box, false);
        /// <summary> Map a <see cref="Point"/> from current <see cref="Box"/> to another </summary>
        /// <param name="point"> Source <see cref="Point"/> </param>
        /// <param name="box"> Target <see cref="Box"/> </param>
        /// <param name="forceInBox"> Force the output to be inside the target <paramref name="box"/> </param>
        public Point MapPointIntoBox(Point point, Box box, bool forceInBox) {
            Point newPoint = new(
                box.X + (box.XLength * (point.X - X) / XLength),
                box.Y + (box.YLength * (point.Y + Y) / YLength),
                box.Z + (box.ZLength * (point.Z + Z) / ZLength)
            );
            if (forceInBox) {
                newPoint.X = sMath.Clamp(newPoint.X, box.X, box.X + box.XLength);
                newPoint.Y = sMath.Clamp(newPoint.Y, box.Y, box.Y + box.YLength);
                newPoint.Z = sMath.Clamp(newPoint.Z, box.Z, box.Z + box.ZLength);
            }
            return newPoint;
        }

        #endregion

        /// <summary> Position on the x-axis </summary>
        public double X { get; set; }
        /// <summary> Position on the y-axis </summary>
        public double Y { get; set; }
        /// <summary> Position on the z-axis </summary>
        public double Z { get; set; }
        /// <summary> Length on the x-axis </summary>
        public virtual double XLength { get; set; }
        /// <summary> Length on the y-axis </summary>
        public virtual double YLength { get; set; }
        /// <summary> Length on the z-axis </summary>
        public virtual double ZLength { get; set; }

        /// <summary> Volume of this <see cref="Box"/> </summary>
        public virtual double Volume
            => XLength * YLength * ZLength;
        /// <summary> Corner points of this <see cref="Box"/> </summary>
        public BoxCorners Points
            => new(this);

        /// <summary> A representation of a box in 3D space
        /// <br/> Generated with position and length data </summary>
        /// <param name="x"> Position on the y-axis </param>
        /// <param name="y"> Position on the y-axis </param>
        /// <param name="z"> Position on the z-axis </param>
        /// <param name="xLength"> Length on the x-axis </param>
        /// <param name="yLength"> Length on the y-axis </param>
        /// <param name="zLength"> Length on the z-axis </param>
        public Box(double x, double y, double z, double xLength, double yLength, double zLength) {
            X = x; Y = y; Z = z;
            XLength = +xLength;
            YLength = +yLength;
            ZLength = +zLength;
        }
        /// <summary> A representation of a box in 3D space
        /// <br/> Generated with position and length data </summary>
        /// <param name="position"> Position </param>
        /// <param name="xLength"> Length on the x-axis </param>
        /// <param name="yLength"> Length on the y-axis </param>
        /// <param name="zLength"> Length on the z-axis </param>
        public Box(Point position, double xLength, double yLength, double zLength)
                    : this(position.X, position.Y, position.Z,
                        xLength, yLength, zLength) { }
        /// <summary> A representation of a box in 3D space
        /// <br/> Generated with <see cref="Point"/>s </summary>
        /// <param name="point1"> Top left <see cref="Point"/> </param>
        /// <param name="point2"> Bottom right <see cref="Point"/> </param>
        public Box(Point point1, Point point2)
                    : this(point1.X, point1.Y, point1.Z,
                        sMath.Max(point1.X, point2.X) - sMath.Min(point1.X, point2.X),
                        sMath.Max(point1.Y, point2.Y) - sMath.Min(point1.Y, point2.Y),
                        sMath.Max(point1.Z, point2.Z) - sMath.Min(point1.Z, point2.Z)) { }
    }
}
