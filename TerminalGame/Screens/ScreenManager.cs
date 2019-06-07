using System.Collections.Generic;

namespace TerminalGame.Screens
{
    public class ScreenManager
    {
        private Dictionary<string, Screen> _availableScreens;

        private static ScreenManager _instance;
        public static ScreenManager GetInstance()
        {
            if (_instance == null)
                _instance = new ScreenManager();
            return _instance;
        }

        private ScreenManager()
        {
            _availableScreens = new Dictionary<string, Screen>();
        }

        public void Initialize()
        {

        }

        public void AddScreen(string key, Screen screen) => _availableScreens.Add(key, screen);

        public bool TryGetScreen(string screen, out Screen outScreen)
        {
            if(_availableScreens.TryGetValue(screen, out Screen retval))
            {
                outScreen = retval;
                return true;
            }
            throw new KeyNotFoundException("The key " + screen + " was not found.");
        }
    }
}
