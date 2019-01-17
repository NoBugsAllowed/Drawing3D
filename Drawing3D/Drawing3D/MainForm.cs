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
        public int temp = 0;
        private System.Timers.Timer rotateModelTimer;

        public MainForm()
        {
            InitializeComponent();

            //Picture = new DirectBitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
            //pictureBox.Image = Picture.Bitmap;
            RenderDevice = new Device(pictureBox);

            Camera = new Camera((int)numericUpDown.Value, (float)pictureBox.Height / pictureBox.Width);

            //Figure = new Cube(0.5f, 0.85f, 0.5f, 0.25f);
            Mesh[] mesh = RenderDevice.LoadJSONFile("monkey.babylon", Color.Wheat);
            Figure = new Model(mesh[0]);

            Angle = 0;

            rotateModelTimer = new System.Timers.Timer();
            rotateModelTimer.Interval = 20;
            rotateModelTimer.Elapsed += (s, e) =>
            {
                Angle += 0.08f;
                UpdateModelMatrix();
                switch (temp)
                {
                    case 0:
                        {
                            Figure.Mesh.Rotation.X += 0.02f;
                            if (Figure.Mesh.Rotation.X >= 1.5f)
                                temp++;
                            break;
                        }
                    case 1:
                        {
                            Figure.Mesh.Rotation.Y += 0.02f;
                            if (Figure.Mesh.Rotation.Y >= 1.5f)
                                temp++;
                            break;
                        }
                    case 2:
                        {
                            Figure.Mesh.Rotation.Z += 0.02f;
                            if (Figure.Mesh.Rotation.Z >= 1.5f)
                            {
                                temp = 0;
                                Figure.Mesh.Rotation = new Point3D(0, 0, 0);
                            }
                            break;
                        }
                }

                RenderDevice.DisplaySceneOnBitmap(Camera, Figure);
            };
            //rotateModelTimer.Start();
            UpdateModelMatrix();
            Figure.Mesh.Rotation = new Point3D(1.5f, -1.5f, 0);

            pictureBox.Image = RenderDevice.Bitmap;

            RenderDevice.DisplaySceneOnBitmap(Camera, Figure);
            //RenderDevice.Clear(255, 255, 255, 255);
            //RenderDevice.Render(Camera, Figure);
            //RenderDevice.Present();
        }

        public void UpdateModelMatrix()
        {
            Figure.ModelMatrix.M11 = (float)Math.Cos(Angle);
            Figure.ModelMatrix.M12 = -(float)Math.Sin(Angle);
            Figure.ModelMatrix.M21 = (float)Math.Sin(Angle);
            Figure.ModelMatrix.M22 = (float)Math.Cos(Angle);
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
                // TODO
                //Camera.ChangeParameters((float)pictureBox.Height / pictureBox.Width);
                //RenderDevice.SetBitmapSize(pictureBox.Width, pictureBox.Height);
                //RenderDevice.DisplaySceneOnBitmap(Camera, Figure);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Camera.ChangeFov((int)(sender as NumericUpDown).Value);
            RenderDevice.DisplaySceneOnBitmap(Camera, Figure);
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
