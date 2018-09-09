using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Scenes
{
    class SceneManager
    {
        public enum Scene
        {
            Menu,
            SettingsMenu,
            LoadMenu,
            Loading,
            Game
        }

        private static IScene _menu, _settingsMenu, _loadMenu, _loading, _game;

        public static IScene GetScene(Scene scene)
        {
            switch(scene)
            {
                case Scene.Menu:
                    return _menu;
                case Scene.SettingsMenu:
                    return _settingsMenu;
                case Scene.LoadMenu:
                    return _loadMenu;
                case Scene.Loading:
                    return _loading;
                case Scene.Game:
                    return _game;
                default:
                    return _menu;
            }
        }

        public static void SetScenes(IScene menu, IScene settingsMenu, IScene loadMenu, IScene loading, IScene game)
        {
            _menu = menu;
            _settingsMenu = settingsMenu;
            _loadMenu = loadMenu;
            _loading = loading;
            _game = game;
        }
    }
}
