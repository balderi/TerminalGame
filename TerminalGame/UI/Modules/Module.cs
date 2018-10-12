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
        /// <summary>
        /// Font used for the window title
        /// </summary>
        public abstract SpriteFont Font { get; set; }
        /// <summary>
        /// BG color of the window
        /// </summary>
        //public abstract Color BackgroundColor { get; set; }
        /// <summary>
        /// Color of the window border
        /// </summary>
        //public abstract Color BorderColor { get; set; }
        /// <summary>
        /// BG color of the window header
        /// </summary>
        //public abstract Color HeaderColor { get; set; }
        /// <summary>
        /// Whether or not the module is active
        /// </summary>
        public abstract bool IsActive { get; set; }
        /// <summary>
        /// Whether or not the module is visible
        /// </summary>
        public abstract bool IsVisible { get; set; }
        /// <summary>
        /// Window title
        /// </summary>
        public abstract string Title { get; set; }
        /// <summary>
        /// Container for the module
        /// </summary>
        public abstract Rectangle Container{ get; set; }

        /// <summary>
        /// Graphics Device
        /// </summary>
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
