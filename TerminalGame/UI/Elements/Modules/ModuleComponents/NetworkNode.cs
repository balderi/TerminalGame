using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Computers;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents
{
    class NetworkNode : UIElement
    {
        private readonly Computer _computer;
        private readonly Texture2D _texture;
        private Color _nodeColor;

        public NetworkNode(Game game, Point location, Point size, Computer computer, Texture2D texture, bool hasBorder = true, bool fadeIn = true) : base(game, location, size, hasBorder, fadeIn)
        {
            _computer = computer;
            _texture = texture;
        }

        public override void Initialize()
        {
            base.Initialize();
            _nodeColor = Color.White * _opacity; // TODO: Part of theme
            BackgroundColor = Color.Transparent;
        }

        private Point ClampNodeToMap(Point location)
        {
            Console.WriteLine("Recieved node location: " + location.X + ", " + location.Y + ".\nMap bouds are: " + Rectangle.X + ", " + Rectangle.Y + ", " + (Rectangle.X + Rectangle.Width) + ", " + (Rectangle.Y + Rectangle.Height) + "(x,y,w,h)");
            int x = 0;
            int y = 0;
            if (location.X < Rectangle.X + 32)
                x = Rectangle.X + 32;
            else if (location.X > Rectangle.X + Rectangle.Width - 32)
                x = (Rectangle.X + Rectangle.Width) - 32;
            else
                x = location.X;

            if (location.Y < Rectangle.Y + 25 + 32)
                y = Rectangle.Y + 25 + 32;
            else if (location.Y > Rectangle.Y + Rectangle.Height - 32)
                y = (Rectangle.Y + Rectangle.Height) - 32;
            else
                y = location.Y;
            if(x != location.X || y != location.Y)
                Console.WriteLine("Clamped coord from " + location.X + ", " + location.Y + " to " + x + ", " + y);
            return new Point(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            _nodeColor = Color.White * _opacity; // TODO: Part of theme
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.Draw(_texture, Rectangle, _nodeColor);
            _spriteBatch.End();
        }

        protected override void OnMouseHover(object sender, MouseEventArgs e)
        {
            _nodeColor = Color.Orange * _opacity; // TODO: Part of theme
            base.OnMouseHover(sender, e);
        }

        protected override void OnClick(object sender, MouseEventArgs e)
        {
            base.OnClick(sender, e);
            Game.Terminal.RunCommand("connect " + _computer.IP);
        }

        protected override void OnMouseEnter(object sender, MouseEventArgs e)
        {
            base.OnMouseEnter(sender, e);
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            base.OnMouseLeave(sender, e);
        }

        protected override void OnLeftButtonDown(object sender, MouseEventArgs e)
        {
            base.OnLeftButtonDown(sender, e);
            _nodeColor = Color.Red * _opacity; // TODO: Part of theme
        }
    }
}
