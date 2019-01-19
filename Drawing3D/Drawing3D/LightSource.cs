using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public abstract class LightSource
    {
        public (float R, float G, float B) Color { get; set; }
        public abstract (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1);

        public LightSource() { Color = (255, 255, 255); }
    }

    public class DirectionalLightSource : LightSource
    {
        public Point3D Direction { get; set; }

        public DirectionalLightSource(Point3D direction)
        {
            Direction = direction;
        }
        public DirectionalLightSource(Point3D direction, Color color)
        {
            Direction = direction;
            Color = (color.R, color.G, color.B);
        }

        public override (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1)
        {
            Point3D li = -Direction;
            Point3D cameraToTargetVector = cameraPosition - target;
            Point3D v = cameraToTargetVector / cameraToTargetVector.DistanceFromOrigin();
            Point3D ri = normal * (2 * Point3D.DotProduct(normal, li)) - li;
            ri *= 1.0f / ri.DistanceFromOrigin();

            float r, g, b;

            var tmp1 = Point3D.DotProduct(normal, li);
            var tmp2 = Math.Max(Point3D.DotProduct(v, ri), 0);

            float k = Math.Max(Point3D.DotProduct(normal, li), 0);
            float p = (float)Math.Pow(Math.Max(Point3D.DotProduct(v, ri), 0), m);

            r = Color.R * (kd.X * k + ks.X * p);
            g = Color.G * (kd.Y * k + ks.Y * p);
            b = Color.B * (kd.Z * k + ks.Z * p);

            return (r, g, b);
        }
    }
}
