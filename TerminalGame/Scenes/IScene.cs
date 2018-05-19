using Microsoft.Xna.Framework;

namespace TerminalGame.Scenes
{
    public interface IScene
    {
        // TODO: Is an interface the best way to do this?
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
