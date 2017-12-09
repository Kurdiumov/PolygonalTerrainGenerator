using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.GameObjects
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

        public static void CreateTestObject(RootGameObject root)
        {
            var cube = new TerrainObject("Cube", App.GetApp().Content.Load<Model>("MonoCube"));
            cube.IsEnabled = true;
            cube.IsStatic = true;
            cube.Type = "TerrainObject";
            cube.Position = new Vector3(0, 0, 0);
            cube.Rotation = new Vector3(0, 0, 0);
            cube.Scale = new Vector3(1, 1, 1);
            cube.CreateTransformationMatrix();

            root.AddChild(cube);
        }
    }
}
