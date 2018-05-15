using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Utilities;

namespace TerminalGame.UI
{
    class PopUpBox
    {
        private readonly Texture2D backgroundTexture;
        private readonly Texture2D borderTexture;
        private SpriteFont font;
        private Color fontColor, backColor, borderColor;
        public Rectangle Container { get; set; }
        public Point Location { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Creates a pop-up box with info
        /// </summary>
        /// <param name="Text">Pop-up box text</param>
        /// <param name="Container">Specifies width/height of Pop-up box</param>
        /// <param name="Font">Font used to draw Pop-up box text</param>
        /// <param name="FontColor">Text color</param>
        /// <param name="BackColor">Pop-up box color</param>
        /// <param name="BorderColor">Pop-up box color when hovering</param>
        /// <param name="GraphicsDevice">GraphicsDevice used to render</param>
        public PopUpBox(string Text, Rectangle Container, SpriteFont Font, Color FontColor, Color BackColor, Color BorderColor, GraphicsDevice GraphicsDevice)
        {
            this.Text = Text;
            this.Container = Container;
            font = Font;
            fontColor = FontColor;
            backColor = BackColor;
            borderColor = BorderColor;
            backgroundTexture = Drawing.DrawBlankTexture(GraphicsDevice);
            borderTexture = Drawing.DrawBlankTexture(GraphicsDevice);
        }

        /// <summary>
        /// Creates an auto-sizing pop-up box with info
        /// </summary>
        /// <param name="Text">Pop-up box text</param>
        /// <param name="Location">Specifies position of Pop-up box</param>
        /// <param name="Font">Font used to draw Pop-up box text</param>
        /// <param name="FontColor">Text color</param>
        /// <param name="BackColor">Pop-up box color</param>
        /// <param name="BorderColor">Pop-up box color when hovering</param>
        /// <param name="GraphicsDevice">GraphicsDevice used to render</param>
        public PopUpBox(string Text, Point Location, SpriteFont Font, Color FontColor, Color BackColor, Color BorderColor, GraphicsDevice GraphicsDevice)
        {
            this.Text = Text;
            font = Font;
            fontColor = FontColor;
            backColor = BackColor;
            borderColor = BorderColor;
            backgroundTexture = Drawing.DrawBlankTexture(GraphicsDevice);
            borderTexture = Drawing.DrawBlankTexture(GraphicsDevice);
            Container = new Rectangle(Location.X, Location.Y, (int)font.MeasureString(Text).Length() + 20, (int)font.MeasureString(Text).Y + 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                int stringHeight = (int)font.MeasureString(Text).Y;
                int stringWidth = (int)font.MeasureString(Text).X;
                var x = Container.X + 10;
                var y = Container.Y + (Container.Height / 2 - stringHeight / 2);

                spriteBatch.Draw(backgroundTexture, Container, backColor);
                spriteBatch.DrawString(font, Text, new Vector2(x, y), fontColor);
                Drawing.DrawBorder(spriteBatch, Container, borderTexture, 1, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
