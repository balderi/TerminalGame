using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;

namespace TerminalGame.UI.Modules
{
    class StatusBar : Module
    {
        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }

        private readonly SpriteFont lilFont;
        private string connectionInfo;
        private readonly string buildNumber;

        public StatusBar(GraphicsDevice graphics, Rectangle container, SpriteFont spriteFont) : base(graphics, container)
        {
            buildNumber = String.Format("Version {0}.{1}-alpha\n  Build {2}", System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Major, 
                System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Minor, 
                System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyyMMdd"));

            lilFont = spriteFont;
            connectionInfo = "";
            if (BackgroundColor == null)
            {
                BackgroundColor = Color.LightPink;
            }
            if (BorderColor == null)
            {
                BackgroundColor = Color.Chartreuse;
            }
            if (HeaderColor == null)
            {
                BackgroundColor = Color.Red;
            }
            if (string.IsNullOrEmpty(Title))
            {
                Title = "!!! UNNAMED WINDOW !!!";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Font.LineSpacing = 0;
            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, container, texture, 1, BorderColor);
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(container.X + 10, container.Height / 2 - (int)Font.MeasureString("A").Y / 2 + 5), Color.White);
            spriteBatch.DrawString(lilFont, connectionInfo, new Vector2(container.X + 10 + Font.MeasureString(Title).X, 
                container.Height / 2 - (int)Font.MeasureString("A").Y / 2 + 7), Color.White);
            spriteBatch.DrawString(lilFont, buildNumber, new Vector2(container.Right - lilFont.MeasureString(buildNumber).Length() - 5, 5), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            Title = "Connected to: ";
            connectionInfo = Player.GetInstance().ConnectedComputer.Name + "\n" + Player.GetInstance().ConnectedComputer.IP;
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle();
        }
    }
}
