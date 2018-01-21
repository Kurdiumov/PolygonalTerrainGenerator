﻿namespace Engine
{
    public class ConfigurationManager
    {
        public bool IsFullScreen;
        public bool FpsEnabled;
        public bool MouseVisible;
        public bool SeaEnabled;

        public int HeightResolution = 1080;
        public int WidthResolution = 1920;
        public GeneratorAlgorithm Algorithm;

        public ConfigurationManager()
        {
            IsFullScreen = false;
            HeightResolution = 768;
            WidthResolution = 1366;
            FpsEnabled = true;
            MouseVisible = false;
            SeaEnabled = true;
            Algorithm = GeneratorAlgorithm.RandomWalkGenerator;
        }
    }
}
