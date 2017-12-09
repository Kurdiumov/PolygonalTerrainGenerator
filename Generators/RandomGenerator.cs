using System;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RandomGenerator: IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public RandomGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var gridSize = 128;
            var arr = GenerateVertices(gridSize);

            return new DiamondSquareObject(_graphicDevice, _graphicDeviceManeger, arr, gridSize);
        }

        private float[][] GenerateVertices(int gridSize)
        {
            var random = new Random();
            var arr = new float[gridSize][];
            for (int i = 0; i < gridSize; i++)
            {
                arr[i] = new float[gridSize];
                for (int j = 0; j < gridSize; j++)
                    arr[i][j] = random.Next(3);
            }
            return arr;
        }
    }
}
