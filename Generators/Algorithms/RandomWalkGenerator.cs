﻿using System;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RandomWalkGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public readonly int Size = 1024;
        public readonly int StepHeight = 1;

        public readonly int MinSteps = 5500;
        public readonly int MaxSteps = 15000;
        public readonly Random rand;
        public readonly int Walkers = 1000;
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
            var direction = GetDirection();

            if (direction == 0)
            {
                //UP
                if (x != 0)
                    x = x - 1;
            }
            else if (direction == 1)
            {
                //RIGHT
                if (y != Size - 1)
                    y = y + 1;
            }
            else if (direction == 2)
            {
                //DOWN
                if (x != Size - 1)
                    x = x + 1;

            }
            else if (direction == 3)
            {
                //LEFT
                if (y != 0)
                    y = y - 1;
            }
            arr[x][y] += StepHeight;
        }

        public int GetDirection()
        {
            return rand.Next(0, 4); //0 = UP //1 = Right // // 2 = DOWN  3 = Left
        }
    }
}