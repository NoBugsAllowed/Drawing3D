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
        //public DirectBitmap Picture { get; set; }
        public Device RenderDevice { get; set; }
        public Camera Camera { get; set; }
        public Model Figure { get; set; } = null;
        public float Angle { get; set; }
        private System.Timers.Timer rotateModelTimer;

        public MainForm()
        {
            InitializeComponent();

            //Picture = new DirectBitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
            //pictureBox.Image = Picture.Bitmap;
            RenderDevice = new Device(new Bitmap(pictureBox.Width, pictureBox.Height));

            Camera = new Camera((int)numericUpDown.Value, (float)pictureBox.Height / pictureBox.Width);

            Figure = new Cube(0.5f, 0.85f, 0.5f, 0.25f);

            rotateModelTimer = new System.Timers.Timer();
            rotateModelTimer.Interval = 10;
            rotateModelTimer.Elapsed += (s, e) =>
            {
                Angle += 0.02f;
                UpdateModelMatrix();
            };
            rotateModelTimer.Start();
            Angle += 5.5f;
            UpdateModelMatrix();
        }

        private void UpdateBitmap()
        {
            //Picture.Clear();

            //Parallel.ForEach(Figure.Triangles, tr =>
            //{
            //    Point3D p1 = VertexShader(tr.Vertices[0]);
            //    Point3D p2 = VertexShader(tr.Vertices[1]);
            //    Point3D p3 = VertexShader(tr.Vertices[2]);
            //    p1.X = (int)(((p1.X / p1.W) + 1) * pictureBox.Width / 2);
            //    p1.Y = (int)(((p1.Y / p1.W) + 1) * pictureBox.Height / 2);

            //    p2.X = (int)(((p2.X / p2.W) + 1) * pictureBox.Width / 2);
            //    p2.Y = (int)(((p2.Y / p2.W) + 1) * pictureBox.Height / 2);

            //    p3.X = (int)(((p3.X / p3.W) + 1) * pictureBox.Width / 2);
            //    p3.Y = (int)(((p3.Y / p3.W) + 1) * pictureBox.Height / 2);

            //    FillTriangle(Picture, p1, p2, p3);

            //    foreach (Line edge in tr.Edges)
            //    {
            //        Point3D pe1 = VertexShader(edge.First);
            //        Point3D pe2 = VertexShader(edge.Second);

            //        int x1 = (int)(((pe1.X / pe1.W) + 1) * pictureBox.Width / 2);
            //        int y1 = (int)(((pe1.Y / pe1.W) + 1) * pictureBox.Height / 2);
            //        int x2 = (int)(((pe2.X / pe2.W) + 1) * pictureBox.Width / 2);
            //        int y2 = (int)(((pe2.Y / pe2.W) + 1) * pictureBox.Height / 2);

            //        DrawLine(Color.Black, new Point(x1, y1), new Point(x2, y2));
            //    }
            //});

            //foreach (Triangle tr in Figure.Triangles)
            //{
            //    Point3D p1 = VertexShader(tr.Vertices[0]);
            //    Point3D p2 = VertexShader(tr.Vertices[1]);
            //    Point3D p3 = VertexShader(tr.Vertices[2]);
            //    p1.X = (int)(((p1.X / p1.W) + 1) * pictureBox.Width / 2);
            //    p1.Y = (int)(((p1.Y / p1.W) + 1) * pictureBox.Height / 2);

            //    p2.X = (int)(((p2.X / p2.W) + 1) * pictureBox.Width / 2);
            //    p2.Y = (int)(((p2.Y / p2.W) + 1) * pictureBox.Height / 2);

            //    p3.X = (int)(((p3.X / p3.W) + 1) * pictureBox.Width / 2);
            //    p3.Y = (int)(((p3.Y / p3.W) + 1) * pictureBox.Height / 2);

            //    //FillTriangle(Picture, p1, p2, p3);

            //    foreach (Line edge in tr.Edges)
            //    {
            //        Point3D pe1 = VertexShader(edge.First);
            //        Point3D pe2 = VertexShader(edge.Second);

            //        int x1 = (int)(((pe1.X / pe1.W) + 1) * pictureBox.Width / 2);
            //        int y1 = (int)(((pe1.Y / pe1.W) + 1) * pictureBox.Height / 2);
            //        int x2 = (int)(((pe2.X / pe2.W) + 1) * pictureBox.Width / 2);
            //        int y2 = (int)(((pe2.Y / pe2.W) + 1) * pictureBox.Height / 2);

            //        DrawLine(Color.Black, new Point(x1, y1), new Point(x2, y2));
            //    }
            //}
            pictureBox.Refresh();
        }

        //private void DrawLine(Color color, Point p1, Point p2)
        //{
        //    int x = p1.X, y = p1.Y, w = p2.X - p1.X, h = p2.Y - p1.Y;
        //    int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        //    if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        //    if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        //    if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        //    int longest = Math.Abs(w);
        //    int shortest = Math.Abs(h);
        //    if (!(longest > shortest))
        //    {
        //        longest = Math.Abs(h);
        //        shortest = Math.Abs(w);
        //        if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
        //        dx2 = 0;
        //    }
        //    int numerator = longest >> 1;
        //    for (int i = 0; i <= longest; i++)
        //    {
        //        if (x >= 0 && x < Picture.Width && y >= 0 && y < Picture.Height)
        //            Picture.SetPixel(x, y, color);
        //        numerator += shortest;
        //        if (!(numerator < longest))
        //        {
        //            numerator -= longest;
        //            x += dx1;
        //            y += dy1;
        //        }
        //        else
        //        {
        //            x += dx2;
        //            y += dy2;
        //        }
        //    }
        //}

        //private void pictureBox_Paint(object sender, PaintEventArgs e)
        //{
        //    Pen pen = new Pen(new SolidBrush(Color.Black));
        //    Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);

        //    foreach (Triangle tr in Figure.Triangles)
        //    {
        //        Point3D p1 = VertexShader(tr.Vertices[0]);
        //        Point3D p2 = VertexShader(tr.Vertices[1]);
        //        Point3D p3 = VertexShader(tr.Vertices[2]);
        //        p1.X = (int)(((p1.X / p1.W) + 1) * pictureBox.Width / 2);
        //        p1.Y = (int)(((p1.Y / p1.W) + 1) * pictureBox.Height / 2);

        //        p2.X = (int)(((p2.X / p2.W) + 1) * pictureBox.Width / 2);
        //        p2.Y = (int)(((p2.Y / p2.W) + 1) * pictureBox.Height / 2);

        //        p3.X = (int)(((p3.X / p3.W) + 1) * pictureBox.Width / 2);
        //        p3.Y = (int)(((p3.Y / p3.W) + 1) * pictureBox.Height / 2);

        //        FillTriangle(bmp, p1, p2, p3);

        //        foreach (Line edge in tr.Edges)
        //        {
        //            Point3D pe1 = VertexShader(edge.First);
        //            Point3D pe2 = VertexShader(edge.Second);

        //            int x1 = (int)(((pe1.X / pe1.W) + 1) * pictureBox.Width / 2);
        //            int y1 = (int)(((pe1.Y / pe1.W) + 1) * pictureBox.Height / 2);
        //            int x2 = (int)(((pe2.X / pe2.W) + 1) * pictureBox.Width / 2);
        //            int y2 = (int)(((pe2.Y / pe2.W) + 1) * pictureBox.Height / 2);

        //            e.Graphics.DrawLine(pen, x1, y1, x2, y2);
        //        }
        //    }
        //    pictureBox.Image = bmp;
        //}

        //public void UpdateProjectionMatrix()
        //{
        //    e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
        //    a = (float)pictureBox.Height / pictureBox.Width;

        //    ProjectionMatrix.M11 = e;
        //    ProjectionMatrix.M22 = e / a;

        //    UpdateBitmap();
        //    //pictureBox.Refresh();
        //}

        public void UpdateModelMatrix()
        {
            Figure.ModelMatrix.M11 = (float)Math.Cos(Angle);
            Figure.ModelMatrix.M12 = -(float)Math.Sin(Angle);
            Figure.ModelMatrix.M21 = (float)Math.Sin(Angle);
            Figure.ModelMatrix.M22 = (float)Math.Cos(Angle);

            RenderDevice.Render(Camera, Figure);
            pictureBox.Image = RenderDevice.bitmap.Bitmap;
            //UpdateBitmap();
            //pictureBox.Refresh();
        }

        //private void FillTriangle(DirectBitmap bmp, Point3D p1, Point3D p2, Point3D p3)
        //{
        //    Point3D down = p1, mid = p2, up = p3, tmp;
        //    if(down.Y>up.Y)
        //    {
        //        tmp = down;
        //        down = up;
        //        up = tmp;
        //    }
        //    if(down.Y>mid.Y)
        //    {
        //        tmp = down;
        //        down = mid;
        //        mid = tmp;
        //    }
        //    if(mid.Y>up.Y)
        //    {
        //        tmp = mid;
        //        mid = up;
        //        up = tmp;
        //    }


        //    if (down.Y == mid.Y && down.Y == up.Y)
        //        return;

        //    double m1, m2;
        //    double x1, x2;

        //    // Flat bottom
        //    if (down.Y == mid.Y)
        //    {
        //        if(down.X<mid.X)
        //        {
        //            x1 = down.X;
        //            x2 = mid.X;
        //            m1 = (double)(up.X - down.X) / (up.Y - down.Y);
        //            m2 = (double)(mid.X - up.X) / (mid.Y - up.Y);
        //        }
        //        else
        //        {
        //            x2 = down.X;
        //            x1 = mid.X;
        //            m2 = (double)(up.X - down.X) / (up.Y - down.Y);
        //            m1 = (double)(mid.X - up.X) / (mid.Y - up.Y);
        //        }
        //    }
        //    // Flat top
        //    else if(mid.Y==up.Y)
        //    {
        //        if (mid.X > up.X)
        //        {
        //            tmp = mid;
        //            mid = up;
        //            up = tmp;
        //        }

        //        x1 = down.X;
        //        x2 = down.X;
        //        m2 = (double)(up.X - down.X) / (up.Y - down.Y);
        //        m1 = (double)(mid.X - down.X) / (mid.Y - down.Y);

        //        if (m1 > m2)
        //        {
        //            double t = m1;
        //            m1 = m2;
        //            m2 = t;
        //        }
        //    }
        //    else
        //    {
        //        x1 = down.X;
        //        x2 = down.X;
        //        m1 = (double)(up.X - down.X) / (up.Y - down.Y);
        //        m2 = (double)(mid.X - down.X) / (mid.Y - down.Y);

        //        if (m1 > m2)
        //        {
        //            double t = m1;
        //            m1 = m2;
        //            m2 = t;
        //        }
        //    }

        //    for (int y = (int)down.Y + 1; y < up.Y; y++)
        //    {
        //        //Update X, sort edges
        //        x1 += m1;
        //        x2 += m2;

        //        if (y >= pictureBox.Height)
        //            return;

        //        if (y >= 0)
        //        {
        //            if (y == mid.Y)
        //            {
        //                if (Math.Abs(mid.X-x1)<Math.Abs(mid.X-x2))
        //                    m1 = (double)(up.X - mid.X) / (up.Y - mid.Y);
        //                else
        //                    m2 = (double)(up.X - mid.X) / (up.Y - mid.Y);
        //            }
        //            for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, pictureBox.Width); j++)
        //            {
        //                bmp.SetPixel(j, y, Color.Red);
        //            }
        //        }
        //    }
        //}

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (Figure != null)
            {
                //UpdateProjectionMatrix();
                Camera.ChangeParameters((float)pictureBox.Height / pictureBox.Width);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Camera.ChangeFov((int)(sender as NumericUpDown).Value);
            //Fov = (int)(sender as NumericUpDown).Value;
            //this.e = (float)(1 / Math.Tan(Fov * Math.PI / 360));
            //UpdateProjectionMatrix();
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
