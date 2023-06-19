// —————————————————————————————————————————————
//? 
//!? 📜 Cube.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: 
//      + (Galacticai) Math/Space3D/Box.cs
//? 
// —————————————————————————————————————————————


using ScreenFIRE.Lib.Math.Space.Space3D;

// —————————————————————————————————————————————
//? 
//!? 📜 Cube.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: 
//      + (Galacticai) Math/Space3D/Box.cs
//? 
// —————————————————————————————————————————————


namespace ScreenFIRE.Lib.Math.Space.Space3D
{
    /// <summary> A representation of a cube in 3D space </summary>
    public class Cube : Box {
        private new double XLength { get; set; }
        private new double YLength { get; set; }
        private new double ZLength { get; set; }
        /// <summary> Length on the x,y,z-axis </summary>
        public double Length {
            get => base.XLength;
            set {
                base.XLength = value;
                base.YLength = value;
                base.ZLength = value;
            }
        }

        /// <summary> Center <see cref="Point"/> of this <see cref="Cube"/> </summary>
        public Point Center
            => new(
                base.X + (Length / 2),
                base.Y + (Length / 2),
                base.Z + (Length / 2)
            );

        /// <summary> A representation of a cube in 3D space 
        /// <br/> Generated with position and length data </summary>
        /// <param name="x"> Position on the y-axis </param>
        /// <param name="y"> Position on the y-axis </param>
        /// <param name="z"> Position on the z-axis </param>
        /// <param name="length"> Length on the x,y,z-axis </param>
        public Cube(double x, double y, double z, double length)
                    : base(x, y, z, length, length, length) { }
        /// <summary> A representation of a cube in 3D space 
        /// <br/> Generated with position and length data </summary>
        /// <param name="position"> Position on the x,y,z-axis </param>
        /// <param name="length"> Length on the x,y,z-axis </param>
        public Cube(Point position, double length)
                    : base(position, length, length, length) { }
    }
}
