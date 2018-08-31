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

        private static SpriteFont fontXS, fontS, fontM, fontL, fontXL;

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
                    return fontXS;
                case FontSize.Small:
                    return fontS;
                case FontSize.Medium:
                    return fontM;
                case FontSize.Large:
                    return fontL;
                case FontSize.XLarge:
                    return fontXL;
                default:
                    return fontM;
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
            fontXS = xsmall;
            fontS = small;
            fontM = medium;
            fontL = large;
            fontXL = xlarge;
        }
    }
}
