using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TerminalGame.Time;
using TerminalGame.UI.Elements.Buttons;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules
{
    public class StatusBar : Module
    {
        private double _dateWidth;
        private Button _pause, _realTime, _single, _double, _triple;
        private List<Button> _buttons;
        private string _connectionInfo, _buildNumber, _playerDeets;
        private SpriteFont _playerDetailFont;

        public StatusBar(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            _buildNumber = $"Version {Game.Version}\n  Build {Game.BuildNumber}";

            _playerDeets = $"   Name: {World.World.GetInstance().Player.Name}\nBalance: ${World.World.GetInstance().Player.Balance}";

            _connectionInfo = "";
            _titleFont = FontManager.GetFont("FontM");
            _playerDetailFont = FontManager.GetFont("FontXS");
            FontColor = _themeManager.CurrentTheme.ModuleFontColor;

            _dateWidth = _titleFont.MeasureString(GameClock.GameTime.ToShortDateString()).X;

            // TODO: Make a SplitButton to replace this.
            _pause = new Button(Game, "||", new Point(Rectangle.X + 10 + (int)_dateWidth, Rectangle.Y + 5), new Point(35));
            _realTime = new Button(Game, "RT", new Point(Rectangle.X + 10 + (int)_dateWidth + 40, Rectangle.Y + 5), new Point(35));
            _single = new Button(Game, ">", new Point(Rectangle.X + 10 + (int)_dateWidth + 80, Rectangle.Y + 5), new Point(35));
            _double = new Button(Game, ">>", new Point(Rectangle.X + 10 + (int)_dateWidth + 120, Rectangle.Y + 5), new Point(35));
            _triple = new Button(Game, ">>>", new Point(Rectangle.X + 10 + (int)_dateWidth + 160, Rectangle.Y + 5), new Point(35));

            _pause.ButtonPressed += Pause_clicked;
            _realTime.ButtonPressed += RealTime_clicked;
            _single.ButtonPressed += Single_clicked;
            _double.ButtonPressed += Double_clicked;
            _triple.ButtonPressed += Triple_clicked;

            _buttons = new List<Button>
            {
                _pause,
                _realTime,
                _single,
                _double,
                _triple
            };

            foreach (var b in _buttons)
                b.Initialize();
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            BackgroundColor = _themeManager.CurrentTheme.ModuleHeaderBackgroundColor;
            _spriteBatch.DrawString(_titleFont, GameClock.GameTime.ToString("yyyy/MM/dd"), new Vector2(Rectangle.X + 5, Rectangle.Y + 5), FontColor * Opacity);
            _spriteBatch.DrawString(_titleFont, GameClock.GameTime.ToString("HH:mm"), new Vector2(Rectangle.X + 5, Rectangle.Y + 25), FontColor * Opacity);
            foreach (var b in _buttons)
                b.Draw(gameTime);
            _spriteBatch.DrawString(_playerDetailFont, _connectionInfo, new Vector2(_buttons[4].Rectangle.X + _buttons[4].Rectangle.Width + 10,
                    5), _themeManager.CurrentTheme.ModuleHeaderFontColor * Opacity);
            _spriteBatch.DrawString(_playerDetailFont, _buildNumber,
                new Vector2(Rectangle.Right - _playerDetailFont.MeasureString(_buildNumber).Length() - 5, 5),
                _themeManager.CurrentTheme.ModuleHeaderFontColor * Opacity);
            _spriteBatch.DrawString(_playerDetailFont, _playerDeets,
                new Vector2(Rectangle.Right - _playerDetailFont.MeasureString(_buildNumber).Length() - 5 - _playerDetailFont.MeasureString(_playerDeets).Length() - 20, 5),
                _themeManager.CurrentTheme.ModuleHeaderFontColor * Opacity);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var b in _buttons)
            {
                b.Opacity = Opacity;
                b.Update(gameTime);
            }
            _connectionInfo = World.World.GetInstance().Player.ConnectedComp.GetPublicName() + "\n" + World.World.GetInstance().Player.ConnectedComp.IP;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        private void Pause_clicked(ButtonPressedEventArgs e)
        {
            Game.CurrentGameSpeed = GameSpeed.Paused;
            foreach (var b in _buttons)
                b.UnDim();
            _pause.Dim();
        }
        private void RealTime_clicked(ButtonPressedEventArgs e)
        {
            Game.CurrentGameSpeed = GameSpeed.RealTime;
            foreach (var b in _buttons)
                b.UnDim();
            _realTime.Dim();
        }
        private void Single_clicked(ButtonPressedEventArgs e)
        {
            Game.CurrentGameSpeed = GameSpeed.Single;
            foreach (var b in _buttons)
                b.UnDim();
            _single.Dim();
        }
        private void Double_clicked(ButtonPressedEventArgs e)
        {
            Game.CurrentGameSpeed = GameSpeed.Double;
            foreach (var b in _buttons)
                b.UnDim();
            _double.Dim();
        }
        private void Triple_clicked(ButtonPressedEventArgs e)
        {
            Game.CurrentGameSpeed = GameSpeed.Triple;
            foreach (var b in _buttons)
                b.UnDim();
            _triple.Dim();
        }
    }
}
