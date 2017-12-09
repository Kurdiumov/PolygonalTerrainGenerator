using Microsoft.Xna.Framework;

namespace GameObjects
{
    public class RootGameObject: GameObject
    {
        public RootGameObject()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            Name = "RootGameObject";
            Type = "RootGameObject";
            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0,0,0);
            Scale = new Vector3(1,1,1);
            CreateTransformationMatrix();
        }

        public override void Update()
        {
            //Leave this empty
        }
    }
}
