using System;
using GameObjects;

namespace Engine
{
    public class Scene
    {
        public string Name;
        public GameObject RootGameObject = null;

        private static Scene _currenScene;

        public Scene(string name)
        {
            Name = name;
            _currenScene = this;
        }

        public static Scene GetCurrentScene()
        {
            if(_currenScene == null)
                throw new TypeInitializationException("_currentLevel was not initialized", null);
            return _currenScene;
        }

        public void Update()
        {
            Camera.GetCurrentCamera().Update();
            
            foreach (var child  in RootGameObject.Childs)
                child.Update();
        }

        public void Draw()
        {
            Camera.GetCurrentCamera().Draw();

            foreach (var child in RootGameObject.Childs)
                child.Draw();
        }
    }
}
