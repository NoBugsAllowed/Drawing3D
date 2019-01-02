using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class Device
    {
        private byte[] backBuffer;
        private readonly float[] depthBuffer;
        public DirectBitmap bitmap;
        private readonly int renderWidth;
        private readonly int renderHeight;

        public Device(Bitmap bmp)
        {
            bitmap = new DirectBitmap(bmp);

            renderWidth = bmp.Width;
            renderHeight = bmp.Height;

            backBuffer = new byte[bmp.Width * bmp.Height * 4];
            depthBuffer = new float[bmp.Width * bmp.Height];
        }

        public void Clear(byte r, byte g, byte b, byte a)
        {
            bitmap.Clear();
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

        private Point3D VertexShader(Camera camera, Matrix4x4 modelMatrix, Point3D point)
        {
            return camera.ProjectionMatrix * (camera.ViewMatrix * (modelMatrix * point));
        }

        public void Render(Camera camera, Model model)
        {
            Clear(255,255,255,255);
            foreach (Triangle tr in model.Triangles)
            {
                Point3D p1 = VertexShader(camera, model.ModelMatrix, tr.Vertices[0]);
                Point3D p2 = VertexShader(camera, model.ModelMatrix, tr.Vertices[1]);
                Point3D p3 = VertexShader(camera, model.ModelMatrix, tr.Vertices[2]);

                p1.X = (int)(((p1.X / p1.W) + 1) * renderWidth / 2);
                p1.Y = (int)(((p1.Y / p1.W) + 1) * renderHeight / 2);

                p2.X = (int)(((p2.X / p2.W) + 1) * renderWidth / 2);
                p2.Y = (int)(((p2.Y / p2.W) + 1) * renderHeight / 2);

                p3.X = (int)(((p3.X / p3.W) + 1) * renderWidth / 2);
                p3.Y = (int)(((p3.Y / p3.W) + 1) * renderHeight / 2);

                FillTriangle(p1, p2, p3);

                DrawLine(p1, p2, Color.Black);
                DrawLine(p1, p3, Color.Black);
                DrawLine(p2, p3, Color.Black);
            }
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

        public void FillTriangle(Point3D p1, Point3D p2, Point3D p3)
        {
            Point3D down = p1, mid = p2, up = p3, tmp;
            if (down.Y > up.Y)
            {
                tmp = down;
                down = up;
                up = tmp;
            }
            if (down.Y > mid.Y)
            {
                tmp = down;
                down = mid;
                mid = tmp;
            }
            if (mid.Y > up.Y)
            {
                tmp = mid;
                mid = up;
                up = tmp;
            }


            if (down.Y == mid.Y && down.Y == up.Y)
                return;

            double m1, m2;
            double x1, x2;

            // Flat bottom
            if (down.Y == mid.Y)
            {
                if (down.X < mid.X)
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
            // Flat top
            else if (mid.Y == up.Y)
            {
                if (mid.X > up.X)
                {
                    tmp = mid;
                    mid = up;
                    up = tmp;
                }

                x1 = down.X;
                x2 = down.X;
                m2 = (double)(up.X - down.X) / (up.Y - down.Y);
                m1 = (double)(mid.X - down.X) / (mid.Y - down.Y);

                if (m1 > m2)
                {
                    double t = m1;
                    m1 = m2;
                    m2 = t;
                }
            }
            else
            {
                x1 = down.X;
                x2 = down.X;
                m1 = (double)(up.X - down.X) / (up.Y - down.Y);
                m2 = (double)(mid.X - down.X) / (mid.Y - down.Y);

                if (m1 > m2)
                {
                    double t = m1;
                    m1 = m2;
                    m2 = t;
                }
            }

            for (int y = (int)down.Y + 1; y < up.Y; y++)
            {
                //Update X, sort edges
                x1 += m1;
                x2 += m2;

                if (y >= renderHeight)
                    return;

                if (y >= 0)
                {
                    if (y == mid.Y)
                    {
                        if (Math.Abs(mid.X - x1) < Math.Abs(mid.X - x2))
                            m1 = (double)(up.X - mid.X) / (up.Y - mid.Y);
                        else
                            m2 = (double)(up.X - mid.X) / (up.Y - mid.Y);
                    }
                    for (int j = Math.Max((int)x1, 0); j < Math.Min((int)x2, renderWidth); j++)
                    {
                        bitmap.SetPixel(j, y, Color.Red);
                    }
                }
            }
        }
    }
}
