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
        public Point3D Position { get; set; } //Position in local coordinate system
        public Point3D NormalVector { get; set; } //Normal vector in local coordinate system
        public Point3D ScenePosition { get; private set; } //Position in global coordinate system
        public Point3D N { get; private set; } //Normal vector in global coordinate system
        public Point3D PBis { get; private set; } //Position multiplicated by view and projection matrix (not normalized)
        public Point3D ProjectedPosition { get; private set; } //PBis normalized

        public Vertice() { }

        public Vertice(Point3D p, Point3D n)
        {
            Position = p;
            NormalVector = n;
            ScenePosition = new Point3D(0,0,0);
            N = new Point3D(0, 0, 0);
            PBis = new Point3D(0, 0, 0);
            ProjectedPosition = new Point3D(0, 0, 0);
        }

        public void CalculateCoordinates(Matrix4x4 model, Matrix4x4 rotation, Matrix4x4 view, Matrix4x4 projection, int w, int h)
        {
            ScenePosition = model * Position;
            N = rotation * NormalVector;
            N = N / N.DistanceFromOrigin();
            PBis = projection * view * ScenePosition;
            ProjectedPosition = PBis / PBis.W;
            ProjectedPosition.X = (ProjectedPosition.X + 1) * (w - 1) / 2;
            ProjectedPosition.Y = (ProjectedPosition.Y + 1) * (h - 1) / 2;
        }
        public override string ToString() => $"(x:{Position.X},y:{Position.Y},z:{Position.Z})";
        public Vertice Clone() => new Vertice(Position.Clone(), NormalVector.Clone());
    }
}
