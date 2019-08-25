using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class SplashScreen : Screen
    {
        private readonly string TITLE, VER;
        private readonly SpriteFont TITLE_FONT, VER_FONT;
        private Timer _timer;

        public SplashScreen(Game game) : base(game)
        {
            var tg = game as TerminalGame;
            TITLE = tg.Title;
            VER = string.Format("v{0}.{1}", tg.version.Major, tg.version.Minor);
            TITLE_FONT = FontManager.GetFont("FontXL");
            VER_FONT = FontManager.GetFont("FontM");
            _timer = new Timer(2000);
            _timer.Elapsed += Timer_Tick;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);
            _spriteBatch.DrawString(TITLE_FONT, TITLE, new Vector2(Globals.GameWidth / 2 - TITLE_FONT.MeasureString(TITLE).X / 2, Globals.GameHeight / 2 - TITLE_FONT.MeasureString(TITLE).Y / 2), Color.White);
            _spriteBatch.DrawString(VER_FONT, VER, new Vector2(Globals.GameWidth / 2 + TITLE_FONT.MeasureString(TITLE).X / 2, Globals.GameHeight / 2), Color.Green);
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
            ScreenManager.GetInstance().ChangeScreen("mainMenu");
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
