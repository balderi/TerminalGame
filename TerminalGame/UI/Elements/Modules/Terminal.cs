using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;
using TerminalGame.Utils.TextHandler;

namespace TerminalGame.UI.Elements.Modules
{
    class Terminal : Module
    {
        private TextBox _textBox;
        private SpriteFont _terminalFont;
        private string _promptText;
        private int _promptWidth;

        public Terminal(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {

        }

        public override void Initialize()
        {
            KeyboardInput.Initialize(Game, 500f, 20);
            _terminalFont = FontManager.GetFont("FontS");
            _promptText = "root@localhost:/$ ";
            _promptWidth = (int)_terminalFont.MeasureString(_promptText).X;
            int tbHeight = (int)_terminalFont.MeasureString("A").Y;
            Rectangle tbArea = new Rectangle(new Point(Rectangle.X + _promptWidth + 2, Rectangle.Y + Rectangle.Height - tbHeight), new Point(Rectangle.Width - _promptWidth, tbHeight));
            _textBox = new TextBox(tbArea, (int)(Rectangle.Width / _terminalFont.MeasureString("A").X) * 10, "", GraphicsDevice, _terminalFont, 
                Color.White, Color.Gray, 30);
            _textBox.Renderer.Color = Color.White * _opacity;
            _textBox.Active = true;
            base.Initialize();
            Console.WriteLine("Terminal initialized");
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

        public override void ScissorDraw(GameTime gameTime)
        {
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
