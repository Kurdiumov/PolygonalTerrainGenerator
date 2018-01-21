using System;
using System.Globalization;
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

        private readonly FpsCounter _frameCounter = new FpsCounter();
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

            _fpsEnabled = _configurationManager.FpsEnabled;

            IsMouseVisible = _configurationManager.MouseVisible;
            _inputHelper = new InputHelper(_configurationManager);

        }

        protected override void LoadContent()
        {
            _spriteFont = Content.Load<SpriteFont>("FPS");
        }

        protected override void UnloadContent()
        {
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


            if (_fpsEnabled)
            {
                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                var fps = $"FPS: {_frameCounter.AverageFramesPerSecond}";

                _spriteBatch.Begin();
                _spriteBatch.DrawString(_spriteFont, fps, new Vector2(10, 10), Color.Black);
                _spriteBatch.End();
            }

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

            switch (_configurationManager.Alghorithm)
            {
                case GeneratorAlghorithm.PerlinNoiseGenerator:
                    generator = new PerlinNoiseGenerator(GraphicsDevice, Graphics);
                    if (_configurationManager.SeaEnabled)
                        Scene.AddObjectToRender(new Sea(GraphicsDevice, Graphics, 14));
                    break;
                case GeneratorAlghorithm.HillGenerator:
                    generator = new HillAlgorithmGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlghorithm.RandomGenerator:
                    generator = new RandomGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlghorithm.RectangleGenerator:
                    generator = new RectangleGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlghorithm.VoronoiGenerator:
                    generator = new VoronoiGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlghorithm.TruePerlinNoiseGenerator:
                    generator = new TruePerlinNoiseGenerator(GraphicsDevice, Graphics);
                    break;
                case GeneratorAlghorithm.DiamondSquareGenerator:
                    generator = new DiamondSquareGenerator(GraphicsDevice, Graphics);
                    break;
                default:
                    throw new NotImplementedException("Unknown alghorithm");
            }
            if (generator == null)
                throw new NullReferenceException("Generator cannot be null");

            var timeStart = System.DateTime.Now;
            Scene.AddObjectToRender(generator.Generate());
            Logger.Log.Info("Generated time = " + (DateTime.Now - timeStart).TotalMilliseconds + " ms for " + _configurationManager.Alghorithm.ToString());

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
