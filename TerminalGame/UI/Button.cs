using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using System;

namespace TerminalGame.UI
{
    class Button : Component
    {
        private MouseState currentMouseState, previousMouseState;
        private bool isHovering;
        private SpriteFont font;
        private string text;
        private GraphicsDevice graphics;
        private int width, height;
        private Texture2D texture;

        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);
        public event ButtonPressedEventHandler Click;
        public bool Clicked { get; private set; }
        public Color FontColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }
        public string Text { get; set; }
        
        public Button(string Text, int Width, int Height, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            font = Font;
            text = Text;
            width = Width;
            height = Height;
            graphics = GraphicsDevice;

            texture = Drawing.DrawBlankTexture(graphics);
            FontColor = Color.Black;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.DarkGray;

            if(isHovering)
            {
                color = Color.LightGray;
            }

            if(!string.IsNullOrEmpty(text))
            {
                var x = (Rectangle.X + 15);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - ((font.MeasureString(text).Y / 2) - 5);

                if(isHovering)
                {
                    spriteBatch.Draw(texture, new Rectangle(Rectangle.X + 2 + TestClass.ShakeStuff(3), Rectangle.Y + 2 + TestClass.ShakeStuff(3), Rectangle.Width - 4, Rectangle.Height - 4), Color.Green);
                }
                else
                {
                    spriteBatch.Draw(texture, Rectangle, Color.Gray);
                }
                spriteBatch.Draw(texture, new Rectangle(Rectangle.X + 2, Rectangle.Y + 2, Rectangle.Width - 4, Rectangle.Height - 4), color);

                spriteBatch.DrawString(font, text, new Vector2(x, y), Color.Black);
            }
        }

        public override void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            isHovering = false;

            if(mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if(currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    ButtonPressedEventArgs bp = new ButtonPressedEventArgs()
                    {
                        Button = text
                    };
                    Click?.Invoke(bp);
                }
            }
        }
    }
    public class ButtonPressedEventArgs : EventArgs
    {
        public string Button { get; set; }
    }
}
