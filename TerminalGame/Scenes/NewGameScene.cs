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
        private bool _dumbUserIsDumb;
        private readonly string _terms;
        private float _scroll;

        public NewGameScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics) : base()
        {
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;
            _dumbUserIsDumb = false;

            _terms = "Terms and Conditions\n\nPlease read these terms and conditions\ncarefully before using the service\noperated by us.\n\nYour access to and use of the service\nis conditioned on your acceptance of\nand compliance with these terms.These\nterms apply to all visitors, users and\nothers who access or use the service.\n\nBy accessing or using the service you\nagree to be bound by these terms.If\nyou disagree with any part of the terms\nthen you may not access the service.\n\n\nAccounts\n\nWhen you create an account with us, you\nmust provide us information that is\naccurate, complete, and current at all\ntimes.Failure to do so constitutes a\nbreach of the terms, which may result\nin immediate termination of your\naccount on our service.\n\nYou are responsible for safeguarding\nthe password that you use to access the\nservice and for any activities or\nactions under your password, whether\nyour password is with our service or a\nthird - party service.\n\nYou agree not to disclose your password\nto any third party.You must notify us\nimmediately upon becoming aware of any\nbreach of security or unauthorized use\nof your account.\n\n\nTermination\n\nWe may terminate or suspend access to\nour service immediately, without prior\nnotice or liability, for any reason\nwhatsoever, including without\nlimitation if you breach the terms.\n\nAll provisions of the terms which by\ntheir nature should survive termination\nshall survive termination, including,\nwithout limitation, ownership\nprovisions, warranty disclaimers,\nindemnity and limitations of liability.\n\nWe may terminate or suspend your\naccount immediately, without prior\nnotice or liability, for any reason\nwhatsoever, including without\nlimitation if you breach the terms.\n\nUpon termination, your right to use the\nservice will immediately cease. If you\nwish to terminate your account, you may\nsimply discontinue using the service.\n\nAll provisions of the terms which by\ntheir nature should survive termination\nshall survive termination, including,\nwithout limitation, ownership\nprovisions, warranty disclaimers,\nindemnity and limitations of liability.\n\n\nGoverning Law\n\nThese terms shall be governed and\nconstrued in accordance with the law,\nwithout regard to its conflict of law\nprovisions.\n\nOur failure to enforce any right or\nprovision of these terms will not be\nconsidered a waiver of those rights. If\nany provision of these terms is held to\nbe invalid or unenforceable by a court,\nthe remaining provisions of these terms\nwill remain in effect.These terms\nconstitute the entire agreement between\nus regarding our service, and supersede\nand replace any prior agreements we\nmight have between us regarding the\nservice.\n\nChanges\n\nWe reserve the right, at our sole\ndiscretion, to modify or replace these\nterms at any time. If a revision is\nmaterial we will try to provide at\nleast 30 days notice prior to any new\nterms taking effect.What constitutes\na material change will be determined at\nour sole discretion.\n\nBy continuing to access or use our\nservice after those revisions become\neffective, you agree to be bound by the\nrevised terms. If you do not agree to\nthe new terms, please stop using the\nservice.";
            _scroll = _gameWindow.ClientBounds.Height + 100;

            _username = new TextBox(new Rectangle(200, 200, 300, (int)FontManager.GetFont(FontManager.FontSize.Medium).MeasureString("A").Y), 32, "", graphics, FontManager.GetFont(FontManager.FontSize.Medium), Color.LightGray, Color.Green, 10);
            _password = new TextBox(new Rectangle(200, 230, 300, (int)FontManager.GetFont(FontManager.FontSize.Medium).MeasureString("A").Y), 32, "", graphics, FontManager.GetFont(FontManager.FontSize.Medium), Color.LightGray, Color.Green, 10);
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
            float tw = FontManager.GetFont(FontManager.FontSize.Medium).MeasureString("Enter a username and password first.").Length();
            float th = FontManager.GetFont(FontManager.FontSize.Medium).MeasureString("A").Y;

            Vector2 vector2s = new Vector2(_continueButton.Position.X - tw - 20 + TestClass.ShakeStuff(2),
                    _continueButton.Position.Y + (_continueButton.Rectangle.Height / 2) - (th / 2) + TestClass.ShakeStuff(2));
            Vector2 vector2 = new Vector2(_continueButton.Position.X - tw - 20,
                _continueButton.Position.Y + (_continueButton.Rectangle.Height / 2) - (th / 2));

            if (_dumbUserIsDumb)
            {
                spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Enter a username and password first.", vector2s, Color.Green);
                spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Enter a username and password first.", vector2, Color.LightGray);
            }

            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.XSmall), _terms, new Vector2(550f, _scroll -= 1.5f), Color.LightGray);

            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Create a new account.", new Vector2(100f, 170f), Color.LightGray);
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Username:", new Vector2(100f, 200f), Color.LightGray);
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Password:", new Vector2(100f, 230f), Color.LightGray);
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "Press 'Continue' to start.", new Vector2(100f, 260f), Color.LightGray);
            _username.Draw(spriteBatch);
            _password.Draw(spriteBatch);
            Drawing.DrawBorder(spriteBatch, _username.Area, Drawing.DrawBlankTexture(_graphics), 1, Color.White);
            Drawing.DrawBorder(spriteBatch, _password.Area, Drawing.DrawBlankTexture(_graphics), 1, Color.White);
            foreach (MainMenuButton b in _buttonList)
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
            if (String.IsNullOrEmpty(_username.Text.String) || String.IsNullOrEmpty(_password.Text.String))
            {
                _dumbUserIsDumb = true;
            }
            else
            {
                Player.GetInstance().CreateNewPlayer(_username.Text.String, _password.Text.String);
                Thread loadThread = new Thread(new ThreadStart(WhatTheFuck.GetInstance().StartNewGame));
                _stateMachine.Transition(GameState.GameLoading);
                loadThread.Start();
            }
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
