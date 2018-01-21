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
            if (_currenScene == null)
                throw new TypeInitializationException("_currentLevel was not initialized", null);
            return _currenScene;
        }

        public void Update()
        {
            try
            {
                Camera.GetCamera().Update();

                foreach (var obj in _objectsToRender)
                    if (obj != null)
                        obj.Update();
            }
            catch (Exception e)
            {
                Logger.Log.Warn("Unchandled expeption in Scene.Update:" + e);
            }
        }

        public void Draw()
        {
            try
            {
                Camera.GetCamera().Draw();

                foreach (var obj in _objectsToRender)
                    if (obj != null)
                        obj.Draw();
            }
            catch (Exception e)
            {
                Logger.Log.Warn("Unchandled expeption in Scene.Draw:" + e);
            }
        }

        public static void AddObjectToRender(IGameObject obj)
        {
            if (obj is PrimitiveBase && (obj as PrimitiveBase).BasicEffect == null)
            {
                Logger.Log.Fatal("Basic effect is null");
             //   throw new NullReferenceException("Basic effect == null!"); 
            }

            _objectsToRender.Add(obj);
            
        }
    }
}
