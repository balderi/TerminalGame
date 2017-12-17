using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TerminalGame.Utilities;

namespace TerminalGame.UI
{
    class MainMenuButton : Component
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

        public MainMenuButton(string Text, int Width, int Height, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            font = Font;
            Console.WriteLine("Button '" + Text + "' font loaded");
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

            if (isHovering)
            {
                color = Color.LightGray;
            }
            if (Clicked)
                color = Color.DarkGray;

            if (!string.IsNullOrEmpty(text))
            {
                var x = (Rectangle.X + 15);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - ((font.MeasureString(text).Y / 2) - 5);

                if (isHovering)
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
            Clicked = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                    Clicked = true;

                if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
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
}
