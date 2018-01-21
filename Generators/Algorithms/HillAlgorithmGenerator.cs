using System;
using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class HillAlgorithmGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;
        private readonly System.Random Rand = new Random();

        public int Iterations = 10000;
        public int RadiusMin = 10;
        public int RadiusMax = 40;
        public int GridSize = 1024;
        public int Flattening = 8;
        public float Height = 35;

        public HillAlgorithmGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            var arr = GenerateVertices(GridSize);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, GridSize, offsetX * GridSize / 4, offsetY * GridSize / 4);
        }


        private float[][] GenerateVertices(int gridSize)
        {
            var arr = Utils.GetEmptyArray(gridSize, gridSize);


            for (var i = 0; i < Iterations; i++)
            {
                var centerX = Rand.Next(0, GridSize);
                var centerY = Rand.Next(0, GridSize);
                float radius = (float)(Rand.Next(RadiusMin * 100, RadiusMax * 100)) / 100;

                arr = RaiseHill(arr, radius, centerX, centerY);
            }

            arr = PostModifications.NormalizeAndFlatten(arr, GridSize, Flattening, Height);
            return arr;
        }

        private float[][] RaiseHill(float[][] arr, float radius, int centerX, int centerY)
        {
            var xMin = (int)(centerX - radius - 1);
            var xMax = (int)(centerX + radius + 1);
            if (xMin < 0) xMin = 0;
            if (xMax >= GridSize) xMax = GridSize - 1;

            var yMin = (int)(centerY - radius - 1.0f);
            var yMax = (int)(centerY + radius + 1);
            if (yMin < 0) yMin = 0;
            if (yMax >= GridSize) yMax = GridSize - 1;


            for (var x = xMin; x <= xMax; ++x)
            {
                for (var y = yMin; y <= yMax; ++y)
                {
                    // z = r^2 - ((x2-x1)^2 + (y2-y1)^2)
                    float distanceSquare = (centerX - x) * (centerX - x) + (centerY - y) * (centerY - y);
                    var height = Utils.Square(radius) - distanceSquare;

                    if (height > 0 && arr[y][x] < (height))
                        arr[y][x] = height;
                }
            }

            return arr;
        }
    }
}

