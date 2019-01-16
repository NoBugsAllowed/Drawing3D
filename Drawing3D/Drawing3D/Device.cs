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

        public Bitmap Bitmap
        {
            get => bitmap.Bitmap;
        }

        public Device(PictureBox pb)
        {
            pictureBox = pb;
            SetBitmapSize(pb.Width, pb.Height);
            pictureBox.Image = Bitmap;
            //Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            //bitmap = new DirectBitmap(bmp);
            //pictureBox = pb;
            //pictureBox.Image = Bitmap;

            //renderWidth = bmp.Width;
            //renderHeight = bmp.Height;

            //backBuffer = new byte[bmp.Width * bmp.Height * 4];
            //depthBuffer = new float[bmp.Width * bmp.Height];
        }

        public void SetBitmapSize(int w, int h)
        {
            Bitmap bmp = new Bitmap(w, w);
            bitmap = new DirectBitmap(bmp);

            renderWidth = bmp.Width;
            renderHeight = bmp.Height;

            backBuffer = new byte[bmp.Width * bmp.Height * 4];
            depthBuffer = new float[bmp.Width * bmp.Height];
        }

        public void Clear(byte r, byte g, byte b, byte a)
        {
            //bitmap.Clear();
            for (var index = 0; index < backBuffer.Length; index += 4)
            {
                backBuffer[index] = b;
                backBuffer[index + 1] = g;
                backBuffer[index + 2] = r;
                backBuffer[index + 3] = a;
            }

            for (var index = 0; index < depthBuffer.Length; index++)
            {
                depthBuffer[index] = float.MaxValue;
            }
        }

        //private Point3D VertexShader(Camera camera, Matrix4x4 modelMatrix, Point3D point)
        //{
        //    return camera.ProjectionMatrix * (camera.ViewMatrix * (modelMatrix * point));
        //}
        public void DisplaySceneOnBitmap(Camera camera, Model model)
        {
            Clear(255, 255, 255, 255);
            Render(camera, model);
            Present();
        }

        public void Render(Camera camera, Model model)
        {
            Matrix4x4 matrix = camera.ProjectionMatrix * camera.ViewMatrix * model.ModelMatrix;
            //foreach (Triangle tr in model.Triangles)
            //{
            //    //Point3D p1 = VertexShader(camera, model.ModelMatrix, tr.Vertices[0]);
            //    //Point3D p2 = VertexShader(camera, model.ModelMatrix, tr.Vertices[1]);
            //    //Point3D p3 = VertexShader(camera, model.ModelMatrix, tr.Vertices[2]);

            //    Point3D p1 = matrix * tr.Vertices[0];
            //    Point3D p2 = matrix * tr.Vertices[1];
            //    Point3D p3 = matrix * tr.Vertices[2];

            //    p1.X = (int)(((p1.X / p1.W) + 1) * renderWidth / 2);
            //    p1.Y = (int)(((p1.Y / p1.W) + 1) * renderHeight / 2);

            //    p2.X = (int)(((p2.X / p2.W) + 1) * renderWidth / 2);
            //    p2.Y = (int)(((p2.Y / p2.W) + 1) * renderHeight / 2);

            //    p3.X = (int)(((p3.X / p3.W) + 1) * renderWidth / 2);
            //    p3.Y = (int)(((p3.Y / p3.W) + 1) * renderHeight / 2);

            //    FillTriangle(p1, p2, p3, tr.Color);

            //    //DrawLine(p1, p2, Color.Black);
            //    //DrawLine(p1, p3, Color.Black);
            //    //DrawLine(p2, p3, Color.Black);
            //}
            //Parallel.ForEach(model.Triangles, tr =>
            // {
            //     Point3D p1 = matrix * tr.Vertices[0];
            //     Point3D p2 = matrix * tr.Vertices[1];
            //     Point3D p3 = matrix * tr.Vertices[2];

            //     p1.X = (int)(((p1.X / p1.W) + 1) * renderWidth / 2);
            //     p1.Y = (int)(((p1.Y / p1.W) + 1) * renderHeight / 2);

            //     p2.X = (int)(((p2.X / p2.W) + 1) * renderWidth / 2);
            //     p2.Y = (int)(((p2.Y / p2.W) + 1) * renderHeight / 2);

            //     p3.X = (int)(((p3.X / p3.W) + 1) * renderWidth / 2);
            //     p3.Y = (int)(((p3.Y / p3.W) + 1) * renderHeight / 2);

            //     FillTriangle(p1, p2, p3, tr.Color);
            // });

            Parallel.ForEach(model.Mesh.Faces, face =>
            {
                Point3D p1 = matrix * model.Mesh.Vertices[face.A];
                Point3D p2 = matrix * model.Mesh.Vertices[face.B];
                Point3D p3 = matrix * model.Mesh.Vertices[face.C];


                p1.X = (int)(((p1.X / p1.W) + 1) * renderWidth / 2);
                p1.Y = (int)(((p1.Y / p1.W) + 1) * renderHeight / 2);

                p2.X = (int)(((p2.X / p2.W) + 1) * renderWidth / 2);
                p2.Y = (int)(((p2.Y / p2.W) + 1) * renderHeight / 2);

                p3.X = (int)(((p3.X / p3.W) + 1) * renderWidth / 2);
                p3.Y = (int)(((p3.Y / p3.W) + 1) * renderHeight / 2);

                FillTriangle(p1, p2, p3, face.Color);
            });
        }

        float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }

        public void SetPixel(int x, int y, float z, Color col)
        {
            int index = (x + y * renderWidth);
            int index4 = index * 4;

            if (depthBuffer[index] < z)
                return;

            depthBuffer[index] = z;

            // Odwrotnie?
            backBuffer[index4] = col.B;
            backBuffer[index4 + 1] = col.G;
            backBuffer[index4 + 2] = col.R;
            backBuffer[index4 + 3] = col.A;
            //backBuffer[index4] = col.A;
            //backBuffer[index4+1] = col.R;
            //backBuffer[index4+2] = col.G;
            //backBuffer[index4+3] = col.B;
        }

        public void Present()
        {
            using (var stream = new MemoryStream(bitmap.Bits))
            {
                stream.Write(backBuffer, 0, backBuffer.Length);
            }
            //pictureBox.Refresh();
            pictureBox.Image = Bitmap;
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

        //public void FillTriangle(Point3D p1, Point3D p2, Point3D p3)
        //{
        //    Point3D down = p1, mid = p2, up = p3, tmp;
        //    if (down.Y > up.Y)
        //    {
        //        tmp = down;
        //        down = up;
        //        up = tmp;
        //    }
        //    if (down.Y > mid.Y)
        //    {
        //        tmp = down;
        //        down = mid;
        //        mid = tmp;
        //    }
        //    if (mid.Y > up.Y)
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
        //        if (down.X < mid.X)
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
        //    else if (mid.Y == up.Y)
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

        //    //for (int y = (int)down.Y + 1; y < up.Y; y++)
        //    //{
        //    //    //Update X
        //    //    x1 += m1;
        //    //    x2 += m2;

        //    //    var gradient1 = down.Y != up.Y ? (y - down.Y) / (up.Y - down.Y) : 1;
        //    //    var gradient2 = down.Y != mid.Y ? (y - down.Y) / (mid.Y - down.Y) : 1;

        //    //    int sx = (int)Interpolate(down.X, pb.X, gradient1);
        //    //    int ex = (int)Interpolate(pc.X, pd.X, gradient2);

        //    //    // starting Z & ending Z
        //    //    float z1 = Interpolate(pa.Z, pb.Z, gradient1);
        //    //    float z2 = Interpolate(pc.Z, pd.Z, gradient2);

        //    //    if (y >= renderHeight)
        //    //        return;

        //    //    if (y >= 0)
        //    //    {
        //    //        if (y == mid.Y)
        //    //        {
        //    //            if (Math.Abs(mid.X - x1) < Math.Abs(mid.X - x2))
        //    //                m1 = (double)(up.X - mid.X) / (up.Y - mid.Y);
        //    //            else
        //    //                m2 = (double)(up.X - mid.X) / (up.Y - mid.Y);
        //    //        }
        //    //        for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, renderWidth); j++)
        //    //        {
        //    //            bitmap.SetPixel(j, y, Color.Red);
        //    //        }
        //    //    }
        //    //}

        //    float gradient1, gradient2;
        //    int y = (int)down.Y + 1;
        //    for (; y < mid.Y; y++)
        //    {
        //        //Update X
        //        x1 += m1;
        //        x2 += m2;

        //        gradient1 = down.Y != up.Y ? (y - down.Y) / (up.Y - down.Y) : 1;
        //        gradient2 = down.Y != mid.Y ? (y - down.Y) / (mid.Y - down.Y) : 1;

        //        // starting Z & ending Z
        //        float z1 = Interpolate(down.Z, up.Z, gradient1);
        //        float z2 = Interpolate(down.Z, mid.Z, gradient2);

        //        if (y >= renderHeight)
        //            return;

        //        if (y >= 0)
        //        {
        //            for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, renderWidth); j++)
        //            {
        //                bitmap.SetPixel(j, y, Color.Red);
        //            }
        //        }
        //    }
        //    if (Math.Abs(mid.X - x1) < Math.Abs(mid.X - x2))
        //        m1 = (double)(up.X - mid.X) / (up.Y - mid.Y);
        //    else
        //        m2 = (double)(up.X - mid.X) / (up.Y - mid.Y);

        //    for (; y < mid.Y; y++)
        //    {
        //        //Update X
        //        x1 += m1;
        //        x2 += m2;

        //        var gradient1 = down.Y != up.Y ? (y - down.Y) / (up.Y - down.Y) : 1;
        //        var gradient2 = down.Y != mid.Y ? (y - down.Y) / (mid.Y - down.Y) : 1;

        //        int sx = (int)Interpolate(down.X, pb.X, gradient1);
        //        int ex = (int)Interpolate(pc.X, pd.X, gradient2);

        //        // starting Z & ending Z
        //        float z1 = Interpolate(pa.Z, pb.Z, gradient1);
        //        float z2 = Interpolate(pc.Z, pd.Z, gradient2);

        //        if (y >= renderHeight)
        //            return;

        //        if (y >= 0)
        //        {
        //            for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, renderWidth); j++)
        //            {
        //                bitmap.SetPixel(j, y, Color.Red);
        //            }
        //        }
        //    }
        //}

        public void FillTriangle(Point3D p1, Point3D p2, Point3D p3, Color col)
        {
            Point3D down = p1, mid = p2, up = p3, tmp;
            //if (down.Y > up.Y)
            //{
            //    tmp = down;
            //    down = up;
            //    up = tmp;
            //}
            //if (down.Y > mid.Y)
            //{
            //    tmp = down;
            //    down = mid;
            //    mid = tmp;
            //}
            //if (mid.Y > up.Y)
            //{
            //    tmp = mid;
            //    mid = up;
            //    up = tmp;
            //}
            if (down.Y > mid.Y)
            {
                tmp = mid;
                mid = down;
                down = tmp;
            }

            if (mid.Y > up.Y)
            {
                tmp = mid;
                mid = up;
                up = tmp;
            }
            if (down.Y > mid.Y)
            {
                tmp = mid;
                mid = down;
                down = tmp;
            }


            if (down.Y == mid.Y)
            {
                if (down.Y == up.Y)
                    return;

                if (mid.X < down.X)
                {
                    tmp = mid;
                    mid = down;
                    down = tmp;
                }
            }

            float m1, m2;

            if (mid.Y > down.Y)
                m1 = (mid.X - down.X) / (mid.Y - down.Y);
            else
                m1 = float.MaxValue;

            m2 = (up.X - down.X) / (up.Y - down.Y);

            int yMax = Math.Min(renderHeight, (int)up.Y);
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

        public void ProcessScanLine(int y, Point3D pa, Point3D pb, Point3D pc, Point3D pd, Color color)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            // starting Z & ending Z
            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            // drawing a line from left (sx) to right (ex) 
            int xMax = Math.Min(renderWidth, ex);
            for (int x = Math.Max(0, sx); x < xMax; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                float z = Interpolate(z1, z2, gradient);
                SetPixel(x, y, z, color);
            }
        }

        // Loading the JSON file in an asynchronous manner
        public Mesh[] LoadJSONFile(string path)
        {
            //var triangles = new List<Triangle>();
            var meshes = new List<Mesh>();
            Random rand = new Random();
            //var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileName);
            //var data = await Windows.Storage.FileIO.ReadTextAsync(file);
            var data = File.ReadAllText(path,Encoding.UTF8);
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
                    mesh.Vertices[index] = new Point3D(x, y, z);
                }

                // Then filling the Faces array
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.Faces[index] = new Face { A = a, B = b, C = c, Color = Color.FromArgb(rand.Next() % 256, rand.Next() % 256, rand.Next() % 256) };
                }

                // Getting the position you've set in Blender
                var position = jsonObject.meshes[meshIndex].position;
                mesh.Position = new Point3D((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                meshes.Add(mesh);
            }
            //for (var meshIndex = 0; meshIndex < jsonObject.meshes.Count; meshIndex++)
            //{
            //    var verticesArray = jsonObject.meshes[meshIndex].vertices;
            //    // Faces
            //    var indicesArray = jsonObject.meshes[meshIndex].indices;

            //    var uvCount = jsonObject.meshes[meshIndex].uvCount.Value;
            //    var verticesStep = 1;

            //    // Depending of the number of texture's coordinates per vertex
            //    // we're jumping in the vertices array  by 6, 8 & 10 windows frame
            //    switch ((int)uvCount)
            //    {
            //        case 0:
            //            verticesStep = 6;
            //            break;
            //        case 1:
            //            verticesStep = 8;
            //            break;
            //        case 2:
            //            verticesStep = 10;
            //            break;
            //    }

            //    // the number of interesting vertices information for us
            //    var verticesCount = verticesArray.Count / verticesStep;
            //    // number of faces is logically the size of the array divided by 3 (A, B, C)
            //    var facesCount = indicesArray.Count / 3;
            //    //var mesh = new Mesh(jsonObject.meshes[meshIndex].name.Value, verticesCount, facesCount);

            //    Point3D a, b, c;
            //    float x, y, z;
            //    // Filling the Vertices array of our mesh first
            //    x = (float)verticesArray[0].Value;
            //    y = (float)verticesArray[1].Value;
            //    z = (float)verticesArray[2].Value;
            //    a = new Point3D(x, y, z);
            //    x = (float)verticesArray[3].Value;
            //    y = (float)verticesArray[4].Value;
            //    z = (float)verticesArray[5].Value;
            //    b = new Point3D(x, y, z);
            //    x = (float)verticesArray[6].Value;
            //    y = (float)verticesArray[7].Value;
            //    z = (float)verticesArray[8].Value;
            //    c = new Point3D(x, y, z);

            //    // Then filling the Faces array
            //    //for (var index = 0; index < facesCount; index++)
            //    //{
            //    //    var a = (int)indicesArray[index * 3].Value;
            //    //    var b = (int)indicesArray[index * 3 + 1].Value;
            //    //    var c = (int)indicesArray[index * 3 + 2].Value;
            //    //    mesh.Faces[index] = new Face { A = a, B = b, C = c };
            //    //}

            //    // Getting the position you've set in Blender
            //    var position = jsonObject.meshes[meshIndex].position;
            //    //mesh.Position = new Vector3((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
            //    triangles.Add(new Triangle(a, b, c, Color.FromArgb(rand.Next() % 256, rand.Next() % 256, rand.Next() % 256)));
            //}
            //return new Model(triangles);
            return meshes.ToArray();
        }
    }
}
