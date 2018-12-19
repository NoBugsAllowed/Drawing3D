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
        public Model Figure { get; set; } = null;
        public Matrix4x4 ProjectionMatrix;
        public Matrix4x4 ViewMatrix;
        public Matrix4x4 ModelMatrix;
        public int Fov { get; set; }
        public float Angle { get; set; }
        private System.Timers.Timer rotateModelTimer;

        private float e;
        private float n;
        private float f;
        private float a;

        public MainForm()
        {
            InitializeComponent();

            Fov = (int)numericUpDown.Value;
            Angle = 0;

            e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
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

            Figure = new Cube(0.5f, 0.85f, 0.5f, 0.25f);

            rotateModelTimer = new System.Timers.Timer();
            rotateModelTimer.Interval = 10;
            rotateModelTimer.Elapsed += (s, e) =>
            {
                Angle += 0.02f;
                UpdateModelMatrix();
            };
            rotateModelTimer.Start();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(new SolidBrush(Color.Black));
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);

            foreach (Triangle tr in Figure.Triangles)
            {
                Point3D p1 = VertexShader(tr.Vertices[0]);
                Point3D p2 = VertexShader(tr.Vertices[1]);
                Point3D p3 = VertexShader(tr.Vertices[2]);
                p1.X = (int)(((p1.X / p1.W) + 1) * pictureBox.Width / 2);
                p1.Y = (int)(((p1.Y / p1.W) + 1) * pictureBox.Height / 2);

                p2.X = (int)(((p2.X / p2.W) + 1) * pictureBox.Width / 2);
                p2.Y = (int)(((p2.Y / p2.W) + 1) * pictureBox.Height / 2);

                p3.X = (int)(((p3.X / p3.W) + 1) * pictureBox.Width / 2);
                p3.Y = (int)(((p3.Y / p3.W) + 1) * pictureBox.Height / 2);

                FillTriangle(bmp, p1, p2, p3);

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
            pictureBox.Image = bmp;
        }
        private Point3D VertexShader(Point3D point)
        {
            return ProjectionMatrix * (ViewMatrix * (ModelMatrix * point));
        }

        public void UpdateProjectionMatrix()
        {
            e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
            a = (float)pictureBox.Height / pictureBox.Width;

            ProjectionMatrix.M11 = e;
            ProjectionMatrix.M22 = e / a;

            pictureBox.Refresh();
        }

        public void UpdateModelMatrix()
        {
            ModelMatrix.M11 = (float)Math.Cos(Angle);
            ModelMatrix.M12 = -(float)Math.Sin(Angle);
            ModelMatrix.M21 = (float)Math.Sin(Angle);
            ModelMatrix.M22 = (float)Math.Cos(Angle);

            pictureBox.Refresh();
        }

        private void FillTriangle(Bitmap bmp, Point3D p1, Point3D p2, Point3D p3)
        {
            Point3D down = p1.Y < p2.Y ? p1 : p2;
            Point3D mid, up;
            if (p3.Y < down.Y)
            {
                down = p3;
                if (p1.Y < p2.Y)
                {
                    mid = p1;
                    up = p2;
                }
                else
                {
                    mid = p2;
                    up = p1;
                }
            }
            else
            {
                if (p3.Y > p1.Y && p3.Y > p2.Y)
                {
                    up = p3;
                    if (p1.Y > p2.Y)
                        mid = p1;
                    else
                        mid = p2;
                }
                else
                {
                    mid = p3;
                    up = p1.Y > p2.Y ? p1 : p2;
                }
            }

            if (down.Y == mid.Y && down.Y == up.Y)
                return;

            double m1, m2;
            double x1, x2;

            if (down.Y == mid.Y)
            {
                if(down.X<mid.X)
                {
                    x1 = down.X;
                    x2 = mid.X;
                    m1 = (double)(up.X - down.X) / (up.Y - down.Y);
                    m2 = (double)(mid.X - up.X) / (mid.Y - up.Y);
                }
                else
                {
                    x2 = down.X;
                    x1 = mid.X;
                    m2 = (double)(up.X - down.X) / (up.Y - down.Y);
                    m1 = (double)(mid.X - up.X) / (mid.Y - up.Y);
                }
            }
            else
            {
                x1 = down.X;
                x2 = down.X;
                m1 = (double)(up.X - down.X) / (up.Y - down.Y);
                m2 = (double)(mid.X - down.X) / (mid.Y - down.Y);
            }
            if (m1 < m2)
            {
                double t = m1;
                m1 = m2;
                m2 = t;
            }

            for (int y = (int)down.Y + 1; y < up.Y; y++)
            {
                //Update X, sort edges
                x1 += m1;
                x2 += m2;

                if (y >= pictureBox.Height)
                    return;

                if (y >= 0)
                {
                    if (y == mid.Y)
                    {
                        if (x1 < mid.X)
                            m2 = (double)(up.X - mid.X) / (up.Y - mid.Y);
                        else
                            m1 = (double)(up.X - mid.X) / (up.Y - mid.Y);
                    }
                    for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, pictureBox.Width); j++)
                    {
                        bmp.SetPixel(j, y, Color.Red);
                    }
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (Figure != null)
            {
                UpdateProjectionMatrix();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Fov = (int)(sender as NumericUpDown).Value;
            this.e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
            UpdateProjectionMatrix();
        }
    }
    class Edge : IComparable
    {
        public int YMax { get; set; }
        public double X { get; set; }
        public double Ctg { get; set; }
        public Edge Next { get; set; }

        public static bool operator ==(Edge e1, Edge e2) => e1.YMax == e2.YMax && e1.Ctg == e2.Ctg;

        public static bool operator !=(Edge e1, Edge e2) => !(e1 == e2);

        public static bool operator >=(Edge e1, Edge e2) => e1.X >= e2.X;

        public static bool operator <=(Edge e1, Edge e2) => e1.X <= e2.X;

        public int CompareTo(object obj)
        {
            if (X < ((Edge)obj).X) return -1;
            else if (X == ((Edge)obj).X) return 0;
            return 1;
        }
        public override bool Equals(object obj) => obj is Edge && this == (obj as Edge);
        public override int GetHashCode() => base.GetHashCode();

    }
}
