using System;
using System.Xml.Serialization.Advanced;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class NewGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private int Size = 1024;

        public NewGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate()
        {
            var arr = Utils.GetEmptyArray(Size, Size, 0);
            arr[512][512] = 100;

            arr = PostModifications.Smoothify(arr, 1);
            return new GameObjects.PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, Size);
        }
    }
}

