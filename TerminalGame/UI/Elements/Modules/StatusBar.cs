using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.Time;
using TerminalGame.UI.Elements.Buttons;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules
{
    class StatusBar : Module
    {
        private double _dateWidth, _timeWidth;
        private Button _pause, _realTime, _single, _double, _triple;
        private List<Button> _buttons;

        public StatusBar(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            BackgroundColor = _themeManager.CurrentTheme.ModuleHeaderBackgroundColor;
            _titleFont = FontManager.GetFont("FontM");
            FontColor = _themeManager.CurrentTheme.ModuleFontColor;
            
            _dateWidth = _titleFont.MeasureString(GameClock.GameTime.ToShortDateString()).X;
            _timeWidth = _titleFont.MeasureString(GameClock.GameTime.ToShortTimeString()).X;

            // TODO: Make a SplitButton to replace this.
            _pause    = new Button(Game, "||", new Point(Rectangle.X + 10 + (int)_dateWidth, Rectangle.Y + 5), new Point(35));
            _realTime = new Button(Game, "RT", new Point(Rectangle.X + 10 + (int)_dateWidth + 40, Rectangle.Y + 5), new Point(35));
            _single   = new Button(Game, ">", new Point(Rectangle.X + 10 + (int)_dateWidth + 80, Rectangle.Y + 5), new Point(35));
            _double   = new Button(Game, ">>", new Point(Rectangle.X + 10 + (int)_dateWidth + 120, Rectangle.Y + 5), new Point(35));
            _triple   = new Button(Game, ">>>", new Point(Rectangle.X + 10 + (int)_dateWidth + 160, Rectangle.Y + 5), new Point(35));

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
            _spriteBatch.DrawString(_titleFont, GameClock.GameTime.ToShortDateString(), new Vector2(Rectangle.X + 5, Rectangle.Y + 5), FontColor * _opacity);
            _spriteBatch.DrawString(_titleFont, GameClock.GameTime.ToShortTimeString(), new Vector2(Rectangle.X + 5, Rectangle.Y + 25), FontColor * _opacity);
            foreach (var b in _buttons)
                b.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var b in _buttons)
                b.Update(gameTime);
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
