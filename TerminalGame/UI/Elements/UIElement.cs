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
        protected bool _fadingUp, _fadingDown, _isHovering, _newMouseHoverState, _previousMouseHoverState, _mouseDown;
        protected MouseState _previousMouseState, _currentMouseState;
        protected readonly MouseEventArgs HOVER, CLICK, ENTER, LEAVE;
        protected RasterizerState _rasterizerState;
        protected ContentManager Content;
        protected ThemeManager _themeManager;
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        public bool MouseIsHovering { get => _isHovering; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public bool HasBorder { get; private set; }
        #endregion

        #region events
        public delegate void MouseHoverEventHandler(MouseEventArgs e);
        public delegate void MouseEnterEventHandler(MouseEventArgs e);
        public delegate void MouseLeaveEventHandler(MouseEventArgs e);
        public delegate void MouseClickEventHandler(MouseEventArgs e);

        public event MouseHoverEventHandler MouseHover;
        public event MouseLeaveEventHandler MouseEnter;
        public event MouseEnterEventHandler MouseLeave;
        public event MouseClickEventHandler Click;
        #endregion

        public UIElement(Game game, Point location, Point size, bool hasBorder = true) : base(game)
        {
            Content = Game.Content;
            _themeManager = ThemeManager.GetInstance();
            Rectangle = new Rectangle(location, size);
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _opacity = 0;
            _fadeTarget = 1;
            _fadingDown = false;
            _fadingUp = true;
            HasBorder = hasBorder;

            HOVER = new MouseEventArgs();
            ENTER = new MouseEventArgs();
            LEAVE = new MouseEventArgs();
            CLICK = new MouseEventArgs();

            MouseHover += OnMouseHover;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            Click += OnClick;
        }

        public override void Initialize()
        {
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
            _currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouseState.X, 
                                               _currentMouseState.Y, 1, 1);

            _isHovering = false;
            _mouseDown = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                MouseHover?.Invoke(HOVER);

                if (_currentMouseState.LeftButton == ButtonState.Released &&
                    _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(CLICK);
                }
                else if(_currentMouseState.LeftButton == ButtonState.Pressed)
                    _mouseDown = true;
            }

            _newMouseHoverState = _isHovering;
            if (_newMouseHoverState != _previousMouseHoverState)
            {
                if (_newMouseHoverState)
                    MouseEnter?.Invoke(ENTER);
                else
                    MouseLeave?.Invoke(LEAVE);
            }
            _previousMouseHoverState = _newMouseHoverState;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible && !_fadingDown && !_fadingUp)
                return;

            _spriteBatch.Draw(Utils.Globals.DummyTexture(), Rectangle,
                              _themeManager.CurrentTheme.ModuleBackgroundColor * _opacity);
            
            if(HasBorder)
                Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(), 1,
                                          _themeManager.CurrentTheme.ModuleOutlineColor * _opacity);
        }

        /// <summary>
        /// Fades the element out to 50% transparency
        /// </summary>
        public void Dim()
        {
            _fadeTarget = 0.5f;
            _fadingDown = true;
            _fadingUp = false;
        }

        /// <summary>
        /// Fades the element up to 100% opacity
        /// </summary>
        public void UnDim()
        {
            _fadeTarget = 1.0f;
            _fadingUp = true;
            _fadingDown = false;
        }

        /// <summary>
        /// Fades the element down to 100% transparency
        /// </summary>
        public void FadeOut()
        {
            _fadeTarget = 0.0f;
            _fadingDown = true;
            _fadingUp = false;
        }

        /// <summary>
        /// Fades the element up to 100% opacity 
        /// </summary>
        public void FadeIn()
        {
            _fadeTarget = 1.0f;
            _fadingUp = true;
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
                _opacity = 1.0f;
                _fadingUp = false;
                return;
            }
            if (_opacity > target)
            {
                _opacity = target;
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
                _opacity = 0.0f;
                _fadingDown = false;
                return;
            }
            if (_opacity < target)
            {
                _opacity = target;
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

        protected virtual void OnMouseHover(MouseEventArgs e) { }

        protected virtual void OnMouseEnter(MouseEventArgs e) { }

        protected virtual void OnMouseLeave(MouseEventArgs e) { }

        protected virtual void OnClick(MouseEventArgs e) { }
    }

    /// <summary>
    /// Generic mouse event args
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        // TODO: Specify MouseEventArg content
    }
}
