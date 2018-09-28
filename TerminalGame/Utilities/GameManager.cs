using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    public class GameManager
    {
        private static GameManager _instance;
        private GraphicsDeviceManager _graphics;
        public bool IsGameRunning { get; set; }
        public bool IsFullScreen { get; set; }
        public bool BloomEnabled { get; set; }
        public int ResolutionW { get; set; }
        public int ResolutionH { get; set; }

        private GameManager()
        {
            //this space intentionally left blank
        }

        public static GameManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }

        public void SetGraphicsDeviceManager(GraphicsDeviceManager graphics)
        {
            if (_graphics == null)
                _graphics = graphics;
        }

        public void ChangeResolution(int width, int height)
        {
            ResolutionW = width;
            ResolutionH = height;
            _graphics.PreferredBackBufferWidth = ResolutionW;
            _graphics.PreferredBackBufferHeight = ResolutionH;
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Reset();
        }

        public void ToggleFullScreen()
        {
            IsFullScreen = !IsFullScreen;
            _graphics.IsFullScreen = IsFullScreen;
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Reset();
        }
    }
}
