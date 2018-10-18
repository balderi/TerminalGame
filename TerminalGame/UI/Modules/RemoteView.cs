using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class RemoteView : Module
    {
        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }

        public Computer ConnectedComputer { get; private set; }
        public override Rectangle Container { get; set; }

        private readonly SpriteFont _titleFont, _subtitleFont;

        public RemoteView(GraphicsDevice graphics, Rectangle container, SpriteFont titleFont, SpriteFont subtitleFont) : base(graphics, container)
        {
            _titleFont = titleFont;
            _subtitleFont = subtitleFont;
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);
                Drawing.DrawBorder(spriteBatch, Container, texture, 1, _themeManager.CurrentTheme.ModuleOutlineColor);
                spriteBatch.Draw(texture, Container, _themeManager.CurrentTheme.ModuleBackgroundColor);
                spriteBatch.Draw(texture, RenderHeader(), _themeManager.CurrentTheme.ModuleHeaderBackgroundColor);
                spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), _themeManager.CurrentTheme.ModuleHeaderFontColor);
                ConnectedComputer.RemoteUI.Draw(spriteBatch, Container);
            }
        }

        public override void Update(GameTime gameTime)
        {
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
            ConnectedComputer.RemoteUI.Update(gameTime);
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
