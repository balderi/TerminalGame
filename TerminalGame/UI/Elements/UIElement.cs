using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.UI.Elements
{
    public class UIElement : DrawableGameComponent
    {
        #region fields
        protected SpriteBatch _spriteBatch;
        protected float _opacity, _fadeTarget;
        protected bool _fadingUp, _fadingDown;
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        #endregion

        #region events
        public event EventHandler MouseHover;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Click;
        #endregion

        public UIElement(Game game, Point location, Point size) : base(game)
        {
            Rectangle = new Rectangle(location, size);
            _opacity = 0;
            _fadeTarget = 1;
            _fadingDown = false;
            _fadingUp = true;
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            if (Enabled)
            {
                if (_fadingUp)
                    FadeUp(_fadeTarget);
                if (_fadingDown)
                    FadeDown(_fadeTarget);
                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(Utils.Globals.DummyTexture(GraphicsDevice), Rectangle,
                        Utils.Globals.ModuleBackgroundColor() * _opacity);
                
                Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(GraphicsDevice), 1,
                    Utils.Globals.ModuleBorderColor() * _opacity);

                base.Draw(gameTime);
                _spriteBatch.End();
            }
        }

        protected virtual void OnMouseHover(object sender, EventArgs e)
        {
            MouseHover?.Invoke(this, e);
        }

        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            MouseEnter?.Invoke(this, e);
        }

        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            MouseLeave?.Invoke(this, e);
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public void Dim()
        {
            _fadeTarget = 0.5f;
            _fadingDown = true;
        }

        public void UnDim()
        {
            _fadeTarget = 1.0f;
            _fadingUp = true;
        }

        public void FadeOut()
        {
            _fadeTarget = 0.0f;
            _fadingDown = true;
        }

        public void FadeIn()
        {
            _fadeTarget = 1.0f;
            _fadingUp = true;
        }

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
            base.OnEnabledChanged(sender, args);
        }

        protected override void OnUpdateOrderChanged(object sender, EventArgs args)
        {
            base.OnUpdateOrderChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }
    }
}
