using System;
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
        private int Flattening = 1;
        private float Frequency = 0.005f;
        private int Seed = 134245685;

        public PerlinNoiseGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            if (Parameters.ContainsKey("Height"))
                Height = (float)Parameters["Height"];

            if (Parameters.ContainsKey("GridSize"))
                GridSize = (int)Parameters["GridSize"];

            if (Parameters.ContainsKey("Seed"))
                Seed = (int)Parameters["Seed"];
            else
                Seed = new Random().Next(1000000000);

            if (Parameters.ContainsKey("Frequency"))
                Frequency = (float)Parameters["Frequency"];

            if (Parameters.ContainsKey("Flattening"))
                Flattening = (int)Parameters["Flattening"];

            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var arr = GenerateVertices(GridSize, Seed);
            arr = PostModifications.NormalizeAndFlatten(arr, GridSize, Flattening, Height);

            arr = Utils.ShiftTerrain(arr);

            HeightMapGenerator.Generate(_graphicDevice, arr, "Perlin noise");
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, GridSize);
        }

        private float[][] GenerateVertices(int gridSize, int seed)
        {
            var perlinNoise = Utils.GetEmptyArray(gridSize, gridSize);
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    perlinNoise[i][j] = Noises.Perlin(seed, i* Frequency, j * Frequency);
            return perlinNoise;
        }
    }
}

