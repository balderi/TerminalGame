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
                return $"Connected to:\n{_computer.Name.Replace("§¤§", "\n")}\nIP: {_computer.IP}\n" +
                       $"Company: {_computer.Owner.Name}\nOwner:\n{_computer.Owner.Owner}\nAdmin:" +
                       $"\n{_computer.Owner.Admin}";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " - ignore this (localhost has no company).");
                return $"Connected to: {_computer.Name.Replace("§¤§", "\n")}\n" +
                       $"          IP: {_computer.IP}\n" +
                        "     Company: ?\n" +
                        "       Owner: ?\n" +
                        "       Admin: ?\n";
            }
        }
    }
}
