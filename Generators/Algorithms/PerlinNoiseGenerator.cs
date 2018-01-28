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
        private readonly Dictionary<string, object> Parameters;

        private float Height = 100;
        private int MapSize = 1024;
        private int OctaveCount = 7;
        private int MaxRandomSize = 25;


        public PerlinNoiseGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            this.Parameters = Parameters;
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            if (Parameters.ContainsKey("GridSize"))
                MapSize = (int)Parameters["GridSize"];

            if (Parameters.ContainsKey("Height"))
                Height = (float)Parameters["Height"];

            var arr = GenerateVertices(MapSize);

            arr = PostModifications.NormalizeAndFlatten(arr, MapSize, 1, Height);

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, MapSize);
        }



        private float[][] GenerateVertices(int gridSize)
        {
            if (Parameters.ContainsKey("MaxRandomSize"))
                MaxRandomSize = (int)Parameters["MaxRandomSize"];

            if (Parameters.ContainsKey("OctaveCount"))
                OctaveCount = (int)Parameters["OctaveCount"];


            var randomeNoise = Noises.GenerateRandom(gridSize, MaxRandomSize);
            var perlinNoise = Noises.GeneratePerlinNoise(randomeNoise, OctaveCount);

            return perlinNoise;
        }
    }
}

