using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class NetworkNode
    {
        public event NodeHoverEventHandler Hover;
        public event NodeClickedEventHandler Click;

        public delegate void NodeClickedEventHandler(NodeClickedEventArgs e);
        public delegate void NodeHoverEventHandler(NodeHoverEventArgs e);

        public Computer Computer { get; private set; }
        public Point Position { get; private set; }
        public PopUpBox InfoBox { get; private set; }
        public bool IsHovering { get; private set; }
        
        private MouseState CurrentMouseState, PreviousMouseState;
        private Texture2D Texture;
        private Color Color, HoverColor, CurrentColor, ConnectedColor, PlayerColor;
        private Rectangle Container;

        public NetworkNode(Texture2D texture, Computer computer, Point position, PopUpBox infoBox)
        {
            Texture = texture;
            Computer = computer;
            Container = new Rectangle(position, new Point(25, 25));
            Color = Color.RoyalBlue;
            HoverColor = Color.DarkOrange;
            ConnectedColor = Color.Green;
            PlayerColor = Color.Blue;
            Position = Container.Location;
            InfoBox = infoBox;
            var holder = InfoBox.Container;
            holder.Location = Position + new Point(30, -5);
            InfoBox.Container = holder;
            InfoBox.Text = Computer.Name + "\n" + Computer.IP;

        }

        public void Update(GameTime gameTime)
        {
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);

            IsHovering = false;

            if (mouseRectangle.Intersects(Container))
            {
                IsHovering = true;

                NodeHoverEventArgs nh = new NodeHoverEventArgs()
                {
                    IP = Computer.IP,
                    Name = Computer.Name,
                    Location = Position
                };
                Hover?.Invoke(nh);

                if (CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
                {
                    NodeClickedEventArgs nc = new NodeClickedEventArgs()
                    {
                        IP = Computer.IP
                    };
                    Click?.Invoke(nc);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentColor = Color;

            if (IsHovering)
            {
                CurrentColor = HoverColor;
                //InfoBox.Draw(spriteBatch);
            }
            else if(Computer == Player.GetInstance().ConnectedComputer)
            {
                CurrentColor = ConnectedColor;
            }
            else if (Computer == Player.GetInstance().PlayersComputer)
            {
                CurrentColor = PlayerColor;
            }

            spriteBatch.Draw(Texture, Container, CurrentColor);
        }
    }

    public class NodeClickedEventArgs : EventArgs
    {
        public string IP { get; set; }
    }

    public class NodeHoverEventArgs : EventArgs
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}
