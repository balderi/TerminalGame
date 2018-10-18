using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Themes;

namespace TerminalGame.UI.Modules
{
    /// <summary>
    /// Base UI module
    /// </summary>
    public abstract class Module
    {
        public abstract SpriteFont Font { get; set; }

        public abstract bool IsActive { get; set; }

        public abstract bool IsVisible { get; set; }

        public abstract string Title { get; set; }

        public abstract Rectangle Container{ get; set; }

        protected GraphicsDevice _graphics;

        protected ThemeManager _themeManager;

        /// <summary>
        /// Base UI module constructor
        /// </summary>
        /// <param name="graphics">Graphics Device</param>
        /// <param name="container">Container for the module</param>
        protected Module(GraphicsDevice graphics, Rectangle container)
        {
            _themeManager = ThemeManager.GetInstance();
            _graphics = graphics;
            Container = container;
        }

        /// <summary>
        /// Draws the window header
        /// </summary>
        /// <returns>Header as Rectangle</returns>
        protected abstract Rectangle RenderHeader();
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public abstract void Update(GameTime gameTime);
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
