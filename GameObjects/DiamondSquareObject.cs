using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class DiamondSquareObject : IGameObject
    {
        private readonly int _gridSize;


        private readonly GraphicsDevice _graphicDevice;
        private readonly BasicEffect _basicEffect;

        private VertexPositionNormalTexture[] _verts;
        private int[] _indices;

        public DiamondSquareObject(GraphicsDevice gd, GraphicsDeviceManager gdm, float[][] inputVertices, int gridSize )
        {
            _graphicDevice = gd;

            _gridSize = gridSize;
            inputVertices = SetCorners(inputVertices, gridSize);
            GenerateTerrain(inputVertices);

            _basicEffect = new BasicEffect(gdm.GraphicsDevice)
            {
                LightingEnabled = true,
                PreferPerPixelLighting = true
            };
            _basicEffect.DirectionalLight0.Direction = new Vector3(0.0f, -1.0f, -1.0f);
            _basicEffect.DirectionalLight0.DiffuseColor = Color.OliveDrab.ToVector3();
        }

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

        private void GenerateTerrain(float[][] _inputVertices)
        {
            _verts = new VertexPositionNormalTexture[_gridSize * _gridSize];
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    _verts[x + y * _gridSize] = new VertexPositionNormalTexture(
                        new Vector3((x - (_gridSize / 2)) * 0.25f,
                            (_inputVertices[x][y] / 5) * 1.0f,
                            (y - (_gridSize / 2)) * 0.25f),
                        Vector3.Zero, Vector2.Zero);
                }
            }

            int ctr = 0;
            _indices = new int[(_gridSize - 1) * (_gridSize - 1) * 6];
            for (int y = 0; y < _gridSize - 1; y++)
            {
                for (int x = 0; x < _gridSize - 1; x++)
                {
                    int tl = x + (y) * _gridSize;
                    int tr = (x + 1) + (y) * _gridSize;
                    int bl = x + (y + 1) * _gridSize;
                    int br = (x + 1) + (y + 1) * _gridSize;

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
