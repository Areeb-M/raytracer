using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class Color
    {
        public int r, g, b, alpha;
        public Color(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.alpha = 255;
        }
        public Color(int r, int g, int b, int alpha)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.alpha = alpha;
        }

        public static Color mix(Color a, Color b, double weight=0.5)
        {
            int red = (int)(a.r * weight + b.r * (1 - weight));
            int green = (int)(a.g * weight + b.g * (1 - weight));
            int blue = (int)(a.b * weight + b.b * (1 - weight));
            int alpha = (int)(a.alpha * weight + b.alpha * (1 - weight));
            return new Color(red, green, blue, alpha);
        }

        public System.Drawing.Color toSystem()
        {
            return System.Drawing.Color.FromArgb(this.alpha, this.r, this.g, this.b);
        }

        public static Color fromSystem(System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public override String ToString()
        {
            return "Color(" + r + "," + g + "," + b + ")";
        }
    }
}
