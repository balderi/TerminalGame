//From https://github.com/UnterrainerInformatik/Monogame-Textbox <3 -b

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.Utils.TextHandler
{
    /// <summary>
    /// Extensions for TextBox
    /// </summary>
    public static class Extensions
    {
        private static Texture2D pixel;

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
        public static Texture2D GetWhitePixel(this SpriteBatch spriteBatch)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false,
                    SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            return pixel;
        }

        /// <summary>
        /// Who knows
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(this int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            if (value < min)
            {
                return min;
            }
            return value;
        }
    }
}
