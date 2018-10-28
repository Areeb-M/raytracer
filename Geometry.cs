using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    //  Math Functions
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    class Angle
    {
        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    //  Points and Vectors
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    class Point
    {
        public double x, y, z;
        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return "Point(" + x + "," + y + "," + z + ")";
        }

        public Point subtract(Point other)
        {
            return new Point(x - other.x, y - other.y, z - other.z);
        }

        public double distance(Point other)
        {
            return Math.Sqrt((other.x - x) * (other.x - x) + (other.y - y) * (other.y - y) + (other.z - z) * (other.z - z));
        }
    }

    class Vector
    {
        public double dx, dy, dz;
        public double magnitude;
        public Vector(Point origin, Point end, bool normalize)
        {
            dx = end.x - origin.x;
            dy = end.y - origin.y;
            dz = end.z - origin.z;
            magnitude = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            if (normalize)
            {
                dx /= magnitude;
                dy /= magnitude;
                dz /= magnitude;
                magnitude = 1;
            }
        }

        public Vector(double dx, double dy, double dz)
        {
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            magnitude = Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public double dot(Vector other)
        {
            return this.dx * other.dx + this.dy * other.dy + this.dz * other.dz;
        }

        public double angle_between(Vector other)
        {
            return Math.Acos(dot(other) / (this.magnitude * other.magnitude));
        }

        public Vector scale(double scale)
        {
            return new Vector(scale * dx, scale * dy, scale * dz);
        }

        public Vector reflectAcross(Vector normal)
        {
            normal = normal.scale(scale(2).dot(normal));
            // r=d−2(d⋅n1)n2
            Vector d = this.copy();
            Vector n1 = normal.copy();
            Vector n2 = normal.copy();

            //Vector result = normal.copy().scale(this.copy().dot(normal.copy())).scale(2);
            Vector result = n1.scale(d.dot(n2)).scale(2);

            return new Vector(dx - result.dx, dy - result.dy, dz - result.dz);
        }

        public Vector copy()
        {
            return new Vector(this.dx, this.dy, this.dz);
        }

        public override string ToString()
        {
            return "Vector(" + dx + "," + dy + "," + dz + ")";
        }
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    //  Rays
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    class Ray
    {
        public Vector direction;
        public Point origin;

        public Ray(Point origin, Point end)
        {
            this.origin = origin;
            direction = new Vector(origin, end, false);
        }

        public Ray(Point origin, Vector direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Point step(double time)
        {
            return new Point(
                    origin.x + time * direction.dx,
                    origin.y + time * direction.dy,
                    origin.z + time * direction.dz
                );
        }

        public override string ToString()
        {
            return "Ray[" + origin + " " + direction + "]";
        }

    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    //  Basic Shapes
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    abstract class Shape
    {
        abstract public double intersectionTime(Ray ray);
        abstract public Color shade(Point intersection);
        abstract public Vector getSurfaceNormal(Point intersection);

        public double reflectivity = 0;
        public double transparency = 0;
    }

    class Sphere : Shape
    {
        public Point position;
        public double radius;

        private Shader shader;

        public Sphere(Point position, double radius, Shader shader, double reflectivity=0)
        {
            this.position = position;
            this.radius = radius;
            this.shader = shader;
            this.reflectivity = reflectivity;
        }

        public override double intersectionTime(Ray ray)
        {
            double A = ray.direction.magnitude * ray.direction.magnitude;
            double B = 2.0 * (ray.origin.x * ray.direction.dx + ray.origin.y * ray.direction.dy + ray.origin.z * ray.direction.dz - ray.direction.dx * position.x - ray.direction.dy * position.y - ray.direction.dz * position.z);
            double C = ray.origin.x * ray.origin.x - 2 * ray.origin.x * position.x + position.x * position.x + ray.origin.y * ray.origin.y - 2 * ray.origin.y * position.y + position.y * position.y +
                       ray.origin.z * ray.origin.z - 2 * ray.origin.z * position.z + position.z * position.z - radius * radius;
            double D = B * B - 4 * A * C;
            double t = -1.0;
            if (D >= 0)
            {
                double t1 = (-B - System.Math.Sqrt(D)) / (2.0 * A);
                double t2 = (-B + System.Math.Sqrt(D)) / (2.0 * A);
                if (t1 < t2)
                    t = t1;
                else
                    t = t2;  // we choose the nearest t from the first point
            }
            return t;
        }

        public override Color shade(Point intersection)
        {
            return shader.shade(this, intersection);
        }

        public override Vector getSurfaceNormal(Point intersection)
        {
            return new Vector(position, intersection, true);
        }
    }
}
