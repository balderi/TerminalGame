﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Elements.Modules.ModuleComponents;

namespace TerminalGame.UI.Elements.Modules
{
    public partial class Module : UIElement
    {
        #region fields
        protected Header _header;
        protected SpriteFont _titleFont;
        protected string _title;
        protected bool _hasHeader;
        #endregion

        #region properties
        #endregion

        public Module(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, hasBorder)
        {
            DrawOrderChanged += OnDrawOrderChanged;
            EnabledChanged += OnEnabledChanged;
            VisibleChanged += OnVisibleChanged;
            UpdateOrderChanged += OnUpdateOrderChanged;
            _title = title;
            _hasHeader = hasHeader;
            if (_hasHeader)
            {
                SpriteFont headerFont = Utils.FontManager.GetFont("FontXS");
                _header = new Header(_title, headerFont, Rectangle.Width,
                                    (int)(headerFont.LineSpacing * 1.25), Rectangle.X, Rectangle.Y);
            }
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
            if (!Enabled)
                return;

            base.Update(gameTime);
            if (_hasHeader)
                _header.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                               null, null, _rasterizerState);
            if (!Visible)
                return;

            base.Draw(gameTime);
            Rectangle currentRect = _spriteBatch.GraphicsDevice.ScissorRectangle;
            _spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;
            ScissorDraw(gameTime);
            if (_hasHeader)
                _header.Draw(_spriteBatch, Opacity);
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
        }

        public virtual void ScissorDraw(GameTime gameTime)
        {

        }
    }
}
