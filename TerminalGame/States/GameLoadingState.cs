using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.States
{
    class GameLoadingState : State
    {
        public static GameLoadingState Instance => new GameLoadingState();

        private GameLoadingState() { }

        public override State Next(GameState state)
        {
            if (state == GameState.GameRunning)
                return GameRunningState.Instance;
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Loading).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Loading).Update(gameTime);
        }
    }
}
