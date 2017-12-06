using System;
using Microsoft.Xna.Framework;

namespace ProceduralTerrainGenerator.GameObjects
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

        protected override void Initialize()
        {
            Direction.Normalize();
        }


        public override void Update()
        {
            CreateLookAt();
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
            var camera = new Camera("GodModeCamera");
            camera.Up = new Vector3(0, 1, 0);
            camera.Direction = new Vector3(0, 0, -1);
            camera.IsEnabled = true;
            camera.Position = new Vector3(0, 0, 15);
            camera.Speed = 0.3f;
            float NearDistance = 1;
            float FarDistance = 1000;
            float Angle = 45;

            camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Angle),
                ProceduralTerrainGenerator.PolygonalTerrainGenerator().GraphicsDevice.Viewport.AspectRatio, NearDistance, FarDistance);


            Camera.SetCurrentCamera(camera);
        }
    }
}
