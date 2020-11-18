using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData
{
    public class DefaultView : IRemoteViewData
    {
        private Computer _computer;
        private string _text;
        private SpriteFont _font;
        private float _opacity;

        public DefaultView(Computer computer)
        {
            _computer = computer;
            _text = SetString();
            _font = FontManager.GetFont("FontM");
            _opacity = 1;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle remoteViewRectangle)
        {
            spriteBatch.DrawString(_font, _text, new Vector2(remoteViewRectangle.X + 5, remoteViewRectangle.Y + 25), Color.White * _opacity);
        }

        public void Update(GameTime gameTime)
        {
            if (_computer != World.World.GetInstance().Player.ConnectedComp)
            {
                _computer = World.World.GetInstance().Player.ConnectedComp;
                _text = SetString();
            }
        }

        private string SetString()
        {
            try
            {
                return $"Connected to:\n{_computer.Name.Replace("§¤§", "\n")}\nIP: {_computer.IP}\n" +
                       $"Company: {_computer.Owner.Name}\nOwner:\n{_computer.Owner.Owner}\nAdmin:" +
                       $"\n{_computer.Owner.Admin}";
            }
            catch (Exception e)
            {
                //Console.WriteLine("Trying to fix the world...");
                //World.World.GetInstance().FixWorld();
                //return SetString();
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
