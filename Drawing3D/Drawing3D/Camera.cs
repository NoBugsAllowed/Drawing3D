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
        private float n;
        private float f;
        private float a;

        public float Fov { get; set; }
        public Matrix4x4 ProjectionMatrix;
        public Matrix4x4 ViewMatrix;

        public Point3D Position { get; set; }
        public Point3D Target { get; set; }
        public Point3D Up { get; set; }

        public Camera(Point3D position, Point3D target, Point3D up, float fov, float a, float n = 0.01f, float f = 1.0f)
        {
            Fov = fov;
            Position = position;
            Target = target;
            Up = up;

            this.n = n;
            this.f = f;
            this.a = a;

            //ProjectionMatrix = new Matrix4x4(e, 0, 0, 0,
            //    0, e / a, 0, 0,
            //    0, 0, -(f + n) / (f - n), -2 * f * n / (f - n),
            //    0, 0, -1, 0);

            //ViewMatrix = new Matrix4x4(0, 1, 0, -0.5f,
            //    0, 0, 1, -0.5f,
            //    1, 0, 0, -3,
            //    0, 0, 0, 1);

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
            this.n = n;
            this.f = f;
            this.a = a;

            ProjectionMatrix.M22 = ProjectionMatrix.M11 / a;
            ProjectionMatrix.M33 = -(f + n) / (f - n);
            ProjectionMatrix.M34 = -2 * f * n / (f - n);
        }
    }
}
