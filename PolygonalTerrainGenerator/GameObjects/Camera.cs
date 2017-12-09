using System;
using Microsoft.Xna.Framework;

namespace Engine.GameObjects
{
    public class Camera : GameObject
    {  
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;

        public Vector3 Direction = Vector3.Forward;
        public Vector3 Up = Vector3.Up;

        public float Speed = 0.3F;

        private static Camera _currentCamera;

        public Camera(string Name)
        {
            this.Name = Name;
            Initialize();
        }

        public static void SetCurrentCamera(Camera cam)
        {
            _currentCamera = cam;
        }

        protected sealed override void Initialize()
        {
            Direction.Normalize();
        }


        public override void Update()
        {
            CreateLookAt();
            Logger.Log.Info("Direction: " + Direction.X + " " +  Direction.Y + " " + Direction.Z);
        }

        private void CreateLookAt()
        {
            ViewMatrix = Matrix.CreateLookAt(Position, Position + Direction, Up);
        }

        public override void Draw()
        {

        }

        public static Camera GetCurrentCamera()
        {
            if (_currentCamera == null)
            {
                Logger.Log.Error("_currenCamera was not initialized");
                throw new TypeInitializationException("_currenCamera was not initialized", null);
            }
            return

            _currentCamera;
        }

        public static void CreateCamera()
        {
            var camera = new Camera("GodModeCamera")
            {
                Up = new Vector3(0, 0, 1),
                Direction = new Vector3(0, -1, 0),
                IsEnabled = true,
                Position = new Vector3(0, 25, 0),
                Speed = 0.3f
            };
            const float nearDistance = 1;
            const float farDistance = 1000;
            const float angle = 45;

            camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(angle),
                App.GetApp().GraphicsDevice.Viewport.AspectRatio, nearDistance, farDistance);
            SetCurrentCamera(camera);
        }
    }
}
