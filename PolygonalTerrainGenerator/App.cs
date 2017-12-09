using System;
using Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class App : Game
    {
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
            var graphics = new GraphicsDeviceManager(this);

            _configurationManager = new ConfigurationManager();
            if (_configurationManager.IsFullScreen)
                graphics.IsFullScreen = true;

            graphics.PreferredBackBufferHeight = _configurationManager.HeightResolution;
            graphics.PreferredBackBufferWidth = _configurationManager.WidthResolution;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Log.Debug("Initializing");
            base.Initialize();
            _scene = new Scene("Level1")
            {
                RootGameObject = new RootGameObject()
            };
            Camera.CreateCamera();
            TerrainObject.CreateTestObject(_scene.RootGameObject as RootGameObject);

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
    }
}
