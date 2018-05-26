using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    // this is either the dumbest thing in the world or it's genious!
    public class FontManager
    {
        public enum FontSize
        {
            XSmall,
            Small,
            Medium,
            Large,
            XLarge
        }

        private static SpriteFont fontXS, fontS, fontM, fontL, fontXL;

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
