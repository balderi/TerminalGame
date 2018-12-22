using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Utils;

namespace TerminalGame.UI.Modules.ModuleComponents
{
    public class Header
    {
        #region fields
        private readonly string _text;
        private readonly GraphicsDevice _graphics;
        private SpriteFont _titleFont;
        private Vector2 _titlePos;
        #endregion

        #region properties
        public Rectangle Rectangle { get; }
        #endregion

        public Header(string text, SpriteFont titleFont, int width, int height, int x, int y, GraphicsDevice graphicsDevice)
        {
            _text = text;
            Rectangle = new Rectangle(x, y, width, height);
            _graphics = graphicsDevice;
            _titleFont = titleFont;
            _titlePos = new Vector2(Rectangle.X + 5f, Rectangle.Y + (Rectangle.Height / 2) - (_titleFont.LineSpacing / 2));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, float opacity)
        {
            spriteBatch.Draw(Globals.DummyTexture(_graphics), Rectangle, Globals.ModuleHeaderBackgroundColor() * opacity);
            spriteBatch.DrawString(_titleFont, _text, _titlePos , Globals.ModuleFontColor() * opacity);
        }
    }
}
