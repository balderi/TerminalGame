using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalGame.States;
using TerminalGame.UI;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using static TerminalGame.Utilities.TextHandler.KeyboardInput;

namespace TerminalGame.Scenes
{
    class NewGameScene : Scene
    {
        private readonly SpriteFont _font;
        private readonly GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;
        private Keys _prevKbState, _newKbState;
        private MainMenuButton _backButton, _continueButton;
        private List<MainMenuButton> _buttonList;
        private TextBox _username, _password;

        public NewGameScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics) : base()
        {
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;

            _username = new TextBox(new Rectangle(200, 125, 300, 50), 30, "", graphics, FontManager.GetFont(FontManager.FontSize.Medium), Color.LightGray, Color.Green, 10);
            _password = new TextBox(new Rectangle(200, 175, 300, 50), 30, "", graphics, FontManager.GetFont(FontManager.FontSize.Medium), Color.LightGray, Color.Green, 10);
            _username.UpArrow += OnPressed;
            _username.DnArrow += OnPressed;
            _username.TabDown += OnPressed;
            _password.UpArrow += OnPressed;
            _password.DnArrow += OnPressed;
            _password.TabDown += OnPressed;
            _username.Active = true;
            _password.Active = false;
            _username.Renderer.Color = Color.LightGray;
            _password.Renderer.Color = Color.LightGray;
            _buttonList = new List<MainMenuButton>();

            _backButton = new MainMenuButton("< Back", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50, _graphics.Viewport.Height - 50 - 50)
            };
            _backButton.Click += OnBackButtonClick;
            _buttonList.Add(_backButton);
            _continueButton = new MainMenuButton("Continue >", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(_graphics.Viewport.Width - 250, _graphics.Viewport.Height - 50 - 50)
            };
            _continueButton.Click += OnContButtonClick;
            _buttonList.Add(_continueButton);
            _password.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Username:", new Vector2(100f, 125f), Color.LightGray);
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Password:", new Vector2(100f, 175f), Color.LightGray);
            _username.Draw(spriteBatch);
            _password.Draw(spriteBatch);
            foreach(MainMenuButton b in _buttonList)
            {
                b.Draw(spriteBatch);
            }
            Vector2 textMiddlePoint = _font.MeasureString("Create Account") / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(_font, "Create Account", position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_font, "Create Account", position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            //if(_username.Active)
                _username.Update();
            //if(_password.Active)
                _password.Update();
            foreach (MainMenuButton b in _buttonList)
            {
                b.Update();
            }
        }

        private void OnBackButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }

        private void OnContButtonClick(ButtonPressedEventArgs e)
        {
            Player.GetInstance().CreateNewPlayer(_username.Text.String, _password.Text.String);
            Thread loadThread = new Thread(new ThreadStart(WhatTheFuck.GetInstance().StartNewGame));
            _stateMachine.Transition(GameState.GameLoading);
            loadThread.Start();
        }

        private void OnPressed(object sender, KeyEventArgs e)
        {
            _newKbState = e.KeyCode;
            if (_newKbState != _prevKbState)
            {
                _username.Active = !_username.Active;
                _password.Active = !_username.Active;
                _username.Cursor.TextCursor = _username.Text.Length;
                _password.Cursor.TextCursor = _password.Text.Length;
            }
            _prevKbState = _newKbState;
        }
    }
}
