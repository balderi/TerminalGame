using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Themes;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents
{
    public class Header
    {
        #region fields
        private readonly string TEXT;
        private readonly SpriteFont _titleFont;
        private Vector2 _titlePos;
        private readonly ThemeManager _themeManager;
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
        public Header(string text, SpriteFont titleFont, int width, int height, int x, int y)
        {
            _themeManager = ThemeManager.GetInstance();
            TEXT = text;
            Rectangle = new Rectangle(x, y, width, height);
            _titleFont = titleFont;
            _titlePos = new Vector2(Rectangle.X + 5f, Rectangle.Y + (Rectangle.Height / 2) - (_titleFont.LineSpacing / 2));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, float opacity)
        {
            spriteBatch.Draw(Globals.Utils.DummyTexture(), Rectangle, _themeManager.CurrentTheme.ModuleHeaderBackgroundColor * opacity);
            spriteBatch.DrawString(_titleFont, TEXT, _titlePos, _themeManager.CurrentTheme.ModuleHeaderFontColor * opacity);
        }
    }
}
