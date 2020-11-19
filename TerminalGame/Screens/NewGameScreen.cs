using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalGame.UI.Elements;
using TerminalGame.UI.Elements.Buttons;
using TerminalGame.Utils;
using TerminalGame.Utils.TextHandler;
using static TerminalGame.Utils.TextHandler.KeyboardInput;

namespace TerminalGame.Screens
{
    class NewGameScreen : Screen
    {
        private Button _backButton, _continueButton;
        private Keys _newKbState, _prevKbState;
        private List<Button> _buttonList;
        private TextBox _username, _password;
        private bool _dumbUserIsDumb;
        private readonly string _terms;
        private float _scroll;
        private readonly SpriteFont _font, _titleFont, _versionFont;
        private readonly string _title, _version;

        public NewGameScreen(Game game, bool fadeIn = false) : base(game, fadeIn)
        {
            _dumbUserIsDumb = false;
            
            _font = FontManager.GetFont("FontM");
            _titleFont = FontManager.GetFont("FontXL");
            _versionFont = FontManager.GetFont("FontM");
            _terms = "Terms and Conditions\n\nPlease read these terms and conditions\ncarefully before using the service\noperated by us.\n\nYour access to and use of the service\nis conditioned on your acceptance of\nand compliance with these terms.These\nterms apply to all visitors, users and\nothers who access or use the service.\n\nBy accessing or using the service you\nagree to be bound by these terms.If\nyou disagree with any part of the terms\nthen you may not access the service.\n\n\nAccounts\n\nWhen you create an account with us, you\nmust provide us information that is\naccurate, complete, and current at all\ntimes.Failure to do so constitutes a\nbreach of the terms, which may result\nin immediate termination of your\naccount on our service.\n\nYou are responsible for safeguarding\nthe password that you use to access the\nservice and for any activities or\nactions under your password, whether\nyour password is with our service or a\nthird - party service.\n\nYou agree not to disclose your password\nto any third party.You must notify us\nimmediately upon becoming aware of any\nbreach of security or unauthorized use\nof your account.\n\n\nTermination\n\nWe may terminate or suspend access to\nour service immediately, without prior\nnotice or liability, for any reason\nwhatsoever, including without\nlimitation if you breach the terms.\n\nAll provisions of the terms which by\ntheir nature should survive termination\nshall survive termination, including,\nwithout limitation, ownership\nprovisions, warranty disclaimers,\nindemnity and limitations of liability.\n\nWe may terminate or suspend your\naccount immediately, without prior\nnotice or liability, for any reason\nwhatsoever, including without\nlimitation if you breach the terms.\n\nUpon termination, your right to use the\nservice will immediately cease. If you\nwish to terminate your account, you may\nsimply discontinue using the service.\n\nAll provisions of the terms which by\ntheir nature should survive termination\nshall survive termination, including,\nwithout limitation, ownership\nprovisions, warranty disclaimers,\nindemnity and limitations of liability.\n\n\nGoverning Law\n\nThese terms shall be governed and\nconstrued in accordance with the law,\nwithout regard to its conflict of law\nprovisions.\n\nOur failure to enforce any right or\nprovision of these terms will not be\nconsidered a waiver of those rights. If\nany provision of these terms is held to\nbe invalid or unenforceable by a court,\nthe remaining provisions of these terms\nwill remain in effect.These terms\nconstitute the entire agreement between\nus regarding our service, and supersede\nand replace any prior agreements we\nmight have between us regarding the\nservice.\n\nChanges\n\nWe reserve the right, at our sole\ndiscretion, to modify or replace these\nterms at any time. If a revision is\nmaterial we will try to provide at\nleast 30 days notice prior to any new\nterms taking effect.What constitutes\na material change will be determined at\nour sole discretion.\n\nBy continuing to access or use our\nservice after those revisions become\neffective, you agree to be bound by the\nrevised terms. If you do not agree to\nthe new terms, please stop using the\nservice.";
            _title = "Create Account";
            _version = Game.Version;

            _scroll = _rectangle.Height + 100;

            _username = new TextBox(new Rectangle(220, 200, 300, (int)_font.MeasureString("A").Y), 32, "", GraphicsDevice, _font, Color.LightGray, Color.Green, 10);
            _password = new TextBox(new Rectangle(220, 230, 300, (int)_font.MeasureString("A").Y), 32, "", GraphicsDevice, _font, Color.LightGray, Color.Green, 10);

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

            _buttonList = new List<Button>();

            _backButton = new Button(game, "< Back", new Point(50, _rectangle.Height - 50 - 50), new Point(200, 50));
            _backButton.LeftClick += OnBackButtonClick;
            _buttonList.Add(_backButton);

            _continueButton = new Button(game, "Continue >", new Point(_rectangle.Width - 250, _rectangle.Height - 50 - 50), new Point(200, 50));
            _continueButton.LeftClick += OnContButtonClick;
            _buttonList.Add(_continueButton);
            _password.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);

