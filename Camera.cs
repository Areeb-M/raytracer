using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class Camera
    {
        public Point position;

        public World world;

        public int screenWidth, screenHeight;

        public double worldSize;

        public double yaw, pitch, roll;

        public double FOV;

        public double focus, aspectRatioX, aspectRatioY;

        public Camera(Point position, World world, int screenWidth, int screenHeight, double worldSize=10.0, double yaw=0.0, double pitch=0.0, double roll=0.0, double FOV=90.0)
        {
            this.position = position;
            this.world = world;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.worldSize = worldSize;
            this.yaw = yaw;
            this.pitch = pitch;
            this.roll = roll;
            this.FOV = FOV;

            this.focus = worldSize / 2.0 * Math.Tan(Angle.DegreesToRadians(FOV / 2));
            this.aspectRatioX = 1 / ((double)screenWidth / worldSize);
            this.aspectRatioY = 1 / ((double)screenHeight / (worldSize * ((double)screenHeight / screenWidth)));
        }

        public Color[] Render()
        {
            Color[] result = new Color[screenWidth * screenHeight];

            for (int x = 0; x < screenWidth; x++)
            {
                for (int y = 0; y < screenHeight; y++)
                {
                    Point end = new Point(
                            (x - screenWidth/2) * aspectRatioX, (y - screenHeight/2) * aspectRatioY, focus
                        );
                    Ray ray = new Ray(position, end);
                    result[x * screenHeight + y] = world.calculateColor(ray);
                }
            }

            return result;
        }

    }
}
