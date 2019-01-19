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
using System.IO;

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

            // Układ lewoskrętny
            Camera.Position = new Point3D(0, 0, -5f);
            Camera.Target = new Point3D(0, 0, 0);
            Camera.Up = new Point3D(0, 1, 0);

            Mesh[] mesh = LoadJSONFile("monkey.babylon", Color.Wheat);
            Figure = new Model(mesh[0]);
            Figure.Mesh.Position = new Point3D(0, 0, 0);

            Angle = 0;

            rotateModelTimer = new System.Timers.Timer();
            rotateModelTimer.Interval = 20;
            rotateModelTimer.Elapsed += (s, e) =>
            {
                Angle += 0.08f;
                UpdateModelMatrix();
                //Figure.Mesh.Position.X += 0.02f;
                //Figure.Mesh.Position.Y += 0.02f;

                //Figure.Mesh.Rotation.Y += 0.02f;
                switch (temp)
                {
                    //case 0:
                    //    {
                    //        Figure.Mesh.Rotation.X += 0.02f;
                    //        if (Figure.Mesh.Rotation.X >= 1.5f)
                    //            temp++;
                    //        break;
                    //    }
                    //case 1:
                    //    {
                    //        Figure.Mesh.Rotation.Y += 0.02f;
                    //        if (Figure.Mesh.Rotation.Y >= 1.5f)
                    //            temp=0;
                    //        break;
                    //    }
                    case 0:
                        {
                            Camera.Position.X += 0.1f;
                            if (Camera.Position.X >= 5f)
                                temp++;
                            break;
                        }
                    case 1:
                        {
                            Camera.Position.X -= 0.1f;
                            if (Camera.Position.X <= -5f)
                                temp = 0;
                            break;
                        }
                }

                RenderDevice.DisplaySceneOnBitmap(Camera, Figure);
            };
            rotateModelTimer.Start();
            UpdateModelMatrix();
            //Figure.Mesh.Rotation = new Point3D(1.5f, -1.5f, 0);

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

        private Mesh[] LoadJSONFile(string path, Color color)
        {
            var meshes = new List<Mesh>();
            Random rand = new Random();
            var data = File.ReadAllText(path, Encoding.UTF8);
            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(data);

            for (var meshIndex = 0; meshIndex < jsonObject.meshes.Count; meshIndex++)
            {
                var verticesArray = jsonObject.meshes[meshIndex].vertices;
                // Faces
                var indicesArray = jsonObject.meshes[meshIndex].indices;

                var uvCount = jsonObject.meshes[meshIndex].uvCount.Value;
                var verticesStep = 1;

                // Depending of the number of texture's coordinates per vertex
                // we're jumping in the vertices array  by 6, 8 & 10 windows frame
                switch ((int)uvCount)
                {
                    case 0:
                        verticesStep = 6;
                        break;
                    case 1:
                        verticesStep = 8;
                        break;
                    case 2:
                        verticesStep = 10;
                        break;
                }

                // the number of interesting vertices information for us
                var verticesCount = verticesArray.Count / verticesStep;
                // number of faces is logically the size of the array divided by 3 (A, B, C)
                var facesCount = indicesArray.Count / 3;
                var mesh = new Mesh(jsonObject.meshes[meshIndex].name.Value, verticesCount, facesCount);

                // Filling the Vertices array of our mesh first
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = (float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;

                    var nx = (float)verticesArray[index * verticesStep + 3].Value;
                    var ny = (float)verticesArray[index * verticesStep + 4].Value;
                    var nz = (float)verticesArray[index * verticesStep + 5].Value;
                    mesh.Vertices[index] = new Vertice()
                    {
                        Position = new Point3D(x, y, z),
                        NormalVector = new Point3D(nx, ny, nz, 0)
                    };
                }

                // Then filling the Faces array
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.Faces[index] = new Face { A = a, B = b, C = c, Color = Color.FromArgb(rand.Next() % 256, rand.Next() % 256, rand.Next() % 256) };
                    //mesh.Faces[index] = new Face { A = a, B = b, C = c, Color = color };
                }

                // Getting the position you've set in Blender
                var position = jsonObject.meshes[meshIndex].position;
                mesh.Position = new Point3D((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                meshes.Add(mesh);
            }

            return meshes.ToArray();
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
