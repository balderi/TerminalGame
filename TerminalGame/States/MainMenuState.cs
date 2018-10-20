using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TerminalGame.Utilities;

namespace TerminalGame.States
{
    class MainMenuState : State
    {
        public static MainMenuState Instance => new MainMenuState();

        private MainMenuState()
        {
            if(MediaPlayer.Queue.ActiveSong != MusicManager.GetInstance().Songs[1])
                MediaPlayer.Play(MusicManager.GetInstance().Songs[1]);
        }

        public override State Next(GameState state)
        {
            switch(state)
            {
                case GameState.GameRunning:
                    return GameRunningState.Instance;
                case GameState.NewGame:
                    return NewGameState.Instance;
                case GameState.SettingsMenu:
                    return SettingsMenuState.Instance;
                case GameState.LoadMenu:
                    return LoadMenuState.Instance;
                default:
                    return this;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Menu).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Menu).Update(gameTime);
        }
    }
}
