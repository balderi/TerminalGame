using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using System;

namespace TerminalGame.UI.Modules
{
    class TestModule : Module
    {

        public override SpriteFont Font { get; set; }
        public override Rectangle Rectangle { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool isActive { get; set; }
        public override string Title { get; set; }

        public TestModule(GraphicsDevice Graphics) : base(Graphics)
        {
            if(BackgroundColor == null)
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
            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, Rectangle, texture, 1, BorderColor);
            spriteBatch.Draw(texture, Rectangle, BackgroundColor);
            spriteBatch.Draw(texture, renderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(renderHeader().X + 5, renderHeader().Y + 2), Color.White);
        }

        protected override Rectangle renderHeader()
        {
            return new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, (int)Font.MeasureString(Title).Y);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
