using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI
{
    public abstract class Component
    {
        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
