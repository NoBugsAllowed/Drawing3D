using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Vertice
    {
        public Point3D Position { get; set; }
        public Point3D NormalVector { get; set; }
        public Point3D ScenePosition { get; private set; }
        public Point3D N { get; private set; }
        public Point3D PBis { get; private set; }
        public Point3D ProjectedPosition { get; private set; }

        public Vertice() { }

        public Vertice(Point3D p, Point3D n)
        {
            Position = p;
            NormalVector = n;
        }

        public void CalculateCoordinates(Matrix4x4 model,Matrix4x4 view, Matrix4x4 projection, int w, int h)
        {
            ScenePosition = model * Position;
            N = model * NormalVector;
            PBis = projection * view * ScenePosition;
            ProjectedPosition = PBis / PBis.W;
            ProjectedPosition.X = (ProjectedPosition.X + 1) * (w - 1) / 2;
            ProjectedPosition.Y = (ProjectedPosition.Y + 1) * (h - 1) / 2;
        }
    }
}
