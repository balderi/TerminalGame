using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TerminalGame.Utilities;

namespace TerminalGame.States
{
    class GameRunningState : State
    {
        public static GameRunningState Instance => new GameRunningState();

        private GameRunningState()
        {
            if (MediaPlayer.Queue.ActiveSong != MusicManager.GetInstance().Songs[0])
                MediaPlayer.Play(MusicManager.GetInstance().Songs[0]);
        }

        public override State Next(GameState state)
        {
            if (state == GameState.MainMenu)
                return MainMenuState.Instance;
            else if (state == GameState.GameOver)
                return GameOverState.Instance;
            else
                return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.GameRunning).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.GameRunning).Update(gameTime);
        }
    }
}
