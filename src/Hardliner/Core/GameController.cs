﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hardliner.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Core
{
    internal sealed class GameController : Game
    {
        private Dictionary<Type, IGameComponent> _components;
        private Rectangle _viewportBounds;

        internal GraphicsDeviceManager GraphicsDeviceManager { get; }

        internal event Action ViewportSizeChanged;
        internal Rectangle Bounds => GraphicsDevice.Viewport.Bounds;

        public GameController()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            LoadComponents();

            GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            GraphicsDeviceManager.ApplyChanges();

            GraphicsDevice.SetGraphicsProfile(GraphicsProfile.HiDef);

            base.Initialize();

            _viewportBounds = GraphicsDevice.Viewport.Bounds;

            GetComponent<ScreenManager>().Push(new Screens.Game.GameScreen());
        }

        private void LoadComponents()
        {
            var gameComponentInterfaceType = typeof(IGameComponent);
            _components = typeof(GameController).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(gameComponentInterfaceType))
                .ToDictionary(t => t, t => Activator.CreateInstance(t) as IGameComponent);

            foreach (var componentEntry in _components)
                componentEntry.Value.Initialize();
        }

        internal T GetComponent<T>() where T : IGameComponent
            => (T)_components[typeof(T)];

        protected override void Update(GameTime gameTime)
        {
            UpdateViewport();

            GetComponent<ScreenManager>().Update();

            base.Update(gameTime);
        }

        private void UpdateViewport()
        {
            if (_viewportBounds != GraphicsDevice.Viewport.Bounds)
            {
                _viewportBounds = GraphicsDevice.Viewport.Bounds;
                ViewportSizeChanged?.Invoke();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GetComponent<ScreenManager>().Draw();

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, new Color(24, 20, 74), 1.0f, 0);
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            GraphicsDevice.SamplerStates[0] = new SamplerState
            {
                Filter = TextureFilter.Point
            };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GetComponent<ScreenManager>().Render();

            base.Draw(gameTime);
        }
    }
}
