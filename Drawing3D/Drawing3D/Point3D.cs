using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Point3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Point3D(float x, float y, float z, float w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString() => $"({X},{Y},{Z},{W})";

        public static Point3D operator*(Matrix4x4 matrix,Point3D point)
        {
            float x = matrix.M11 * point.X + matrix.M12 * point.Y + matrix.M13 * point.Z + matrix.M14 * point.W;
            float y = matrix.M21 * point.X + matrix.M22 * point.Y + matrix.M23 * point.Z + matrix.M24 * point.W;
            float z = matrix.M31 * point.X + matrix.M32 * point.Y + matrix.M33 * point.Z + matrix.M34 * point.W;
            float w = matrix.M41 * point.X + matrix.M42 * point.Y + matrix.M43 * point.Z + matrix.M44 * point.W;

            return new Point3D(x,y,z,w);
        }
    }
}
