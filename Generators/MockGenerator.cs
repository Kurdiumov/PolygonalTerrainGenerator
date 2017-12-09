using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Generators
{
    public class MockGenerator: IGenerator
    {
        private readonly ContentManager _contentManager;

        public MockGenerator(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public IGameObject Generate()
        {
            return TerrainObject.CreateTestObject( _contentManager);
        }
    }
}
