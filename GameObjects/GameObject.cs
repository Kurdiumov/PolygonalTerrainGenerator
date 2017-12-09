using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public abstract class GameObject : IGameObject
    {
        public string Name = "GameObject";
        public string Type;

        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Rotation = new Vector3(0, 0, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);

        public bool IsStatic = false;
        public bool IsEnabled = true;

        public Model Model;

        public Matrix TransformationMatrix;

        protected abstract void Initialize();
        public abstract void Update();

        public virtual void Draw()
        {
            if (!IsEnabled)
                return;

            if (Model != null)
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (var effect1 in mesh.Effects)
                    {
                        var effect = (BasicEffect)effect1;
                        //effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.View = Camera.GetCamera().ViewMatrix;

                        effect.World = TransformationMatrix;
                        effect.Projection = Camera.GetCamera().ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        public override string ToString()
        {
            return "Name: " + this.Name + ", Type:" + this.Type;
        }

        public Matrix CreateTransformationMatrix()
        {
            TransformationMatrix = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                Matrix.CreateTranslation(Position);

            return TransformationMatrix;
        }
    }
}
