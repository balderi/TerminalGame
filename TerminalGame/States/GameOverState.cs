using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TerminalGame.Utilities;

namespace TerminalGame.States
{
    class GameOverState : State
    {
        public static GameOverState Instance => new GameOverState();

        private GameOverState()
        {
            if (MediaPlayer.Queue.ActiveSong != MusicManager.GetInstance().Songs[1])
                MediaPlayer.Play(MusicManager.GetInstance().Songs[1]);
        }

        public override State Next(GameState state)
        {
            if (state == GameState.MainMenu)
                return MainMenuState.Instance;
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.GameOver).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.GameOver).Update(gameTime);
        }
    }
}
