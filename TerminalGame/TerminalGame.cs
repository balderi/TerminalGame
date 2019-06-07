using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Utils;
using TerminalGame.UI.Elements.Modules;
using Microsoft.Xna.Framework.Media;
using TerminalGame.States;
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
        private SpriteBatch _spriteBatch;

        private MusicManager _musicManager;
        private ThemeManager _themeManager;

        private KeyboardState _oldState;

        private StateMachine stateMachine;

        public readonly string TitleAndVersion;
        public Version version;

        public string Title { get; private set; }
        public string Version { get; private set; }
        public string BuildNumber { get; private set; }
        public bool IsGameRunning { get; set; }
        public Player Player { get; set; }
        public GameSpeed CurrentGameSpeed { get; set; }
        public Terminal Terminal { get; set; }

        public TerminalGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            version = Assembly.GetEntryAssembly().GetName().Version;
            Title = Assembly.GetEntryAssembly().GetName().Name;
            Version = String.Format("v{0}.{1}a", version.Major, version.Minor);
            TitleAndVersion = String.Format("{0} {1}", Title, Version);
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
            _graphics.PreferredBackBufferHeight = 768;// (int)(GraphicsDevice.DisplayMode.Height * 0.8);
            _graphics.PreferredBackBufferWidth = 1366;// (int)(GraphicsDevice.DisplayMode.Width * 0.8);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Globals.GameHeight = _graphics.PreferredBackBufferHeight;
            Globals.GameWidth = _graphics.PreferredBackBufferWidth;
            FontManager.InitializeFonts(Content);

            Window.Title = TitleAndVersion;

            //TODO: Load this from file
            Globals.SetGlobalFontSize();
            Globals.GenerateDummyTexture(GraphicsDevice);
            IsMouseVisible = true;

            stateMachine = StateMachine.GetInstance();
            stateMachine.Initialize(SplashState.GetInstance(), _graphics, new SplashScreen(this), this);

            _themeManager = ThemeManager.GetInstance();

            Theme test = new Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);

            _themeManager.AddTheme(test);
            _themeManager.ChangeTheme("test");

            GameClock.Initialize();
           
            Console.WriteLine("init done");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            stateMachine.Update(gameTime);

            _oldState = Keyboard.GetState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            stateMachine.Draw(gameTime);
        }
    }
}
