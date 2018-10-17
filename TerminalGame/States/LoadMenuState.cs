using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Utilities;

namespace TerminalGame.States
{
    class LoadMenuState : State
    {
        public static LoadMenuState Instance => new LoadMenuState();

        private LoadMenuState() { }

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
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.LoadMenu).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.LoadMenu).Update(gameTime);
        }
    }
}
