using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public class TerrainObject: GameObject
    {

        public TerrainObject(string Name, Model model)
        {
            this.Name = Name;
            Model = model;
            Initialize();
        }

        protected override void Initialize()
        {
            
        }

        public override void Update()
        {
            if (!IsStatic)
                CreateTransformationMatrix();
        }

        public static IGameObject CreateTestObject(ContentManager contentManager)
        {
            var cube = new TerrainObject("Cube", contentManager.Load<Model>("MonoCube"));
            cube.IsEnabled = true;
            cube.IsStatic = true;
            cube.Type = "TerrainObject";
            cube.Position = new Vector3(0, 0, 0);
            cube.Rotation = new Vector3(0, 0, 0);
            cube.Scale = new Vector3(1, 1, 1);
            cube.CreateTransformationMatrix();
            return cube;
        }
    }
}
