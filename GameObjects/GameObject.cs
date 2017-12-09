using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public abstract class GameObject
    {
        public string Name = "GameObject";
        public string Type;

        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Rotation = new Vector3(0, 0, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);

        public bool IsStatic = false;
        public bool IsEnabled = true;

        public Model Model;
        public GameObject Parent;
        public List<GameObject> Childs = new List<GameObject>();

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
                        var effect = (BasicEffect) effect1;
                        //effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.View = Camera.GetCurrentCamera().ViewMatrix;

                        effect.World = TransformationMatrix;
                        effect.Projection = Camera.GetCurrentCamera().ProjectionMatrix;
                    }
                    mesh.Draw();
                    foreach (var child in Childs)
                    {
                        child.Draw();
                    }
                }
            }
        }

        public void AddChild(GameObject obj)
        {
            Childs.Add(obj);
            obj.Parent = this;
        }

        public void RemoveChild(GameObject obj)
        {
            Childs.Remove(obj);

        }

        public bool RemoveChild(string name)
        {
            bool removed = false;
            for (int i = 0; i < Childs.Count; i++)
                if (Childs[i].Name == name)
                {
                    Childs.RemoveAt(i);
                    i--;

                    removed = true;
                }
            return removed;
        }

        public GameObject FindGameObjectInLevelByName(string name)
        {
            //TODO: Implement reccursive search
            foreach (var item in Childs)
                if (item.Name == name)
                    return item;
            return null;
        }

        public List<GameObject> FindGameObjectsInLevelByName(string name)
        {
            //TODO: Implement reccursive search
            List<GameObject> foundedItems = new List<GameObject>();

            foreach (var item in Childs)
                if (item.Name == name)
                    foundedItems.Add(item);


            if (foundedItems.Count != 0)
                return foundedItems;
            return null;
        }

        public override string ToString()
        {
            if (Parent == null)
                return "Name: " + this.Name + ", Type:" + this.Type;
            return "Name: " + this.Name + ", Type: " + this.Type + ", Parent: " + Parent.Name;
        }

        public  Matrix CreateTransformationMatrix()
        {
            TransformationMatrix = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                Matrix.CreateTranslation(Position);

            if (Parent != null)
                TransformationMatrix *= Parent.TransformationMatrix;

            return TransformationMatrix;
        }

        public List<GameObject> GetAllChilds(GameObject obj)
        {
            List<GameObject> childs = new List<GameObject>();
            foreach (var child in obj.Childs)
            {
                childs.Add(child);
                childs = childs.Concat(GetAllChilds(child)).ToList();
            }
            return childs;
        }
    }
}
