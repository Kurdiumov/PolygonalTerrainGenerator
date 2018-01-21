using System;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RandomWalkGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;
        private readonly Random rand;

        public readonly int Size = 1024;
        public readonly float StepHeight = 0.7f;
        public readonly int MinSteps = 5000;
        public readonly int MaxSteps = 150000;
        public readonly int Walkers = 150;
        public readonly int Height = 50;

        public RandomWalkGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
            rand = new Random();
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            var arr = Utils.GetEmptyArray(Size, Size);

            for (int j = 0; j < Walkers; j++)
            {
                int PointX = rand.Next(0, 1024);
                int PointY = rand.Next(0, 1024);

                int Steps = rand.Next(MinSteps, MaxSteps);
                for (int i = 0; i < Steps; i++)
                {
                    Walk(arr, ref PointX, ref PointY);
                }
            }


            arr = PostModifications.Smooth(arr, 1);
            arr = PostModifications.Normalize(arr, Size, Height);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, Size, offsetX, offsetY);

        }

        public void Walk(float[][] arr, ref int x, ref int y)
        {
            bool tryAgain = true;
            while (tryAgain)
            {
                var direction = GetDirection();

                if (direction == 0)
                {
                    //UP
                    if (x <= 1)
                        continue;

                    x = x - 1;
                }
                else if (direction == 1)
                {
                    //RIGHT
                    if (y >= Size - 2)
                        continue;
                    y = y + 1;
                }
                else if (direction == 2)
                {
                    //DOWN
                    if (x >= Size - 2)
                        continue;
                    x = x + 1;

                }
                else if (direction == 3)
                {
                    //LEFT
                    if (y <= 1)
                        continue;
                    y = y - 1;
                }
                tryAgain = false;
            }

            arr[x][y] += StepHeight;
        }

        public int GetDirection()
        {
            return rand.Next(0, 4); 
        }
    }
}
