using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.Computers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents
{
    class NetworkNode : UIElement
    {
        private readonly Texture2D _texture;
        private readonly Dictionary<string, Texture2D> _networkNodeSpinners;
        //private Texture2D _spinner;
        private Color _currentColor, _connectedSpinnerColor, _playerSpinnerColor, _hoverSpinnerColor;
        private float _rotationCW, _rotationCCW;
        private Point _spinnerS, _spinnerN, _spinnerL;
        private readonly NetworkMap _nMap;
        
        public Computer Computer { get; private set; }
        public bool IsHovering { get; private set; }
        public NodeInfoBox InfoBox { get; private set; }

        public NetworkNode(Game game, NetworkMap map, Point location, Point size, Computer computer, Texture2D texture, 
            Dictionary<string, Texture2D> spinners, bool hasBorder = true, bool fadeIn = true) : 
            base(game, location, size, hasBorder, fadeIn)
        {
            Computer = computer;
            _texture = texture;
            _networkNodeSpinners = spinners;
            _nMap = map;
            _selectionRect = new Rectangle(new Point(Rectangle.Location.X + 4, Rectangle.Location.Y + 4), 
                new Point(Rectangle.Width - 8, Rectangle.Height - 8));
            InfoBox = new NodeInfoBox(Game, new Point(Rectangle.X + Rectangle.Width + 20, Rectangle.Y), 
                new Point((int)FontManager.GetFont("FontXS").MeasureString(Computer.ToString()).X + 10, 
                FontManager.GetFont("FontS").LineSpacing * 3), Computer.ToString(), true, false);
        }

        public override void Initialize()
        {
            base.Initialize();
            BackgroundColor = Color.Transparent;

            if (Computer == Player.GetInstance().PlayerComp)
                _nMap.PlayerCompNode = this;

            _spinnerS = new Point((int)(Rectangle.Width * 1.25));
            _spinnerN = new Point((int)(Rectangle.Width * 1.72));
            _spinnerL = new Point((int)(Rectangle.Width * 2.12));

            InfoBox.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Computer.IsShownOnMap)
                return;

            _rotationCW += 0.01f;
            _rotationCCW -= 0.01f;
            _connectedSpinnerColor = _themeManager.CurrentTheme.NetworkMapConnectedSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) + 1) * 0.5f + 0.2f);
            _playerSpinnerColor = _themeManager.CurrentTheme.NetworkMapHomeSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1) + 1) * 0.5f + 0.2f);
            _hoverSpinnerColor = _themeManager.CurrentTheme.NetworkMapHoverSpinnerColor * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.5) + 1) * 0.5f + 0.2f);

            _currentColor = _themeManager.CurrentTheme.NetworkMapNodeColor * _opacity;
            
            if (Computer == Player.GetInstance().PlayerComp)
            {
                _currentColor = _themeManager.CurrentTheme.NetworkMapHomeSpinnerColor * _opacity;
            }
            if (Computer == Player.GetInstance().ConnectedComp)
            {
                if (_nMap.ConnectedNode != this)
                    _nMap.ConnectedNode = this;
                _currentColor = _themeManager.CurrentTheme.NetworkMapConnectedSpinnerColor * _opacity;

            }
            if (_isHovering && _nMap.HoverNode == this)
            {
                 _currentColor = _themeManager.CurrentTheme.NetworkMapHoverSpinnerColor * _opacity;

            }

            if (IsHovering == true && _nMap.HoverNode == null)
            {
                _nMap.HoverNode = this;
                InfoBox.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Computer.IsShownOnMap)
                return;
            
            _networkNodeSpinners.TryGetValue("ConnectedSpinner", out Texture2D connectedSpinner);
            _networkNodeSpinners.TryGetValue("PlayerSpinner", out Texture2D playerSpinner);
            _networkNodeSpinners.TryGetValue("HoverSpinner", out Texture2D hoverSpinner);
            _networkNodeSpinners.TryGetValue("MissionSpinner", out Texture2D missionSpinner);

            _spriteBatch.Begin();
            base.Draw(gameTime);
            if (Computer == Player.GetInstance().PlayerComp)
            {
                _spriteBatch.Draw(playerSpinner, new Rectangle(new Point(Rectangle.Location.X + (Rectangle.Width / 2), Rectangle.Location.Y + (Rectangle.Height / 2)), _spinnerS), null, _playerSpinnerColor, _rotationCW, new Vector2(playerSpinner.Width / 2, playerSpinner.Height / 2), SpriteEffects.None, 0);
            }
            if (Computer == Player.GetInstance().ConnectedComp)
            {
                if (_nMap.ConnectedNode != this)
                    _nMap.ConnectedNode = this;
                _spriteBatch.Draw(connectedSpinner, new Rectangle(new Point(Rectangle.Location.X + (Rectangle.Width / 2), Rectangle.Location.Y + (Rectangle.Height / 2)), _spinnerN), null, _connectedSpinnerColor, _rotationCCW, new Vector2(connectedSpinner.Width / 2, connectedSpinner.Height / 2), SpriteEffects.None, 0);

            }
            if (_isHovering && _nMap.HoverNode == this)
            {
                _spriteBatch.Draw(hoverSpinner, new Rectangle(new Point(Rectangle.Location.X + (Rectangle.Width / 2), Rectangle.Location.Y + (Rectangle.Height / 2)), _spinnerL), null, _hoverSpinnerColor, _rotationCW, new Vector2(hoverSpinner.Width / 2, hoverSpinner.Height / 2), SpriteEffects.None, 0);
                InfoBox.Draw(gameTime);
            }
            _spriteBatch.Draw(_texture, Rectangle, _currentColor);
            _spriteBatch.End();
        }

        protected override void OnMouseHover(object sender, MouseEventArgs e)
        {
            base.OnMouseHover(sender, e);
        }

        protected override void OnClick(object sender, MouseEventArgs e)
        {
            base.OnClick(sender, e);
            if (_nMap.HoverNode != this)
                return;
            if (Computer == Player.GetInstance().PlayerComp)
                Game.Terminal.RunCommand("disconnect");
            else
                Game.Terminal.RunCommand("connect " + Computer.IP);
        }

        protected override void OnMouseEnter(object sender, MouseEventArgs e)
        {
            base.OnMouseEnter(sender, e);
            IsHovering = true;
            if (_nMap.HoverNode == null)
                _nMap.HoverNode = this;
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            base.OnMouseLeave(sender, e);
            IsHovering = false;
            if (_nMap.HoverNode == this)
                _nMap.HoverNode = null;
        }

        protected override void OnLeftButtonDown(object sender, MouseEventArgs e)
        {
            base.OnLeftButtonDown(sender, e);
            if (_nMap.HoverNode != this)
                return;
            _currentColor = Color.Red * _opacity;
        }
    }
}
