using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.UI
{
    class DropDownElement
    {
        public bool Hover { get; set; }
        public bool Active { get; set; }
        public bool Selected { get; set; }
        public string ElementText { get; private set; }
        public Rectangle Container { get; private set; }

        private SpriteFont _font;
        private Texture2D _texture;

        public DropDownElement(string elementText, Rectangle container, SpriteFont font, GraphicsDevice graphics)
        {
            ElementText = elementText;
            Container = container;
            _font = font;
            _texture = Utilities.Drawing.DrawBlankTexture(graphics);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Hover = false;
            Active = false;
            Selected = false;
        }
    }
}
