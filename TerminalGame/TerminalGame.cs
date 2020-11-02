using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Utils;
using TerminalGame.UI.Elements.Modules;
using Microsoft.Xna.Framework.Media;
using TerminalGame.Screens;
using System;
using System.Reflection;
using TerminalGame.UI.Themes;
using TerminalGame.Time;
using System.IO;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TerminalGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private MusicManager _musicManager;
        private ThemeManager _themeManager;
        private ScreenManager _screenManager;

        private KeyboardState _oldState;

        public readonly string TitleAndVersion;
        public Version version;

        public string Title { get; private set; }
        public string Version { get; private set; }
        public string BuildNumber { get; private set; }
        public bool IsGameRunning { get; set; }
        public Player Player { get; set; }
        public GameSpeed CurrentGameSpeed { get; set; }
        public Terminal Terminal { get; set; }
        //public static object Companies { get; internal set; }

        public TerminalGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            version = Assembly.GetEntryAssembly().GetName().Version;
            Title = Assembly.GetEntryAssembly().GetName().Name;
            Version = $"v{version.Major}.{version.Minor}a";
            TitleAndVersion = $"{Title} {Version}";
            BuildNumber = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("yyyyMMdd").ToString();
            IsFixedTimeStep = true;
            IsGameRunning = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            CurrentGameSpeed = GameSpeed.Paused;

            _oldState = Keyboard.GetState();

            _graphics.HardwareModeSwitch = false;
            _graphics.PreferredBackBufferHeight = 1080;// (int)(GraphicsDevice.DisplayMode.Height * 0.8);
            _graphics.PreferredBackBufferWidth = 1920;// (int)(GraphicsDevice.DisplayMode.Width * 0.8);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Globals.Settings.GameHeight = _graphics.PreferredBackBufferHeight;
            Globals.Settings.GameWidth = _graphics.PreferredBackBufferWidth;
            FontManager.InitializeFonts(Content);

            Window.Title = TitleAndVersion;

            //TODO: Load this from file
            Globals.Utils.SetGlobalFontSize();
            Globals.Utils.GenerateDummyTexture(GraphicsDevice);
            IsMouseVisible = true;
            Globals.Settings.MasterVolume = 1.0f;
            Globals.Settings.SoundVolume = 0.5f;
            Globals.Settings.MusicVolume = 0.5f;

            _screenManager = ScreenManager.GetInstance();
            _screenManager.Initialize(_graphics);
            _screenManager.AddScreen("splash", new SplashScreen(this));
            _screenManager.AddScreen("mainMenu", new MainMenuScreen(this));
            _screenManager.AddScreen("settingsMenu", new SettingsScreen(this));
            _screenManager.AddScreen("gameLoading", new GameLoadingScreen(this));
            _screenManager.AddScreen("gameRunning", new GameRunningScreen(this));
            _screenManager.AddScreen("loadGame", new LoadGameScreen(this));
            _screenManager.ChangeScreen("splash");

            _themeManager = ThemeManager.GetInstance();

            Theme test = new Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);

            _themeManager.AddTheme(test);
            _themeManager.ChangeTheme("test");
           
            Console.WriteLine("init done");

            Console.WriteLine(Companies.Generator.CompanyGenerator.Lengths);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _musicManager = MusicManager.GetInstance();
            _musicManager.AddSong("mainMenuBgm", Content.Load<Song>("Audio/Music/mainmenu"));
            _musicManager.AddSong("gameBgm", Content.Load<Song>("Audio/Music/ambientbgm1_2"));
            _musicManager.AddSong("hackLoop", Content.Load<Song>("Audio/Music/hackloop1"));
            Console.WriteLine("load done");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        public void StartNewGame()
        {
            Console.WriteLine("Starting new game...");
            IsGameRunning = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_oldState.IsKeyDown(Keys.M) && Keyboard.GetState().IsKeyUp(Keys.M) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            }

            if (_oldState.IsKeyDown(Keys.F1) && Keyboard.GetState().IsKeyUp(Keys.F1))
            {
                _musicManager.FadeOut();
            }

            if (_oldState.IsKeyDown(Keys.F2) && Keyboard.GetState().IsKeyUp(Keys.F2))
            {
                _musicManager.FadeIn();
            }

            _themeManager.Update(gameTime);
            _musicManager.Update(gameTime);
            _screenManager.Update(gameTime);

            _oldState = Keyboard.GetState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _screenManager.Draw(gameTime);
        }
    }
}
