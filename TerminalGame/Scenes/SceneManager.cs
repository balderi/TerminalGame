namespace TerminalGame.Scenes
{
    //TODO: Rework this whole class - make a factory or something.
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
            NewGame,
        }

        private static IScene _menu, _settingsMenu, _loadMenu, _loading, _gameRunning, _gameOver, _newGame;

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
                case Scene.NewGame:
                    return _newGame;
                default:
                    return _menu;
            }
        }

        public static void SetScenes(IScene menu, IScene settingsMenu, IScene loadMenu, IScene loading, IScene gameRunning, IScene gameOver, IScene newGame)
        {
            _menu = menu;
            _settingsMenu = settingsMenu;
            _loadMenu = loadMenu;
            _loading = loading;
            _gameRunning = gameRunning;
            _gameOver = gameOver;
            _newGame = newGame;
        }
    }
}
