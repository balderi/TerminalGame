using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utils
{
    public static class Globals
    {
        public static FontManager.GlobalFontSize GlobalFontSize;
        private static Texture2D _dummy;

        public static int GameWidth;
        public static int GameHeight;

        public static void SetGlobalFontSize(FontManager.GlobalFontSize fontSize = FontManager.GlobalFontSize.normal)
        {
            GlobalFontSize = fontSize;
        }

        public static void GenerateDummyTexture(GraphicsDevice graphicsDevice)
        {
            _dummy = new Texture2D(graphicsDevice, 1, 1);
            _dummy.SetData(new Color[] { Color.White });
        }

        public static Texture2D DummyTexture()
        {
            return _dummy;
        }

        public static void DrawOuterBorder(SpriteBatch spriteBatch, Rectangle rectangle, Texture2D texture, int borderWidth, Color borderColor)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left - borderWidth, rectangle.Top - borderWidth, 
                                                    borderWidth, rectangle.Height + 2 * borderWidth), borderColor); // Left

            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top - borderWidth, 
                                                    borderWidth, rectangle.Height + 2 * borderWidth), borderColor); // Right

            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top - borderWidth, 
                                                    rectangle.Width, borderWidth), borderColor); // Top

            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, 
                                                    rectangle.Width, borderWidth), borderColor); // Bottom
        }

        public static void DrawInnerBorder(SpriteBatch spriteBatch, Rectangle rectangle, Texture2D texture, int borderWidth, Color borderColor)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, borderWidth, rectangle.Height), borderColor); // Left
            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top - borderWidth, borderWidth, rectangle.Height), borderColor); // Right
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top - borderWidth, rectangle.Width, borderWidth), borderColor); // Top
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom - borderWidth, rectangle.Width, borderWidth), borderColor); // Bottom
        }
    }
}
