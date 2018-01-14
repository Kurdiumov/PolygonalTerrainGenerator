using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class FractalGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public FractalGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            float size = 1024;
            var v1 = new VertexPosition(new Vector3(0, 0, 0));
            var v2 = new VertexPosition(new Vector3(0, 0, size));
            var v3 = new VertexPosition(new Vector3(size, 0, size));
            var v4 = new VertexPosition(new Vector3(size, 0, 0));
            
            return new GameObjects.Rectangle(_graphicDevice, _graphicDeviceManeger, v1, v2, v3, v4, offsetX*size, offsetY*size);
        }
    }
}

