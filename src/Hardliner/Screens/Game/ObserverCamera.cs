using Hardliner.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class ObserverCamera : Camera
    {
        public ObserverCamera(GraphicsDevice device)
            : base(device)
        {
            GameInstance.ViewportSizeChanged += CreateProjection;
            FOVChanged += CreateProjection;
            
            // create initial matrices
            CreateProjection();
            CreateView();
        }

        public override void Update()
        {
            bool viewChanged = false;

            var kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Left))
            {
                Yaw += 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                Yaw -= 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.Up))
            {
                Pitch += 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                Pitch -= 0.1f;
                viewChanged = true;
            }

            if (kState.IsKeyDown(Keys.W))
            {
                var rotationMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
                var translation = Vector3.Transform(Vector3.Forward, rotationMatrix);
                Position += translation * 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                var rotationMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
                var translation = Vector3.Transform(Vector3.Backward, rotationMatrix);
                Position += translation * 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                var rotationMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
                var translation = Vector3.Transform(Vector3.Left, rotationMatrix);
                Position += translation * 0.1f;
                viewChanged = true;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                var rotationMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
                var translation = Vector3.Transform(Vector3.Right, rotationMatrix);
                Position += translation * 0.1f;
                viewChanged = true;
            }

            if (viewChanged)
                CreateView();
        }
    }
}
