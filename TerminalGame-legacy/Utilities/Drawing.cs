using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.Utils
{
    class Drawing
    {
        private static Texture2D _texture2D;
        public static void SetBlankTexture(GraphicsDevice graphics)
        {
            _texture2D = new Texture2D(graphics, 1, 1);
            _texture2D.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Will draw a blank texture. Add color in draw call with SpriteBatch.
        /// </summary>
        /// <param name="graphics">GraphicsAdapter</param>
        /// <returns>Blank (white) Texture2D</returns>
        public static Texture2D DrawBlankTexture(GraphicsDevice graphics)
        {
            //Texture2D retval = new Texture2D(Graphics, 1, 1);
            //retval.SetData(new[] { Color.White });
            //return retval;
            return _texture2D;
        }

        /// <summary>
        /// Draws a border around the object.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="rectangle">Size of border</param>
        /// <param name="texture"></param>
        /// <param name="borderWidth">Width of border. Negative values draws the border inside the object. (I think -b)</param>
        /// <param name="borderColor">Color of the border. Use eg. Color.Red * 0.5f to make border 50% transparent</param>
        public static void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangle, Texture2D texture, int borderWidth, Color borderColor)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left - borderWidth, rectangle.Top - borderWidth, borderWidth, rectangle.Height + 2 * borderWidth), borderColor); // Left
            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top - borderWidth, borderWidth, rectangle.Height + 2 * borderWidth), borderColor); // Right
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top - borderWidth, rectangle.Width, borderWidth), borderColor); // Top
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, borderWidth), borderColor); // Bottom
        }

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);


            spriteBatch.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None, 0);

        }
    }
}
