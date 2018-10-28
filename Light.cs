using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    abstract class Light
    {
        public Point position;
        public double brightness;
        public Color color;
    }

    class PointLight : Light
    {
        public PointLight (Point position, double brightness, Color color)
        {
            this.position = position;
            this.brightness = brightness;
            this.color = color;
        }
    }
}
