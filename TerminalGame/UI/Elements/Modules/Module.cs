using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.UI.Elements.Modules.ModuleComponents;

namespace TerminalGame.UI.Elements.Modules
{
    public class Module : UIElement
    {
        #region fields
        protected Header _header;
        protected SpriteFont _titleFont;
        protected string _title;
        #endregion

        #region properties
        #endregion

        public Module(Game game, Point location, Point size, string title) : base(game, location, size)
        {
            DrawOrderChanged += OnDrawOrderChanged;
            EnabledChanged += OnEnabledChanged;
            VisibleChanged += OnVisibleChanged;
            UpdateOrderChanged += OnUpdateOrderChanged;
            _title = title;
            SpriteFont headerFont = Utils.FontManager.GetFont("FontXS");
            _header = new Header(_title, headerFont, Rectangle.Width, (int)(headerFont.LineSpacing * 1.25), Rectangle.X, Rectangle.Y, GraphicsDevice);
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
            _header.Draw(_spriteBatch, _opacity);
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
        }
    }
}
