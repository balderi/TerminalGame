using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI.Modules
{
    public abstract class Module
    {
        public abstract SpriteFont Font { get; set; }
        public abstract Color BackgroundColor { get; set; }
        public abstract Color BorderColor { get; set; }
        public abstract Color HeaderColor { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract string Title { get; set; }
        
        protected GraphicsDevice graphics;
        protected Rectangle container;

        protected Module(GraphicsDevice Graphics, Rectangle Container)
        {
            graphics = Graphics;
            container = Container;
        }

        protected abstract Rectangle RenderHeader();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
