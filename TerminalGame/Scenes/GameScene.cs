using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class GameScene : IScene
    {
        public Texture2D Background { get; private set; }
        private Rectangle _bgRect;
        private Color _bgColor;
        private bool _prevKbState, _newKbState;
        private readonly StateMachine _stateMachine;

        public GameScene(Texture2D background, Rectangle bgRect, StateMachine stateMachine)
        {
            _bgRect = bgRect;
            Background = background;
            _bgColor = Color.White;
            _stateMachine = stateMachine;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, _bgRect, _bgColor);
            OS.OS.GetInstance().Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
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
