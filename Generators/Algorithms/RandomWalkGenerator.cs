using System;
using System.Collections.Generic;
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

        public readonly int MapSize = 1024;
        public readonly float StepHeight = 0.7f;
        public readonly int MinSteps = 5000;
        public readonly int MaxSteps = 150000;
        public readonly int Walkers = 150;
        public readonly int Height = 50;

        public RandomWalkGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            if (Parameters.ContainsKey("MapSize"))
                MapSize = (int)Parameters["MapSize"];

            if (Parameters.ContainsKey("StepHeight"))
                StepHeight = (float)Parameters["StepHeight"];

            if (Parameters.ContainsKey("MinSteps"))
                MinSteps = (int)Parameters["MinSteps"];

            if (Parameters.ContainsKey("MaxSteps"))
                MaxSteps = (int)Parameters["MaxSteps"];

            if (Parameters.ContainsKey("Walkers"))
                Walkers = (int)Parameters["Walkers"];

            if (Parameters.ContainsKey("Height"))
                Height = (int)Parameters["Height"];

            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
            rand = new Random();
        }

        public IGameObject Generate()
        {
            var arr = Utils.GetEmptyArray(MapSize, MapSize);

            //for(int l = 0; l < 5; l++)
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
            arr = PostModifications.Normalize(arr, MapSize, Height);
            arr = Utils.ShiftTerrain(arr);
            HeightMapGenerator.Generate(_graphicDevice, arr, "Random walk");
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, MapSize);

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
                    if (y >= MapSize - 2)
                        continue;
                    y = y + 1;
                }
                else if (direction == 2)
                {
                    //DOWN
                    if (x >= MapSize - 2)
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
            /*
                        if (level > 0)
                        {
                            if (arr[x][y] != 0 && arr[x][y] != level*StepHeight)
                                arr[x][y] += StepHeight;
                        }
                        else*/
            arr[x][y] += StepHeight;
            arr[x + 1][y + 1] += StepHeight;

        }

        public int GetDirection()
        {
            return rand.Next(0, 4);
        }
    }
}
