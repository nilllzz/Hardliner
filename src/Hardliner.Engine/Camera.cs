using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine
{
    public abstract class Camera
    {
        private float _fov = 90f;

        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public Vector3 Position { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        public float FOV
        {
            get { return _fov; }
            set
            {
                _fov = value;
                FOVChanged?.Invoke();
            }
        }

        public event Action FOVChanged;

        public Camera(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }

        protected virtual void CreateView()
        {
            var rotationMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
            var transformed = Vector3.Transform(Vector3.Forward, rotationMatrix);
            var lookAt = Position + transformed;
            View = Matrix.CreateLookAt(Position, lookAt, Vector3.Up);
        }

        protected virtual void CreateProjection()
        {
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fov), GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000f);
        }

        public abstract void Update();
    }
}
