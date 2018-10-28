using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Vector a = new Vector(-1, -1, 0);
            Vector normal = new Vector(-1, 0, 0);
            Console.WriteLine(a + " reflected across " + normal + " is " + a.reflectAcross(normal));
            */

            System.Drawing.Bitmap test = new System.Drawing.Bitmap("life.png");

            int WIDTH = 1920;
            int HEIGHT = 1080;


            Shape[] shapes = new Shape[] {
                new Sphere(new Point(2.6, 0, 10), 2.5, new DefaultShader(new Color(0, 255, 0)), 0.5),
                new Sphere(new Point(-2.6, 0, 10), 2.5, new DefaultShader(new Color(0, 0, 255)), 0.5),
                //new Sphere(new Point(2.6, 0, 10), 2.5, new SphereTextureShader(test))
                //new Sphere(new Point(2, 0, 10), 2.5, new DefaultShader(new Color(0, 0, 255)), 0.1),
                //new Sphere(new Point(2.6, 0, 10), 2.5, new SphereTextureShader(test))
            };

            Light[] lights = new Light[]
            {
                //new PointLight(new Point(0, 0, 10), 1, new Color(255, 255, 255)),
                //new PointLight(new Point(0, -8, 10), 1, new Color(255, 0, 0))
            };

            Console.WriteLine("Shapes created");

            World world = new World(shapes, lights, new DefaultShader(new Color(255, 255, 255)));

            Console.WriteLine("World created");

            Camera cam = new Camera(new Point(0, 0, 0), world, WIDTH, HEIGHT, 10, 0, 0, 0, 100);

            Console.WriteLine("Camera created");
            int start = DateTime.UtcNow.Millisecond;
            Color[] result = cam.Render();
            int end = DateTime.UtcNow.Millisecond;
            Console.WriteLine("Scene rendered in " + (end-start) + "ms");

            System.Drawing.Bitmap rawr = new System.Drawing.Bitmap(WIDTH+1, HEIGHT+1);
            
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    rawr.SetPixel(x, HEIGHT - y, result[x * HEIGHT + y].toSystem());
                }
            }


            rawr.Save("result.png");
            Console.ReadKey();
        }
    }
}
