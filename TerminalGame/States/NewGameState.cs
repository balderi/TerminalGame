using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.States
{
    class NewGameState : State
    {
        public static NewGameState Instance => new NewGameState();

        private NewGameState() { }

        public override State Next(GameState state)
        {
            if (state == GameState.MainMenu)
                return MainMenuState.Instance;
            if (state == GameState.GameLoading)
                return GameLoadingState.Instance;
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.NewGame).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.NewGame).Update(gameTime);
        }
    }
}
