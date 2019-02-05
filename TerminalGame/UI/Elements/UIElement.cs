using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.UI.Themes;

namespace TerminalGame.UI.Elements
{
    public partial class UIElement : DrawableGameComponent
    {
        #region fields
        protected SpriteBatch _spriteBatch;
        protected float _opacity, _fadeTarget;
        protected bool _fadingUp, _fadingDown, _isHovering, _newMouseHoverState, _previousMouseHoverState, _mouseLeftDown, _mouseRightDown, _fadeIn;
        protected MouseState _previousMouseState, _currentMouseState;
        protected readonly MouseEventArgs _hover, _leftClick, _rightClick, _enter, _leave, _leftDn, _leftUp, _rightDn, _rightUp;
        protected RasterizerState _rasterizerState;
        protected ContentManager Content;
        protected ThemeManager _themeManager;
        protected new TerminalGame Game;
        protected Rectangle _selectionRect;
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        public bool MouseIsHovering { get => _isHovering; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color FontColor { get; set; }
        public bool HasBorder { get; private set; }
        #endregion

        #region events
        public delegate void MouseHoverEventHandler(object sender, MouseEventArgs e);
        public delegate void MouseEnterEventHandler(object sender, MouseEventArgs e);
        public delegate void MouseLeaveEventHandler(object sender, MouseEventArgs e);
        public delegate void MouseClickEventHandler(object sender, MouseEventArgs e);
        public delegate void MouseEventHandler(object sender, MouseEventArgs e);

        public event MouseHoverEventHandler MouseHover;
        public event MouseLeaveEventHandler MouseEnter;
        public event MouseEnterEventHandler MouseLeave;
        public event MouseClickEventHandler LeftClick;
        public event MouseClickEventHandler RightClick;
        public event MouseEventHandler LeftButtonDown;
        public event MouseEventHandler LeftButtonUp;
        public event MouseEventHandler RightButtonDown;
        public event MouseEventHandler RightButtonUp;
        #endregion

        public UIElement(Game game, Point location, Point size, bool hasBorder = true, bool fadeIn = true) : base(game)
        {
            Game = game as TerminalGame;
            Content = Game.Content;
            _themeManager = ThemeManager.GetInstance();
            Rectangle = new Rectangle(location, size);
            _selectionRect = Rectangle;
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fadeIn = fadeIn;
            _fadeTarget = 1;
            if (_fadeIn)
            {
                _opacity = 0;
                _fadingUp = true;
            }
            else
            {
                _opacity = 1;
                _fadingUp = false;
            }
            _fadingDown = false;
            HasBorder = hasBorder;

            _hover = new MouseEventArgs();
            _enter = new MouseEventArgs();
            _leave = new MouseEventArgs();
            _leftClick = new MouseEventArgs();
            _rightClick = new MouseEventArgs();
            _leftDn = new MouseEventArgs();
            _leftUp = new MouseEventArgs();
            _rightDn = new MouseEventArgs();
            _rightUp = new MouseEventArgs();

            MouseHover += OnMouseHover;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            LeftClick += OnClick;
            LeftButtonDown += OnLeftButtonDown;
            LeftButtonUp += OnLeftButtonUp;
            RightButtonDown += OnRightButtonDown;
            RightButtonUp += OnRightButtonUp;
        }

        public override void Initialize()
        {
            BackgroundColor = _themeManager.CurrentTheme.ModuleBackgroundColor;
            BorderColor     = _themeManager.CurrentTheme.ModuleOutlineColor;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_fadingUp)
                FadeUp(_fadeTarget);
            if (_fadingDown)
                FadeDown(_fadeTarget);

            if (!Enabled)
                return;
            
            _previousMouseState = _currentMouseState;
            _currentMouseState  = Mouse.GetState();

            var mouseRectangle  = new Rectangle(_currentMouseState.X, 
                                                _currentMouseState.Y, 1, 1);

            _isHovering = false;
            _mouseLeftDown = false;
            if (mouseRectangle.Intersects(_selectionRect))
            {
                _isHovering = true;
                MouseHover?.Invoke(this, _hover);

                if (_currentMouseState.LeftButton == ButtonState.Released &&
                    _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    LeftClick?.Invoke(this, _leftClick);
                    LeftButtonUp?.Invoke(this, _leftUp);
                }
                else if (_currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    _mouseLeftDown = true;
                    LeftButtonDown?.Invoke(this, _leftDn);
                }

                if (_currentMouseState.RightButton == ButtonState.Released &&
                    _previousMouseState.RightButton == ButtonState.Pressed)
                {
                    RightClick?.Invoke(this, _leftClick);
                    RightButtonUp?.Invoke(this, _rightUp);
                }
                else if (_currentMouseState.RightButton == ButtonState.Pressed)
                {
                    _mouseRightDown = true;
                    RightButtonDown?.Invoke(this, _rightDn);
                }
            }

            _newMouseHoverState = _isHovering;
            if (_newMouseHoverState != _previousMouseHoverState)
            {
                if (_newMouseHoverState)
                    MouseEnter?.Invoke(this, _enter);
                else
                    MouseLeave?.Invoke(this, _leave);
            }
            _previousMouseHoverState = _newMouseHoverState;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible && !_fadingDown && !_fadingUp)
                return;

