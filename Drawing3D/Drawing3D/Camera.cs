using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Camera
    {
        private float a;

        public float Fov { get; set; }
        public float N { get; set; } //near
        public float F { get; set; } //far
        public Matrix4x4 ProjectionMatrix;
        public Matrix4x4 ViewMatrix;

        public Point3D Position { get; private set; }
        public Point3D Target { get; private set; }
        public Point3D Up { get; private set; }

        public Camera(Point3D position, Point3D target, Point3D up, float fov, float a, float n = 0.01f, float f = 1.0f)
        {
            Fov = fov;
            Position = position;
            Target = target;
            Up = up;

            N = n;
            F = f;
            this.a = a;

            Matrix4x4 m = new Matrix4x4();
            Point3D tmp = (Position - Target);
            Point3D cZ = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(Up, cZ);
            Point3D cX = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(cZ, cX);
            Point3D cY = tmp / tmp.DistanceFromOrigin();

            m.M11 = -cX.X;
            m.M12 = -cX.Y;
            m.M13 = -cX.Z;
            m.M14 = -Point3D.DotProduct(cX, Position);
            m.M21 = cY.X;
            m.M22 = cY.Y;
            m.M23 = cY.Z;
            m.M24 = -Point3D.DotProduct(cY, Position);
            m.M31 = cZ.X;
            m.M32 = cZ.Y;
            m.M33 = cZ.Z;
            m.M34 = -Point3D.DotProduct(cZ, Position);
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;

            ViewMatrix = m;

            m = new Matrix4x4();
            m.M11 = (float)(1.0f / Math.Tan(Fov * 0.5f));
            m.M22 = -(m.M11 / a);
            m.M33 = (-f - n) / (f - n);
            m.M34 = (-2 * f * n) / (f - n);
            m.M43 = -1;

            ProjectionMatrix = m;
        }

        public void ChangeFov(float fov)
        {
            Fov = fov;

            ProjectionMatrix.M11 = (float)(1.0f / Math.Tan(Fov * 0.5f));
            ProjectionMatrix.M22 = -ProjectionMatrix.M11 / a;
        }

        public void ChangeParameters(float a, float n = 0.01f, float f = 1.0f)
        {
            N = n;
            F = f;
            this.a = a;

            ProjectionMatrix.M22 = ProjectionMatrix.M11 / a;
            ProjectionMatrix.M33 = -(f + n) / (f - n);
            ProjectionMatrix.M34 = -2 * f * n / (f - n);
        }

        public void SetPosition(Point3D position, Point3D target)
        {
            Position = position;
            Target = target;

            Point3D tmp = (Position - Target);
            Point3D cZ = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(Up, cZ);
            Point3D cX = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(cZ, cX);
            Point3D cY = tmp / tmp.DistanceFromOrigin();

            ViewMatrix.M11 = -cX.X;
            ViewMatrix.M12 = -cX.Y;
            ViewMatrix.M13 = -cX.Z;
            ViewMatrix.M14 = -Point3D.DotProduct(cX, Position);
            ViewMatrix.M21 = cY.X;
            ViewMatrix.M22 = cY.Y;
            ViewMatrix.M23 = cY.Z;
            ViewMatrix.M24 = -Point3D.DotProduct(cY, Position);
            ViewMatrix.M31 = cZ.X;
            ViewMatrix.M32 = cZ.Y;
            ViewMatrix.M33 = cZ.Z;
            ViewMatrix.M34 = -Point3D.DotProduct(cZ, Position);
            ViewMatrix.M41 = 0;
            ViewMatrix.M42 = 0;
            ViewMatrix.M43 = 0;
            ViewMatrix.M44 = 1;
        }
    }
}
