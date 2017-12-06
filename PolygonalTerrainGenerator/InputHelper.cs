using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProceduralTerrainGenerator.GameObjects;

namespace ProceduralTerrainGenerator
{
    public class InputHelper
    {
        private MouseState PrevMouseState;
        private readonly ConfigurationManager _configurationManager;

        public InputHelper(ConfigurationManager configurationManager)
        {
            Mouse.SetPosition(ProceduralTerrainGenerator.PolygonalTerrainGenerator().Window.ClientBounds.Width / 2, ProceduralTerrainGenerator.PolygonalTerrainGenerator().Window.ClientBounds.Height / 2);
            PrevMouseState = Mouse.GetState();
            _configurationManager = configurationManager;
        }

        public void Update(GameState gameState)
        {
            switch (gameState.GetCurrentGameState())
            {
                case State.GodMode:
                    if (_configurationManager.GodModeEnabled)
                        UpdateGodMod(gameState);
                    else
                        UpdateFirstPerson(gameState);
                    break;
                case State.FirstPerson:
                    UpdateFirstPerson(gameState);
                    break;
                case State.Strategic:
                    UpdateStrategic(gameState);
                    break;
                case State.Paused:
                    UpdatePaused(gameState);
                    break;
                case State.Menu:
                    UpdateMenu(gameState);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                ProceduralTerrainGenerator.PolygonalTerrainGenerator().Exit();
        }

        private void UpdateGodMod(GameState gameState)
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
                var PickedObject = PickObject();

                if (PickedObject != null)
                {
                    Logger.Log.Debug("Object picked: " + PickedObject.ToString());
                    System.Diagnostics.Debug.Print("Object picked: " + PickedObject.ToString());
                }
                else
                {
                    Logger.Log.Debug("Object picked: NULL");
                    System.Diagnostics.Debug.Print("Object picked: NULL");

                }
            }

            if (Mouse.GetState() != PrevMouseState)
            {
                Camera.GetCurrentCamera().Direction = Vector3.Transform(
                     Camera.GetCurrentCamera().Direction,
                     Matrix.CreateFromAxisAngle(Camera.GetCurrentCamera().Up,
                         (-MathHelper.PiOver4 / 150) * (Mouse.GetState().X - PrevMouseState.X)));


                Camera.GetCurrentCamera().Direction = Vector3.Transform(
                    Camera.GetCurrentCamera().Direction,
                    Matrix.CreateFromAxisAngle(
                        Vector3.Cross(Camera.GetCurrentCamera().Up, Camera.GetCurrentCamera().Direction),
                        (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - PrevMouseState.Y)));

                // Reset PrevMouseState
                PrevMouseState = Mouse.GetState();

            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                gameState.SetNewGameState(State.Paused);
            }
        }

        private void UpdateFirstPerson(GameState gameState)
        {
            throw new NotImplementedException();
        }

        private void UpdateStrategic(GameState gameState)
        {
            throw new NotImplementedException();
        }

        private void UpdatePaused(GameState gameState)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                gameState.SetNewGameState(gameState.GetPreviousGameState());
            }
        }

        private void UpdateMenu(GameState gameState)
        {
            throw new NotImplementedException();
        }

        private GameObject PickObject()
        {
            Logger.Log.Debug("Mouse pressed");

            var pickRay = CalculateRay(new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                Camera.GetCurrentCamera().ViewMatrix, Camera.GetCurrentCamera().ProjectionMatrix,
                ProceduralTerrainGenerator.PolygonalTerrainGenerator().GraphicsDevice.Viewport);

            GameObject closestObject = null;
            float? closestObjectDistance = null;

            foreach (var child in Level.GetCurrentLevel().RootGameObject.GetAllChilds(Level.GetCurrentLevel().RootGameObject))
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
