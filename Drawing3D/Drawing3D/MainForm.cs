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
        public Device RenderDevice { get; set; }
        public Camera StaticFrontCamera { get; set; }
        public Camera StaticBackCamera { get; set; }
        public Camera FocusedCamera { get; set; }
        public Camera MovingCamera { get; set; }
        public DirectionalLightSource DirectionalLight { get; set; }
        public SpotLightSource Reflector { get; set; }
        public PointLightSource PointLight { get; set; }
        private System.Timers.Timer animationTimer;

        private const float BALL_START_Z = -30f;
        private const float BALL_V = 1.4f;
        private const float MOVING_CAM_DIST_Z = 20f;
        private const float MOVING_CAM_DIST_Y = 20f;
        private int temp = 0;

        public MainForm()
        {
            InitializeComponent();

            RenderDevice = new Device(pictureBox);

            float fov = (float)(((int)numericUpDown.Value) * Math.PI / 180);
            float a = (float)pictureBox.Height / pictureBox.Width;

            //Load models and set positions
            Mesh[] bowlingPinMesh = LoadJSONFile("bowlingpin2.babylon", Color.Wheat, out Point3D pinSize);
            Mesh[] ballMesh = LoadJSONFile("ball.babylon", Color.Red, out Point3D ballSize);
            Model ball = new Model(ballMesh[0]);
            Model floor = CreateRectangle(new Point3D(0, 0, -15), 10f, 45f, Color.Yellow);
            Model bowlingPin = new Model(bowlingPinMesh[0]);
            List<Model> bowlingPins = CreateBowlingTetractys(bowlingPinMesh[0], new Point3D(0, 0, 5), 2.5f);
            bowlingPin.Position = new Point3D(0, 10, 5);
            ball.Position = new Point3D(0, ballSize.Y / 2.0f, BALL_START_Z);

            //Set cameras positions
            StaticFrontCamera = new Camera(new Point3D(0, 10.0f, -60f), new Point3D(0, 0, 0), new Point3D(0, 1, 0), fov, a);
            StaticBackCamera = new Camera(new Point3D(0, 20, 40), new Point3D(0, 0, 0), new Point3D(0, 1, 0), fov, a);
            FocusedCamera = new Camera(new Point3D(0, 10.0f, BALL_START_Z - 5f), ball.Position, new Point3D(0, 1, 0), fov, a);
            MovingCamera = new Camera(new Point3D(0, ball.Position.Y + MOVING_CAM_DIST_Y, ball.Position.Z - MOVING_CAM_DIST_Z), ball.Position, new Point3D(0, 1, 0), fov, a);

            //Set light sources
            DirectionalLight = new DirectionalLightSource(new Point3D(0, -1, 1)) { IsOn = true };
            Reflector = new SpotLightSource(new Point3D(0, 25, -15), new Point3D(0, 0, 15), Color.White) { IsOn = false };
            PointLight = new PointLightSource(new Point3D(5.0f, 15.0f, -5.0f), Color.White) { IsOn = false };

            //Passing objects to render device 
            RenderDevice.Camera = StaticFrontCamera;
            RenderDevice.Lights.Add(DirectionalLight);
            RenderDevice.Lights.Add(Reflector);
            RenderDevice.Lights.Add(PointLight);
            RenderDevice.Models.AddRange(bowlingPins);
            RenderDevice.Models.Add(floor);
            RenderDevice.Models.Add(ball);
            RenderDevice.Models.Add(bowlingPin);

            //Set up timer to animate scene
            animationTimer = new System.Timers.Timer();
            animationTimer.Interval = 80;
            animationTimer.Elapsed += (s, e) =>
            {
                if (temp < 10)
                {
                    temp++;
                }
                else if (temp == 11)
                {
                    ball.Position.Z += BALL_V;
                    ball.Mesh.Rotation.X += 0.2f;
                    MovingCamera.Position.Z += BALL_V;
                    if (ball.Position.Z >= -2.0f)
                        temp++;
                }
                else if (temp < 26)
                {
                    temp++;
                }
                else
                {
                    ball.Position.Z = BALL_START_Z;
                    MovingCamera.Position.Z = ball.Position.Z - MOVING_CAM_DIST_Z;
                    temp = 0;
                }
                bowlingPin.Rotation.Z += 0.2f;
                if (bowlingPin.Rotation.Z > 2 * Math.PI)
                    bowlingPin.Rotation.Z -= (float)(2 * Math.PI);
                RenderDevice.UpdateBitmap();
            };
            animationTimer.Start();

            pictureBox.Image = RenderDevice.Bitmap;
        }

        private Model CreateRectangle(Point3D center, float a, float b, Color color)
        {
            float a2 = a / 2, b2 = b / 2;
            Mesh mesh = new Mesh("Rectangle", 4, 2);

            mesh.Vertices[0] = new Vertice(new Point3D(-a2, 0, -b2), new Point3D(0, 1, 0));
            mesh.Vertices[1] = new Vertice(new Point3D(-a2, 0, b2), new Point3D(0, 1, 0));
            mesh.Vertices[2] = new Vertice(new Point3D(a2, 0, b2), new Point3D(0, 1, 0));
            mesh.Vertices[3] = new Vertice(new Point3D(a2, 0, -b2), new Point3D(0, 1, 0));

            mesh.Faces[0] = new Face() { A = 0, B = 1, C = 2, Color = color };
            mesh.Faces[1] = new Face() { A = 0, B = 2, C = 3, Color = color };

            //int n = 4, m = 6;
            //Mesh mesh = new Mesh("Rectangle", (n + 1) * (m + 1), n * m * 2);
            //float da = a / n, db = b / m;
            //float sa = -a2, sb = -b2;

            //int fi = 0, k;
            //for (int i = 0; i <= n; i++)
            //{
            //    for (int j = 0; j <= m; j++)
            //    {
            //        k = i * (m + 1) + j;
            //        mesh.Vertices[k] = new Vertice(new Point3D(sa + da * i, 0, sb + db * j), new Point3D(0, 1, 0));
            //        if (i < n && j < m)
            //        {
            //            mesh.Faces[fi] = new Face() { A = k, B = k + 1, C = k + m + 2, Color = color };
            //            mesh.Faces[fi + 1] = new Face() { A = k, B = k + m + 1, C = k + m + 2, Color = color };
            //            fi += 2;
            //        }
            //    }
            //}

            Model model = new Model(mesh);
            model.Position = center;
            return model;
        }
        private List<Model> CreateBowlingTetractys(Mesh pinMesh, Point3D center, float dist)
        {
            List<Model> pins = new List<Model>();

            Model pin;

            float dx = dist / 2.0f;
            float dz = (float)(dist * Math.Sqrt(3) / 2);
            for (int i = -2; i < 2; i++)
            {
                int k = i + 3;
                float sx = center.X - (k - 1) * dx;
                for (int j = 0; j < k; j++)
                {
                    pin = new Model(pinMesh.Clone());
                    pin.Mesh.Position = new Point3D(sx + j * dist, center.Y, center.Z + i * dz);
                    pins.Add(pin);
                }
            }

            return pins;
        }

        //Loading mesh from .babylon file
        private Mesh[] LoadJSONFile(string path, Color color, out Point3D size)
        {
            float minX, minY, minZ, maxX, maxY, maxZ;
            minX = minY = minZ = float.MaxValue;
            maxX = maxY = maxZ = float.MinValue;

            var meshes = new List<Mesh>();
            Random rand = new Random();
            var data = File.ReadAllText(path, Encoding.UTF8);
            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(data);

            for (var meshIndex = 0; meshIndex < jsonObject.meshes.Count; meshIndex++)
            {
                // Vertices with normal vectors
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

                // The actual number of vertices
                var verticesCount = verticesArray.Count / verticesStep;
                var facesCount = indicesArray.Count / 3;
                var mesh = new Mesh(jsonObject.meshes[meshIndex].name.Value, verticesCount, facesCount);

                // Filling the vertices array
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = (float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;

                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (z < minZ) minZ = z;

                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                    if (z > maxZ) maxZ = z;

                    var nx = (float)verticesArray[index * verticesStep + 3].Value;
                    var ny = (float)verticesArray[index * verticesStep + 4].Value;
                    var nz = (float)verticesArray[index * verticesStep + 5].Value;
                    mesh.Vertices[index] = new Vertice()
                    {
                        Position = new Point3D(x, y, z),
                        NormalVector = new Point3D(nx, ny, nz, 0)
                    };
                }

                // Faces
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.Faces[index] = new Face { A = a, B = b, C = c, Color = color };
                }

                var position = jsonObject.meshes[meshIndex].position;
                mesh.Position = new Point3D((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                meshes.Add(mesh);
            }
            size = new Point3D(maxX - minX, maxY - minY, maxZ - minZ);
            return meshes.ToArray();
        }

        //Radiobuttons & comboboxes handlers
        private void rbShadingFlat_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Shading = Shading.Flat;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbShadingGouraud_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Shading = Shading.Gouraud;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbShadingPhong_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Shading = Shading.Phong;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbStaticFrontCamera_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Camera = StaticFrontCamera;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbStaticBackCamera_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Camera = StaticBackCamera;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbFocusedCamera_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Camera = FocusedCamera;
                RenderDevice.UpdateBitmap();
            }
        }
        private void rbMovingCamera_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RenderDevice.Camera = MovingCamera;
                RenderDevice.UpdateBitmap();
            }
        }
        private void cbDirectional_CheckedChanged(object sender, EventArgs e)
        {
            DirectionalLight.IsOn = (sender as CheckBox).Checked;
        }
        private void cbReflector_CheckedChanged(object sender, EventArgs e)
        {
            Reflector.IsOn = (sender as CheckBox).Checked;
        }
        private void cbPointLight_CheckedChanged(object sender, EventArgs e)
        {
            PointLight.IsOn = (sender as CheckBox).Checked;
        }
        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RenderDevice.Camera.ChangeFov((float)(((int)(sender as NumericUpDown).Value) * Math.PI / 180));
            RenderDevice.UpdateBitmap();
        }
    }
}
