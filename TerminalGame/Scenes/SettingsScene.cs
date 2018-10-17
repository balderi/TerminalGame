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
    class SettingsScene : Scene
    {
        
        private readonly SpriteFont _font;
        private readonly GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;
        private bool _prevKbState, _newKbState;
        private MainMenuButton _backButton, _applyButton;
        private Checkbox _fullScreenCheckBox, _bloomCheckBox;
        private List<Component> _components;

        public SettingsScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics) : base()
        {
            _components = new List<Component>();
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;
            _backButton = new MainMenuButton("< Back", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50, _graphics.Viewport.Height - 50 - 50)
            };
            _backButton.Click += OnBackButtonClick;
            _components.Add(_backButton);
            _applyButton = new MainMenuButton("Apply changes", 250, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50 + 200 + 20, _graphics.Viewport.Height - 50 - 50)
            };
            _applyButton.Click += OnApplyButtonClick;
            _components.Add(_applyButton);
            _fullScreenCheckBox = new Checkbox("Full Screen", 20, buttonFont, _graphics, GameManager.GetInstance().IsFullScreen)
            {
                Position = new Vector2(100, 100)
            };
            _components.Add(_fullScreenCheckBox);
            _bloomCheckBox = new Checkbox("Bloom", 20, buttonFont, _graphics, GameManager.GetInstance().BloomEnabled)
            {
                Position = new Vector2(100, 150)
            };
            _components.Add(_bloomCheckBox);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            Vector2 textMiddlePoint = _font.MeasureString("Settings") / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(_font, "Settings", position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_font, "Settings", position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            //spriteBatch.DrawString(_font, "SettingsScene", new Vector2(10, 10), Color.White);
            foreach(Component comp in _components)
            {
                comp.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component comp in _components)
            {
                comp.Update();
            }
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

        private void OnBackButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }

        private void OnApplyButtonClick(ButtonPressedEventArgs e)
        {
            if(_fullScreenCheckBox.Checked != GameManager.GetInstance().IsFullScreen)
            {
                GameManager.GetInstance().ToggleFullScreen();
            }
            if(_bloomCheckBox.Checked != GameManager.GetInstance().BloomEnabled)
            {
                GameManager.GetInstance().BloomEnabled = !GameManager.GetInstance().BloomEnabled;
            }
        }
    }
}
