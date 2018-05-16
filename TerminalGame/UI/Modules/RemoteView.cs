using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class RemoteView : Module
    {
        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }

        public Computer ConnectedComputer { get; private set; }
        private readonly SpriteFont TitleFont, SubtitleFont;
        private string ConnectionName, ConnectionIP, UserLevel;

        public RemoteView(GraphicsDevice Graphics, Rectangle Container, SpriteFont titleFont, SpriteFont subtitleFont) : base(Graphics, Container)
        {
            TitleFont = titleFont;
            SubtitleFont = subtitleFont;
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
            ConnectionName = "";
            ConnectionIP = "";
            UserLevel = "";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, container, texture, 1, BorderColor);
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
            spriteBatch.DrawString(TitleFont, ConnectionName, new Vector2(container.X + 10, container.Y + RenderHeader().Height + 10), Color.White);
            spriteBatch.DrawString(SubtitleFont, ConnectionIP, new Vector2(container.X + 10, TitleFont.MeasureString("A").Y + container.Y + RenderHeader().Height + 10), Color.White);
            spriteBatch.DrawString(SubtitleFont, UserLevel, new Vector2(SubtitleFont.MeasureString(ConnectionIP).Length() + container.X + 20, TitleFont.MeasureString("A").Y + container.Y + RenderHeader().Height + 10), Player.GetInstance().ConnectedComputer.PlayerHasRoot ? Color.Lime : Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            ConnectedComputer = Player.GetInstance().ConnectedComputer;
            ConnectionName = ConnectedComputer.Name;
            ConnectionIP = ConnectedComputer.IP;
            UserLevel = ConnectedComputer.PlayerHasRoot ? "You have root access on this system" : "Access denied";
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
