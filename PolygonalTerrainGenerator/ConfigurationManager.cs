using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json.Linq;

namespace Engine
{
    public class ConfigurationManager
    {
        public readonly string PathToConfigFile = @"Configuration.json";
        private JObject Configuration;
        public readonly Dictionary<string, object> Parameters = new Dictionary<string, object>();

        public bool IsFullScreen;
        public bool MouseVisible;

        public int HeightResolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        public int WidthResolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public GeneratorAlgorithm Algorithm;

        public ConfigurationManager()
        {
            ReadParameters();
            ReadSettings();
            ReadAlgorithmsParameters();
        }

        public void ReadParameters()
        {
            Logger.Log.Debug("Parsing configuration from " + PathToConfigFile + " file");

            if (!File.Exists(PathToConfigFile))
                throw new FileNotFoundException("Configuration File cannot be found!");

            Configuration = JObject.Parse(File.ReadAllText(PathToConfigFile)).GetValue("config").ToObject<JObject>();

            if (Configuration == null)
                throw new ArgumentNullException(PathToConfigFile + " does not contain configuration key");
        }

        public void ReadSettings()
        {
            var settings = Configuration.GetValue("Settings").ToObject<JObject>();

            if (ValueExist("FullScreen", settings))
                IsFullScreen = ToBool(GetValue("FullScreen", settings));

            if (ValueExist("HeightResolution", settings))
                HeightResolution = ToInt(GetValue("HeightResolution", settings));

            if (ValueExist("WidthResolution", settings))
                WidthResolution = ToInt(GetValue("WidthResolution", settings));

            if (ValueExist("MouseVisible", settings))
                MouseVisible = ToBool(GetValue("MouseVisible", settings));

            if (ValueExist("AlgorithmType", settings))
                Algorithm = ParseAlgorithm(GetValue("AlgorithmType", settings));
            else
                throw new Exception("Algorithm cannot be empty!");
        }

