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
            GameRunning,
            GameOver,
        }

        private static IScene _menu, _settingsMenu, _loadMenu, _loading, _gameRunning, _gameOver;

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
                case Scene.GameRunning:
                    return _gameRunning;
                case Scene.GameOver:
                    return _gameOver;
                default:
                    return _menu;
            }
        }

        public static void SetScenes(IScene menu, IScene settingsMenu, IScene loadMenu, IScene loading, IScene gameRunning, IScene gameOver)
        {
            _menu = menu;
            _settingsMenu = settingsMenu;
            _loadMenu = loadMenu;
            _loading = loading;
            _gameRunning = gameRunning;
            _gameOver = gameOver;
        }
    }
}
