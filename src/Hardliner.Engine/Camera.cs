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
        protected GraphicsDevice GraphicsDevice { get; private set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
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
            var up = Vector3.Up;
            var forward = Vector3.Forward;

            // yaw:
            {
                forward.Normalize();
                forward = Vector3.Transform(forward, Matrix.CreateFromAxisAngle(up, Yaw));
            }

            // pitch:
            {
                forward.Normalize();
                var left = Vector3.Cross(up, forward);
                left.Normalize();

                forward = Vector3.Transform(forward, Matrix.CreateFromAxisAngle(left, -Pitch));
                up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(left, -Pitch));
            }

            // roll:
            {
                up.Normalize();
                var left = Vector3.Cross(up, forward);
                left.Normalize();
                up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(forward, Roll));
            }

            View = Matrix.CreateLookAt(Position, forward + Position, up);
        }

        protected virtual void CreateProjection()
        {
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fov), GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000f);
        }

        public abstract void Update();
    }
}
