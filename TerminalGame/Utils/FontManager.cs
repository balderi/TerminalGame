using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TerminalGame.Utils
{
    /// <summary>
    /// Possibly the dumbest thing in the world
    /// </summary>
    public class FontManager
    {
        private static Dictionary<string, SpriteFont> _fontList;

        public enum GlobalFontSize
        {
            small,
            normal,
            large,
        };

        public static void InitializeFonts(ContentManager content)
        {
            _fontList = new Dictionary<string, SpriteFont>
            {

                //Extra Small
                { "FontXSsmall", content.Load<SpriteFont>("fonts/fontxssmall") },
                { "FontXSnormal", content.Load<SpriteFont>("fonts/fontxsnormal") },
                { "FontXSlarge", content.Load<SpriteFont>("fonts/fontxslarge") },

                //Small
                { "FontSsmall", content.Load<SpriteFont>("fonts/fontssmall") },
                { "FontSnormal", content.Load<SpriteFont>("fonts/fontsnormal") },
                { "FontSlarge", content.Load<SpriteFont>("fonts/fontslarge") },

                //Medium
                { "FontMsmall", content.Load<SpriteFont>("fonts/fontmsmall") },
                { "FontMnormal", content.Load<SpriteFont>("fonts/fontmnormal") },
                { "FontMlarge", content.Load<SpriteFont>("fonts/fontmlarge") },

                //Large
                { "FontLsmall", content.Load<SpriteFont>("fonts/fontlsmall") },
                { "FontLnormal", content.Load<SpriteFont>("fonts/fontlnormal") },
                { "FontLlarge", content.Load<SpriteFont>("fonts/fontllarge") },

                //Extra Large
                { "FontXLsmall", content.Load<SpriteFont>("fonts/fontxlsmall") },
                { "FontXLnormal", content.Load<SpriteFont>("fonts/fontxlnormal") },
                { "FontXLlarge", content.Load<SpriteFont>("fonts/fontxllarge") }
            };
        }

        /// <summary>
        /// Returns the given font, if it exists
        /// </summary>
        /// <param name="fontKey">"<c>FontM</c>" for medium font, "<c>FontXL</c>" for XL font, etc.</param>
        /// <returns></returns>
        public static SpriteFont GetFont(string fontKey)
        {
            if (_fontList.Count < 1)
                throw new InvalidOperationException("FontList has not been initialized");
            if (_fontList.TryGetValue(fontKey + Globals.Utils.GlobalFontSize.ToString(), out SpriteFont retval))
                return retval;
            else
                throw new KeyNotFoundException("FontKey '" + fontKey + Globals.Utils.GlobalFontSize.ToString() + "' does not exist in the FontList");
        }
    }
}
