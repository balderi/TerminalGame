using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.States;

namespace TerminalGame.Utilities
{
    public class GameManager
    {
        private static GameManager _instance;
        private GraphicsDeviceManager _graphics;
        private GameIntensity _intensity;
        private RenderTarget2D _renderTarget;
        public bool IsGameRunning { get; set; }
        public bool IsFullScreen { get; set; }
        public bool BloomEnabled { get; set; }
        public int ResolutionW { get; set; }
        public int ResolutionH { get; set; }

        private GameManager()
        {
            _intensity = GameIntensity.Peaceful;
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
            RefreshRenderTarget();
        }

        public void ToggleFullScreen()
        {
            IsFullScreen = !IsFullScreen;
            _graphics.IsFullScreen = IsFullScreen;
            if(IsFullScreen)
            {
                _graphics.PreferredBackBufferHeight = _graphics.GraphicsDevice.DisplayMode.Height;
                _graphics.PreferredBackBufferWidth = _graphics.GraphicsDevice.DisplayMode.Width;
            }
            else
            {
                _graphics.PreferredBackBufferHeight = 768;
                _graphics.PreferredBackBufferWidth = 1366;
            }
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Reset();
            RefreshRenderTarget();
        }

        public void UpIntensity()
        {
            if ((int)_intensity != 2)
            {
                _intensity++;
                TimeSpan pos = MediaPlayer.PlayPosition;
                //MediaPlayer.Play(MusicManager.GetInstance().Songs[(int)_intensity], pos);
                Console.WriteLine("Intensity upped to {0}.", _intensity);
            }
        }

        public void SetIntensity(int level)
        {
            if (_intensity != (GameIntensity)level)
            {
                _intensity = (GameIntensity)level;
                TimeSpan pos = MediaPlayer.PlayPosition;
                //MediaPlayer.Play(MusicManager.GetInstance().Songs[0], pos);
                Console.WriteLine("Intensity set to {0}.", _intensity);
            }
            else
                Console.WriteLine("Intensity is already {0} - no change.", _intensity);
        }

        public void ResetIntensity()
        {
            if (_intensity != 0)
            {
                _intensity = GameIntensity.Peaceful;
                TimeSpan pos = MediaPlayer.PlayPosition;
                //MediaPlayer.Play(MusicManager.GetInstance().Songs[0], pos);
                Console.WriteLine("Intensity reset. ({0})", _intensity);
            }
        }

        private void RefreshRenderTarget()
        {
            _renderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                _graphics.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
        }

        public RenderTarget2D GetRenderTarget()
        {
            return _renderTarget;
        }
    }
}
