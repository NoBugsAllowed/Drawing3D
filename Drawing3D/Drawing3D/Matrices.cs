using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    class Matrices
    {
        public static Matrix4x4 Projection(float fov, float a, float n, float f)
        {
            Matrix4x4 m = new Matrix4x4();
            m.M11 = (float)(1.0f / Math.Tan(fov * 0.5f));
            m.M22 = -(m.M11 / a);
            m.M33 = (-f - n) / (f - n);
            m.M34 = (-2 * f * n) / (f - n);
            m.M43 = -1;
            return m;
        }
        public static Matrix4x4 View(Camera c)
        {
            Matrix4x4 m = new Matrix4x4();
            Point3D tmp = (c.Position - c.Target);
            Point3D cZ = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(c.Up, cZ);
            Point3D cX = tmp / tmp.DistanceFromOrigin();
            tmp = Point3D.CrossProduct(cZ, cX);
            Point3D cY = tmp / tmp.DistanceFromOrigin();

            m.M11 = -cX.X;
            m.M12 = -cX.Y;
            m.M13 = -cX.Z;
            m.M14 = -Point3D.DotProduct(cX, c.Position);
            m.M21 = cY.X;
            m.M22 = cY.Y;
            m.M23 = cY.Z;
            m.M24 = -Point3D.DotProduct(cY, c.Position);
            m.M31 = cZ.X;
            m.M32 = cZ.Y;
            m.M33 = cZ.Z;
            m.M34 = -Point3D.DotProduct(cZ, c.Position);
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;
            return m;
        }
        public static Matrix4x4 Translation(Point3D t)
        {
            Matrix4x4 m = new Matrix4x4();
            m.M11 = 1;
            m.M12 = 0;
            m.M13 = 0;
            m.M14 = t.X;
            m.M21 = 0;
            m.M22 = 1;
            m.M23 = 0;
            m.M24 = t.Y;
            m.M31 = 0;
            m.M32 = 0;
            m.M33 = 1;
            m.M34 = t.Z;
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;
            return m;
        }
        public static Matrix4x4 RotationX(double alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.M11 = 1;
            m.M12 = 0;
            m.M13 = 0;
            m.M14 = 0;
            m.M21 = 0;
            m.M22 = (float)Math.Cos(alpha);
            m.M23 = (float)-Math.Sin(alpha);
            m.M24 = 0;
            m.M31 = 0;
            m.M32 = (float)Math.Sin(alpha);
            m.M33 = (float)Math.Cos(alpha);
            m.M34 = 0;
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;
            return m;
        }
        public static Matrix4x4 RotationY(double alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.M11 = (float)Math.Cos(-alpha);
            m.M12 = 0;
            m.M13 = (float)Math.Sin(-alpha);
            m.M14 = 0;
            m.M21 = 0;
            m.M22 = 1;
            m.M23 = 0;
            m.M24 = 0;
            m.M31 = (float)-Math.Sin(-alpha);
            m.M32 = 0;
            m.M33 = (float)Math.Cos(-alpha);
            m.M34 = 0;
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;
            return m;
        }
        public static Matrix4x4 RotationZ(double alpha)
        {
            Matrix4x4 m = new Matrix4x4();
            m.M11 = (float)Math.Cos(alpha);
            m.M12 = (float)-Math.Sin(alpha);
            m.M13 = 0;
            m.M14 = 0;
            m.M21 = (float)Math.Sin(alpha);
            m.M22 = (float)Math.Cos(alpha);
            m.M23 = 0;
            m.M24 = 0;
            m.M31 = 0;
            m.M32 = 0;
            m.M33 = 1;
            m.M34 = 0;
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.M44 = 1;
            return m;
        }
    }
}
