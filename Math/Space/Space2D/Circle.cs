// —————————————————————————————————————————————
//?
//!? 📜 Circle.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space2D/Point.cs
//?
// —————————————————————————————————————————————


using ScreenFIRE.Lib.Math.Numerics.Numbers;
using sMath = System.Math;

namespace ScreenFIRE.Lib.Math.Space.Space2D {
    /// <summary> A representation of a circle in 2D space </summary>
    internal class Circle<T>
            where T :
                struct,
                IComparable, IComparable<T>,
                IConvertible, IEquatable<T>,
                IFormattable {
        /// <summary> Unit <see cref="Circle{T}"/> of size 1 and <see cref="Point{T}.ORIGIN"/> as the center </summary>
        public static readonly Circle<T> UNIT_CIRCLE = new(Point<T>.ORIGIN, (T)(object)1);

        /// <summary> Determines whether the <paramref name="point"/> is included in this <see cref="Circle"/> </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> true if the <paramref name="point"/> exists within this <see cref="Circle"/> </returns>
        public bool IncludesPoint(Point<T> point) => Center.Distance(point) <= Radius.Double;
        /// <summary> Determines whether this <see cref="Circle"/> passes through the <paramref name="point"/> </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> true if this <see cref="Circle"/>'s edge passes through the <paramref name="point"/> </returns>
        public bool PassThroughPoint(Point<T> point) => Center.Distance(point) == Radius;

        /// <summary> Get the distance between the <see cref="Center"/> points of this <see cref="Circle"/> and another one </summary>
        /// <param name="circle"> Target <see cref="Circle"/> </param>
        /// <returns> Distance from the <see cref="Center"/> of this <see cref="Circle"/> to the <see cref="Center"/> of the other <paramref name="circle"/> </returns>
        public Number<double> Distance_Center(Circle<T> circle) => Center.Distance(circle.Center);

        /// <summary> Get the distance between the edge of this <see cref="Circle"/> and another one </summary>
        /// <param name="circle"> Target <see cref="Circle"/> </param>
        /// <returns>
        ///     <list type="bullet">
        ///     <item> &gt;0 • No intersection • The distance between the spheres </item>
        ///     <item> =0 • Touching edges </item>
        ///     <item> &lt;0 • Intersecting </item>
        ///     </list>
        /// </returns>
        public Number<double> Distance_Edge(Circle<T> circle) => Distance_Center(circle) - (Radius + circle.Radius).Double;

        /// <summary> Determine whether this <see cref="Circle"/> and another one are intersecting </summary>
        /// <param name="circle"> Target <see cref="Circle"/> </param>
        /// <returns> true if the distance between the edges of this <see cref="Circle"/> and <paramref name="circle"/> is &lt;=0</returns>
        public bool IntersectsSphere(Circle<T> circle) => Distance_Edge(circle) <= 0;

        //TODO: TEST Circle.howClose(Point)
        /// <returns> Ratio of how close <paramref name="point"/> is to <see cref="Center"/>
        /// relative to <see cref="Radius"/> </returns>
        public Number<double> HowClose(Point<T> point) {
            Number<double> distance = Center.Distance(point);
            //? close 0 -- 1 far
            Number<double> ratio_inverse = distance / Radius.Double;
            //? close 1 -- 0 far
            Number<double> ratio = NumberMath<double>.Abs(ratio_inverse - 1);
            return NumberMath<double>.Clamp(ratio, 0, 1);
        }

        /// <summary> Center <see cref="Point"/> of this <see cref="Circle"/> </summary>
        public Point<T> Center { get; set; }
        /// <summary> Radius of this <see cref="Circle"/> </summary>
        public Number<T> Radius { get; set; }
        /// <summary> Area of this <see cref="Circle"/> </summary>
        public Number<double> Area
            => sMath.PI * sMath.Pow(Radius.Double, 2);

        /// <summary> A representation of a circle in 2D space
        /// <br/> Generated with center poistion and radius data </summary>
        /// <param name="x"> Position of <see cref="Center"/> on the x-axis </param>
        /// <param name="y"> Position of <see cref="Center"/> on the y-axis </param>
        /// <param name="radius"> Radius of this <see cref="Circle"/> </param>
        public Circle(Number<T> x, Number<T> y, Number<T> radius) {
            Center = new Point<T>(x, y);
            Radius = radius;
        }
        /// <summary> A representation of a circle in 2D space
        /// <br/> Generated with center <see cref="Point"/> and radius data </summary>
        /// <param name="center"> Center <see cref="Point"/> of this <see cref="Circle"/> </param>
        /// <param name="radius"> Radius of this <see cref="Circle"/> </param>
        public Circle(Point<T> center, Number<T> radius) {
            Center = center;
            Radius = radius;
        }
    }
}
