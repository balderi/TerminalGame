using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    // this is either the dumbest thing in the world or it's genious!
    /// <summary>
    /// Possibly the dumbest thing in the world
    /// </summary>
    public class FontManager
    {
        /// <summary>
        /// Pre-defined font sizes
        /// </summary>
        public enum FontSize
        {
            /// <summary>
            /// Extra small font
            /// </summary>
            XSmall,
            /// <summary>
            /// Small font
            /// </summary>
            Small,
            /// <summary>
            /// Medium font
            /// </summary>
            Medium,
            /// <summary>
            /// Large font
            /// </summary>
            Large,
            /// <summary>
            /// Extra large font
            /// </summary>
            XLarge
        }

        private static SpriteFont _fontXS, _fontS, _fontM, _fontL, _fontXL;

        /// <summary>
        /// Get the font relative to the provided font size
        /// </summary>
        /// <param name="fontSize">Size of the font</param>
        /// <returns>The appropriately sized font</returns>
        public static SpriteFont GetFont(FontSize fontSize)
        {
            switch(fontSize)
            {
                case FontSize.XSmall:
                    return _fontXS;
                case FontSize.Small:
                    return _fontS;
                case FontSize.Medium:
                    return _fontM;
                case FontSize.Large:
                    return _fontL;
                case FontSize.XLarge:
                    return _fontXL;
                default:
                    return _fontM;
            }
        }

        /// <summary>
        /// Initialize the fonts
        /// </summary>
        /// <param name="xsmall"></param>
        /// <param name="small"></param>
        /// <param name="medium"></param>
        /// <param name="large"></param>
        /// <param name="xlarge"></param>
        public static void SetFonts(SpriteFont xsmall, SpriteFont small, SpriteFont medium, SpriteFont large, SpriteFont xlarge)
        {
            _fontXS = xsmall;
            _fontS = small;
            _fontM = medium;
            _fontL = large;
            _fontXL = xlarge;
        }
    }
}
