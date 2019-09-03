using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class SplashScreen : Screen
    {
        private readonly string _title, _version;
        private readonly SpriteFont _titleFont, _versionFont;
        private readonly Timer _timer;

        public SplashScreen(Game game) : base(game)
        {
            var tg = game as TerminalGame;
            _title = tg.Title;
            _version = tg.Version;
            _titleFont = FontManager.GetFont("FontXL");
            _versionFont = FontManager.GetFont("FontM");
            _timer = new Timer(2000);
            _timer.Elapsed += Timer_Tick;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);
            _spriteBatch.DrawString(_titleFont, _title, new Vector2(Globals.Settings.GameWidth / 2 - _titleFont.MeasureString(_title).X / 2, Globals.Settings.GameHeight / 2 - _titleFont.MeasureString(_title).Y / 2), Color.White);
            _spriteBatch.DrawString(_versionFont, _version, new Vector2(Globals.Settings.GameWidth / 2 + _titleFont.MeasureString(_title).X / 2, Globals.Settings.GameHeight / 2), Color.Green);
            _spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            _timer.Start();
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            ScreenManager.GetInstance().ChangeScreenAndInit("mainMenu");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
