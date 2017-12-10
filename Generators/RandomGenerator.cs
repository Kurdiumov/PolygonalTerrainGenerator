﻿using GameObjects;
using Microsoft.Xna.Framework;
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

            return new Primitive(_graphicDevice, _graphicDeviceManeger, arr, gridSize);
        }

        private float[][] GenerateVertices(int gridSize)
        {
            return NoiseGenerator.GenerateRandom(gridSize, 3);
        }
    }
}
