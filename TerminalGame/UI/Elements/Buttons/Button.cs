using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerminalGame.UI.Elements.Buttons
{
    class Button : UIElement
    {
        protected readonly string TEXT;
        protected SpriteFont _font;
        protected ButtonPressedEventArgs _buttonPressed;

        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);
        public event ButtonPressedEventHandler ButtonPressed;

        public Button(Game game, string text, Point location, Point size, bool fadeIn = true) :
            base(game, location, size, fadeIn: fadeIn)
        {
            BorderColor = Color.White * Opacity;
            TEXT = text;
            _font = Utils.FontManager.GetFont("FontL");
            _buttonPressed = new ButtonPressedEventArgs();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible && !_fadingDown && !_fadingUp)
                return;
            if (Enabled)
            {
                FontColor = Color.White * Opacity;
                if (_mouseLeftDown)
                {
                    BackgroundColor = Color.Green * Opacity;
                    BorderColor = Color.LimeGreen * Opacity;
                }
                else if (_isHovering)
                {
                    BackgroundColor = Color.DarkGray * Opacity;
                    BorderColor = Color.Green * Opacity;
                }
                else
                {
                    BackgroundColor = Color.Gray * Opacity;
                    BorderColor = Color.LimeGreen * Opacity;
                }
            }
            else
            {
                FontColor = Color.LightGray * Opacity;
                BackgroundColor = Color.DimGray * Opacity;
                BorderColor = Color.DarkOliveGreen * Opacity;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(Globals.Utils.DummyTexture(), Rectangle,
                              BackgroundColor * Opacity);

            _spriteBatch.DrawString(_font, TEXT, new Vector2(Rectangle.Center.X - _font.MeasureString(TEXT).X / 2,
                Rectangle.Center.Y - _font.MeasureString(TEXT).Y / 2), FontColor);

            Globals.Utils.DrawOuterBorder(_spriteBatch, Rectangle, Globals.Utils.DummyTexture(), 1,
                                          BorderColor * Opacity);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (BackgroundColor == null)
                BackgroundColor = Color.DarkGray;
        }

        protected override void OnClick(object sender, MouseEventArgs e)
        {
            base.OnClick(this, e);
            ButtonPressed?.Invoke(new ButtonPressedEventArgs { Message = TEXT });
        }

        protected override void OnMouseEnter(object sender, MouseEventArgs e)
        {
            base.OnMouseEnter(this, e);
        }

        protected override void OnMouseHover(object sender, MouseEventArgs e)
        {
            base.OnMouseHover(this, e);
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            base.OnMouseLeave(this, e);
        }
    }

    /// <summary>
    /// Generic button event args
    /// </summary>
    public class ButtonPressedEventArgs : EventArgs
    {
        public string Message { get; set; }
        // TODO: Specify ButtonPressedEventArgs content
    }
}
