using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI.Elements.Buttons
{
    class Button : UIElement
    {
        protected readonly string TEXT;
        protected SpriteFont _font;
        protected ButtonPressedEventArgs _buttonPressed;

        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);
        public event ButtonPressedEventHandler ButtonPressed;

        public Button(Game game, string text, Point location, Point size, bool fadeIn = true) : base(game, location, size, fadeIn: fadeIn)
        {
            BorderColor = Color.White * _opacity;
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
                FontColor = Color.White * _opacity;
                if (_mouseDown)
                {
                    BackgroundColor = Color.Green * _opacity;
                    BorderColor = Color.LimeGreen * _opacity;
                }
                else if (_isHovering)
                {
                    BackgroundColor = Color.DarkGray * _opacity;
                    BorderColor = Color.Green * _opacity;
                }
                else
                {
                    BackgroundColor = Color.Gray * _opacity;
                    BorderColor = Color.LimeGreen * _opacity;
                }
            }
            else
            {
                FontColor = Color.LightGray * _opacity;
                BackgroundColor = Color.DimGray * _opacity;
                BorderColor = Color.DarkOliveGreen * _opacity;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(Utils.Globals.DummyTexture(), Rectangle,
                              BackgroundColor * _opacity);

            _spriteBatch.DrawString(_font, TEXT, new Vector2(Rectangle.Center.X - _font.MeasureString(TEXT).X / 2, Rectangle.Center.Y - _font.MeasureString(TEXT).Y / 2), FontColor);

            Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(), 1,
                                          BorderColor * _opacity);
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
            ButtonPressed?.Invoke(_buttonPressed);
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
        // TODO: Specify ButtonPressedEventArgs content
    }
}
