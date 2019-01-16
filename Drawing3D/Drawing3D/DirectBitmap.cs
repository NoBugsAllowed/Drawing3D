using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public byte[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Size Size { get => Bitmap.Size; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(Bitmap bmp) : this(bmp.Width, bmp.Height)
        {
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                    Bitmap.SetPixel(i, j, bmp.GetPixel(i, j));
        }
        public DirectBitmap(int width, int height) : this(width, height, Color.White) { }
        public DirectBitmap(int width, int height, Color background)
        {
            Width = width;
            Height = height;
            Bits = new byte[4 * width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
            BackgroundColor = background;
        }

        //We assume coordinates are correct
        public void SetPixel(int x, int y, Color colour)
        {
            int index = (x + (y * Width)) * 4;
            Bits[index] = colour.A;
            Bits[index + 1] = colour.R;
            Bits[index + 2] = colour.G;
            Bits[index + 3] = colour.B;
        }

        public Color GetPixel(int x, int y)
        {
            int index = (x + (y * Width)) * 4;
            Color result = Color.FromArgb(Bits[index], Bits[index + 1], Bits[index + 2], Bits[index + 3]);

            return result;
        }

        public void Clear()
        {
            for (int i = 0; i < Bits.Length; i+=4)
            {
                Bits[i] = BackgroundColor.A;
                Bits[i+1] = BackgroundColor.R;
                Bits[i+2] = BackgroundColor.G;
                Bits[i+3] = BackgroundColor.B;
            }
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
