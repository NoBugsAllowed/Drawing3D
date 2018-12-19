using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Line
    {
        public Point3D First { get; set; }
        public Point3D Second { get; set; }

        public Line(Point3D first, Point3D second)
        {
            First = first;
            Second = second;
        }
    }
}
