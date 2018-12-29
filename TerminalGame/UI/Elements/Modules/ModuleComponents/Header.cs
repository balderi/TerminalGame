using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.UI.Themes;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents
{
    public class Header
    {
        #region fields
        private readonly string _text;
        private readonly GraphicsDevice _graphics;
        private SpriteFont _titleFont;
        private Vector2 _titlePos;
        private ThemeManager _themeManager;
        #endregion

        #region properties
        public Rectangle Rectangle { get; }
        #endregion

        /// <summary>
        /// A header (or "TitleBar") for <c>Module</c>s
        /// </summary>
        /// <param name="text">Title text</param>
        /// <param name="titleFont">Font used for title text</param>
        /// <param name="width">Header width (typically <c>Module</c> width</param>
        /// <param name="height">Header height (typically font height)</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="graphicsDevice"><c>GraphicsDevice</c> used to generate dummy textures (mainly for the background)</param>
        public Header(string text, SpriteFont titleFont, int width, int height, int x, int y, GraphicsDevice graphicsDevice)
        {
            _themeManager = ThemeManager.GetInstance();
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
            spriteBatch.Draw(Globals.DummyTexture(_graphics), Rectangle, _themeManager.CurrentTheme.ModuleHeaderBackgroundColor * opacity);
            spriteBatch.DrawString(_titleFont, _text, _titlePos , _themeManager.CurrentTheme.ModuleHeaderFontColor * opacity);
        }
    }
}
