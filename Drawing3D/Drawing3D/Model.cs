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
        public Mesh Mesh { get; set; }
        public Point3D Position
        {
            get => Mesh.Position;
            set { Mesh.Position = value; }
        }
        public Point3D Rotation
        {
            get => Mesh.Rotation;
            set { Mesh.Rotation = value; }
        }

        public Model(Mesh mesh)
        {
            Mesh = mesh;
        }

    }
}
