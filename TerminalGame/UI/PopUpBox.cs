using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Themes;
using TerminalGame.Utilities;

namespace TerminalGame.UI
{
    class PopUpBox
    {
        private readonly Texture2D _backgroundTexture, _borderTexture;
        private SpriteFont _font;
        private Color _fontColor, _backColor, _borderColor;
        public Rectangle Container { get; set; }
        public Point Location { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }

        private ThemeManager _themeManager;

        /// <summary>
        /// Creates a pop-up box with info
        /// </summary>
        /// <param name="text">Pop-up box text</param>
        /// <param name="container">Specifies width/height of Pop-up box</param>
        /// <param name="font">Font used to draw Pop-up box text</param>
        /// <param name="fontColor">Text color</param>
        /// <param name="backColor">Pop-up box color</param>
        /// <param name="borderColor">Pop-up box color when hovering</param>
        /// <param name="graphicsDevice">GraphicsDevice used to render</param>
        public PopUpBox(string text, Rectangle container, SpriteFont font, Color fontColor, Color backColor, Color borderColor, GraphicsDevice graphicsDevice)
        {
            Text = text;
            Location = container.Location;
            Container = container;
            _themeManager = ThemeManager.GetInstance();
            _font = font;
            _fontColor = fontColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _backgroundTexture = Drawing.DrawBlankTexture(graphicsDevice);
            _borderTexture = Drawing.DrawBlankTexture(graphicsDevice);
        }

        /// <summary>
        /// Creates an auto-sizing pop-up box with info
        /// </summary>
        /// <param name="text">Pop-up box text</param>
        /// <param name="location">Specifies position of Pop-up box</param>
        /// <param name="font">Font used to draw Pop-up box text</param>
        /// <param name="fontColor">Text color</param>
        /// <param name="backColor">Pop-up box color</param>
        /// <param name="borderColor">Pop-up box color when hovering</param>
        /// <param name="graphicsDevice">GraphicsDevice used to render</param>
        public PopUpBox(string text, Point location, SpriteFont font, Color fontColor, Color backColor, Color borderColor, GraphicsDevice graphicsDevice)
        {
            Text = text;
            Location = location;
            _themeManager = ThemeManager.GetInstance();
            _font = font;
            _fontColor = fontColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _backgroundTexture = Drawing.DrawBlankTexture(graphicsDevice);
            _borderTexture = Drawing.DrawBlankTexture(graphicsDevice);
            Container = new Rectangle(Location.X, Location.Y, (int)_font.MeasureString(Text).Length() + 20, (int)_font.MeasureString(Text).Y + 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                int stringHeight = (int)_font.MeasureString(Text).Y;
                int stringWidth = (int)_font.MeasureString(Text).X;
                var x = Container.X + 10;
                var y = Container.Y + (Container.Height / 2 - stringHeight / 2);

                spriteBatch.Draw(_backgroundTexture, Container, _themeManager.CurrentTheme.ModuleBackgroundColor);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), _themeManager.CurrentTheme.ModuleFontColor);
                Drawing.DrawBorder(spriteBatch, Container, _borderTexture, 1, _themeManager.CurrentTheme.ModuleOutlineColor);
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void ChangeLocation(Point location)
        {
            Location = location;
            Container = new Rectangle(Location.X, Location.Y, (int)_font.MeasureString(Text).Length() + 20, (int)_font.MeasureString(Text).Y + 10);
        }
    }
}
