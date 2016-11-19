using System.Collections.Generic;
using System.Linq;
using Hardliner.Screens.Game;
using Microsoft.Xna.Framework;

namespace Hardliner.Screens
{
    internal class ScreenManager : IGameComponent
    {
        private Stack<Screen> _screens;
        
        public void Initialize()
        {
            _screens = new Stack<Screen>();
            Push(new GameScreen());
        }

        internal void Push(Screen newScreen)
        {
            Screen preScreen = null;

            if (newScreen.IsRoot)
            {
                foreach (var screen in _screens)
                    screen.Deactivate();
                _screens.Clear();
            }
            else
            {
                if (_screens.Count > 0)
                    preScreen = _screens.Last();
            }

            _screens.Push(newScreen);
            newScreen.Activate(preScreen);
        }

        internal Screen Pop()
        {
            var screen = _screens.Pop();
            screen.Deactivate();
            return screen;
        }

        internal void Update()
        {
            _screens.LastOrDefault()?.Update();
        }

        internal void Draw()
        {
            _screens.LastOrDefault()?.Draw();
        }

        internal void Render()
        {
            _screens.LastOrDefault()?.Render();
        }
    }
}
