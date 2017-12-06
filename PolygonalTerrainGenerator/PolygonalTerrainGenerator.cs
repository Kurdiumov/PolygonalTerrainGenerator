using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProceduralTerrainGenerator.GameObjects;

namespace ProceduralTerrainGenerator
{
    public class ProceduralTerrainGenerator : Game
    {
        public static GraphicsDeviceManager Graphics;
        public static Level CurrentLevel;
        public bool FpsEnabled = false;

        private static ProceduralTerrainGenerator _pts;
        private readonly FPSCounter _frameCounter = new FPSCounter();
        private SpriteFont _spriteFont;
        private SpriteBatch _spriteBatch;
        private GameState _gameState;
        private InputHelper _inputHelper;
        private ConfigurationManager _configurationManager;

        public ProceduralTerrainGenerator()
        {
            _pts = this;
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
            CurrentLevel = new Level("Level1");

            CurrentLevel.RootGameObject =  new RootGameObject();  
            
            Camera.CreateCamera();
            TerrainObject.CreateTestObject(CurrentLevel.RootGameObject as RootGameObject);

           _spriteBatch = new SpriteBatch(GraphicsDevice);

            FpsEnabled = _configurationManager.FPSEnabled;

            IsMouseVisible = _configurationManager.MouseVisible;
            _gameState  = new GameState(_configurationManager.GameState);
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
            _inputHelper.Update(_gameState);

            if (_gameState.GetCurrentGameState() == State.Paused)
                return;
            
            CurrentLevel.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (FpsEnabled)
            {
                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

                _spriteBatch.Begin();
                _spriteBatch.DrawString(_spriteFont, fps, new Vector2(10, 10), Color.Black);
                _spriteBatch.End();
            }

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            CurrentLevel.Draw();
            base.Draw(gameTime);
        }

        //Used in scene parser and in inputHelper
        public static ProceduralTerrainGenerator PolygonalTerrainGenerator()
        {
            if (_pts == null)
                throw new NullReferenceException("PolygonalTerrainGenerator siege is not created");
            return _pts;
        }
    }
}
