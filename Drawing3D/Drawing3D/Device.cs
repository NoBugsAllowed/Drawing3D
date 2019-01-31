using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawing3D
{
    public class Device
    {
        private byte[] backBuffer;
        private float[] depthBuffer;
        private DirectBitmap bitmap;
        private int renderWidth;
        private int renderHeight;
        private PictureBox pictureBox;
        private bool isRendering = false;

        public Shading Shading { get; set; }
        public List<Model> Models { get; set; }
        public List<LightSource> Lights { get; set; }
        public Camera Camera { get; set; }
        public Bitmap Bitmap
        {
            get => bitmap.Bitmap;
        }

        public Device(PictureBox pb)
        {
            pictureBox = pb;
            SetBitmapSize(pb.Width, pb.Height);
            pictureBox.Image = Bitmap;

            Models = new List<Model>();
            Lights = new List<LightSource>();

            Shading = Shading.Flat;
        }
        public void SetBitmapSize(int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            bitmap = new DirectBitmap(bmp);

            renderWidth = bmp.Width;
            renderHeight = bmp.Height;

            backBuffer = new byte[bmp.Width * bmp.Height * 4];
            depthBuffer = new float[bmp.Width * bmp.Height];
        }

        public void UpdateBitmap()
        {
            if (isRendering)
                return;

            Clear();
            Render(Camera, Models);
            Present();
        }
        public void Clear(byte r = 0, byte g = 0, byte b = 0, byte a = 255)
        {
            for (var index = 0; index < backBuffer.Length; index += 4)
            {
                backBuffer[index] = b;
                backBuffer[index + 1] = g;
                backBuffer[index + 2] = r;
                backBuffer[index + 3] = a;
            }
            //Set z buffer values to +infinity
            for (var index = 0; index < depthBuffer.Length; index++)
            {
                depthBuffer[index] = float.MaxValue;
            }
        }
        public void Render(Camera camera, List<Model> modelList)
        {
            //Prevent from flickering
            if (isRendering)
                return;

            isRendering = true;
            Matrix4x4 projection = Matrices.Projection(camera.Fov, (float)renderHeight / renderWidth, camera.N, camera.F);
            Matrix4x4 view = Matrices.View(Camera);
            foreach (Model model in modelList)
            {
                //Calculate model matrix
                Matrix4x4 modelMatrix = Matrices.Translation(model.Mesh.Position) * Matrices.RotationX(model.Mesh.Rotation.X) * Matrices.RotationY(model.Mesh.Rotation.Y) * Matrices.RotationZ(model.Mesh.Rotation.Z);

                Parallel.ForEach(model.Mesh.Faces, face =>
                {
                    Vertice v1 = model.Mesh.Vertices[face.A].Clone();
                    Vertice v2 = model.Mesh.Vertices[face.B].Clone();
                    Vertice v3 = model.Mesh.Vertices[face.C].Clone();

                    //Calculate position in global coords, position on bitmap and normal vectors for each vertice
                    v1.CalculateCoordinates(modelMatrix, view, projection, renderWidth, renderHeight);
                    v2.CalculateCoordinates(modelMatrix, view, projection, renderWidth, renderHeight);
                    v3.CalculateCoordinates(modelMatrix, view, projection, renderWidth, renderHeight);

                    FillTriangle(v1, v2, v3, face.Color, Shading);
                });
            }
            isRendering = false;
        }
        public void Present()
        {
            //Write data to bitmap
            using (var stream = new MemoryStream(bitmap.Bits))
            {
                stream.Write(backBuffer, 0, backBuffer.Length);
            }
            pictureBox.Image = Bitmap;
        }

        public void SetPixel(int x, int y, float z, Color col)
        {
            int index = (x + y * renderWidth);
            int index4 = index * 4;

            //Discard if point is behind current closest point
            if (depthBuffer[index] < z)
                return;
            //Update z buffer
            depthBuffer[index] = z;
            //Set desired color in buffer
            backBuffer[index4] = col.B;
            backBuffer[index4 + 1] = col.G;
            backBuffer[index4 + 2] = col.R;
            backBuffer[index4 + 3] = col.A;
        }
        public void DrawLine(Point3D p1, Point3D p2, Color color)
        {
            int x = (int)p1.X, y = (int)p1.Y, w = (int)(p2.X - p1.X), h = (int)(p2.Y - p1.Y);
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                if (x >= 0 && x < renderWidth && y >= 0 && y < renderHeight)
                    bitmap.SetPixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
        public void FillTriangle(Vertice v1, Vertice v2, Vertice v3, Color col, Shading shading = Shading.Flat)
        {
            Vertice t;
            //Sort vertices
            if (v1.ProjectedPosition.Y > v2.ProjectedPosition.Y)
            {
                t = v2;
                v2 = v1;
                v1 = t;
            }

            if (v2.ProjectedPosition.Y > v3.ProjectedPosition.Y)
            {
                t = v2;
                v2 = v3;
                v3 = t;
            }
            if (v1.ProjectedPosition.Y > v2.ProjectedPosition.Y)
            {
                t = v2;
                v2 = v1;
                v1 = t;
            }

            if (v1.ProjectedPosition.Y == v2.ProjectedPosition.Y)
            {
                if (v1.ProjectedPosition.Y == v3.ProjectedPosition.Y)
                    return;

                if (v2.ProjectedPosition.X < v1.ProjectedPosition.X)
                {
                    t = v2;
                    v2 = v1;
                    v1 = t;
                }
            }
            Point3D down = v1.ProjectedPosition, mid = v2.ProjectedPosition, up = v3.ProjectedPosition;

            float m1, m2;

            if (mid.Y > down.Y)
                m1 = (mid.X - down.X) / (mid.Y - down.Y);
            else
                m1 = float.MaxValue;

            m2 = (up.X - down.X) / (up.Y - down.Y);

            Color colDown, colMid, colUp;

            Point3D ks = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);
            Point3D kd = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);
            Point3D ka = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);

            int yMax = Math.Min(renderHeight - 1, (int)up.Y);
            if (shading == Shading.Flat)
            {
                //Calculate color only once for tringle center
                Point3D point = (v1.ScenePosition + v2.ScenePosition + v3.ScenePosition) / 3;
                Point3D normal = (v1.N + v2.N + v3.N) / 3;
                col = PhongLightingModel.CalculateColor(ks, kd, ka, point, normal, Camera.Position, Lights, new Point3D(0, 0, 0), 1);

                if (m1 > m2)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLine(y, down, up, down, mid, col);
                        }
                        else
                        {
                            ProcessScanLine(y, down, up, mid, up, col);
                        }
                    }
                }
                else if (m1 == 0 && m2 == 0)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        ProcessScanLine(y, down, up, mid, up, col);
                    }
                }
                else
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLine(y, down, mid, down, up, col);
                        }
                        else
                        {
                            ProcessScanLine(y, mid, up, down, up, col);
                        }
                    }
                }
            }
            else if (shading == Shading.Gouraud)
            {
                //Calculate color for each vertice and then interpolate color for each pixel
                colDown = PhongLightingModel.CalculateColor(ks, kd, ka, v1.Position, v1.N, Camera.Position, Lights, new Point3D(0, 0, 0), 1);
                colMid = PhongLightingModel.CalculateColor(ks, kd, ka, v2.Position, v2.N, Camera.Position, Lights, new Point3D(0, 0, 0), 1);
                colUp = PhongLightingModel.CalculateColor(ks, kd, ka, v3.Position, v3.N, Camera.Position, Lights, new Point3D(0, 0, 0), 1);

                if (m1 > m2)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLineGouraud(y, down, up, down, mid, colDown, colUp, colDown, colMid);
                        }
                        else
                        {
                            ProcessScanLineGouraud(y, down, up, mid, up, colDown, colUp, colMid, colUp);
                        }
                    }
                }
                else if (m1 == 0 && m2 == 0)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        ProcessScanLineGouraud(y, down, up, mid, up, colDown, colUp, colMid, colUp);
                    }
                }
                else
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLineGouraud(y, down, mid, down, up, colDown, colMid, colDown, colUp);
                        }
                        else
                        {
                            ProcessScanLineGouraud(y, mid, up, down, up, colMid, colUp, colDown, colUp);
                        }
                    }
                }
            }
            else if (shading == Shading.Phong)
            {
                //For each point interpolate normal vector and calculate color
                if (m1 > m2)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLinePhong(y, v1, v3, v1, v2, col);
                        }
                        else
                        {
                            ProcessScanLinePhong(y, v1, v3, v2, v3, col);
                        }
                    }
                }
                else if (m1 == 0 && m2 == 0)
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        ProcessScanLinePhong(y, v1, v3, v2, v3, col);
                    }
                }
                else
                {
                    for (var y = Math.Max(0, (int)down.Y); y <= yMax; y++)
                    {
                        if (y < mid.Y)
                        {
                            ProcessScanLinePhong(y, v1, v2, v1, v3, col);
                        }
                        else
                        {
                            ProcessScanLinePhong(y, v2, v3, v1, v3, col);
                        }
                    }
                }
            }
        }

        public void ProcessScanLine(int y, Point3D pa, Point3D pb, Point3D pc, Point3D pd, Color color)
        {
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            int xMax = Math.Min(renderWidth - 1, ex);
            for (int x = Math.Max(0, sx); x < xMax; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                float z = Interpolate(z1, z2, gradient);
                SetPixel(x, y, z, color);
            }
        }
        public void ProcessScanLineGouraud(int y, Point3D pa, Point3D pb, Point3D pc, Point3D pd, Color ca, Color cb, Color cc, Color cd)
        {
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            float r1 = Interpolate(ca.R, cb.R, gradient1);
            float g1 = Interpolate(ca.G, cb.G, gradient1);
            float b1 = Interpolate(ca.B, cb.B, gradient1);

            float r2 = Interpolate(cc.R, cd.R, gradient2);
            float g2 = Interpolate(cc.G, cd.G, gradient2);
            float b2 = Interpolate(cc.B, cd.B, gradient2);

            int xMax = Math.Min(renderWidth - 1, ex);
            for (int x = Math.Max(0, sx); x < xMax; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                float z = Interpolate(z1, z2, gradient);
                int r = (int)Interpolate(r1, r2, gradient);
                int g = (int)Interpolate(g1, g2, gradient);
                int b = (int)Interpolate(b1, b2, gradient);

                SetPixel(x, y, z, Color.FromArgb(r, g, b));
            }
        }
        public void ProcessScanLinePhong(int y, Vertice va, Vertice vb, Vertice vc, Vertice vd, Color col)
        {
            Point3D pa = va.ProjectedPosition;
            Point3D pb = vb.ProjectedPosition;
            Point3D pc = vc.ProjectedPosition;
            Point3D pd = vd.ProjectedPosition;

            Point3D ks = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);
            Point3D kd = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);
            Point3D ka = new Point3D((float)col.R / 255, (float)col.G / 255, (float)col.B / 255);

            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            float nx1 = Interpolate(va.N.X, vb.N.X, gradient1);
            float ny1 = Interpolate(va.N.Y, vb.N.Y, gradient1);
            float nz1 = Interpolate(va.N.Z, vb.N.Z, gradient1);

            float nx2 = Interpolate(vc.N.X, vd.N.X, gradient2);
            float ny2 = Interpolate(vc.N.Y, vd.N.Y, gradient2);
            float nz2 = Interpolate(vc.N.Z, vd.N.Z, gradient2);

            float px1 = Interpolate(va.ScenePosition.X, vb.ScenePosition.X, gradient1);
            float py1 = Interpolate(va.ScenePosition.Y, vb.ScenePosition.Y, gradient1);
            float pz1 = Interpolate(va.ScenePosition.Z, vb.ScenePosition.Z, gradient1);

            float px2 = Interpolate(vc.ScenePosition.X, vd.ScenePosition.X, gradient2);
            float py2 = Interpolate(vc.ScenePosition.Y, vd.ScenePosition.Y, gradient2);
            float pz2 = Interpolate(vc.ScenePosition.Z, vd.ScenePosition.Z, gradient2);

            int xMax = Math.Min(renderWidth - 1, ex);
            for (int x = Math.Max(0, sx); x < xMax; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                float z = Interpolate(z1, z2, gradient);

                float nx = Interpolate(nx1, nx2, gradient);
                float ny = Interpolate(ny1, ny2, gradient);
                float nz = Interpolate(nz1, nz2, gradient);

                float px = Interpolate(px1, px2, gradient);
                float py = Interpolate(py1, py2, gradient);
                float pz = Interpolate(pz1, pz2, gradient);

                Point3D n = new Point3D(nx, ny, nz);
                n /= n.DistanceFromOrigin();
                SetPixel(x, y, z, PhongLightingModel.CalculateColor(ks, kd, ka, new Point3D(px, py, pz), n, Camera.Position, Lights, new Point3D(0, 0, 0), 1));
            }
        }

        float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }
        float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }
    }

    public enum Shading { Flat, Gouraud, Phong }
}
