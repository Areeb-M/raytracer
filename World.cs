using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class World
    {
        private Shape[] shapes;
        private Light[] lights;
        private Shader background;

        public World(Shape[] shapes, Light[] lights, Shader background)
        {
            this.shapes = shapes;
            this.lights = lights;
            this.background = background;
        }

        public double[] shortestIntersectionTime(Ray ray, double threshold=0.00000000000000000000001)
        {
            double shortestID = -1;
            double shortestTime = Double.MaxValue;
            for (int i = 0; i < shapes.Length; i++)
            {
                double time = shapes[i].intersectionTime(ray);
                if (threshold < time && time < shortestTime)
                {
                    shortestTime = time;
                    shortestID = i;
                }
            }
            return new double[] { shortestID, shortestTime };
        }

        public Color calculateColor(Ray ray, int depth=5, int exclude=-1)
        {
            /*
                1. Find shortest intersection
                2. Shoot out light rays
                3. Shoot out reflection rays
                4. Shoot out refraction rays
                5. Return color
            */
            double[] intersection = shortestIntersectionTime(ray);

            if (intersection[0] >= 0)
            {
                Point pointOfIntersection = ray.step(intersection[1]);
                Shape shape = shapes[(int)intersection[0]];
                Color color = shape.shade(pointOfIntersection);

                foreach (Light light in lights)
                {
                    Ray lightRay = new Ray(pointOfIntersection, light.position);
                    double distance = lightRay.direction.magnitude;
                    double[] data = shortestIntersectionTime(lightRay);
                    double intersectDistance = lightRay.step(data[1]).distance(pointOfIntersection);
                    if (data[0] < 0 || intersectDistance > distance)
                    {
                        color = Color.mix(color, light.color,
                                Shader.clamp(Math.Sin(ray.direction.angle_between(lightRay.direction)) * light.brightness / distance, 0, 1)
                            );
                    }
                }

                if (depth > 0) {
                    if (shape.reflectivity > 0)
                    {
                        Vector normal = shape.getSurfaceNormal(pointOfIntersection);
                        Ray reflection = new Ray(pointOfIntersection, ray.direction.reflectAcross(normal));
                        color = Color.mix(calculateColor(reflection, depth - 1), color, shape.reflectivity);
                    }
                }
                return color;
            }
            else
            { 
                return background.shade(null, null);
            }
        }
    }
}
