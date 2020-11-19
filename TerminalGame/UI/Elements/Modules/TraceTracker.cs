using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Tracers;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules
{
    public class TraceTracker : Module
    {
        private string _text;
        private SpriteFont _font;

        public TraceTracker(Game game, Point location, Point size, string title, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _font = FontManager.GetFont("FontM");
            _text = $"Trace: {ActiveTracer.GetInstance().GetTracePercentage()}";
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            base.ScissorDraw(gameTime);
            _spriteBatch.DrawString(_font, _text, new Vector2(Rectangle.X + 5, Rectangle.Y + 25), Color.White * Opacity);
        }

        public override void Update(GameTime gameTime)
        {
            _text = $"Trace: {ActiveTracer.GetInstance().GetTracePercentage()}";
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
