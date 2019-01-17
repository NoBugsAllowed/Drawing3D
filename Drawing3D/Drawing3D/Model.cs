using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Model
    {
        public List<Triangle> Triangles { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public Matrix4x4 ModelMatrix;
        public Mesh Mesh { get; set; }

        public Model() { Triangles = new List<Triangle>(); }
        public Model(Mesh mesh)
        {
            Mesh = mesh;
            ModelMatrix = new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

    }
}
