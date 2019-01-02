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
        private float e;
        private float n;
        private float f;
        private float a;

        public int Fov { get; set; }
        public Matrix4x4 ProjectionMatrix;
        public Matrix4x4 ViewMatrix;

        public Camera(int fov, float a)
        {
            Fov = fov;

            e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
            n = 1;
            f = 100;
            this.a = a;

            ProjectionMatrix = new Matrix4x4(e, 0, 0, 0,
                0, e / a, 0, 0,
                0, 0, -(f + n) / (f - n), -2 * f * n / (f - n),
                0, 0, -1, 0);

            ViewMatrix = new Matrix4x4(0, 1, 0, -0.5f,
                0, 0, 1, -0.5f,
                1, 0, 0, -3,
                0, 0, 0, 1);
        }

        public void ChangeFov(int fov)
        {
            Fov = fov;

            e = (float)(1 / Math.Tan(Fov * Math.PI / 360));

            ProjectionMatrix.M11 = e;
            ProjectionMatrix.M22 = e / a;
        }

        public void ChangeParameters(float a, float n = 1, float f = 100)
        {
            this.n = n;
            this.f = f;
            this.a = a;

            ProjectionMatrix.M22 = e / a;
            ProjectionMatrix.M33 = -(f + n) / (f - n);
            ProjectionMatrix.M34 = -2 * f * n / (f - n);
        }
    }
}
