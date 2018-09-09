using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.UI;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class LoadGameScene : IScene
    {

        private readonly SpriteFont _font;
        private readonly GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;
        KeyboardState _prevKbState, _newKbState;
        readonly StateMachine _stateMachine;
        MainMenuButton backButton;

        public LoadGameScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics, StateMachine stateMachine)
        {
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;
            _stateMachine = stateMachine;
            backButton = new MainMenuButton("< Back", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50, _graphics.Viewport.Height - 50 - 50)
            };
            backButton.Click += OnButtonClick;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            Vector2 textMiddlePoint = _font.MeasureString("Load Game") / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(_font, "Load Game", position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_font, "Load Game", position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            //spriteBatch.DrawString(_font, "LoadGameScene", new Vector2(10, 10), Color.White);
            backButton.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            backButton.Update();
            _newKbState = Keyboard.GetState();
            if (_newKbState != _prevKbState)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    _stateMachine.Transition(GameState.MainMenu);
                }
            }
            _prevKbState = _newKbState;
        }

        private void OnButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }
    }
}
