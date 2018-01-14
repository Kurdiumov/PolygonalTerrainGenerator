using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class PerlinNoiseGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public PerlinNoiseGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            var gridSize = 256;
            var arr = GenerateVertices(gridSize);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, gridSize, offsetX * gridSize/4, offsetY * gridSize / 4);
        }



        private float[][] GenerateVertices(int gridSize)
        {
            var randomeNoise = NoiseGenerator.GenerateRandom(gridSize, 25);
            var perlinNoise = NoiseGenerator.GeneratePerlinNoise(randomeNoise, 7);

            return perlinNoise;
        }
    }
}

