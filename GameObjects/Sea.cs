using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class Sea : Rectangle
    {
        public readonly int Size = 32;
        public readonly float Level = 1;

        public Sea(GraphicsDevice gd, GraphicsDeviceManager gdm) : base(gd, gdm)
        {
            GridSize = 2;
            var arr = new VertexPosition[4];

            arr[0] = new VertexPosition(new Vector3(-Size, Level + 0.001f, -Size));
            arr[1] = new VertexPosition(new Vector3(Size, Level + 0.001f, -Size));
            arr[2] = new VertexPosition(new Vector3(-Size, Level + 0.001f, Size));
            arr[3] = new VertexPosition(new Vector3(Size, Level + 0.001f, Size));

            GenerateVertices(arr);


            BasicEffect.DirectionalLight0.DiffuseColor = Color.DarkCyan.ToVector3();
        }
    }
}
