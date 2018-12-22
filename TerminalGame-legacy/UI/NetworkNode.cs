using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;
using TerminalGame.UI.Themes;
using TerminalGame.Utils;

namespace TerminalGame.UI
{
    class NetworkNode
    {
        public event NodeHoverEventHandler Hover;
        public event NodeClickedEventHandler Click;
        public event MouseEnterEventHandler Enter;
        public event MouseLeaveEventHandler Leave;

        public delegate void NodeHoverEventHandler(NodeHoverEventArgs e);
        public delegate void NodeClickedEventHandler(NodeClickedEventArgs e);
        public delegate void MouseEnterEventHandler(MouseEventArgs e);
        public delegate void MouseLeaveEventHandler(MouseEventArgs e);

        public Computer Computer { get; private set; }
        public Point Position { get; private set; }
        public Point CenterPosition { get; private set; }
        public PopUpBox InfoBox { get; private set; }
        public bool IsHovering { get; private set; }
        public Rectangle Container { get; private set; }

        private MouseState _currentMouseState, _previousMouseState;
        private readonly Texture2D _texture;
        readonly Dictionary<string, Texture2D> _nodeSpinners;
        private Color _currentColor, _connectedSpinnerColor, _playerSpinnerColor, _hoverSpinnerColor;
        private float _rotationCW, _rotationCCW;
        private Point _spinnerS, _spinnerN, _spinnerL;

        private readonly NodeHoverEventArgs _nh;
        private readonly NodeClickedEventArgs _nc;
        private readonly MouseEventArgs _enter, _leave;
        private bool _newMouseHoverState, _previousMouseHoverState;
        private ThemeManager _themeManager;

        public NetworkNode(Texture2D texture, Computer computer, Rectangle container, PopUpBox infoBox, Dictionary<string, Texture2D> nodeSpinners)
        {
            _themeManager = ThemeManager.GetInstance();
            _texture = texture;
            Computer = computer;
            Container = container;
            Position = Container.Location;
            InfoBox = infoBox;
            InfoBox.Text = InfoBox.Text + "\n" + Computer.TraceTime;
            var holder = InfoBox.Container;
            holder.Width = (int)FontManager.GetFont(FontManager.FontSize.Small).MeasureString(InfoBox.Text).Length() + 10;
            holder.Location = Position + new Point(Container.Width + 5, 0);
            InfoBox.Container = holder;
            _nodeSpinners = nodeSpinners;
            _rotationCW = 0.0f;

            CenterPosition = new Point(Position.X + Container.Width / 2, Position.Y + Container.Height / 2);

            _spinnerS = new Point((int)(container.Width * 1.25));
            _spinnerN = new Point((int)(container.Width * 1.72));
            _spinnerL = new Point((int)(container.Width * 2.12));

            _nh = new NodeHoverEventArgs()
            {
                IP = Computer.IP,
                Name = Computer.Name,
                Location = Position
            };

            _nc = new NodeClickedEventArgs()
            {
                IP = Computer.IP
            };

            _enter = new MouseEventArgs();
            _leave = new MouseEventArgs();
        }

        public void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            IsHovering = false;

            if (mouseRectangle.Intersects(Container))
            {
                IsHovering = true;
                Hover?.Invoke(_nh);

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(_nc);
                }
            }

            _newMouseHoverState = IsHovering;
            if(_newMouseHoverState != _previousMouseHoverState)
            {
                if (_newMouseHoverState)
                    Enter?.Invoke(_enter);
                else
                    Leave?.Invoke(_leave);
            }
            _previousMouseHoverState = _newMouseHoverState;

            _rotationCW += 0.01f;
            _rotationCCW -= 0.01f;
            _connectedSpinnerColor = _themeManager.CurrentTheme.NetworkMapConnectedSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) + 1) * 0.5f + 0.2f);
            _playerSpinnerColor = _themeManager.CurrentTheme.NetworkMapHomeSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1) + 1) * 0.5f + 0.2f);
            _hoverSpinnerColor = _themeManager.CurrentTheme.NetworkMapHoverSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.5) + 1) * 0.5f + 0.2f);
            InfoBox.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentColor = _themeManager.CurrentTheme.NetworkMapNodeColor;
            Texture2D connectedSpinner, playerSpinner, hoverSpinner, missionSpinner;

            connectedSpinner = SelectSpinner("ConnectedSpinner");
            playerSpinner = SelectSpinner("PlayerSpinner");
            hoverSpinner = SelectSpinner("HoverSpinner");
            missionSpinner = SelectSpinner("MissionSpinner");

            if (Computer == Player.GetInstance().PlayersComputer)
            {
                _currentColor = _themeManager.CurrentTheme.NetworkMapHomeSpinnerColor;
                spriteBatch.Draw(playerSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), _spinnerS), null, _playerSpinnerColor, _rotationCW, new Vector2(playerSpinner.Width / 2, playerSpinner.Height / 2), SpriteEffects.None, 0);
            }
            if (Computer == Player.GetInstance().ConnectedComputer)
            {
                _currentColor = _themeManager.CurrentTheme.NetworkMapConnectedSpinnerColor;
                spriteBatch.Draw(connectedSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), _spinnerN), null, _connectedSpinnerColor, _rotationCCW, new Vector2(connectedSpinner.Width / 2, connectedSpinner.Height / 2), SpriteEffects.None, 0);

            }
            if (IsHovering)
            {
                _currentColor = _themeManager.CurrentTheme.NetworkMapHoverSpinnerColor;
                spriteBatch.Draw(hoverSpinner, new Rectangle(new Point(Container.Location.X + (Container.Width / 2), Container.Location.Y + (Container.Height / 2)), _spinnerL), null, _hoverSpinnerColor, _rotationCW, new Vector2(hoverSpinner.Width / 2, hoverSpinner.Height / 2), SpriteEffects.None, 0);

            }

            spriteBatch.Draw(_texture, Container, _currentColor * 0.9f);
        }

        private Texture2D SelectSpinner(string key)
        {
            foreach(KeyValuePair<string, Texture2D> spinner in _nodeSpinners)
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
    /// Generic mouse event args
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Arguments for when the node is clicked
    /// </summary>
    public class NodeClickedEventArgs : EventArgs
    {
        /// <summary>
        /// IP address of the active node
        /// </summary>
        public string IP { get; set; }
    }

    /// <summary>
    /// Arguments for when mouse hovers over the node
    /// </summary>
    public class NodeHoverEventArgs : EventArgs
    {
        /// <summary>
        /// IP address of the computer belonging to the active node
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// Host name of the computer belonging to the active node
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Node location on map
        /// </summary>
        public Point Location { get; set; }
    }
}
