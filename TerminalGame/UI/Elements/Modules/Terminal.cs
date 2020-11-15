using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;
using TerminalGame.Parsing;
using TerminalGame.Utils;
using TerminalGame.Utils.TextHandler;
using static TerminalGame.Utils.TextHandler.KeyboardInput;

namespace TerminalGame.UI.Elements.Modules
{
    public class Terminal : Module
    {
        private TextBox _textBox;
        private SpriteFont _terminalFont;
        private string _promptText;
        private int _promptWidth, _histIndex, _maxChars, _maxCharsP;
        private Rectangle _terminalInputArea, _terminalOutputArea;
        private List<string> _history, _output;
        private Computer _computer;

        public Terminal(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : 
            base(game, location, size, title, hasHeader, hasBorder)
        {
            //Nothing to do here
        }

        public override void Initialize()
        {
            KeyboardInput.Initialize(Game, 500f, 20);

            _computer = World.World.GetInstance().Player.ConnectedComp;

            _terminalFont = FontManager.GetFont("FontS");

            _history = new List<string>();
            _output = new List<string>();

            _histIndex = -1;

            _history.Add("");

            _promptText = BuildPrompt();
            _promptWidth = (int)_terminalFont.MeasureString(_promptText).X;

            int tbHeight = (int)_terminalFont.MeasureString("A").Y;

            _terminalInputArea = new Rectangle(new Point(Rectangle.X + _promptWidth + 2, Rectangle.Y + Rectangle.Height - tbHeight), 
                new Point(Rectangle.Width - _promptWidth, tbHeight));
            _terminalOutputArea = new Rectangle(new Point(Rectangle.X + 2, Rectangle.Y + 2 + _header.Rectangle.Height), 
                new Point(Rectangle.Width - 4, Rectangle.Height - 4 - tbHeight - _header.Rectangle.Height));

            _textBox = new TextBox(_terminalInputArea, (int)(Rectangle.Width / _terminalFont.MeasureString("A").X) * 10, 
                "", GraphicsDevice, _terminalFont, Color.White, Color.Gray, 30);
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Active = true;
            _textBox.EnterDown += Enter_Pressed;
            _textBox.UpArrow += Up_Pressed;
            _textBox.DnArrow += Down_Pressed;
            _textBox.TabDown += Tab_Pressed;

            _maxChars = (int)(_terminalOutputArea.Width / _terminalFont.MeasureString("A").X);
            _maxCharsP = (int)((_terminalOutputArea.Width - _promptWidth) / _terminalFont.MeasureString("A").X);

            base.Initialize();

            Console.WriteLine("Terminal initialized");
            WriteLine("Terminal initialized");
            Console.WriteLine("Terminal max chars: " + 
                ((int)((_terminalOutputArea.Width - _promptWidth) / _terminalFont.MeasureString("A").X)).ToString() + 
                " (" + ((int)(_terminalOutputArea.Width / _terminalFont.MeasureString("A").X)).ToString() + ")");

            WriteLine("Terminal max chars: " +
                ((int)((_terminalOutputArea.Width - _promptWidth) / _terminalFont.MeasureString("A").X)).ToString() +
                " (" + ((int)(_terminalOutputArea.Width / _terminalFont.MeasureString("A").X)).ToString() + ")");
        }

        private string BuildPrompt()
        {
            string retval = _computer.AccessLevel.ToString().ToLower() + "@" + _computer.IP + ":";
            string path = _computer.FileSystem.CurrentDir.GetFullPath();
            retval += path == "" ? "/" : path;
            return retval += "$ ";
        }

        private void Tab_Pressed(object sender, KeyEventArgs e)
        {
            // TODO: Auto-finish commands/file names.
        }

        private void Down_Pressed(object sender, KeyEventArgs e)
        {
            if (--_histIndex < -1)
            {
                _histIndex = -1;
                return;
            }
            if(_histIndex == -1)
            {
                _textBox.Text.RemoveCharacters(0, _textBox.Text.Length);
                _textBox.Cursor.TextCursor = 0;
                return;
            }

            _textBox.Text.String = _history[_histIndex];
            _textBox.Cursor.TextCursor = _textBox.Text.String.Length;
        }

        private void Up_Pressed(object sender, KeyEventArgs e)
        {
            if (++_histIndex > _history.Count - 1)
            {
                _histIndex = _history.Count - 1;
                return;
            }
            _textBox.Text.String = _history[_histIndex];
            _textBox.Cursor.TextCursor = _textBox.Text.String.Length;
        }

        private void Enter_Pressed(object sender, KeyEventArgs e)
        {
            _histIndex = -1;
            if (_history[0] != _textBox.Text.String)
            {
                _history.Reverse();
                _history.Add(_textBox.Text.String);
                _history.Reverse();
            }
            WriteLine(_promptText + _textBox.Text.String);
            RunCommand(_textBox.Text.String);
            _textBox.Text.RemoveCharacters(0, _textBox.Text.Length);
            _textBox.Cursor.TextCursor = 0;
            _promptText = BuildPrompt();
            UpdateTextBox();
        }

        private string WordWrap(string text)
        {
            if (text.Length < _maxCharsP)
                return text;
            List<string> temp = new List<string>();

            string[] holder = text.Split('\n');
            foreach (var line in holder)
            {
                for (int i = 0; i < Math.Ceiling((decimal)(line.Length) / _maxChars); i++)
                {
                    temp.Add(line.Substring(i * _maxChars, Math.Min(line.Length - (i * _maxChars), _maxChars)));
                }
            }
            return string.Join("\n", temp);
        }

        public void Clear()
        {
            _output.Clear();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Unload(); // Unsubscribe from text event
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Computer computer = World.World.GetInstance().Player.ConnectedComp;
            if(_computer != computer)
                UpdateTextBox();
            KeyboardInput.Update();
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Cursor.Color = Color.White * _opacity;
            _textBox.Cursor.Selection = Color.Gray * _opacity;
            _textBox.Update();
        }

        /// <summary>
        /// Creates a new textbox, and disposes of the old one.
        /// </summary>
        private void UpdateTextBox()
        {
            _textBox.Dispose();
            _textBox = null;
            _computer = World.World.GetInstance().Player.ConnectedComp;
            _promptText = BuildPrompt();
            _promptWidth = (int)_terminalFont.MeasureString(_promptText).X;
            int tbHeight = (int)_terminalFont.MeasureString("A").Y;
            _terminalInputArea = new Rectangle(new Point(Rectangle.X + _promptWidth + 2, Rectangle.Y + Rectangle.Height - tbHeight),
                 new Point(Rectangle.Width - _promptWidth, tbHeight));
            _textBox = new TextBox(_terminalInputArea, (int)(Rectangle.Width / _terminalFont.MeasureString("A").X) * 10,
                "", GraphicsDevice, _terminalFont, Color.White, Color.Gray, 30);
            
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Active = true;
            _textBox.EnterDown += Enter_Pressed;
            _textBox.UpArrow += Up_Pressed;
            _textBox.DnArrow += Down_Pressed;
            _textBox.TabDown += Tab_Pressed;
        }

        /// <summary>
        /// Writes the specified text to the terminal.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void Write(string text)
        {
            _output.Add(WordWrap(text));
        }

        /// <summary>
        /// Writes the specified text to the terminal with a prepended newline
        /// NOTE: PRE-prended newline!
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text)
        {
            _output.Add("\n" + WordWrap(text));
        }

