using System;
using System.Threading;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Generators;

namespace Engine
{
    public class App : Game
    {
        public GraphicsDeviceManager Graphics;

        private static Scene _scene;
        private static App _app;

        private readonly ConfigurationManager _configurationManager;
        private bool _fpsEnabled;
        private SpriteFont _spriteFont;
        private InputHelper _inputHelper;

        public App()
        {
            try
            {
                _app = this;
                Graphics = new GraphicsDeviceManager(this);

                _configurationManager = new ConfigurationManager();
                if (_configurationManager.IsFullScreen)
                    Graphics.IsFullScreen = true;

                Graphics.PreferredBackBufferHeight = _configurationManager.HeightResolution;
                Graphics.PreferredBackBufferWidth = _configurationManager.WidthResolution;
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
                throw;
            }
        }

        protected override void Initialize()
        {
            try
            {
                Logger.Log.Debug("Initializing");
                base.Initialize();
                _scene = new Scene("Scene");

                Camera.CreateCamera(GraphicsDevice.Viewport.AspectRatio);
                Thread terrainGeneratorThread = new Thread(_generateTerrain);
                terrainGeneratorThread.Start();

                IsMouseVisible = _configurationManager.MouseVisible;
                _inputHelper = new InputHelper(_configurationManager);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
                throw;
            }

        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                _inputHelper.Update();
                _scene.Update();
                base.Update(gameTime);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
                throw;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.White);

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                _scene.Draw();
                base.Draw(gameTime);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
                throw;
            }
        }

        public static App GetApp()
        {
            if (_app == null)
                throw new NullReferenceException("GetApp siege is not created");
            return _app;
        }

        private void _generateTerrain()
        {
            try
            {
                IGenerator generator = null;

                switch (_configurationManager.Algorithm)
                {
                    case GeneratorAlgorithm.PerlinNoise:
                        generator = new PerlinNoiseGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.Hill:
                        generator = new HillAlgorithmGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.Random:
                        generator = new RandomGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.Rectangle:
                        generator = new RectangleGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.Voronoi:
                        generator = new VoronoiGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.SimplexNoise:
                        generator = new SimplexNoise(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.DiamondSquare:
                        generator = new DiamondSquareGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.RandomWalk:
                        generator = new RandomWalkGenerator(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    case GeneratorAlgorithm.DrunkardWalk:
                        generator = new DrunkardWalk(GraphicsDevice, Graphics, _configurationManager.Parameters);
                        break;
                    default:
                        throw new NotImplementedException("Unknown algorithm");
                }
                if (generator == null)
                    throw new NullReferenceException("Generator cannot be null");

                var timeStart = System.DateTime.Now;
                Scene.AddObjectToRender(generator.Generate());
                Logger.Log.Info("Generated time = " + (DateTime.Now - timeStart).TotalMilliseconds + " ms for " + _configurationManager.Algorithm.ToString());


            }
            catch (Exception e)
            {
                Logger.Log.Error(e);
                throw;
            }
        }
    }
}
