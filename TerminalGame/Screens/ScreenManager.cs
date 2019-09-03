using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TerminalGame.Screens
{
    public class ScreenManager
    {
        public bool IsInitialized { get; set; }
        public Dictionary<string, Screen> Screens { get; set; }
        public Screen CurrentScreen { get; set; }
        public Screen PreviousScreen { get; set; }
        protected GraphicsDeviceManager _graphics;

        private static ScreenManager _instance;
        public static ScreenManager GetInstance()
        {
            if (_instance == null)
                _instance = new ScreenManager();
            return _instance;
        }

        private ScreenManager()
        {
            Screens = new Dictionary<string, Screen>();
            IsInitialized = false;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            IsInitialized = true;
        }

        public void AddScreen(string key, Screen screen) => Screens.Add(key, screen);

        public bool TryGetScreen(string screen, out Screen outScreen)
        {
            if(Screens.TryGetValue(screen, out Screen retval))
            {
                outScreen = retval;
                return true;
            }
            throw new KeyNotFoundException("The key \"" + screen + "\" was not found.");
        }

        public void ChangeScreen(string name)
        {
            if(TryGetScreen(name, out Screen screen))
            {
                Console.WriteLine($"Switching to screen {name}");
                PreviousScreen = CurrentScreen;
                CurrentScreen = screen;
                CurrentScreen.SwitchOn();
            }
        }

        public void ChangeScreenAndInit(string name)
        {
            if (TryGetScreen(name, out Screen screen))
            {
                Console.WriteLine($"Switching to screen {name}");
                PreviousScreen = CurrentScreen;
                CurrentScreen = screen;
                CurrentScreen.Initialize(_graphics);
                CurrentScreen.SwitchOn();
            }
        }

        public void GoBack()
        {
            Console.WriteLine("Going to previous screen");
            CurrentScreen = PreviousScreen;
            PreviousScreen = CurrentScreen;
            CurrentScreen.SwitchOn();
        }

        public virtual void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            CurrentScreen.Draw(gameTime);
        }
    }
}
