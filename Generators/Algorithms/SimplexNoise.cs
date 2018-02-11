using System;
using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class SimplexNoise : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private int GridSize = 1024;
        private float Height = 100;
        private int Flattening = 2;
        private float Frequency = 0.005f;
        private int Seed = 134245685;

        public SimplexNoise(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            if (Parameters.ContainsKey("GridSize"))
                GridSize = (int)Parameters["GridSize"];

            if (Parameters.ContainsKey("Height"))
                Height = (float)Parameters["Height"];

            if (Parameters.ContainsKey("Flattening"))
                Flattening = (int)Parameters["Flattening"];

            if (Parameters.ContainsKey("Seed"))
                Seed = (int)Parameters["Seed"];
            else
                Seed = new Random().Next(1000000000);

            if (Parameters.ContainsKey("Frequency"))
                Frequency = (float)Parameters["Frequency"];


            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var arr = Generate(GridSize, Seed);
            arr = PostModifications.NormalizeAndFlatten(arr, GridSize, Flattening, Height);
            arr = Utils.ShiftTerrain(arr);
            HeightMapGenerator.Generate(_graphicDevice, arr, "Simplex Noise");
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, GridSize);
        }

        public float[][] Generate(int gridSize, int seed)
        {
            var perlinNoise = Utils.GetEmptyArray(gridSize, gridSize);
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                 perlinNoise[i][j] = Noises.Simplex(seed, i * Frequency, j * Frequency);
            return perlinNoise;
        }
    }
}


