using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;

namespace TerminalGame.UI.Modules
{
    class StatusBar : Module
    {
        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set; }

        private readonly SpriteFont _lilFont;
        private string _connectionInfo;
        private readonly string _buildNumber, _playerDeets;

        public StatusBar(GraphicsDevice graphics, Rectangle container, SpriteFont spriteFont) : base(graphics, container)
        {
            _buildNumber = String.Format("Version {0}\n  Build {1}", GameManager.GetInstance().Version, GameManager.GetInstance().BuildNumber);

            _playerDeets = "   Name: " + Player.GetInstance().Name + "\nBalance: $" + Player.GetInstance().Balance;

            _lilFont = spriteFont;
            _connectionInfo = "";

            if (string.IsNullOrEmpty(Title))
            {
                Title = "!!! UNNAMED WINDOW !!!";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Font.LineSpacing = 0;
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);
                Drawing.DrawBorder(spriteBatch, Container, texture, 1, _themeManager.CurrentTheme.StatusBarBackgroundColor);
                spriteBatch.Draw(texture, Container, _themeManager.CurrentTheme.StatusBarBackgroundColor);
                spriteBatch.Draw(texture, RenderHeader(), _themeManager.CurrentTheme.StatusBarBackgroundColor);
                spriteBatch.DrawString(Font, Title, 
                    new Vector2(Container.X + 10, Container.Height / 2 - (int)Font.MeasureString("A").Y / 2 + 5),
                    _themeManager.CurrentTheme.ModuleHeaderFontColor);
                spriteBatch.DrawString(_lilFont, _connectionInfo, new Vector2(Container.X + 10 + Font.MeasureString(Title).X,
                    Container.Height / 2 - (int)Font.MeasureString("A").Y / 2 + 7), _themeManager.CurrentTheme.ModuleHeaderFontColor);
                spriteBatch.DrawString(_lilFont, _buildNumber, 
                    new Vector2(Container.Right - _lilFont.MeasureString(_buildNumber).Length() - 5, 5),
                    _themeManager.CurrentTheme.ModuleHeaderFontColor);
                spriteBatch.DrawString(_lilFont, _playerDeets, 
                    new Vector2(Container.Right - _lilFont.MeasureString(_buildNumber).Length() - 5 - _lilFont.MeasureString(_playerDeets).Length() - 20, 5),
                    _themeManager.CurrentTheme.ModuleHeaderFontColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Title = "Connected to: ";
            _connectionInfo = Player.GetInstance().ConnectedComputer.Name + "\n" + Player.GetInstance().ConnectedComputer.IP;
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle();
        }
    }
}
