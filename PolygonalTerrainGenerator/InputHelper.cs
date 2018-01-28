using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Engine
{
    public class InputHelper
    {
        private MouseState _prevMouseState;
        private readonly ConfigurationManager _configurationManager;

        public InputHelper(ConfigurationManager configurationManager)
        {
            Mouse.SetPosition(App.GetApp().Window.ClientBounds.Width / 2, App.GetApp().Window.ClientBounds.Height / 2);
            _prevMouseState = Mouse.GetState();
            _configurationManager = configurationManager;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Camera.GetCamera().Position += Vector3.Cross(Camera.GetCamera().Up, Camera.GetCamera().Direction) * Camera.GetCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Camera.GetCamera().Position -= Vector3.Cross(Camera.GetCamera().Up, Camera.GetCamera().Direction) * Camera.GetCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Camera.GetCamera().Position += Camera.GetCamera().Direction *
                                                      Camera.GetCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Camera.GetCamera().Position -= Camera.GetCamera().Direction *
                                                      Camera.GetCamera().Speed;
            }

            if (Mouse.GetState() != _prevMouseState)
            {
                
                Camera.GetCamera().Direction = Vector3.Transform(Camera.GetCamera().Direction,
                    Matrix.CreateFromAxisAngle(Vector3.Normalize(Camera.GetCamera().Up), 
                    (-MathHelper.PiOver4 / 150) * 
                    (Mouse.GetState().X - _prevMouseState.X)
                    ));

                
                Camera.GetCamera().Direction = Vector3.Transform(Camera.GetCamera().Direction,
                    Matrix.CreateFromAxisAngle(Vector3.Normalize(Vector3.Cross(Camera.GetCamera().Up, Camera.GetCamera().Direction)),
                        (MathHelper.PiOver4 / 100) * 
                        (Mouse.GetState().Y - _prevMouseState.Y)
                        ));


                // Reset PrevMouseState
                Mouse.SetPosition(App.GetApp().Window.ClientBounds.Width / 2, App.GetApp().Window.ClientBounds.Height / 2);

            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                App.GetApp().Exit();
        }


    }
}
