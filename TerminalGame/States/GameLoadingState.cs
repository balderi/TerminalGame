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
    class GameLoadingState : State
    {
        public static GameLoadingState Instance => new GameLoadingState();

        private GameLoadingState() { }

        public override State Next(Keys key)
        {
            if (key == Keys.Attn)
                return Next();
            return this;
        }

        public override State Next()
        {
            return GameRunningState.Instance;
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
