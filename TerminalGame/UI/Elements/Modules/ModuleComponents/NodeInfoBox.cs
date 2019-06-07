using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents
{
    class NodeInfoBox : UIElement
    {
        private SpriteFont _labelFont;
        private readonly string _text;

        public NodeInfoBox(Game game, Point location, Point size, string text, bool hasBorder = true, bool fadeIn = true) : base(game, location, size, hasBorder, fadeIn)
        {
            _text = text;
        }

        public override void Initialize()
        {
            base.Initialize();
            BackgroundColor = Color.Black;
            BorderColor = Color.White;
            _labelFont = FontManager.GetFont("FontXS");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.DrawString(_labelFont, _text, new Vector2(Rectangle.X + 5, Rectangle.Y + 5), Color.White);
            _spriteBatch.End();
        }

        public void ChangeLocation(Point location)
        {
            Point Location = location;
            Rectangle = new Rectangle(Location.X, Location.Y, Rectangle.Width, Rectangle.Height);
        }
    }
}