            _spriteBatch.Draw(Utils.Globals.DummyTexture(), Rectangle,
                              BackgroundColor * _opacity);
            
            if(HasBorder)
                Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(), 1,
                                          BorderColor * _opacity);
        }

        /// <summary>
        /// Fades the element out to 50% transparency
        /// </summary>
        public void Dim()
        {
            _fadeTarget = 0.5f;
            _fadingDown = true;
            _fadingUp   = false;
        }

        /// <summary>
        /// Fades the element up to 100% opacity
        /// </summary>
        public void UnDim()
        {
            _fadeTarget = 1.0f;
            _fadingUp   = true;
            _fadingDown = false;
        }

        /// <summary>
        /// Fades the element down to 100% transparency
        /// </summary>
        public void FadeOut()
        {
            _fadeTarget = 0.0f;
            _fadingDown = true;
            _fadingUp   = false;
        }

        /// <summary>
        /// Fades the element up to 100% opacity 
        /// </summary>
        public void FadeIn()
        {
            _fadeTarget = 1.0f;
            _fadingUp   = true;
            _fadingDown = false;
        }

        /// <summary>
        /// Fades the element up to <c>target</c> opacity at <c>delta</c> speed
        /// </summary>
        /// <param name="target">Target opacity</param>
        /// <param name="delta">Fade speed</param>
        private void FadeUp(float target, float delta = 0.01f)
        {
            _opacity += delta;
            if (_opacity > 1.0f)
            {
                _opacity  = 1.0f;
                _fadingUp = false;
                return;
            }
            if (_opacity > target)
            {
                _opacity  = target;
                _fadingUp = false;
                return;
            }
        }

        /// <summary>
        /// Fades the element down to <c>target</c> opacity at <c>delta</c> speed
        /// </summary>
        /// <param name="target">Target opacity</param>
        /// <param name="delta">Fade speed</param>
        private void FadeDown(float target, float delta = 0.01f)
        {
            _opacity -= delta;
            if (_opacity < 0.0f)
            {
                _opacity    = 0.0f;
                _fadingDown = false;
                return;
            }
            if (_opacity < target)
            {
                _opacity    = target;
                _fadingDown = false;
                return;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is UIElement o)
            {
                return o.Rectangle.X == Rectangle.X && o.Rectangle.Y == Rectangle.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Rectangle.X + Rectangle.Y);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            _spriteBatch.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnDrawOrderChanged(object sender, EventArgs args)
        {
            base.OnDrawOrderChanged(sender, args);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            //base.OnEnabledChanged(sender, args);
            if (Enabled)
                Dim();
            else
                UnDim();
        }

        protected override void OnUpdateOrderChanged(object sender, EventArgs args)
        {
            base.OnUpdateOrderChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            //base.OnVisibleChanged(sender, args);
            if (Visible)
                FadeIn();
            else
                FadeOut();
        }

        protected virtual void OnMouseHover(object sender, MouseEventArgs e) { }

        protected virtual void OnMouseEnter(object sender, MouseEventArgs e) { }

        protected virtual void OnMouseLeave(object sender, MouseEventArgs e) { }

        protected virtual void OnClick(object sender, MouseEventArgs e) { }
        protected virtual void OnLeftButtonDown(object sender, MouseEventArgs e) { }
        protected virtual void OnLeftButtonUp(object sender, MouseEventArgs e) { }
        protected virtual void OnRightButtonDown(object sender, MouseEventArgs e) { }
        protected virtual void OnRightButtonUp(object sender, MouseEventArgs e) { }
    }

    /// <summary>
    /// Generic mouse event args
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        // TODO: Specify MouseEventArg content
    }
}
