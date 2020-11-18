using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TerminalGame.UI.Themes
{
    public class ThemeManager
    {
        public Theme CurrentTheme { get; private set; }
        public List<Theme> Themes { get; private set; }

        private static ThemeManager _instance;

        private ThemeManager()
        {
            Themes = new List<Theme>()
            {
                new Theme("BaseTheme"),
            };
            CurrentTheme = Themes.First();
        }

        public static ThemeManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ThemeManager();
            }
            return _instance;
        }

        public void AddTheme(Theme theme)
        {
            Themes.Add(theme);
        }

        public void ChangeTheme(string themeName)
        {
            foreach (Theme t in Themes)
            {
                if (t.ThemeName.ToLower() == themeName.ToLower())
                    CurrentTheme = t;
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentTheme.Update(gameTime);
        }
    }
}
