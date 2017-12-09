using System;
using System.ComponentModel;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
                Camera.GetCurrentCamera().Position += Vector3.Cross(Camera.GetCurrentCamera().Up, Camera.GetCurrentCamera().Direction) * Camera.GetCurrentCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Camera.GetCurrentCamera().Position -= Vector3.Cross(Camera.GetCurrentCamera().Up, Camera.GetCurrentCamera().Direction) * Camera.GetCurrentCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Camera.GetCurrentCamera().Position += Camera.GetCurrentCamera().Direction *
                                                      Camera.GetCurrentCamera().Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Camera.GetCurrentCamera().Position -= Camera.GetCurrentCamera().Direction *
                                                      Camera.GetCurrentCamera().Speed;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _configurationManager.PickingEnabled)
            {
                var pickedObject = PickObject();

                if (pickedObject != null)
                {
                    Logger.Log.Debug("Object picked: " + pickedObject.ToString());
                    System.Diagnostics.Debug.Print("Object picked: " + pickedObject.ToString());
                }
                else
                {
                    Logger.Log.Debug("Object picked: NULL");
                    System.Diagnostics.Debug.Print("Object picked: NULL");

                }
            }

            if (Mouse.GetState() != _prevMouseState)
            {
                Camera.GetCurrentCamera().Direction = Vector3.Transform(
                    Camera.GetCurrentCamera().Direction,
                    Matrix.CreateFromAxisAngle(Camera.GetCurrentCamera().Up,
                        (-MathHelper.PiOver4 / 150) * (Mouse.GetState().X - _prevMouseState.X)));


                Camera.GetCurrentCamera().Direction = Vector3.Transform(
                    Camera.GetCurrentCamera().Direction,
                    Matrix.CreateFromAxisAngle(
                        Vector3.Cross(Camera.GetCurrentCamera().Up, Camera.GetCurrentCamera().Direction),
                        (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - _prevMouseState.Y)));

                // Reset PrevMouseState
                _prevMouseState = Mouse.GetState();

            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                App.GetApp().Exit();
        }


        private GameObject PickObject()
        {
            Logger.Log.Debug("Mouse pressed");

            var pickRay = CalculateRay(new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                Camera.GetCurrentCamera().ViewMatrix, Camera.GetCurrentCamera().ProjectionMatrix,
                App.GetApp().GraphicsDevice.Viewport);

            GameObject closestObject = null;
            float? closestObjectDistance = null;

            foreach (var child in Scene.GetCurrentScene().RootGameObject.GetAllChilds(Scene.GetCurrentScene().RootGameObject))
            {
                if (child.Model == null || child.IsEnabled == false)
                    continue;

                foreach (var mesh in child.Model.Meshes)
                {
                    BoundingSphere sphere = mesh.BoundingSphere;
                    sphere = sphere.Transform(child.TransformationMatrix);

                    var currentDistance = pickRay.Intersects(sphere);
                    if (currentDistance != null)
                        if (closestObjectDistance == null || currentDistance < closestObjectDistance)
                        {
                            closestObject = child;
                            closestObjectDistance = currentDistance;
                        }
                }
            }
            return closestObject;
        }

        private Ray CalculateRay(Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 0.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 1.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }
    }
}
