using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;
using TerminalGame.Utils.TextHandler;
using static TerminalGame.Utils.TextHandler.KeyboardInput;

namespace TerminalGame.UI.Elements.Modules
{
    class Terminal : Module
    {
        private TextBox _textBox;
        private SpriteFont _terminalFont;
        private string _promptText;
        private int _promptWidth;
        private Rectangle _terminalInputArea, _terminalOutputArea;
        private List<string> _history;

        public Terminal(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {

        }

        public override void Initialize()
        {
            KeyboardInput.Initialize(Game, 500f, 20);

            _terminalFont = FontManager.GetFont("FontS");

            _history = new List<string>();

            _promptText = "root@localhost:/$ ";
            _promptWidth = (int)_terminalFont.MeasureString(_promptText).X;

            int tbHeight = (int)_terminalFont.MeasureString("A").Y;

            _terminalInputArea = new Rectangle(new Point(Rectangle.X + _promptWidth + 2, Rectangle.Y + Rectangle.Height - tbHeight), new Point(Rectangle.Width - _promptWidth, tbHeight));
            _terminalOutputArea = new Rectangle(new Point(Rectangle.X + 2, Rectangle.Y + 2 + _header.Rectangle.Height), new Point(Rectangle.Width - 4, Rectangle.Height - 4 - tbHeight - _header.Rectangle.Height));

            _textBox = new TextBox(_terminalInputArea, (int)(Rectangle.Width / _terminalFont.MeasureString("A").X) * 10, "", GraphicsDevice, _terminalFont, 
                Color.White, Color.Gray, 30);
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Active = true;
            _textBox.EnterDown += Enter_Pressed;

            base.Initialize();

            Console.WriteLine("Terminal initialized");
            Console.WriteLine("Terminal max chars: " + ((int)((_terminalOutputArea.Width - _promptWidth) / _terminalFont.MeasureString("A").X)).ToString() + " (" + ((int)(_terminalOutputArea.Width / _terminalFont.MeasureString("A").X)).ToString() + ")");
        }

        private void Enter_Pressed(object sender, KeyEventArgs e)
        {
            _history.Add(_textBox.Text.String);
            _textBox.Text.RemoveCharacters(0, _textBox.Text.Length);
            _textBox.Cursor.TextCursor = 0;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardInput.Update();
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Cursor.Color = Color.White * _opacity;
            _textBox.Cursor.Selection = Color.Gray * _opacity;
            _textBox.Update();
        }

        private string HistoryToString(List<string> hist)
        {
            string retval = "";
            foreach (var l in hist)
                retval += "\n" + _promptText + l;
            return retval;
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            string text = HistoryToString(_history);
            _spriteBatch.DrawString(_terminalFont, text, new Vector2(_terminalOutputArea.X, _terminalOutputArea.Y + _terminalOutputArea.Height - _terminalFont.MeasureString(text).Y), Color.White * _opacity);
            _spriteBatch.DrawString(_terminalFont, _promptText, new Vector2(Rectangle.X + 2, Rectangle.Y + Rectangle.Height - (int)_terminalFont.MeasureString("A").Y), Color.White * _opacity);
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
    }
}
