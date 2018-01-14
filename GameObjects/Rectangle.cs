using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class Rectangle : PrimitiveBase
    {
        private VertexPosition v1, v2, v3, v4;

        public Rectangle(GraphicsDevice gd, GraphicsDeviceManager gdm) : base(gd, gdm)
        {

        }

        public Rectangle(GraphicsDevice gd, GraphicsDeviceManager gdm, VertexPosition v1, VertexPosition v2, VertexPosition v3, VertexPosition v4, float offsetX, float offsetY) : base(gd, gdm)
        {
            v1.Position = new Vector3(v1.Position.X + offsetX, v1.Position.Y, v1.Position.Z + offsetY);
            v2.Position = new Vector3(v2.Position.X + offsetX, v2.Position.Y, v2.Position.Z + offsetY);
            v3.Position = new Vector3(v3.Position.X + offsetX, v3.Position.Y, v3.Position.Z + offsetY);
            v4.Position = new Vector3(v4.Position.X + offsetX, v4.Position.Y, v4.Position.Z + offsetY);


            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;

            GridSize = 2;
            var arr = new VertexPosition[4];

            arr[0] = v1;
            arr[1] = v4;
            arr[2] = v2;
            arr[3] = v3;

            GenerateVertices(arr);
        }
    }
}
