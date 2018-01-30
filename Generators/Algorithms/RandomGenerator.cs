using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RandomGenerator: IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;
        private readonly Dictionary<string, object> Parameters;

        private int MapSize = 1024;
        private int MaxHeight = 5;

        public RandomGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            this.Parameters = Parameters;
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            if (Parameters.ContainsKey("MapSize"))
                MapSize = (int)Parameters["MapSize"];
            var arr = GenerateVertices(MapSize);
            arr = Utils.ShiftTerrain(arr);
            HeightMapGenerator.Generate(_graphicDevice, arr, "Random");
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, MapSize);
        }

        private float[][] GenerateVertices(int gridSize)
        {
            if (Parameters.ContainsKey("MaxHeight"))
                MaxHeight = (int)Parameters["MaxHeight"];
            return Noises.GenerateRandom(gridSize, MaxHeight);
        }
    }
}
