using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.UI.Themes
{
    class ThemeManager
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
        }

        public static ThemeManager GetInstance()
        {
            if(_instance == null)
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
            foreach(Theme t in Themes)
            {
                if (t.ThemeName.ToLower() == themeName.ToLower())
                    CurrentTheme = t;
            }
        }
    }
}
