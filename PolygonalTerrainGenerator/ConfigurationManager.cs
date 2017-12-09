namespace Engine
{
    public class ConfigurationManager
    {
        public bool IsFullScreen;
        public bool FpsEnabled;
        public bool MouseVisible;

        public int HeightResolution = 600;
        public int WidthResolution = 800;

        public ConfigurationManager()
        {
            IsFullScreen = true;
            HeightResolution = 1080;
            WidthResolution = 1920;
            FpsEnabled = true;
            MouseVisible = true;
        }
    }
}
