using Microsoft.Xna.Framework;

namespace TerminalGame.Scenes
{
    abstract class Scene : IScene
    {
        // TODO: Make a scene for wach game state - somehow
        public Scene()
        {

        }

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
