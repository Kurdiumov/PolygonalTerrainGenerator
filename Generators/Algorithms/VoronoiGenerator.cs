using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class VoronoiGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private int MapSize = 1024;
        public VoronoiGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var v1 = new VertexPosition(new Vector3(0, 0, 0));
            var v2 = new VertexPosition(new Vector3(0, 0, MapSize));
            var v3 = new VertexPosition(new Vector3(MapSize, 0, MapSize));
            var v4 = new VertexPosition(new Vector3(MapSize, 0, 0));
            var v5 = new VertexPosition(new Vector3(MapSize, 0, 0));
            var v6 = new VertexPosition(new Vector3(MapSize / 2, 0, MapSize / -2));
            
            return new GameObjects.Hexagon(_graphicDevice, _graphicDeviceManeger, v1, v2, v3, v4, v5, v6);
        }
    }
}

