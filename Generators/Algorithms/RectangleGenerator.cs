using System;
using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RectangleGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private int MapSize = 1024;
        private float[][] _heightMap; 
        private int GenStep = 1024;
        private float ZScale = 512;
        private int Density = 52;
        private float Height = 40;
        private float Smoothness = 15.0f;
        private readonly Random _rand = new Random();

        public RectangleGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics, Dictionary<string, object> Parameters)
        {
            if (Parameters.ContainsKey("MapSize"))
                MapSize = (int)Parameters["MapSize"];

            if (Parameters.ContainsKey("GenStep"))
                GenStep = (int)Parameters["GenStep"];

            if (Parameters.ContainsKey("Zscale"))
                ZScale = (float)Parameters["Zscale"];

            if (Parameters.ContainsKey("Density"))
                Density = (int)Parameters["Density"];

            if (Parameters.ContainsKey("Height"))
                Height = (float)Parameters["Height"];

            if (Parameters.ContainsKey("Smoothness"))
                Smoothness = (float)Parameters["Smoothness"];

            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
            _heightMap = Utils.GetEmptyArray(MapSize, MapSize);
        }

        public IGameObject Generate()
        {
            for (var i = 0; i < GenStep; i++)
            {
                var x1 = _rand.Next() % MapSize;
                var y1 = _rand.Next() % MapSize;
                var x2 = x1 + Density / 4 + _rand.Next() % Density;
                var y2 = y1 + Density / 4 + _rand.Next() % Density;
                if (y2 > MapSize) y2 = MapSize;
                if (x2 > MapSize) x2 = MapSize;
                for (int i2 = x1; i2 < x2; i2++)
                for (int j2 = y1; j2 < y2; j2++)
                    _heightMap[i2][j2] = (ZScale / GenStep + _rand.Next() % Height) / Smoothness;
            }
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, _heightMap, MapSize);
        }
    }
}

