using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Triangle
    {
        public Point3D[] Vertices { get; set; }
        public Line[] Edges { get; set; }

        public Triangle(Point3D a, Point3D b, Point3D c)
        {
            Vertices = new Point3D[3];
            Vertices[0] = a;
            Vertices[1] = b;
            Vertices[2] = c;

            Edges = new Line[3];
            Edges[0] = new Line(a, b);
            Edges[1] = new Line(b, c);
            Edges[2] = new Line(c, a);
        }
    }
}
