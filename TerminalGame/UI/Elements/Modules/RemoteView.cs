using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules
{
    public class RemoteView : Module
    {
        private Player _player;
        private Computer _computer;
        //private string _text;
        private SpriteFont _font;

        public RemoteView(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _font = FontManager.GetFont("FontM");
            _player = World.World.GetInstance().Player;
            _computer = _player.ConnectedComp;
            //_text = SetString();
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            base.ScissorDraw(gameTime);
            _computer?.ViewData.Draw(_spriteBatch, Rectangle);
            //_spriteBatch.DrawString(_font, _text, new Vector2(Rectangle.X + 5, Rectangle.Y + 25), Color.White * _opacity);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_computer != _player.ConnectedComp)
            {
                _computer = _player.ConnectedComp;
                _computer.ViewData.Update(gameTime);
                //_text = SetString();
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
