﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            _computer = Player.GetInstance().ConnectedComp;

            _terminalFont = FontManager.GetFont("FontS");

            _history = new List<string>();
            _output = new List<string>();

            _histIndex = -1;

            _promptText = _computer.AccessLevel.ToString().ToLower() + "@" + _computer.IP + ":/$ ";
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
            _history.Reverse();
            _history.Add(_textBox.Text.String);
            _history.Reverse();
            WriteLine(WordWrap(_promptText + _textBox.Text.String));
            RunCommand(_textBox.Text.String);
            _textBox.Text.RemoveCharacters(0, _textBox.Text.Length);
            _textBox.Cursor.TextCursor = 0;
            UpdateTextBox();
        }

        private string WordWrap(string text)
        {
            if (text.Length < _maxCharsP)
                return text;
            string holder = text;
            List<string> temp = new List<string>();
            for(int i = 0; i < Math.Ceiling((decimal)(text.Length) / _maxChars); i++)
            {
                temp.Add(holder.Substring(i * _maxChars, Math.Min(holder.Length - (i * _maxChars), _maxChars)));
            }
            return String.Join("\n", temp);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Computer computer = Player.GetInstance().ConnectedComp;
            if(_computer != computer)
                UpdateTextBox();
            KeyboardInput.Update();
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Cursor.Color = Color.White * _opacity;
            _textBox.Cursor.Selection = Color.Gray * _opacity;
            _textBox.Update();
        }

        private void UpdateTextBox()
        {
            _textBox.Dispose();
            _textBox = null;
            _computer = Player.GetInstance().ConnectedComp;
            _promptText = _computer.AccessLevel.ToString().ToLower() + "@" + _computer.IP + ":/$ ";
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
            _output.Add(text);
        }

        /// <summary>
        /// Writes the specified text to the terminal with a prepended newline
        /// NOTE: PRE-prended newline!
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text)
        {
            _output.Add("\n" + text);
        }

        /// <summary>
        /// Pass a command to the terminal. It will be interpreted as if the user typed it,
        /// but will not add it to the terminal's command history, nor print it in the
        /// terminal's output.
        /// </summary>
        /// <param name="command">The command (and args) to be evaluated.</param>
        public void RunCommand(string command)
        {
            if (CommandParser.TryTokenize(command, false, out CommandToken token))
            {
                CommandParser.Parse(token, Game);
                UpdateTextBox();
            }
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
            return String.Join("", hist);
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
