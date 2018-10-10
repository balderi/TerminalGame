using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;
using TerminalGame.Computers;
using TerminalGame.UI.Themes;

namespace TerminalGame.UI.Modules
{
    class RemoteView : Module
    {
        public override SpriteFont Font { get; set; }
        //public override Color BackgroundColor { get; set; }
        //public override Color BorderColor { get; set; }
        //public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }

        public Computer ConnectedComputer { get; private set; }
        public override Rectangle Container { get; set; }

        private readonly SpriteFont _titleFont, _subtitleFont;
        //private string _connectionName, _connectionIP, _userLevel;

        public RemoteView(GraphicsDevice graphics, Rectangle container, SpriteFont titleFont, SpriteFont subtitleFont) : base(graphics, container)
        {
            _titleFont = titleFont;
            _subtitleFont = subtitleFont;
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
            //_connectionName = "";
            //_connectionIP = "";
            //_userLevel = "";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);
                Drawing.DrawBorder(spriteBatch, Container, texture, 1, ThemeManager.GetInstance().CurrentTheme.ModuleOutlineColor);
                spriteBatch.Draw(texture, Container, ThemeManager.GetInstance().CurrentTheme.ModuleBackgroundColor);
                spriteBatch.Draw(texture, RenderHeader(), ThemeManager.GetInstance().CurrentTheme.ModuleHeaderBackgroundColor);
                spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
                ConnectedComputer.RemoteUI.Draw(spriteBatch, Container);
                /*
                spriteBatch.DrawString(_titleFont, _connectionName, new Vector2(Container.X + 10, Container.Y + RenderHeader().Height + 10), Color.White);
                spriteBatch.DrawString(_subtitleFont, _connectionIP, new Vector2(Container.X + 10, _titleFont.MeasureString("A").Y + Container.Y + RenderHeader().Height + 10), Color.White);
                spriteBatch.DrawString(_subtitleFont, _userLevel, new Vector2(_subtitleFont.MeasureString(_connectionIP).Length() + Container.X + 20, _titleFont.MeasureString("A").Y + Container.Y + RenderHeader().Height + 10), Player.GetInstance().ConnectedComputer.PlayerHasRoot ? Color.Lime : Color.Red);
                */
            }
        }

        public override void Update(GameTime gameTime)
        {
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
            ConnectedComputer.RemoteUI.Update(gameTime);
            //_connectionName = ConnectedComputer.Name;
            //_connectionIP = ConnectedComputer.IP;
            //_userLevel = ConnectedComputer.PlayerHasRoot ? "You have root access on this system" : "Access denied";
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
