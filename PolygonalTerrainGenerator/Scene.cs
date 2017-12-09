using System;
using System.Collections.Generic;
using GameObjects;

namespace Engine
{
    public class Scene
    {
        public string Name;

        private static Scene _currenScene;
        private static readonly List<IGameObject> _objectsToRender = new List<IGameObject>();
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
            
            foreach (var obj  in _objectsToRender)
                if(obj != null)
                    obj.Update();
        }

        public void Draw()
        {
            Camera.GetCurrentCamera().Draw();

            foreach (var obj in _objectsToRender)
                if (obj != null)
                    obj.Draw();
        }

        public static void AddObjectToRender(IGameObject obj)
        {
            _objectsToRender.Add(obj);
        }
    }
}
