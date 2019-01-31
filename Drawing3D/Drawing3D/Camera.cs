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
        private float a; //renderHeight/renderWidth

        public float Fov { get; set; } //Field of view
        public float N { get; set; } //Near
        public float F { get; set; } //Far

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
        }

        public void ChangeFov(float fov)
        {
            Fov = fov;
        }

        public void ChangeParameters(float a, float n = 0.01f, float f = 1.0f)
        {
            N = n;
            F = f;
            this.a = a;
        }
    }
}
