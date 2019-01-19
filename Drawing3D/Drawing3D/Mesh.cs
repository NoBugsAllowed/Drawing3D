﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Mesh
    {
        public string Name { get; set; }
        public Vertice[] Vertices { get; private set; }
        public Face[] Faces { get; set; }
        public Point3D Position { get; set; }
        public Point3D Rotation { get; set; }

        public Mesh(string name, int verticesCount, int facesCount)
        {
            Vertices = new Vertice[verticesCount];
            Faces = new Face[facesCount];
            Name = name;
            Position = new Point3D(0, 0, 0);
            Rotation = new Point3D(0, 0, 0);
        }
    }

    public struct Face
    {
        public int A;
        public int B;
        public int C;
        public Color Color;
    }
}
