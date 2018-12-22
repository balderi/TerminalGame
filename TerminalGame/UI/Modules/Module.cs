using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.UI.Modules.ModuleComponents;

namespace TerminalGame.UI.Modules
{
    public class Module : DrawableGameComponent
    {
        #region fields
        protected SpriteBatch _spriteBatch;
        protected Header _header;
        protected SpriteFont _titleFont;
        protected string _title;
        protected float _opacity, _fadeTarget;
        protected bool _fadingUp, _fadingDown;
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        #endregion

        public Module(Game game, Point location, Point size, string title) : base(game)
        {
            DrawOrderChanged += OnDrawOrderChanged;
            EnabledChanged += OnEnabledChanged;
            VisibleChanged += OnVisibleChanged;
            UpdateOrderChanged += OnUpdateOrderChanged;
            _title = title;
            _opacity = 0;
            _fadeTarget = 1;
            _fadingDown = false;
            _fadingUp = true;
            Rectangle = new Rectangle(location, size);
            SpriteFont headerFont = Utils.FontManager.GetFont("FontXS");
            _header = new Header(_title, headerFont, Rectangle.Width, (int)(headerFont.LineSpacing * 1.25), Rectangle.X, Rectangle.Y, GraphicsDevice);
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
                _header.Update(gameTime);
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

                _header.Draw(_spriteBatch, _opacity);

                Utils.Globals.DrawOuterBorder(_spriteBatch, Rectangle, Utils.Globals.DummyTexture(GraphicsDevice), 1,
                    Utils.Globals.ModuleBorderColor() * _opacity);

                base.Draw(gameTime);
                _spriteBatch.End();
            }
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
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (Rectangle.X + Rectangle.Y);
        }

        public override string ToString()
        {
            return _title;
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
