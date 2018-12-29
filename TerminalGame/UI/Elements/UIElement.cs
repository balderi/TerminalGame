﻿using Microsoft.Xna.Framework;
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
    public class UIElement : DrawableGameComponent
    {
        #region fields
        protected SpriteBatch _spriteBatch;
        protected float _opacity, _fadeTarget;
        protected bool _fadingUp, _fadingDown, _isHovering, _newMouseHoverState, _previousMouseHoverState;
        protected MouseState _previousMouseState, _currentMouseState;
        protected readonly MouseEventArgs _hover, _click, _enter, _leave;
        protected RasterizerState _rasterizerState;
        protected ContentManager Content;
        private ThemeManager _themeManager;
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        #endregion

        #region events
        public delegate void MouseHoverEventHandler(MouseEventArgs e);
        public delegate void MouseEnterEventHandler(MouseEventArgs e);
        public delegate void MouseLeaveEventHandler(MouseEventArgs e);
        public delegate void MouseClickEventHandler(MouseEventArgs e);

        public event MouseHoverEventHandler MouseHover;
        public event MouseClickEventHandler MouseEnter;
        public event MouseEnterEventHandler MouseLeave;
        public event MouseLeaveEventHandler Click;
        #endregion

        public UIElement(Game game, Point location, Point size) : base(game)
        {
            Content = Game.Content;
            _themeManager = ThemeManager.GetInstance();
            Rectangle = new Rectangle(location, size);
            _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
            _opacity = 0;
            _fadeTarget = 1;
            _fadingDown = false;
            _fadingUp = true;

            _hover= new MouseEventArgs();
            _enter = new MouseEventArgs();
            _leave = new MouseEventArgs();
            _click = new MouseEventArgs();

            MouseHover += OnMouseHover;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            Click += OnClick;
        }

        public override void Initialize()
        {
            base.Initialize();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                MouseHover?.Invoke(_hover);

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(_click);
                }
            }

            _newMouseHoverState = _isHovering;
            if (_newMouseHoverState != _previousMouseHoverState)
            {
                if (_newMouseHoverState)
                    MouseEnter?.Invoke(_enter);
                else
                    MouseLeave?.Invoke(_leave);
            }
            _previousMouseHoverState = _newMouseHoverState;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible && !_fadingDown && !_fadingUp)
                return;

            _spriteBatch.Draw(Utils.Globals.DummyTexture(GraphicsDevice), Rectangle,
                    _themeManager.CurrentTheme.ModuleBackgroundColor * _opacity);
                
            Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(GraphicsDevice), 1,
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

        protected virtual void OnClick(MouseEventArgs e) { ThemeManager.GetInstance().CurrentTheme.Flash(); }
    }

    /// <summary>
    /// Generic mouse event args
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        // TODO: Specify MouseEventArg content
    }
}
