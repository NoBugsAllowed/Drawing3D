﻿using System;
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

        public void Clear(byte r=0, byte g=0, byte b=0, byte a=255)
        {
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
            Clear();
            Render(camera, model);
            Present();
        }

        public void Render(Camera camera, Model model)
        {
            //Matrix4x4 matrix = camera.ProjectionMatrix * camera.ViewMatrix * model.ModelMatrix;
            Matrix4x4 matrix = camera.ProjectionMatrix * camera.ViewMatrix * MatrixTransform.RotationX(model.Mesh.Rotation.X) * MatrixTransform.RotationY(model.Mesh.Rotation.Y) * MatrixTransform.RotationZ(model.Mesh.Rotation.Z);

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

            backBuffer[index4] = col.B;
            backBuffer[index4 + 1] = col.G;
            backBuffer[index4 + 2] = col.R;
            backBuffer[index4 + 3] = col.A;
        }

        public void Present()
        {
            using (var stream = new MemoryStream(bitmap.Bits))
            {
                stream.Write(backBuffer, 0, backBuffer.Length);
            }
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

        public void FillTriangle(Point3D p1, Point3D p2, Point3D p3, Color col)
        {
            Point3D down = p1, mid = p2, up = p3, tmp;
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
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            int xMax = Math.Min(renderWidth, ex);
            for (int x = Math.Max(0, sx); x < xMax; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                float z = Interpolate(z1, z2, gradient);
                SetPixel(x, y, z, color);
            }
        }

        public Mesh[] LoadJSONFile(string path, Color color)
        {
            var meshes = new List<Mesh>();
            Random rand = new Random();
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
}
