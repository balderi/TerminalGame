using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    public enum InputType
    {
        std,
        login,
        passwd,
    }

    class Terminal : Module
    {
        // TODO: Add command queue for when input is blocked by running program (maybe, might be dumb)
        
        private TextBox _terminalInput;
        private Rectangle _prompt, _inputViewport, _outputViewport;
        private int _linesToDraw, _currentIndex;
        private readonly int _maxlen;
        private string _terminalOutput, _terminalPrompt, _userName, _password;
        private List<string> _history, _output;
        private SpriteFont _terminalFont;
        private bool _isMultiLine, _isInputBlocked, _updateInp;
        private Computer _connectedComputer;
        private InputType _inputType;
        
        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set; }

        public Terminal(GraphicsDevice graphics, Rectangle container, SpriteFont terminalFont) : base(graphics, container)
        {
            _terminalFont = terminalFont;
            Container = container;
            _updateInp = true;
            if (string.IsNullOrEmpty(Title))
            {
                Title = "!!! UNNAMED WINDOW !!!";
            }
            _maxlen = Container.Width / (int)_terminalFont.MeasureString("_").Length() * 2;
            _maxlen += _maxlen / 5;
        }

        /// <summary>
        /// Called when user hits the Enter key.
        /// Calls the Command Parser with the input text and then handles the output
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs</param>
        private void TerminalInput_EnterDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!_isInputBlocked)
            {
                switch(_inputType)
                {
                    case InputType.login:
                        {
                            Console.WriteLine(_terminalPrompt + _terminalInput.Text.String);
                            if (!String.IsNullOrEmpty(_terminalInput.Text.String))
                            {
                                _history.Insert(0, _terminalInput.Text.String);
                                _userName = _terminalInput.Text.String;
                                _output.Add("\n" + _terminalPrompt + _userName);
                                _currentIndex = 0;
                                UpdateOutput();
                                _terminalInput.Clear();
                                _connectedComputer = Player.GetInstance().ConnectedComputer;
                                _updateInp = true;
                                _inputType = InputType.passwd;
                            }
                            else
                            {
                                Write("\nuser name cannot be blank.");
                            }
                            break;
                        }
                    case InputType.passwd:
                        {
                            Console.WriteLine(_terminalPrompt + _terminalInput.Text.String);
                            _history.Insert(0,  _terminalInput.Text.String);
                            _password = _terminalInput.Text.String;
                            _output.Add("\n" + _terminalPrompt + _password);
                            _currentIndex = 0;
                            UpdateOutput();
                            _terminalInput.Clear();
                            _connectedComputer = Player.GetInstance().ConnectedComputer;
                            _updateInp = true;
                            _inputType = InputType.std;
                            if(!Player.GetInstance().ConnectedComputer.Login(_userName, _password))
                            {
                                Write("\nIncorrect login information.");
                            }
                            else
                            {
                                Write("\nLogin successful..");
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("CMD: " + _terminalInput.Text.String);
                            _history.Insert(0, _terminalInput.Text.String);
                            string input;
                            if (_terminalInput.Text.String.Contains("§"))
                            {
                                string temp = _terminalInput.Text.String.Replace('§', '?');
                                input = InputWrap("\n" + _terminalPrompt + temp);
                            }
                            else
                                input = InputWrap("\n" + _terminalPrompt + _terminalInput.Text.String);

                            string[] inp = input.Split('§');

                            for (int i = 0; i < inp.Length; i++)
                            {
                                if (inp[i] != "\n")
                                {
                                    _output.Add(inp[i]);
                                }
                            }

                            _currentIndex = 0;

                            if (!string.IsNullOrEmpty(_terminalInput.Text.String) && !_terminalInput.Text.String.Contains("§"))
                                CommandParser.ParseCommand(_terminalInput.Text.String);

                            UpdateOutput();
                            _terminalInput.Clear();

                            if (_isMultiLine)
                                UpdateOutput();

                            _connectedComputer = Player.GetInstance().ConnectedComputer;
                            _updateInp = true;
                            break;
                        }
                }
            }
        }

        public void Write(string text)
        {
            string[] formattedOut = Format(TextWrap(text));
            for(int i = 0; i < formattedOut.Length; i++)
            {
                _output.Add(formattedOut[i]);
                UpdateOutput();
            }
        }

        public void WritePartialLine(string text)
        {
            _output[_output.Count - 1] += text;
            UpdateOutput();
        }

        private string[] Format(string text)
        {
            return text.Split('§');
        }

        public void BlockInput()
        {
            _isInputBlocked = true;
        }

        public void UnblockInput()
        {
            _isInputBlocked = false;
        }

        private void TerminalInput_TabDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!_isInputBlocked)
            {
                // TODO: Make autocomplete work as intended
                _terminalInput.Text.String = "Autocomplete command";
                _terminalInput.Cursor.TextCursor = _terminalInput.Text.String.Length;
            }
        }

        private void TerminalInput_DnArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!_isInputBlocked)
            {
                if (_currentIndex > 0)
                {
                    _terminalInput.Text.String = _history[--_currentIndex];
                }
                _terminalInput.Cursor.TextCursor = _terminalInput.Text.String.Length;
                Console.WriteLine("DN :: CI:{0} | HC:{1}", _currentIndex, _history.Count);
            }
        }

        private void TerminalInput_UpArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!_isInputBlocked)
            {
                if (_history.Count > 0 && _currentIndex < _history.Count)
                {
                    _terminalInput.Text.String = _history[_currentIndex++];
                }
                _terminalInput.Cursor.TextCursor = _terminalInput.Text.String.Length;
                Console.WriteLine("UP :: CI:{0} | HC:{1}", _currentIndex, _history.Count);
            }
        }

        public void BeginLogin()
        {
            if(_inputType == InputType.std)
                _inputType = InputType.login;
        }

        /// <summary>
        /// Clear the terminal window
        /// </summary>
        public void Clear()
        {
            UpdateOutput();
            for (int i = 0; i < _linesToDraw + 1; i++)
            {
                _output.Add("\n");
            }
            _output.RemoveAt(0); // Leave room for the next prepended newline
            UpdateOutput();
        }

        /// <summary>
        /// Disposes of current <c>TextBox</c> and creates a brand new one with the proper dimensions.
        /// </summary>
        /// <returns>A brand new <c>TextBox</c></returns>
        private TextBox TextBox()
        {
            if (_terminalInput == null)
            {
                int maxChars = ((int)(Container.Width - _terminalFont.MeasureString(_terminalPrompt).Length()) / (int)_terminalFont.MeasureString("_").Length()) + 200;
                TextBox retval = new TextBox(_inputViewport, maxChars, "", _graphics, _terminalFont, Color.LightGray, Color.DarkGreen, 30);
                retval.Renderer.Color = Color.LightGray;
                retval.Cursor.Selection = new Color(Color.PeachPuff, .4f);
                retval.Active = true;
                retval.EnterDown += TerminalInput_EnterDown;
                retval.UpArrow += TerminalInput_UpArrow;
                retval.DnArrow += TerminalInput_DnArrow;
                retval.TabDown += TerminalInput_TabDown;
                return retval;
            }
            else
            {
                _terminalInput?.Dispose();
                _terminalInput = null;
                return TextBox();
            }
        }

        /// <summary>
        /// Initialization method for the terminal
        /// </summary>
        public void Init()
        {
            _connectedComputer = Player.GetInstance().ConnectedComputer;
            _terminalPrompt = "root@127.0.0.1 > ";
            _terminalOutput = "";
            _inputType = InputType.std;
            _history = new List<string>();
            _history.Insert(0, "");
            _output = new List<string>();
            _prompt = new Rectangle(Container.X + 3, 
                Container.Height - RenderHeader().Height, 
                (int)(_terminalFont.MeasureString(_terminalPrompt).X), 
                (int)(_terminalFont.MeasureString(_terminalPrompt).Y));
            _inputViewport = new Rectangle(_prompt.Width, _prompt.Y, 
                Container.Width - _prompt.Width, 
                (int)(_terminalFont.MeasureString("MEASURE ME").Y));

            _terminalInput = TextBox();

            _outputViewport = new Rectangle(Container.X, 
                Container.Y + RenderHeader().Height + 2, Container.Width, 
                Container.Height - (_inputViewport.Height) - RenderHeader().Height);
            
            _linesToDraw = (int)(_outputViewport.Height / _terminalFont.MeasureString("MEASURE THIS").Y);
            Console.WriteLine("INIT: " + _linesToDraw + " LN");
            Clear();
            
            foreach(Computer c in Computers.Computers.GetInstance().ComputerList)
            {
                c.Connected += ConnectedComputer_Connected;
                c.Disonnected += ConnectedComputer_Disonnected;
                c.FileSystem.ChangeDirectory += FileSystem_ChangeDir;
            }
        }

        private void FileSystem_ChangeDir(object sender, EventArgs e)
        {
            _updateInp = true;
        }

        private void ConnectedComputer_Disonnected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** DISCONNECTION EVENT FIRED");
            _connectedComputer = Player.GetInstance().ConnectedComputer;
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + 
                ", IS_RT: " + e.IsRoot.ToString());
            UpdateOutput();
            _terminalPrompt = e.IsRoot ? "root@" + e.ConnectionString + 
                " > " : "user@" + e.ConnectionString + " > ";
            _updateInp = true;
        }

        private void ConnectedComputer_Connected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** CONNECTION EVENT FIRED");
            _connectedComputer = Player.GetInstance().ConnectedComputer;
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + 
                ", IS_RT: " + e.IsRoot.ToString());
            UpdateOutput();
            _terminalPrompt = e.IsRoot ? "root@" + e.ConnectionString + 
                " > " : "user@" + e.ConnectionString + " > ";
            _updateInp = true;
        }

        public void ForceQuit()
        {
            Write("\n\nKernel panic - not syncing: Fatal exception in interrupt\n\n");
            UpdateOutput();
        }

        private string TextWrap(string text)
        {
            _isMultiLine = false;
            if (text.Length <= _maxlen || text.Contains("§"))
                return text;

            _isMultiLine = true;
            for (int i = 0; i < (text.Length / _maxlen); i++)
            {
                text = text.Insert((i + 1) * _maxlen - 1, "§\n");
            }
            return text;
        }

        private string InputWrap(string text)
        {
            _isMultiLine = false;
            if (text.Length <= _maxlen)
                return text;

            _isMultiLine = true;
            for (int i = 0; i < (text.Length / _maxlen); i++)
            {
                text = text.Insert((i + 1) * _maxlen - 1, "§\n");
            }
            return text;
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, 
                (int)Font.MeasureString(Title).Y);
        }

        /// <summary>
        /// We have to re-create the textbox each time its length should change,
        /// otherwise the text will get deformed.
        /// </summary>
        private void UpdateInputSize()
        {
            string text = _terminalInput.Text.String;
            int oldWidth = _prompt.Width;
            int newWidth = (int)(_terminalFont.MeasureString(_terminalPrompt).X);
            if (newWidth != oldWidth)
            {
                _prompt.Width = newWidth;
                _inputViewport.X = _prompt.X + _prompt.Width;
                _inputViewport.Width = Container.Width - _prompt.Width;
                _terminalInput = TextBox();
                _terminalInput.Text.String = text;
                _terminalInput.Cursor.TextCursor = text.Length;
            }
        }

        // TODO: BUG: Something breaks during enumeration sometimes.
        /// <summary>
        /// Writes new text to the terminal. Adds any entered commands to the history.
        /// </summary>
        public void UpdateOutput()
        {
            BlockInput();
            List<string> tempOut = _output;
            int lines = 0;
            try
            {
                foreach (string s in tempOut)
                {
                    if (s.Contains("\n"))
                        lines++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            while (lines > _linesToDraw - 1)
            {
                tempOut.RemoveAt(0);
                lines--;
            }
            while (lines < _linesToDraw - 1)
            {
                tempOut.Insert(0, "\n");
                lines++;
            }
            if (_output.Count > 0)
            {
                string holder = "";
                try
                {
                    foreach (string s in tempOut)
                    {
                        holder += s;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                _terminalOutput = holder;
            }
            _output = tempOut;
            UnblockInput();
        }

        private void UpdatePrompt()
        {
            switch (_inputType)
            {
                case InputType.login:
                    {
                        _terminalPrompt = "username: ";
                        break;
                    }
                case InputType.passwd:
                    {
                        _terminalPrompt = "password: ";
                        break;
                    }
                default:
                    {
                        _terminalPrompt = _connectedComputer.PlayerHasRoot ? "root" : "user";
                        _terminalPrompt += "@" + _connectedComputer.IP + 
                            _connectedComputer.FileSystem.CurrentDir.PrintFullPath() + 
                            " > ";
                        break;
                    }
            }
        }

        public override void Update(GameTime gameTime)
        {
            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
            
            _terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, 
                lerpAmount);

            //if (!_isTakingSpecialInput)
            //{
            //    _terminalPrompt = _connectedComputer.PlayerHasRoot ? "root" : "user";
            //    _terminalPrompt += "@" + _connectedComputer.IP + _connectedComputer.FileSystem.CurrentDir.PrintFullPath() + " > ";
            //}

            UpdatePrompt();

            if (_updateInp)
            {
                UpdateInputSize();
                _updateInp = false;
            }
            _terminalInput.Update();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);

                spriteBatch.Draw(texture, Container, 
                    _themeManager.CurrentTheme.ModuleBackgroundColor);

                spriteBatch.Draw(texture, RenderHeader(), 
                    _themeManager.CurrentTheme.ModuleHeaderBackgroundColor);

                spriteBatch.DrawString(Font, Title, 
                    new Vector2(RenderHeader().X + 5, RenderHeader().Y), 
                    _themeManager.CurrentTheme.ModuleHeaderFontColor);

                Drawing.DrawBorder(spriteBatch, Container, texture, 1, 
                    _themeManager.CurrentTheme.ModuleOutlineColor);

                spriteBatch.DrawString(_terminalFont, _terminalPrompt, 
                    new Vector2(_prompt.X, _prompt.Y), 
                    _themeManager.CurrentTheme.ModuleFontColor);

                spriteBatch.DrawString(_terminalFont, _terminalOutput, 
                    new Vector2(_outputViewport.X + 3 + 1, _outputViewport.Y + 1), 
                    Color.Green);

                spriteBatch.DrawString(_terminalFont, _terminalOutput, 
                    new Vector2(_outputViewport.X + 3, _outputViewport.Y), 
                    _themeManager.CurrentTheme.ModuleFontColor);

                _terminalInput.Draw(spriteBatch);
            }
        }
    }
}
