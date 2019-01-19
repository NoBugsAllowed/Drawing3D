using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Triangle
    {
        public Vertice[] Vertices { get; set; }
        public Line[] Edges { get; set; }
        public Color Color { get; set; }

        public Triangle(Point3D a, Point3D b, Point3D c) : this(a, b, c, Color.White) { }
        public Triangle(Point3D a, Point3D b, Point3D c, Color col)
        {
            Vertices = new Vertice[3];
            Vertices[0] = new Vertice(a, new Point3D(0, 0, 0));
            Vertices[1] = new Vertice(b, new Point3D(0, 0, 0));
            Vertices[2] = new Vertice(c, new Point3D(0, 0, 0));

            Edges = new Line[3];
            Edges[0] = new Line(a, b);
            Edges[1] = new Line(b, c);
            Edges[2] = new Line(c, a);

            Color = col;
        }
    }
}
