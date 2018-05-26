using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.Scenes
{
    abstract class Scene : IScene
    {
        // TODO: Make a scene for each game state - somehow
        public Scene()
        {

        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}
