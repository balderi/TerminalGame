using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI.Modules
{
    /// <summary>
    /// Base UI module
    /// </summary>
    public abstract class Module
    {
        public abstract SpriteFont Font { get; set; }
        public abstract Color BackgroundColor { get; set; }
        public abstract Color BorderColor { get; set; }
        public abstract Color HeaderColor { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract string Title { get; set; }
        public abstract Rectangle Container{ get; set; }

        protected GraphicsDevice Graphics;

        /// <summary>
        /// Base UI module constructor
        /// </summary>
        /// <param name="Graphics"></param>
        /// <param name="Container"></param>
        protected Module(GraphicsDevice graphics, Rectangle container)
        {
            Graphics = graphics;
            Container = container;

        }

        protected abstract Rectangle RenderHeader();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
