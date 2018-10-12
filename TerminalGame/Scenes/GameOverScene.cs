using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.States;
using TerminalGame.UI;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class GameOverScene : IScene
    {
        private SpriteFont _largeFont, _smallFont;
        private readonly string _title, _subTitle;
        private Vector2 _loadCenter, _itemCenter, _screenCenter;
        private GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;
        private bool _prevKbState, _newKbState;
        private readonly StateMachine _stateMachine;
        private MainMenuButton backButton;

        public GameOverScene(Vector2 screenCenter, GameWindow gameWindow, SpriteFont buttonFont, GraphicsDevice graphics, StateMachine stateMachine)
        {
            _screenCenter = screenCenter;
            _graphics = graphics;
            _gameWindow = gameWindow;
            _stateMachine = stateMachine;
            _largeFont = FontManager.GetFont(FontManager.FontSize.Large);
            _smallFont = FontManager.GetFont(FontManager.FontSize.Small);
            _title = "End of the line";
            _subTitle = "Your gateway has been traced following an unlawful intrusion into the systems of a foreign entity.\n" +
                "Any further access to the gateway has been revoked.\n" +
                "All hardware is being destroyed, and any traces of your employment with us are being erased.\n\n" +
                "Your services are no longer required - you can consider yourself terminated, effective immediately.";
            GameManager.GetInstance().IsGameRunning = false;
            backButton = new MainMenuButton("< Back", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50, _graphics.Viewport.Height - 50 - 50)
            };
            backButton.Click += OnButtonClick;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            spriteBatch.DrawString(_largeFont, _title, new Vector2(_screenCenter.X - _loadCenter.X, _screenCenter.Y - (int)(_largeFont.MeasureString("A").Y) - 5), Color.LightGray);
            spriteBatch.DrawString(_smallFont, _subTitle, new Vector2(_screenCenter.X - _itemCenter.X, _screenCenter.Y + 5), Color.LightGray);
            backButton.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _loadCenter = new Vector2(_largeFont.MeasureString(_title).X / 2, _largeFont.MeasureString(_title).Y / 2);
            _itemCenter = new Vector2(_smallFont.MeasureString(_subTitle).X / 2, _smallFont.MeasureString(_subTitle).Y / 2);
            _newKbState = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (_newKbState != _prevKbState)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    _stateMachine.Transition(GameState.MainMenu);
                }
            }
            _prevKbState = _newKbState;
            backButton.Update();
        }

        private void OnButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }
    }
}
