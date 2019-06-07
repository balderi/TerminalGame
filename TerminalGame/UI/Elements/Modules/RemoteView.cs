using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules
{
    class RemoteView : Module
    {
        private Player _player;
        private Computer _computer;
        private string _text;
        private SpriteFont _font;

        public RemoteView(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _font = FontManager.GetFont("FontM");
            _player = Player.GetInstance();
            _computer = _player.ConnectedComp;
            _text = SetString();
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            base.ScissorDraw(gameTime);
            _spriteBatch.DrawString(_font, _text, new Vector2(Rectangle.X + 5, Rectangle.Y + 25), Color.White * _opacity);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_computer != _player.ConnectedComp)
            {
                _computer = _player.ConnectedComp;
                _text = SetString();
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        private string SetString()
        {
            try
            {
                return String.Format(
                    "Connected to:\n{0}\n" +
                    "IP: {1}\n" +
                    "Company: {2}\n" +
                    "Owner:\n{3}\n" +
                    "Admin:\n{4}"
                    , _computer.Name.Replace("§¤§", "\n"), _computer.IP, _computer.Owner.Name, _computer.Owner.Owner.ToString(), _computer.Owner.Admin.ToString()
                    );
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " - ignore this (localhost has no company).");
                return String.Format(
                    "Connected to: {0}\n" +
                    "          IP: {1}\n" +
                    "     Company: ?\n" +
                    "       Owner: ?\n" +
                    "       Admin: ?\n"
                    , _computer.Name.Replace("§¤§", "\n"), _computer.IP
                    );
            }
        }
    }
}
