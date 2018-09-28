using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Utilities;

namespace TerminalGame.Computers
{
    class RemoteUI
    {
        private SpriteFont _titleFont, _subtitleFont;
        private string _title, _subtitle, _userLevel;
        private Computer _computer;
        
        public RemoteUI(Computer computer)
        {
            _computer = computer;
            _titleFont = FontManager.GetFont(FontManager.FontSize.Large);
            _subtitleFont = FontManager.GetFont(FontManager.FontSize.Medium);
            _title = _computer.Name;
            _subtitle = _computer.IP;
            _userLevel = "test";
        }

        public void Update(GameTime gameTime)
        {
            _userLevel = _computer.PlayerHasRoot ? "You have root access on this system" : "Access denied";
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle container)
        {
            spriteBatch.DrawString(_titleFont, _title, new Vector2(10 + container.X, 20 + container.Y), Color.White);
            spriteBatch.DrawString(_subtitleFont, _subtitle, new Vector2(10 + container.X, 20 + container.Y + _titleFont.MeasureString("A").Y), Color.White);
            spriteBatch.DrawString(_subtitleFont, _userLevel, new Vector2(20 + 10 + container.X + _subtitleFont.MeasureString(_subtitle).Length(),_titleFont.MeasureString("A").Y + container.Y + 20), _computer.PlayerHasRoot ? Color.Lime : Color.Red);
        }
    }
}
