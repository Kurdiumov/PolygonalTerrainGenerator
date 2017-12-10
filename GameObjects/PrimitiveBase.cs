using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class PrimitiveBase : IGameObject
    {
        protected int GridSize;

        private readonly GraphicsDevice _graphicDevice;
        private readonly BasicEffect _basicEffect;

        private VertexPositionNormalTexture[] _verts;
        private int[] _indices;

        private float[][] SetCorners(float[][] inputVertices, int gridSize)
        {
            for (int i = 0; i < gridSize; i++)
            {
                inputVertices[0][i] = 0;
                inputVertices[gridSize - 1][i] = 0;
                inputVertices[i][0] = 0;
                inputVertices[i][gridSize - 1] = 0;
            }
            return inputVertices;
        }

        public PrimitiveBase(GraphicsDevice gd, GraphicsDeviceManager gdm)
        {
            _graphicDevice = gd;
            _basicEffect = new BasicEffect(gdm.GraphicsDevice)
            {
                LightingEnabled = true,
                PreferPerPixelLighting = true
            };
            _basicEffect.DirectionalLight0.Direction = new Vector3(0.0f, -1.0f, -1.0f);
            _basicEffect.DirectionalLight0.DiffuseColor = Color.OliveDrab.ToVector3();
        }

        public PrimitiveBase(GraphicsDevice gd, GraphicsDeviceManager gdm, float[][] inputVertices, int gridSize)
        {
            _graphicDevice = gd;

            GridSize = gridSize;
            inputVertices = SetCorners(inputVertices, gridSize);
            GenerateVertices(inputVertices);

            _basicEffect = new BasicEffect(gdm.GraphicsDevice)
            {
                LightingEnabled = true,
                PreferPerPixelLighting = true
            };
            _basicEffect.DirectionalLight0.Direction = new Vector3(0.0f, -1.0f, -1.0f);
            _basicEffect.DirectionalLight0.DiffuseColor = Color.OliveDrab.ToVector3();
        }

        protected void GenerateVertices(float[][] _inputVertices)
        {
            _verts = new VertexPositionNormalTexture[GridSize * GridSize];
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    _verts[x + y * GridSize] = new VertexPositionNormalTexture(
                        new Vector3((x - (GridSize / 2)) * 0.25f,
                            (_inputVertices[x][y] / 5) * 1.0f,
                            (y - (GridSize / 2)) * 0.25f),
                        Vector3.Zero, Vector2.Zero);
                }
            }
            GenerateIndices();
        }

        protected void GenerateVertices(VertexPosition[] inputVertices)
        {
            _verts = new VertexPositionNormalTexture[GridSize * GridSize];
            for(int i= 0; i < inputVertices.Length; i++)
            {
                _verts[i] = new VertexPositionNormalTexture(inputVertices[i].Position, Vector3.Up, Vector2.Zero);
            }

            GenerateIndices();
        }

        private void GenerateIndices()
        {
            int ctr = 0;
            _indices = new int[(GridSize - 1) * (GridSize - 1) * 6];
            for (int y = 0; y < GridSize - 1; y++)
            {
                for (int x = 0; x < GridSize - 1; x++)
                {
                    int tl = x + (y) * GridSize;
                    int tr = (x + 1) + (y) * GridSize;
                    int bl = x + (y + 1) * GridSize;
                    int br = (x + 1) + (y + 1) * GridSize;

                    _indices[ctr++] = tl;
                    _indices[ctr++] = br;
                    _indices[ctr++] = bl;

                    Vector3 leg0 = _verts[tl].Position - _verts[bl].Position;
                    Vector3 leg1 = _verts[tl].Position - _verts[br].Position;
                    Vector3 norm = Vector3.Cross(leg0, leg1);

                    _verts[tl].Normal += norm;
                    _verts[br].Normal += norm;
                    _verts[bl].Normal += norm;

                    _indices[ctr++] = tl;
                    _indices[ctr++] = tr;
                    _indices[ctr++] = br;

                    leg0 = _verts[tl].Position - _verts[br].Position;
                    leg1 = _verts[tl].Position - _verts[tr].Position;
                    norm = Vector3.Cross(leg0, leg1);

                    _verts[tl].Normal += norm;
                    _verts[tr].Normal += norm;
                    _verts[br].Normal += norm;
                }
            }

            foreach (VertexPositionNormalTexture v in _verts)
                v.Normal.Normalize();
        }

        public void Update()
        {
            _basicEffect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            _basicEffect.View = Camera.GetCamera().ViewMatrix;
            _basicEffect.Projection = Camera.GetCamera().ProjectionMatrix;
        }

        public void Draw()
        {
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _graphicDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _verts,
                    0,
                    _verts.Length,
                    _indices,
                    0,
                    _indices.Length / 3);
            }
        }
    }
}
