using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.States
{
    class GameSettingsState : State
    {
        public static GameSettingsState Instance => new GameSettingsState();

        private GameSettingsState() { }

        public override State Next(Keys key)
        {
            if (key == Keys.Escape)
            {
                System.Console.WriteLine("*** ESC pressed");
                return Next();
            }
            else
                return this;
        }

        public override State Next()
        {
            return GameMenuState.Instance;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Settings).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.Settings).Update(gameTime);
        }
    }
}
