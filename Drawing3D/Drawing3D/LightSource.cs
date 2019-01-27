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
        public bool IsOn { get; set; }
        public (float R, float G, float B) Color { get; set; }
        public abstract (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1);

        public LightSource() { Color = (1, 1, 1); IsOn = true; }
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
            IsOn = true;
            Color = (color.R / 255, color.G / 255, color.B / 255);
        }

        public override (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1)
        {
            Point3D li = -Direction;
            Point3D cameraToTargetVector = cameraPosition - target;
            Point3D v = cameraToTargetVector / cameraToTargetVector.DistanceFromOrigin();
            Point3D ri = normal * (2 * Point3D.DotProduct(normal, li)) - li;
            ri *= 1.0f / ri.DistanceFromOrigin();

            float r, g, b;

            float k = Math.Max(Point3D.DotProduct(normal, li), 0);
            float p = (float)Math.Pow(Math.Max(Point3D.DotProduct(v, ri), 0), m);

            r = Color.R * (kd.X * k + ks.X * p);
            g = Color.G * (kd.Y * k + ks.Y * p);
            b = Color.B * (kd.Z * k + ks.Z * p);

            return (r, g, b);
        }
    }

    public class PointLightSource : LightSource
    {
        public Point3D Position { get; set; }

        public PointLightSource(Point3D position, Color color)
        {
            Position = position;
            IsOn = true;
            this.Color = (color.R / 255, color.G / 255, color.B / 255);
        }
        public override (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1)
        {
            Point3D positionToTargetVectior = Position - target;
            Point3D cameraToTargetVector = cameraPosition - target;
            Point3D li = positionToTargetVectior / positionToTargetVectior.DistanceFromOrigin();
            Point3D v = cameraToTargetVector / cameraToTargetVector.DistanceFromOrigin();
            Point3D ri = normal * (2 * Point3D.DotProduct(normal, li)) - li;

            float r, g, b;

            float k = Math.Max(Point3D.DotProduct(normal, li), 0);
            float p = (float)Math.Pow(Math.Max(Point3D.DotProduct(v, ri), 0), m);

            r = Color.R * (kd.X * k + ks.X * p);
            g = Color.G * (kd.Y * k + ks.Y * p);
            b = Color.B * (kd.Z * k + ks.Z * p);

            return (r, g, b);
        }
    }

    public class SpotLightSource : LightSource
    {
        public Point3D Position;
        public Point3D Direction;
        //public float Focus;

        public SpotLightSource(Point3D position, Point3D direction, Color color)
        {
            Position = position;
            Direction = direction;
            IsOn = true;
            this.Color = (color.R / 255, color.G / 255, color.B / 255);
            //this.Focus = focus;
        }

        public override (float, float, float) PhongIlumination(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, int m = 1)
        {
            Point3D positionToTargetVectior = Position - target;
            Point3D cameraToTargetVector = cameraPosition - target;
            Point3D li = positionToTargetVectior / positionToTargetVectior.DistanceFromOrigin();
            Point3D v = cameraToTargetVector / cameraToTargetVector.DistanceFromOrigin();
            Point3D ri = normal * (2 * Point3D.DotProduct(normal, li)) - li;

            float r, g, b;

            var tmp = Point3D.DotProduct(-Direction, li);
            float q = Math.Max(Point3D.DotProduct(-Direction, li), 0);

            Point3D Ii = new Point3D(Color.R * q, Color.G * q, Color.B * q);
            float k = Math.Max(Point3D.DotProduct(normal, li), 0);
            float p = (float)Math.Pow(Math.Max(Point3D.DotProduct(v, ri), 0), m);

            r = Ii.X * (kd.X * k + ks.X * p);
            g = Ii.Y * (kd.Y * k + ks.Y * p);
            b = Ii.Z * (kd.Z * k + ks.Z * p);

            return (r, g, b);
        }
    }
}
