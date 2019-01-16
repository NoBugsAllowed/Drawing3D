using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Cube : Model
    {
        private Random rand = new Random();

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
            trList.Add(new Triangle(a, b, d, RandomizeColor()));
            trList.Add(new Triangle(a, d, c, RandomizeColor()));

            trList.Add(new Triangle(a, b, f, RandomizeColor()));
            trList.Add(new Triangle(a, f, e, RandomizeColor()));

            trList.Add(new Triangle(b, d, f, RandomizeColor()));
            trList.Add(new Triangle(d, f, h, RandomizeColor()));

            trList.Add(new Triangle(d, c, h, RandomizeColor()));
            trList.Add(new Triangle(c, h, g, RandomizeColor()));

            trList.Add(new Triangle(c, a, e, RandomizeColor()));
            trList.Add(new Triangle(c, e, g, RandomizeColor()));

            trList.Add(new Triangle(f, h, e));
            trList.Add(new Triangle(h, e, g));

            Triangles = trList;

            // TO CHANGE
            ModelMatrix = new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        private Color RandomizeColor() => Color.FromArgb(rand.Next() % 256, rand.Next() % 256, rand.Next() % 256);

    }
}
