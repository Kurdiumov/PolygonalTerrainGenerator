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
        private SpriteBatch _spriteBatch;
        private InputHelper _inputHelper;

        public App()
        {
            _app = this;
            Graphics = new GraphicsDeviceManager(this);

            _configurationManager = new ConfigurationManager();
            if (_configurationManager.IsFullScreen)
                Graphics.IsFullScreen = true;

            Graphics.PreferredBackBufferHeight = _configurationManager.HeightResolution;
            Graphics.PreferredBackBufferWidth = _configurationManager.WidthResolution;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Log.Debug("Initializing");
            base.Initialize();
            _scene = new Scene("Scene");

            Camera.CreateCamera(GraphicsDevice.Viewport.AspectRatio);
            Thread terrainGeneratorThread = new Thread(_generateTerrain);
            terrainGeneratorThread.Start();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = _configurationManager.MouseVisible;
            _inputHelper = new InputHelper(_configurationManager);

        }

        protected override void Update(GameTime gameTime)
        {
            _inputHelper.Update();
            _scene.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            _scene.Draw();
            base.Draw(gameTime);
        }

        public static App GetApp()
        {
            if (_app == null)
                throw new NullReferenceException("GetApp siege is not created");
            return _app;
        }

        private void _generateTerrain()
        {
            IGenerator generator = null;

            switch (_configurationManager.Algorithm)
            {
                case GeneratorAlgorithm.PerlinNoiseGenerator:
                    generator = new PerlinNoiseGenerator(GraphicsDevice, Graphics);
                    if (_configurationManager.SeaEnabled)
                        Scene.AddObjectToRender(new Sea(GraphicsDevice, Graphics, 14));
                    break;
                case GeneratorAlgorithm.HillGenerator:
                    generator = new HillAlgorithmGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.RandomGenerator:
                    generator = new RandomGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.RectangleGenerator:
                    generator = new RectangleGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.VoronoiGenerator:
                    generator = new VoronoiGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.TruePerlinNoiseGenerator:
                    generator = new TruePerlinNoiseGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.DiamondSquareGenerator:
                    if (_configurationManager.SeaEnabled)
                        Scene.AddObjectToRender(new Sea(GraphicsDevice, Graphics, 20.3f));
                    generator = new DiamondSquareGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlgorithm.RandomWalkGenerator:
                    if (_configurationManager.SeaEnabled)
                        Scene.AddObjectToRender(new Sea(GraphicsDevice, Graphics, 0.1f));

                    generator = new RandomWalkGenerator(GraphicsDevice, Graphics);
                    break;
                default:
                    throw new NotImplementedException("Unknown algorithm");
            }
            if (generator == null)
                throw new NullReferenceException("Generator cannot be null");

            var timeStart = System.DateTime.Now;
            Scene.AddObjectToRender(generator.Generate());
            Logger.Log.Info("Generated time = " + (DateTime.Now - timeStart).TotalMilliseconds + " ms for " + _configurationManager.Algorithm.ToString());

            /*
            for (int iteration = 1; iteration < 1; iteration++)
            {
                Thread.Sleep(1000);
                Scene.AddObjectToRender(generator.Generate(-iteration, -iteration));
                for (int x = iteration * -1; x < iteration; x++)
                {
                    Scene.AddObjectToRender(generator.Generate(x, iteration));
                    Scene.AddObjectToRender(generator.Generate(iteration, x));
                    Scene.AddObjectToRender(generator.Generate(x, -iteration));
                    Scene.AddObjectToRender(generator.Generate(-iteration, x));
                }

                Scene.AddObjectToRender(generator.Generate(iteration, iteration));

            }*/
        }
    }
}
