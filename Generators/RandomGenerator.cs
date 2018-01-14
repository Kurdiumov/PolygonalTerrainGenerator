using GameObjects;
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

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            var gridSize = 256;
            var arr = GenerateVertices(gridSize);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, gridSize, offsetX * gridSize / 4, offsetY * gridSize / 4);
        }

        private float[][] GenerateVertices(int gridSize)
        {
            return NoiseGenerator.GenerateRandom(gridSize, 3);
        }
    }
}
