using System;
using ProceduralTerrainGenerator.GameObjects;


namespace ProceduralTerrainGenerator
{
    public class Level
    {
        public string Name;
        public GameObject RootGameObject = null;

        private static Level _currenLevel;

        public Level(string Name)
        {
            this.Name = Name;
            _currenLevel = this;
        }

        public static Level GetCurrentLevel()
        {
            if(_currenLevel == null)
                throw new TypeInitializationException("_currentLevel was not initialized", null);
            return _currenLevel;
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