        public void ReadAlgorithmsParameters()
        {
            var AlgorithmsParameters = Configuration.GetValue("AlgorithmsParameters").ToObject<JObject>();
            AlgorithmsParameters = AlgorithmsParameters.GetValue(Algorithm.ToString()).ToObject<JObject>();

            switch (Algorithm)
            {
                case GeneratorAlgorithm.PerlinNoise:

                    if (ValueExist("GridSize", AlgorithmsParameters))
                        Parameters.Add("GridSize", ToInt(GetValue("GridSize", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));

                    if (ValueExist("Frequency", AlgorithmsParameters))
                        Parameters.Add("Frequency", ToFloat(GetValue("Frequency", AlgorithmsParameters)));

                    if (ValueExist("Seed", AlgorithmsParameters))
                        Parameters.Add("Seed", ToInt(GetValue("Seed", AlgorithmsParameters)));

                    if (ValueExist("Flattening", AlgorithmsParameters))
                        Parameters.Add("Flattening", ToInt(GetValue("Flattening", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.SimplexNoise:

                    if (ValueExist("GridSize", AlgorithmsParameters))
                        Parameters.Add("GridSize", ToInt(GetValue("GridSize", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));

                    if (ValueExist("Flattening", AlgorithmsParameters))
                        Parameters.Add("Flattening", ToInt(GetValue("Flattening", AlgorithmsParameters)));

                    if (ValueExist("Frequency", AlgorithmsParameters))
                        Parameters.Add("Frequency", ToFloat(GetValue("Frequency", AlgorithmsParameters)));

                    if (ValueExist("Seed", AlgorithmsParameters))
                        Parameters.Add("Seed", ToInt(GetValue("Seed", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.DiamondSquare:
                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));

                    if (ValueExist("Displacement", AlgorithmsParameters))
                        Parameters.Add("Displacement", ToFloat(GetValue("Displacement", AlgorithmsParameters)));

                    if (ValueExist("Iterations", AlgorithmsParameters))
                        Parameters.Add("Iterations", ToInt(GetValue("Iterations", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.DrunkardWalk:
                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("MinSteps", AlgorithmsParameters))
                        Parameters.Add("MinSteps", ToInt(GetValue("MinSteps", AlgorithmsParameters)));

                    if (ValueExist("MaxSteps", AlgorithmsParameters))
                        Parameters.Add("MaxSteps", ToInt(GetValue("MaxSteps", AlgorithmsParameters)));

                    if (ValueExist("Walkers", AlgorithmsParameters))
                        Parameters.Add("Walkers", ToInt(GetValue("Walkers", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.Hill:
                    if (ValueExist("Iterations", AlgorithmsParameters))
                        Parameters.Add("Iterations", ToInt(GetValue("Iterations", AlgorithmsParameters)));

                    if (ValueExist("RadiusMin", AlgorithmsParameters))
                        Parameters.Add("RadiusMin", ToInt(GetValue("RadiusMin", AlgorithmsParameters)));

                    if (ValueExist("RadiusMax", AlgorithmsParameters))
                        Parameters.Add("RadiusMax", ToInt(GetValue("RadiusMax", AlgorithmsParameters)));

                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("Flattening", AlgorithmsParameters))
                        Parameters.Add("Flattening", ToInt(GetValue("Flattening", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));
                    break;
                case GeneratorAlgorithm.Random:
                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("MaxHeight", AlgorithmsParameters))
                        Parameters.Add("MaxHeight", ToInt(GetValue("MaxHeight", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.RandomWalk:
                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("MinSteps", AlgorithmsParameters))
                        Parameters.Add("MinSteps", ToInt(GetValue("MinSteps", AlgorithmsParameters)));

                    if (ValueExist("MaxSteps", AlgorithmsParameters))
                        Parameters.Add("MaxSteps", ToInt(GetValue("MaxSteps", AlgorithmsParameters)));

                    if (ValueExist("Walkers", AlgorithmsParameters))
                        Parameters.Add("Walkers", ToInt(GetValue("Walkers", AlgorithmsParameters)));

                    if (ValueExist("StepHeight", AlgorithmsParameters))
                        Parameters.Add("StepHeight", ToFloat(GetValue("StepHeight", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToInt(GetValue("Height", AlgorithmsParameters)));

                    break;
                case GeneratorAlgorithm.Rectangle:
                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("GenStep", AlgorithmsParameters))
                        Parameters.Add("GenStep", ToInt(GetValue("GenStep", AlgorithmsParameters)));

                    if (ValueExist("Zscale", AlgorithmsParameters))
                        Parameters.Add("Zscale", ToFloat(GetValue("Zscale", AlgorithmsParameters)));

                    if (ValueExist("Density", AlgorithmsParameters))
                        Parameters.Add("Density", ToInt(GetValue("Density", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));

                    if (ValueExist("Smoothness", AlgorithmsParameters))
                        Parameters.Add("Smoothness", ToFloat(GetValue("Smoothness", AlgorithmsParameters)));
                    break;
                case GeneratorAlgorithm.Voronoi:
                    if (ValueExist("MapSize", AlgorithmsParameters))
                        Parameters.Add("MapSize", ToInt(GetValue("MapSize", AlgorithmsParameters)));

                    if (ValueExist("Jitter", AlgorithmsParameters))
                        Parameters.Add("Jitter", ToFloat(GetValue("Jitter", AlgorithmsParameters)));

                    if (ValueExist("Frequency", AlgorithmsParameters))
                        Parameters.Add("Frequency", ToFloat(GetValue("Frequency", AlgorithmsParameters)));

                    if (ValueExist("Height", AlgorithmsParameters))
                        Parameters.Add("Height", ToFloat(GetValue("Height", AlgorithmsParameters)));

                    if (ValueExist("Seed", AlgorithmsParameters))
                        Parameters.Add("Seed", ToInt(GetValue("Seed", AlgorithmsParameters)));

                    if (ValueExist("NoiseType", AlgorithmsParameters))
                        Parameters.Add("NoiseType", GetValue("NoiseType", AlgorithmsParameters));

                    break;
                default: throw new Exception("Unknown algorithm");
            }
        }

        private bool ToBool(string value)
        {
            if (value.ToLower() == "true")
                return true;
            return false;
        }


        private int ToInt(string value)
        {
            return Int32.Parse(value);
        }


        private float ToFloat(string value)
        {
            return float.Parse(value);
        }


        private string GetValue(string value, JObject obj)
        {
            if (ValueExist(value, obj))
                return obj.GetValue(value).ToString();
            throw new ArgumentException("Value " + value + " does not exist in current context. " + obj.ToString());
        }

        private bool ValueExist(string value, JObject obj)
        {
            try
            {
                var exists = !String.IsNullOrEmpty(obj.GetValue(value).ToString());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        private GeneratorAlgorithm ParseAlgorithm(string algorithm)
        {
            if (String.IsNullOrWhiteSpace(algorithm))
                throw new Exception("Algorithm cannot be empty!");
            switch (algorithm.ToLower())
            {
                case "diamondsquare": return GeneratorAlgorithm.DiamondSquare;
                case "hill": return GeneratorAlgorithm.Hill;
                case "drunkardwalk": return GeneratorAlgorithm.DrunkardWalk;
                case "simplexnoise": return GeneratorAlgorithm.SimplexNoise;
                case "perlinnoise": return GeneratorAlgorithm.PerlinNoise;
                case "random": return GeneratorAlgorithm.Random;
                case "randomwalk": return GeneratorAlgorithm.RandomWalk;
                case "rectangle": return GeneratorAlgorithm.Rectangle;
                case "voronoi": return GeneratorAlgorithm.Voronoi;
                default: throw new Exception("Unknown algorithm " + algorithm);
            }
        }
    }
}