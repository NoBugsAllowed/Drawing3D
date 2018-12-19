using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Drawing3D
{
    public partial class MainForm : Form
    {
        public Model Figure { get; set; }
        public Matrix4x4 ProjectionMatrix { get; set; }
        public Matrix4x4 ViewMatrix { get; set; }
        public Matrix4x4 ModelMatrix { get; set; }

        private float e;
        private float n;
        private float f;
        private float a;

        public MainForm()
        {
            InitializeComponent();

            e = (float)(1 / Math.Tan(30 * 180 / Math.PI));
            n = 1;
            f = 100;
            a = (float)pictureBox.Height / pictureBox.Width;

            ModelMatrix = new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            ProjectionMatrix = new Matrix4x4(e, 0, 0, 0,
                0, e / a, 0, 0,
                0, 0, -(f + n) / (f - n), -2 * f * n / (f - n),
                0, 0, -1, 0);

            ViewMatrix = new Matrix4x4(0, 1, 0, -0.5f,
                0, 0, 1, -0.5f,
                1, 0, 0, -3,
                0, 0, 0, 1);

            Figure = new Cube(0.5f,1.0f,0.5f, 0.25f);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(new SolidBrush(Color.Black));

            foreach (Triangle tr in Figure.Triangles)
            {
                foreach (Line edge in tr.Edges)
                {
                    Point3D pe1 = VertexShader(edge.First);
                    Point3D pe2 = VertexShader(edge.Second);

                    int x1 = (int)(((pe1.X / pe1.W) + 1) * pictureBox.Width / 2);
                    int y1 = (int)(((pe1.Y / pe1.W) + 1) * pictureBox.Height / 2);
                    int x2 = (int)(((pe2.X / pe2.W) + 1) * pictureBox.Width / 2);
                    int y2 = (int)(((pe2.Y / pe2.W) + 1) * pictureBox.Height / 2);

                    e.Graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }
        private Point3D VertexShader(Point3D point) => ProjectionMatrix * (ViewMatrix * (ModelMatrix * point));
    }
}