        /// <summary>
        /// Pass a command to the terminal. It will be interpreted as if the user typed it,
        /// but will not add it to the terminal's command history, nor print it in the
        /// terminal's output.
        /// </summary>
        /// <param name="command">The command (and args) to be evaluated.</param>
        public void RunCommand(string command)
        {
            if (CommandParser.TryTokenize(command, command.Contains("\""), out CommandToken token))
            {
                CommandParser.Parse(token, Game);
                UpdateTextBox();
            }
            else
                Console.WriteLine("invalid command \"command\"!");
        }

        /// <summary>
        /// Pass a command to the terminal. It will be interpreted as if the user typed it,
        /// and will also add it to the terminal's command history, and print it in the
        /// terminal's output.
        /// </summary>
        /// <param name="command">The command (and args) to be evaluated.</param>
        public void RunAndPrintCommand(string command)
        {
            _history.Reverse();
            _history.Add(_textBox.Text.String);
            _history.Reverse();
            WriteLine(WordWrap(_promptText + command));
            RunCommand(command);
        }

        private string HistoryToString(List<string> hist)
        {
            return string.Join("", hist);
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            string text = HistoryToString(_output);
            _spriteBatch.DrawString(_terminalFont, text, 
                new Vector2(_terminalOutputArea.X, _terminalOutputArea.Y + _terminalOutputArea.Height - _terminalFont.MeasureString(text).Y), 
                Color.White * _opacity);
            _spriteBatch.DrawString(_terminalFont, _promptText, 
                new Vector2(Rectangle.X + 2, Rectangle.Y + Rectangle.Height - (int)_terminalFont.MeasureString("A").Y), 
                Color.White * _opacity);
            _textBox.Draw(_spriteBatch);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
