using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TerminalGame.Utilities;

namespace TerminalGame.UI
{
    /// <summary>
    /// Customizable UI button
    /// </summary>
    public class Button : Component
    {
        private MouseState _currentMouseState, _previousMouseState;
        private bool _isHovering, _isClicked;
        private readonly Texture2D _texture;
        private SpriteFont _font;
        private Color _fontColor, _backColor, _hoverColor, _activeColor, _currentColor;
        private Rectangle _container;
        private string _text;

        /// <summary>
        /// EventHandler delegate for when the button is clicked
        /// </summary>
        /// <param name="e"></param>
        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);

        /// <summary>
        /// Event for when the button is clicked
        /// </summary>
        public event ButtonPressedEventHandler Click;
        /// <summary>
        /// Button's position on screen
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="container">Specifies width/height of button</param>
        /// <param name="font">Font used to draw button text</param>
        /// <param name="fontColor">Text color</param>
        /// <param name="backColor">Button color</param>
        /// <param name="hoverColor">Button color when hovering</param>
        /// <param name="activeColor">Button color when clicking</param>
        /// <param name="graphicsDevice">GraphicsDevice used to render</param>
        public Button(string text, Rectangle container, SpriteFont font, Color fontColor, Color backColor, Color hoverColor, Color activeColor, GraphicsDevice graphicsDevice)
        {
            _text = text;
            _container = container;
            _font = font;
            _fontColor = fontColor;
            _backColor = backColor;
            _hoverColor = hoverColor;
            _activeColor = activeColor;
            _texture = Drawing.DrawBlankTexture(graphicsDevice);
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="Text">Button text</param>
        /// <param name="Container">Specifies width/height of button</param>
        /// <param name="Font">Font used to draw button text</param>
        /// <param name="GraphicsDevice">GraphicsDevice used to render</param>
        public Button(string Text, Rectangle Container, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            _text = Text;
            _container = Container;
            _font = Font;
            _fontColor = Color.Black;
            _backColor = Color.Gray;
            _hoverColor = Color.LightGray;
            _activeColor = Color.DarkGray;
            _texture = Drawing.DrawBlankTexture(GraphicsDevice);
        }

        /// <summary>
        /// Draw button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            _currentColor = _backColor;
            if (_isHovering)
            {
                _currentColor = _hoverColor;
                if (_isClicked)
                    _currentColor = _activeColor;
            }
            
            if(!string.IsNullOrEmpty(_text))
            {
                int stringHeight = (int)_font.MeasureString(_text).Y;
                int stringWidth = (int)_font.MeasureString(_text).X;
                var x = _container.X + (_container.Width / 2 - stringWidth / 2);
                var y = _container.Y + (_container.Height / 2 - stringHeight / 2);

                spriteBatch.Draw(_texture, _container, _currentColor);
                spriteBatch.DrawString(_font, _text, new Vector2(x, y), _fontColor);
            }
        }

        /// <summary>
        /// Update button
        /// </summary>
        public override void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            _isHovering = false;
            _isClicked = false;

            if (mouseRectangle.Intersects(_container))
            {
                _isHovering = true;

                if(_currentMouseState.LeftButton == ButtonState.Pressed)
                    _isClicked = true;

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

    /// <summary>
    /// Custom EventArgs for when button is pressed
    /// </summary>
    public class ButtonPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Button value
        /// </summary>
        public string Button { get; set; }
    }
}
