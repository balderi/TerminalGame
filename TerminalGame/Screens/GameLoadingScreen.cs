using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class GameLoadingScreen : Screen
    {
        private string _loading, _loadItem, _saveGamePath;
        private SpriteFont _loadingFont, _loadItemFont;
        private Task _loadingTask;
        private Vector2 _loadPos, _itemPos;

        public GameLoadingScreen(Game game, string saveGamePath = null) : base(game)
        {
            _saveGamePath = saveGamePath;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);
            _loading = "Loading";
            _loadItem = "Stand by...";
            _loadingFont = FontManager.GetFont("FontXL");
            _loadItemFont = FontManager.GetFont("FontM");

            _loadPos = new Vector2(Game.Window.ClientBounds.Width / 2 - _loadingFont.MeasureString(_loading).X / 2,
                Game.Window.ClientBounds.Height / 2 - _loadingFont.MeasureString(_loading).Y / 2);
            _itemPos = new Vector2(Game.Window.ClientBounds.Width / 2 - _loadItemFont.MeasureString(_loadItem).X / 2,
                Game.Window.ClientBounds.Height / 2 - _loadItemFont.MeasureString(_loadItem).Y / 2 + _loadingFont.MeasureString(_loading).Y / 2);
            
            if (_saveGamePath == null)
            {
                Console.WriteLine("Creating world");
                _loadingTask = new Task(() => World.World.GetInstance().CreateWorld(Game, 100));
            }
            else
            {
                _loadingTask = new Task(() => World.World.GetInstance(_saveGamePath));
            }

            _loadingTask.Start();
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            MusicManager.GetInstance().FadeOut();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_loadingTask.Status == TaskStatus.RanToCompletion)
            {
                ScreenManager.GetInstance().AddScreen("gameRunning", new GameRunningScreen(Game));
                Console.WriteLine("Done");
                ScreenManager.GetInstance().ChangeScreenAndInit("gameRunning");
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_loadingFont, _loading, _loadPos, Color.White);
            _spriteBatch.DrawString(_loadItemFont, _loadItem, _itemPos, Color.White);
            _spriteBatch.End();
        }
    }
}
