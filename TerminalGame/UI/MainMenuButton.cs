using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TerminalGame.Utilities;

namespace TerminalGame.UI
{
    class MainMenuButton : Component
    {
        private MouseState _currentMouseState, _previousMouseState;
        private bool _isHovering;
        private SpriteFont _font;
        private string _text;
        private readonly GraphicsDevice _graphics;
        private readonly int _width, _height;
        private readonly Texture2D _texture;

        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);
        public event ButtonPressedEventHandler Click;
        public bool Clicked { get; private set; }
        public Color FontColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            }
        }
        public string Text { get; set; }

        public MainMenuButton(string Text, int Width, int Height, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            _font = Font;
            Console.WriteLine("Button '" + Text + "' font loaded");
            _text = Text;
            _width = Width;
            _height = Height;
            _graphics = GraphicsDevice;

            _texture = Drawing.DrawBlankTexture(_graphics);
            FontColor = Color.Black;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.DarkGray;

            if (_isHovering)
            {
                color = Color.LightGray;
            }
            if (Clicked)
                color = Color.SlateGray;

            if (!string.IsNullOrEmpty(_text))
            {
                var x = (Rectangle.X + 15);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - ((_font.MeasureString(_text).Y / 2) - 5);

                if (_isHovering)
                {
                    spriteBatch.Draw(_texture, new Rectangle(Rectangle.X + TestClass.ShakeStuff(2), Rectangle.Y + TestClass.ShakeStuff(2), Rectangle.Width, Rectangle.Height), Color.Green);
                }
                else
                {
                    spriteBatch.Draw(_texture, Rectangle, Color.Gray);
                }
                spriteBatch.Draw(_texture, new Rectangle(Rectangle.X + 2, Rectangle.Y + 2, Rectangle.Width - 4, Rectangle.Height - 4), color);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.Black);
            }
        }

        public override void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            _isHovering = false;
            Clicked = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouseState.LeftButton == ButtonState.Pressed)
                    Clicked = true;

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    ButtonPressedEventArgs bp = new ButtonPressedEventArgs()
                    {
                        Button = _text
                    };
                    Click?.Invoke(bp);
                }
            }
        }
    }
}
