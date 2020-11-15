using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalGame.UI.Elements.Buttons;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class GameOverScreen : Screen
    {
        private readonly SpriteFont _titleFont, _versionFont, _largeFont, _smallFont;
        private readonly string _gameTitle, _version, _title, _subTitle;
        private Vector2 _loadCenter, _itemCenter, _screenCenter;
        private readonly Button _backButton;

        public GameOverScreen(Game game) : base(game)
        {
            _backButton = new Button(game, "<< Back", new Point(100, GraphicsDevice.Viewport.Height - 100), new Point(200, 50), false);

            _backButton.ButtonPressed += Back_Clicked;

            _titleFont = FontManager.GetFont("FontXL");
            _versionFont = FontManager.GetFont("FontM");
            _largeFont = FontManager.GetFont("FontL");
            _smallFont = FontManager.GetFont("FontS");

            _gameTitle = "Game Over";
            _version = Game.Version;
            _title = "End of the line";
            _subTitle = "Your gateway has been traced following an unlawful intrusion into the systems of a foreign entity.\n" +
                "Any further access to the gateway has been revoked.\n" +
                "All hardware is being destroyed, and any traces of your employment with us are being erased.\n\n" +
                "Your services are no longer required - you can consider yourself terminated, effective immediately.";

            _screenCenter = _rectangle.Center.ToVector2();
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            MusicManager.GetInstance().Start("mainMenuBgm");
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);

            base.Draw(gameTime);

            Vector2 textMiddlePoint = _titleFont.MeasureString(_gameTitle) / 2;
            Vector2 position = new Vector2((Game.Window.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((Game.Window.ClientBounds.Width - textMiddlePoint.X - 15) + Generators.ShakeStuff(3),
                textMiddlePoint.Y + 15 + Generators.ShakeStuff(3));
            Vector2 position3 = new Vector2((Game.Window.ClientBounds.Width - _versionFont.MeasureString(_version).X / 2 - 20),
                position.Y + _titleFont.MeasureString(_gameTitle).Y / 2 - 15);
            _spriteBatch.DrawString(_titleFont, _gameTitle, position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawString(_titleFont, _gameTitle, position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawString(_versionFont, _version, position3, Color.Green, 0,
                _versionFont.MeasureString(_version) / 2, 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.DrawString(_largeFont, _title, new Vector2(_screenCenter.X - _loadCenter.X, _screenCenter.Y - (int)(_largeFont.MeasureString("A").Y) - 5), Color.LightGray);
            _spriteBatch.DrawString(_smallFont, _subTitle, new Vector2(_screenCenter.X - _itemCenter.X, _screenCenter.Y + 5), Color.LightGray);

            _backButton.Draw(gameTime);

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _loadCenter = new Vector2(_largeFont.MeasureString(_title).X / 2, _largeFont.MeasureString(_title).Y / 2);
            _itemCenter = new Vector2(_smallFont.MeasureString(_subTitle).X / 2, _smallFont.MeasureString(_subTitle).Y / 2);

            _backButton.Update(gameTime);

            base.Update(gameTime);
        }

        private void Back_Clicked(ButtonPressedEventArgs e)
        {
            ScreenManager.GetInstance().ChangeScreen("mainMenu");
        }
    }
}
