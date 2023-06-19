// —————————————————————————————————————————————
//?
//!? 📜 RectanglePoints.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space2D/Point.cs
//      + (Galacticai) Math/Space2D/Rectangle.cs
//?
// —————————————————————————————————————————————



// —————————————————————————————————————————————
//?
//!? 📜 RectanglePoints.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space2D/Point.cs
//      + (Galacticai) Math/Space2D/Rectangle.cs
//?
// —————————————————————————————————————————————

namespace ScreenFIRE.Lib.Math.Space.Space2D {
    //? Example:
    //  OY--------------OXY
    //  |               |
    //  |               |
    //  |               |
    //  |               |
    //  |               |
    //  |               |
    //  O---------------OX

    /// <summary> Corner points of a <see cref="Rectangle"/> </summary>
    public class RectangleCorners<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                IFormattable {
        /// <summary> (x, y) </summary>
        public Point<T> O { get; }
        /// <summary> (x+w, y) </summary>
        public Point<T> OX { get; }
        /// <summary> (x, y+h) </summary>
        public Point<T> OY { get; }
        /// <summary> (x+w, y+h) </summary>
        public Point<T> OXY { get; }
        /// <summary> Corner points of <paramref name="rectangle"/> </summary>
        /// <param name="rectangle"> Target <see cref="Rectangle"/> </param>
        public RectangleCorners(Rectangle<T> rectangle) {
            //? (x, y)
            O = new(rectangle.X, rectangle.Y);
            //? (x+w, y)
            OX = new(rectangle.X + rectangle.XLength, rectangle.Y);
            //? (x, y+h)
            OY = new(rectangle.X, rectangle.Y + rectangle.YLength);
            //? (x+w, y+h)
            OXY = new(rectangle.X + rectangle.XLength, rectangle.Y + rectangle.YLength);
        }
    }
}
