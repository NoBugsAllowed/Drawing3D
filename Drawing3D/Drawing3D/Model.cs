using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Model
    {
        public List<Triangle> Triangles { get; set; }

        public Model() { Triangles = new List<Triangle>(); }
        public Model(List<Triangle> triangles)
        {
            Triangles = triangles;
        }
    }
}
