// —————————————————————————————————————————————
//?
//!? 📜 Sphere.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Math/Space3D/Point.cs
//?
// —————————————————————————————————————————————

using sMath = System.Math;

namespace ScreenFIRE.Lib.Math.Space.Space3D {
    /// <summary> A representation of a sphere in 3D space </summary>
    public class Sphere {
        /// <summary> Unit <see cref="Sphere"/> of size 1 and <see cref="Point.ORIGIN"/> as the center </summary>
        public static readonly Sphere UNIT_SPHERE = new(Point.ORIGIN, 1);

        /// <summary> Determines whether the <paramref name="point"/> is included in this <see cref="Sphere"/> </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> true if the <paramref name="point"/> exists within this <see cref="Sphere"/> </returns>
        public bool IncludesPoint(Point point)
            => Center.Distance(point) <= Radius;
        /// <summary> Determines whether this <see cref="Sphere"/> passes through the <paramref name="point"/> </summary>
        /// <param name="point"> Target <see cref="Point"/> </param>
        /// <returns> true if this <see cref="Sphere"/>'s edge passes through the <paramref name="point"/> </returns>
        public bool PassThroughPoint(Point point)
            => Center.Distance(point) == Radius;

        /// <summary> Get the distance between the <see cref="Center"/> points of this <see cref="Sphere"/> and another one </summary>
        /// <param name="sphere"> Target <see cref="Sphere"/> </param>
        /// <returns> Distance from the <see cref="Center"/> of this <see cref="Sphere"/> to the <see cref="Center"/> of the other <paramref name="sphere"/> </returns>
        public double Distance_Center(Sphere sphere)
            => Center.Distance(sphere.Center);

        /// <summary> Get the distance between the edge of this <see cref="Sphere"/> and another one </summary>
        /// <param name="sphere"> Target <see cref="Sphere"/> </param>
        /// <returns><list type="bullet">
        ///     <item> &gt;0 • No intersection • The distance between the spheres </item>
        ///     <item> =0 • Touching edges </item>
        ///     <item> &lt;0 • Intersecting </item>
        /// </list></returns>
        public double Distance_Edge(Sphere sphere)
            => Distance_Center(sphere) - (Radius + sphere.Radius);

        /// <summary> Determine whether this <see cref="Sphere"/> and another one are intersecting </summary>
        /// <param name="sphere"> Target <see cref="Sphere"/> </param>
        /// <returns> true if the distance between the edges of this <see cref="Sphere"/> and <paramref name="sphere"/> is &lt;=0</returns>
        public bool IntersectsSphere(Sphere sphere)
            => Distance_Edge(sphere) <= 0;

        //TODO: TEST Sphere.howClose(Point)
        /// <returns> Ratio of how close <paramref name="point"/> is to <see cref="Center"/>
        /// relative to <see cref="Radius"/> </returns>
        public double HowClose(Point point) {
            double distance = Center.Distance(point);
            //? close 0 -- 1 far
            double ratio_inverse = distance / Radius;
            //? close 1 -- 0 far
            double ratio = sMath.Abs(ratio_inverse - 1);
            return sMath.Clamp(ratio, 0, 1);
        }

        /// <summary> Center <see cref="Point"/> of this <see cref="Sphere"/> </summary>
        public Point Center { get; set; }
        /// <summary> Radius of this <see cref="Sphere"/> </summary>
        public double Radius { get; set; }
        /// <summary> Volume of this <see cref="Sphere"/> </summary>
        public double Volume
            => 4 / 3 * sMath.PI * sMath.Pow(Radius, 3);

        /// <summary> A representation of a sphere in 3D space
        /// <br/> Generated with center poistion and radius data </summary>
        /// <param name="x"> Position of <see cref="Center"/> on the x-axis </param>
        /// <param name="y"> Position of <see cref="Center"/> on the y-axis </param>
        /// <param name="z"> Position of <see cref="Center"/> on the z-axis </param>
        /// <param name="radius"> Radius of this <see cref="Sphere"/> </param>
        public Sphere(double x, double y, double z, double radius) {
            Center = new Point(x, y, z);
            Radius = radius;
        }
        /// <summary> A representation of a sphere in 3D space
        /// <br/> Generated with center <see cref="Point"/> and radius data </summary>
        /// <param name="center"> Center <see cref="Point"/> of this <see cref="Sphere"/> </param>
        /// <param name="radius"> Radius of this <see cref="Sphere"/> </param>
        public Sphere(Point center, double radius) {
            Center = center;
            Radius = radius;
        }
    }
}
