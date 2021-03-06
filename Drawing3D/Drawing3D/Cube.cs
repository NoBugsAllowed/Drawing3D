﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Cube : Model
    {
        public Cube(float x, float y, float z, float r)
        {
            Point3D a = new Point3D(x - r, y - r, z - r);
            Point3D b = new Point3D(x + r, y - r, z - r);
            Point3D c = new Point3D(x - r, y + r, z - r);
            Point3D d = new Point3D(x + r, y + r, z - r);

            Point3D e = new Point3D(x - r, y - r, z + r);
            Point3D f = new Point3D(x + r, y - r, z + r);
            Point3D g = new Point3D(x - r, y + r, z + r);
            Point3D h = new Point3D(x + r, y + r, z + r);

            List<Triangle> trList = new List<Triangle>();
            trList.Add(new Triangle(a, b, d));
            trList.Add(new Triangle(a, d, c));

            trList.Add(new Triangle(a, b, f));
            trList.Add(new Triangle(a, f, e));

            trList.Add(new Triangle(b, d, f));
            trList.Add(new Triangle(d, f, h));

            trList.Add(new Triangle(d, c, h));
            trList.Add(new Triangle(c, h, g));

            trList.Add(new Triangle(c, a, e));
            trList.Add(new Triangle(c, e, g));

            trList.Add(new Triangle(f, h, e));
            trList.Add(new Triangle(h, e, g));

            Triangles = trList;

        }
    }
}
