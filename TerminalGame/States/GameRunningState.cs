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
    class GameRunningState : State
    {
        public static GameRunningState Instance => new GameRunningState();

        private GameRunningState() { }

        public override State Next(Keys key)
        {
            if (key == Keys.Escape)
                return Next();
            else
                return this;
        }

        public override State Next()
        {
            return GameMenuState.Instance;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Game).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Game).Update(gameTime);
        }
    }
}
