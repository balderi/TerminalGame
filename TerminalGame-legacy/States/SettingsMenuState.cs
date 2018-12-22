using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.States
{
    class SettingsMenuState : State
    {
        public static SettingsMenuState Instance => new SettingsMenuState();

        private SettingsMenuState() { }

        public override State Next(GameState state)
        {
            if (state == GameState.MainMenu)
            {
                return MainMenuState.Instance;
            }
            else
                return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.SettingsMenu).Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Scenes.SceneManager.GetScene(Scenes.SceneManager.Scene.SettingsMenu).Update(gameTime);
        }
    }
}
