using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    abstract class Shader
    {
        abstract public Color shade(Shape owner, Point intersection);
        public static double clamp(double num, int min, int max)
        {
            if ((int)num < min)
            {
                return min;
            }
            else if ((int)num >= max)
            {
                return max;
            }
            return num;
        }
    }

    class DefaultShader : Shader
    {
        private Color color;
        public DefaultShader(Color color)
        {
            this.color = color;
        }

        public override Color shade(Shape owner, Point intersection)
        {
            return color;
        }

    }

    class SphereTextureShader : Shader
    {
        private System.Drawing.Bitmap texture;
        public SphereTextureShader(System.Drawing.Bitmap texture)
        {
            this.texture = texture;
        }

        public override Color shade(Shape owner, Point intersection)
        {
            Sphere s = (Sphere)owner;
            intersection = intersection.subtract(s.position);
            int x = (int)clamp(((s.radius + intersection.x) * (texture.Width / s.radius / 2.0)), 0, texture.Width-1);
            int y = texture.Height - (int)clamp(((s.radius + intersection.y) * (texture.Height / s.radius / 2.0)), 0, texture.Height-1) - 1;

            return Color.fromSystem(texture.GetPixel(x, y));
        }

    }
}
