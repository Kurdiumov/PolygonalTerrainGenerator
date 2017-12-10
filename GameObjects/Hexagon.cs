using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class Hexagon : PrimitiveBase
    {
        private VertexPosition v1, v2, v3, v4, v5, v6;

        public Hexagon(GraphicsDevice gd, GraphicsDeviceManager gdm, VertexPosition v1, VertexPosition v2, VertexPosition v3, VertexPosition v4, VertexPosition v5, VertexPosition v6) : base(gd, gdm)
        {

            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;

            GridSize = 3;
            var arr = new VertexPosition[6];

            arr[0] = v1;
            arr[1] = v6;
            arr[2] = v5;
            arr[3] = v2;

            arr[4] = v3;
            arr[5] = v4;
            GenerateVertices(arr);
        }
    }
}