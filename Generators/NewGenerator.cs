using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class NewGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        public NewGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var v1 = new VertexPosition(new Vector3(0, 0, 0));
            var v2 = new VertexPosition(new Vector3(0, 0, 10));
            var v3 = new VertexPosition(new Vector3(5, 0, 15));
            var v4 = new VertexPosition(new Vector3(10, 0, 10));
            var v5 = new VertexPosition(new Vector3(10, 0, 0));
            var v6 = new VertexPosition(new Vector3(5, 0, -5));
            
            
            return new GameObjects.Rectangle(_graphicDevice, _graphicDeviceManeger, v1, v2, v3, v4/*, v5, v6*/);
        }
    }
}

