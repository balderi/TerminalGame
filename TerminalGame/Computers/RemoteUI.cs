using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.UI.Themes;
using TerminalGame.Utilities;

namespace TerminalGame.Computers
{
    class RemoteUI
    {
        private SpriteFont _titleFont, _subtitleFont;
        private readonly string _title, _subtitle;
        private string _userLevel, _traceProgress;
        private Computer _computer;
        private float _lerp;
        
        public RemoteUI(Computer computer)
        {
            _computer = computer;
            _titleFont = FontManager.GetFont(FontManager.FontSize.Large);
            _subtitleFont = FontManager.GetFont(FontManager.FontSize.Medium);
            _title = _computer.Name;
            _subtitle = _computer.IP;
            _userLevel = "test";
            _traceProgress = "No Trace";
        }

        public void Update(GameTime gameTime)
        {
            _userLevel = _computer.PlayerHasRoot ? "You have root access on this system" : "Access denied";
            if(_computer.Tracer.IsActive)
            {
                _traceProgress = "TRACE: " + _computer.Tracer.Counter + "%";
                _lerp = (float)gameTime.TotalGameTime.TotalMilliseconds % 1000 / 1000;
            }
            else
            {
                _traceProgress = "No Trace";
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle container)
        {
            spriteBatch.DrawString(_titleFont, _title, new Vector2(10 + container.X, 20 + container.Y), Color.White);
            spriteBatch.DrawString(_subtitleFont, _subtitle, new Vector2(10 + container.X, 20 + container.Y + _titleFont.MeasureString("A").Y), Color.White);
            spriteBatch.DrawString(_subtitleFont, _userLevel, new Vector2(20 + 10 + container.X + _subtitleFont.MeasureString(_subtitle).Length(),_titleFont.MeasureString("A").Y + container.Y + 20), _computer.PlayerHasRoot ? Color.Lime : Color.Red);
            if(_computer.Tracer.IsActive)
            {
                spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.XLarge), _traceProgress, 
                    new Vector2(container.X + 11, 
                    container.Y + container.Height - FontManager.GetFont(FontManager.FontSize.XLarge).MeasureString("A").Y), 
                    Color.Lerp(ThemeManager.GetInstance().CurrentTheme.WarningColor, ThemeManager.GetInstance().CurrentTheme.ModuleFontColor, _lerp));
                spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.XLarge), _traceProgress,
                    new Vector2(container.X + 10,
                    container.Y + container.Height - 1 - FontManager.GetFont(FontManager.FontSize.XLarge).MeasureString("A").Y),
                    ThemeManager.GetInstance().CurrentTheme.WarningColor);
            }
        }
    }
}
