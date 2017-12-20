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
        private MouseState currentMouseState, previousMouseState;
        private bool isHovering, isClicked;
        private Texture2D texture;
        private SpriteFont font;
        private Color fontColor, backColor, hoverColor, activeColor, currentColor;
        private Rectangle container;
        private string text;

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
        /// <param name="Text">Button text</param>
        /// <param name="Container">Specifies width/height of button</param>
        /// <param name="Font">Font used to draw button text</param>
        /// <param name="FontColor">Text color</param>
        /// <param name="BackColor">Button color</param>
        /// <param name="HoverColor">Button color when hovering</param>
        /// <param name="ActiveColor">Button color when clicking</param>
        /// <param name="GraphicsDevice">GraphicsDevice used to render</param>
        public Button(string Text, Rectangle Container, SpriteFont Font, Color FontColor, Color BackColor, Color HoverColor, Color ActiveColor, GraphicsDevice GraphicsDevice)
        {
            text = Text;
            container = Container;
            font = Font;
            fontColor = FontColor;
            backColor = BackColor;
            hoverColor = HoverColor;
            activeColor = ActiveColor;
            texture = Drawing.DrawBlankTexture(GraphicsDevice);
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
            text = Text;
            container = Container;
            font = Font;
            fontColor = Color.Black;
            backColor = Color.Gray;
            hoverColor = Color.LightGray;
            activeColor = Color.DarkGray;
            texture = Drawing.DrawBlankTexture(GraphicsDevice);
        }

        /// <summary>
        /// Draw button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            currentColor = backColor;
            if (isHovering)
            {
                currentColor = hoverColor;
                if (isClicked)
                    currentColor = activeColor;
            }
            
            if(!string.IsNullOrEmpty(text))
            {
                int stringHeight = (int)font.MeasureString(text).Y;
                int stringWidth = (int)font.MeasureString(text).X;
                var x = container.X + (container.Width / 2 - stringWidth / 2);
                var y = container.Y + (container.Height / 2 - stringHeight / 2);

                spriteBatch.Draw(texture, container, currentColor);
                spriteBatch.DrawString(font, text, new Vector2(x, y), fontColor);
            }
        }

        /// <summary>
        /// Update button
        /// </summary>
        public override void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            isHovering = false;
            isClicked = false;

            if (mouseRectangle.Intersects(container))
            {
                isHovering = true;

                if(currentMouseState.LeftButton == ButtonState.Pressed)
                    isClicked = true;

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
