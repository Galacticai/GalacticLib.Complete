using c = Cairo;
using g = Gdk;

namespace ScreenFIRE.Lib.Math.Space.Space2D.Drawing {
    /// <summary> Shapes and stuff math </summary>
    public static class Geometry {
        /// <summary> Find the bounding rectangle of several rectangles </summary>
        /// <param name="rectangles">Rectangles to process</param>
        /// <returns><see cref="g.Rectangle"/> which contains all <paramref name="rectangles"/>[]</returns>
        public static g.Rectangle BoundingRectangle(this Rectangle[] rectangles) {
            int xMin = rectangles.Min(rect => rect.X);
            int yMin = rectangles.Min(rect => rect.Y);
            int xMax = rectangles.Max(rect => rect.X + rect.Width);
            int yMax = rectangles.Max(rect => rect.Y + rect.Height);
            return new g.Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
        }
        /// <summary> Find the bounding rectangle of several rectangles </summary>
        /// <param name="rectangles">Rectangles to process</param>
        /// <returns><see cref="c.Rectangle"/> which contains all <paramref name="rectangles"/>[]</returns>
        public static c.Rectangle BoundingRectangle(this c.Rectangle[] rectangles) {
            double xMin = rectangles.Min(rect => rect.X);
            double yMin = rectangles.Min(rect => rect.Y);
            double xMax = rectangles.Max(rect => rect.X + rect.Width);
            double yMax = rectangles.Max(rect => rect.Y + rect.Height);
            return new c.Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary> Calculates the distance between 2 points </summary>
        /// <returns> Distance = √[ (x₂ - x₁)² + (y₂ - y₁)² ] </returns>
        public static double Distance(c.PointD point1, c.PointD point2)
            => Math.Sqrt(
                Math.Pow(point2.X - point1.X, 2)
                + Math.Pow(point2.Y - point1.Y, 2)
            );
    }
}
