namespace ProceduralTerrainGenerator
{
    public class ConfigurationManager
    {
        public readonly string PathToConfigFile = "application.config";

        public bool GodModeEnabled;
        public  State GameState = State.FirstPerson;
        public bool IsFullScreen;
        public bool FPSEnabled;
        public bool MouseVisible;
        public bool PickingEnabled;

        public int HeightResolution = 600;
        public int WidthResolution = 800;

        public ConfigurationManager()
        {
            Logger.Log.Debug("Parsing configuration from " + PathToConfigFile + " file");

            SetProperties();
        }

        private void SetProperties()
        {
            GodModeEnabled = true;
            GameState = State.GodMode;
            IsFullScreen = true;
            HeightResolution = 1080;
            WidthResolution = 1920;
            FPSEnabled = true;
            MouseVisible = true;
            PickingEnabled = true;
        }
    }
}
