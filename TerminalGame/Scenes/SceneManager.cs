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
            Loading,
            Game
        }

        private static IScene _menu, _loading, _game;

        public static IScene GetScene(Scene scene)
        {
            switch(scene)
            {
                case Scene.Menu:
                    return _menu;
                case Scene.Loading:
                    return _loading;
                case Scene.Game:
                    return _game;
                default:
                    return _menu;
            }
        }

        public static void SetScenes(IScene menu, IScene loading, IScene game)
        {
            _menu = menu;
            _loading = loading;
            _game = game;
        }
    }
}
