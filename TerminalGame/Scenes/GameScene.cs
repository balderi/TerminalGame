using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class GameScene : Scene
    {
        public Texture2D Background { get; private set; }
        private Rectangle _bgRect;
        private Color _bgColor;
        private bool _prevKbState, _newKbState;

        public GameScene(Texture2D background, Rectangle bgRect) : base()
        {
            _bgRect = bgRect;
            Background = background;
            _bgColor = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, _bgRect, _bgColor);
            OS.OS.GetInstance().Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!GameManager.GetInstance().IsGameRunning)
                _stateMachine.Transition(GameState.GameOver);
            OS.OS.GetInstance().Update(gameTime);
            _newKbState = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (_newKbState != _prevKbState)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    _stateMachine.Transition(GameState.MainMenu);
                }
            }
            _prevKbState = _newKbState;
        }
    }
}
