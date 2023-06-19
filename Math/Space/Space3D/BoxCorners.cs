// —————————————————————————————————————————————
//? 
//!? 📜 BoxPoints.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: 
//      + (Galacticai) Math/Space3D/Box.cs
//? 
// —————————————————————————————————————————————

using ScreenFIRE.Lib.Math.Space.Space3D;

// —————————————————————————————————————————————
//? 
//!? 📜 BoxPoints.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: 
//      + (Galacticai) Math/Space3D/Box.cs
//? 
// —————————————————————————————————————————————

namespace ScreenFIRE.Lib.Math.Space.Space3D
{
    //? Example: 
    //     OYZ------------OXYZ
    //    /|             /|
    //   / |            / |
    //  OY-+-----------OXY|
    //  |  |           |  |
    //  |  |           |  |
    //  |  |           |  |
    //  |  OZ----------+--OXZ
    //  | /            | /
    //  |/             |/
    //  O--------------OX

    /// <summary> Corner points of a <see cref="Box"/> </summary>
    public class BoxCorners {
        /// <summary> (x, y, z) </summary>
        public Point O { get; }
        /// <summary> (x+w, y, z) </summary>
        public Point OX { get; }
        /// <summary> (x, y+h, z) </summary>
        public Point OY { get; }
        /// <summary> (x, y, z+d) </summary>
        public Point OZ { get; }
        /// <summary> (x+w, y+h, z) </summary>
        public Point OXY { get; }
        /// <summary> (x+w, y, z+d) </summary>
        public Point OXZ { get; }
        /// <summary> (x, y+h, z+d) </summary>
        public Point OYZ { get; }
        /// <summary> (x+w, y+h, z+d) </summary>
        public Point OXYZ { get; }

        /// <summary> Corner points of <paramref name="box"/> </summary>
        /// <param name="box"> Target <see cref="Box"/> </param>
        public BoxCorners(Box box) {
            //? (x, y, z)
            O = new(box.X, box.Y, box.Z);
            //? (x+w, y, z)
            OX = new(box.X + box.XLength, box.Y, box.Z);
            //? (x, y+h, z)
            OY = new(box.X, box.Y + box.YLength, box.Z);
            //? (x, y, z+d)
            OZ = new(box.X, box.Y, box.Z + box.ZLength);
            //? (x+w, y+h, z)
            OXY = new(box.X + box.XLength, box.Y + box.YLength, box.Z);
            //? (x+w, y, z+d)
            OXZ = new(box.X + box.XLength, box.Y, box.Z + box.ZLength);
            //? (x, y+h, z+d)
            OYZ = new(box.X, box.Y + box.YLength, box.Z + box.ZLength);
            //? (x+w, y+h, z+d)
            OXYZ = new(box.X + box.XLength, box.Y + box.YLength, box.Z + box.ZLength);
        }
    }
}
