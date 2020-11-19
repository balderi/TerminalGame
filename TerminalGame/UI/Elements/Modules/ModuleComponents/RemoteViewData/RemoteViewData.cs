using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData
{
    [DataContract]
    public class RemoteViewData : IRemoteViewData
    {
        [DataMember]
        public Computer Computer;

        private string _text;
        private readonly SpriteFont _font;

        public RemoteViewData(Computer computer)
        {
            Computer = computer;
            _text = SetString();
            _font = FontManager.GetFont("FontM");
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle remoteViewRectangle, float opacity)
        {
            spriteBatch.DrawString(_font, _text, new Vector2(remoteViewRectangle.X + 5, remoteViewRectangle.Y + 25), Color.White * opacity);
        }

        public virtual void Update(GameTime gameTime)
        {
            //_text = SetString();
        }

        private string SetString()
        {
            try
            {
                return $"Connected to:\n{Computer.Name.Replace("§¤§", "\n")}\nIP: {Computer.IP}\n" +
                       $"Company: {Computer.Owner.Name}\nOwner:\n{Computer.Owner.Owner}\nAdmin:" +
                       $"\n{Computer.Owner.Admin}";
            }
            catch (Exception e)
            {
                //Console.WriteLine("Trying to fix the world...");
                //World.World.GetInstance().FixWorld();
                //return SetString();
                Console.WriteLine(e.Message + " - ignore this (localhost has no company).");
                return $"Connected to: {Computer.Name.Replace("§¤§", "\n")}\n" +
                       $"          IP: {Computer.IP}\n" +
                        "     Company: ?\n" +
                        "       Owner: ?\n" +
                        "       Admin: ?\n";
            }
        }
    }
}
