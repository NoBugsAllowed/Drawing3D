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
        public Matrix4x4 ModelMatrix;

        public Model() { Triangles = new List<Triangle>(); }
        public Model(List<Triangle> triangles)
        {
            Triangles = triangles;
            ModelMatrix = new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }
    }
}
