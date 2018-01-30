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

        private float Height = 100;
        private int GridSize = 1024;
        private int OctaveCount = 7;
        private int MaxRandomSize = 25;
        private int Flattening = 1;

        public PerlinNoiseGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            if (Parameters.ContainsKey("Height"))
                Height = (float)Parameters["Height"];

            if (Parameters.ContainsKey("GridSize"))
                GridSize = (int)Parameters["GridSize"];

            if (Parameters.ContainsKey("OctaveCount"))
                OctaveCount = (int)Parameters["OctaveCount"];


            if (Parameters.ContainsKey("MaxRandomSize"))
                MaxRandomSize = (int)Parameters["MaxRandomSize"];

            if (Parameters.ContainsKey("Flattening"))
                Flattening = (int)Parameters["Flattening"];

            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var arr = GenerateVertices(GridSize);
            arr = PostModifications.NormalizeAndFlatten(arr, GridSize, Flattening, Height);

            arr = Utils.ShiftTerrain(arr);

            HeightMapGenerator.Generate(_graphicDevice, arr, "Perlin noise");
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, GridSize);
        }



        private float[][] GenerateVertices(int gridSize)
        {
            var randomeNoise = Noises.GenerateRandom(gridSize, MaxRandomSize);
            var perlinNoise = Noises.GeneratePerlinNoise(randomeNoise, OctaveCount);

            return perlinNoise;
        }
    }
}

