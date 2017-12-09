using GameObjects;
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

        public RootGameObject Generate()
        {
            var rootGameObject = new RootGameObject();
            TerrainObject.CreateTestObject(rootGameObject, _contentManager);


            return rootGameObject;
        }
    }
}
