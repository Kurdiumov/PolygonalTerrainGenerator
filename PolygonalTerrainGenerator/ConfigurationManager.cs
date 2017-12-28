using Generators;

namespace Engine
{
    public class ConfigurationManager
    {
        public bool IsFullScreen;
        public bool FpsEnabled;
        public bool MouseVisible;
        public bool SeaEnabled;

        public int HeightResolution = 600;
        public int WidthResolution = 800;

        public ConfigurationManager()
        {
            IsFullScreen = true;
            HeightResolution = 768; //1080;
            WidthResolution = 1366; //1920;
            FpsEnabled = true;
            MouseVisible = false;
            SeaEnabled = false;
        }
    }
}
