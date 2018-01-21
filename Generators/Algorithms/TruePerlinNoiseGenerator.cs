using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class TruePerlinNoiseGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public TruePerlinNoiseGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            int size = 1024;
            var arr = Utils.GetEmptyArray(size, size);
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, size, offsetX, offsetY);
            
        }
        
    }
}

