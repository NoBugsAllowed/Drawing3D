using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    class MatrixTransform
    {
        // TODO
        //public static Matrix4x44x4 Rotate(float x,float y,float z)
        //{
        //    Matrix4x44x4 Matrix4x4 = new Matrix4x44x4();

        //    return Matrix4x4;
        //}

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
            m.M11 = (float)Math.Cos(alpha);
            m.M12 = 0;
            m.M13 = (float)Math.Sin(alpha);
            m.M14 = 0;
            m.M21 = 0;
            m.M22 = 1;
            m.M23 = 0;
            m.M24 = 0;
            m.M31 = (float)-Math.Sin(alpha);
            m.M32 = 0;
            m.M33 = (float)Math.Cos(alpha);
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
