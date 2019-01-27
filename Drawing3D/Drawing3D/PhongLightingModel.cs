using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing3D
{
    class PhongLightingModel
    {
        public static Color CalculateColor(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, List<LightSource> lightSources, Point3D backgroundLight, int m)
        {
            float r = backgroundLight.X * ka.X;
            float g = backgroundLight.Y * ka.Y;
            float b = backgroundLight.Z * ka.Z;

            foreach (LightSource l in lightSources)
            {
                if(l.IsOn)
                {
                    (float lr, float lg, float lb) = l.PhongIlumination(ks, kd, ka, target, normal, cameraPosition, m);
                    r += lr;
                    g += lg;
                    b += lb;
                }
            }

            int R = (int)(r * 255);
            int G = (int)(g * 255);
            int B = (int)(b * 255);

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;


            return Color.FromArgb(R, G, B);
        }

        public static Point3D CalculateColor2(Point3D ks, Point3D kd, Point3D ka, Point3D target, Point3D normal, Point3D cameraPosition, List<LightSource> lightSources, Point3D backgroundLight, int m)
        {
            float r = backgroundLight.X * ka.X;
            float g = backgroundLight.Y * ka.Y;
            float b = backgroundLight.Z * ka.Z;

            foreach (LightSource l in lightSources)
            {
                (float lr, float lg, float lb) = l.PhongIlumination(ks, kd, ka, target, normal, cameraPosition, m);
                r += lr;
                g += lg;
                b += lb;
            }

            return new Point3D(r, g, b);
        }
    }
}
