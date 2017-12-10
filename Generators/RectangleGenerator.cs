using System;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class RectangleGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private readonly int _mapsize = 256;
        private readonly float[][] _heightMap; 
        private readonly int _genStep = 1024;
        private readonly float _zscale = 512;
        private readonly int _density = 13;
        private readonly float _height = 40;
        private const float Smoothness = 15.0f;
        private readonly Random _rand = new Random();

        public RectangleGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
            _heightMap = NoiseGenerator.GetEmptyArray(_mapsize, _mapsize);
        }

        public IGameObject Generate()
        {
            for (var i = 0; i < _genStep; i++)
            {
                var x1 = _rand.Next() % _mapsize;
                var y1 = _rand.Next() % _mapsize;
                var x2 = x1 + _density / 4 + _rand.Next() % _density;
                var y2 = y1 + _density / 4 + _rand.Next() % _density;
                if (y2 > _mapsize) y2 = _mapsize;
                if (x2 > _mapsize) x2 = _mapsize;
                for (int i2 = x1; i2 < x2; i2++)
                for (int j2 = y1; j2 < y2; j2++)
                    _heightMap[i2][j2] = (_zscale / _genStep + _rand.Next() % _height) / Smoothness;
            }
            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, _heightMap, _mapsize);
        }
    }
}