            float tw = _font.MeasureString("Enter a username and password first.").Length();
            float th = _font.MeasureString("A").Y;

            Vector2 vector2 = new Vector2(_continueButton.Rectangle.X - tw - 20,
                _continueButton.Rectangle.Y + (_continueButton.Rectangle.Height / 2) - (th / 2));

            if (_dumbUserIsDumb)
            {
                _spriteBatch.DrawString(_font, "Enter a username and password first.", vector2, Color.LightGray);
            }

            _spriteBatch.DrawString(FontManager.GetFont("FontXS"), _terms, new Vector2(550f, _scroll -= 1.5f), Color.LightGray);

            _spriteBatch.DrawString(_font, "Create a new account.", new Vector2(100f, 170f), Color.LightGray);
            _spriteBatch.DrawString(_font, "Username:", new Vector2(100f, 200f), Color.LightGray);
            _spriteBatch.DrawString(_font, "Password:", new Vector2(100f, 230f), Color.LightGray);
            _spriteBatch.DrawString(_font, "Press 'Continue' to start.", new Vector2(100f, 260f), Color.LightGray);
            _username.Draw(_spriteBatch);
            _password.Draw(_spriteBatch);

            Globals.Utils.DrawOuterBorder(_spriteBatch, _username.Area, Globals.Utils.DummyTexture(), 1, Color.White);
            Globals.Utils.DrawOuterBorder(_spriteBatch, _password.Area, Globals.Utils.DummyTexture(), 1, Color.White);
            foreach (Button b in _buttonList)
            {
                b.Draw(gameTime);
            }
            Vector2 textMiddlePoint = _titleFont.MeasureString(_title) / 2;
            Vector2 position = new Vector2((Game.Window.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((Game.Window.ClientBounds.Width - textMiddlePoint.X - 15) + Generators.ShakeStuff(3),
                textMiddlePoint.Y + 15 + Generators.ShakeStuff(3));
            Vector2 position3 = new Vector2((Game.Window.ClientBounds.Width - _versionFont.MeasureString(_version).X / 2 - 20),
                position.Y + _titleFont.MeasureString(_title).Y / 2 - 15);
            _spriteBatch.DrawString(_titleFont, _title, position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawString(_titleFont, _title, position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawString(_versionFont, _version, position3, Color.Green, 0,
                _versionFont.MeasureString(_version) / 2, 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //if(_username.Active)
            _username.Update();
            //if(_password.Active)
            _password.Update();
            foreach (Button b in _buttonList)
            {
                b.Update(gameTime);
            }
        }

        private void OnBackButtonClick(object sender, MouseEventArgs e)
        {
            ScreenManager.GetInstance().ChangeScreen("mainMenu");
        }

        private void OnContButtonClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(_username.Text.String) || string.IsNullOrEmpty(_password.Text.String))
            {
                _dumbUserIsDumb = true;
            }
            else
            {
                Player.GetInstance().CreateNewPlayer(_username.Text.String, _password.Text.String);

                Game.StartNewGame();
                ScreenManager.GetInstance().AddScreen("gameLoading", new GameLoadingScreen(Game));
                ScreenManager.GetInstance().ChangeScreenAndInit("gameLoading");
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
