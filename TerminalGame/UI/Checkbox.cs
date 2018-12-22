using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Utils;

namespace TerminalGame.UI
{
    class Checkbox : Component
    {
        private MouseState _currentMouseState, _previousMouseState;
        private bool _isHovering;
        private SpriteFont _font;
        private readonly string _text;
        private readonly GraphicsDevice _graphics;
        private readonly int _width, _height;
        private readonly Texture2D _texture;
        
        public bool Checked { get; private set; }
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

        public Checkbox(string text, int size, SpriteFont font, GraphicsDevice graphicsDevice, bool isChecked = false)
        {
            _font = font;
            _text = text;
            _width = size;
            _height = size;
            _graphics = graphicsDevice;
            Checked = isChecked;
            _texture = Drawing.DrawBlankTexture(_graphics);
            FontColor = Color.DarkGray;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.DarkGray;

            if (_isHovering)
            {
                color = Color.LightGray;
            }

            if (!string.IsNullOrEmpty(_text))
            {
                var x = (Rectangle.X + _width + 10);
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

                if (Checked)
                    spriteBatch.Draw(_texture, new Rectangle(Rectangle.X + 5, Rectangle.Y + 5, Rectangle.Width - 10, Rectangle.Height - 10), Color.Green);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), FontColor);
            }
        }

        public override void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                
                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Checked = !Checked;
                }
            }
        }
    }
}
