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

        public IGameObject Generate()
        {
            var gridSize = 256;
            var arr = GenerateVertices(gridSize);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, gridSize);
        }

        private float[][] GenerateVertices(int gridSize)
        {
            var randomeNoise = NoiseGenerator.GenerateRandom(gridSize, 15);
            var perlinNoise = NoiseGenerator.GeneratePerlinNoise(randomeNoise, 7);

            return perlinNoise;
        }
    }
}

