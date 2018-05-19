using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;

namespace TerminalGame.UI
{
    class NetworkNode
    {
        public event NodeHoverEventHandler Hover;
        public event NodeClickedEventHandler Click;

        public delegate void NodeClickedEventHandler(NodeClickedEventArgs e);
        public delegate void NodeHoverEventHandler(NodeHoverEventArgs e);

        public Computer Computer { get; private set; }
        public Point Position { get; private set; }
        public Point CenterPosition { get; private set; }
        public PopUpBox InfoBox { get; private set; }
        public bool IsHovering { get; private set; }
        public Rectangle Container { get; private set; }

        private MouseState CurrentMouseState, PreviousMouseState;
        private readonly Texture2D Texture;
        readonly Dictionary<string, Texture2D> NodeSpinners;
        private Color Color, HoverColor, CurrentColor, ConnectedColor, PlayerColor, ConnectedSpinnerColor, PlayerSpinnerColor, HoverSpinnerColor;
        private float rotationCW, rotationCCW;
        Point spinnerS, spinnerN, spinnerL;

        public NetworkNode(Texture2D texture, Computer computer, Rectangle container, PopUpBox infoBox, Dictionary<string, Texture2D> nodeSpinners)
        {
            Texture = texture;
            Computer = computer;
            Container = container;
            Color = Color.RoyalBlue;
            HoverColor = Color.DarkOrange;
            ConnectedColor = Color.Green;
            PlayerColor = Color.Blue;
            Position = Container.Location;
            InfoBox = infoBox;
            var holder = InfoBox.Container;
            holder.Location = Position + new Point(Container.Width + 5, 0);
            InfoBox.Container = holder;
            InfoBox.Text = Computer.Name + "\n" + Computer.IP;
            NodeSpinners = nodeSpinners;
            rotationCW = 0.0f;

            CenterPosition = new Point(Position.X + Container.Width / 2, Position.Y + Container.Height / 2);

            spinnerS = new Point(40, 40);
            spinnerN = new Point(55, 55);
            spinnerL = new Point(70, 70);
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
            rotationCW += 0.01f;
            rotationCCW -= 0.01f;
            ConnectedSpinnerColor = ConnectedColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) + 1) * 0.5f + 0.2f);
            PlayerSpinnerColor = PlayerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1) + 1) * 0.5f + 0.2f);
            HoverSpinnerColor = HoverColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.5) + 1) * 0.5f + 0.2f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentColor = Color;
            Texture2D connectedSpinner, playerSpinner, hoverSpinner, missionSpinner;

            connectedSpinner = SelectSpinner("ConnectedSpinner");
            playerSpinner = SelectSpinner("PlayerSpinner");
            hoverSpinner = SelectSpinner("HoverSpinner");
            missionSpinner = SelectSpinner("MissionSpinner");

            if (Computer == Player.GetInstance().PlayersComputer)
            {
                CurrentColor = PlayerColor;
                spriteBatch.Draw(playerSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), spinnerS), null, PlayerSpinnerColor, rotationCW, new Vector2(playerSpinner.Width / 2, playerSpinner.Height / 2), SpriteEffects.None, 0);

            }
            if (Computer == Player.GetInstance().ConnectedComputer)
            {
                CurrentColor = ConnectedColor;
                spriteBatch.Draw(connectedSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), spinnerN), null, ConnectedSpinnerColor, rotationCCW, new Vector2(connectedSpinner.Width / 2, connectedSpinner.Height / 2), SpriteEffects.None, 0);

            }
            if (IsHovering)
            {
                CurrentColor = HoverColor;
                spriteBatch.Draw(hoverSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), spinnerL), null, HoverSpinnerColor, rotationCW, new Vector2(hoverSpinner.Width / 2, hoverSpinner.Height / 2), SpriteEffects.None, 0);

            }

            spriteBatch.Draw(Texture, Container, CurrentColor);
        }

        private Texture2D SelectSpinner(string key)
        {
            foreach(KeyValuePair<string, Texture2D> spinner in NodeSpinners)
            {
                if (spinner.Key == key)
                {
                    return spinner.Value;
                }
            }
            throw new Exception("No element matches key \'" + key + "\'");
        }
    }

    /// <summary>
    /// Fires when node is clicked
    /// </summary>
    public class NodeClickedEventArgs : EventArgs
    {
        public string IP { get; set; }
    }

    /// <summary>
    /// Fires when mouse hovers over the node
    /// </summary>
    public class NodeHoverEventArgs : EventArgs
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}
