using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using TerminalGame.UI.Modules;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.Computers;
using TerminalGame.States;
using System.Reflection;
using TerminalGame.Utilities;
using TerminalGame.Scenes;
using System.Threading;
using TerminalGame.Computers.FileSystems;
using TerminalGame.UI.Shaders;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        StateMachine _stateMachine;

        KeyboardState _prevKbState, _newKbState;

        Thread _loadingThread;

        LoadingScreen _load;

        OS.OS _os;

        private SpriteFont _font, _fontL, _fontXL, _menuFont, _fontS, _fontXS;
        private Song bgm_game, bgm_menu;
        private SoundEffect networkMapNodeHover, networkMapNodeClick;
        private readonly string GameTitle;
        private float musicVolume, /*audioVolume,*/ masterVolume;

        MenuScene mainMenuScene;
        SettingsScene settingsMenuScene;
        LoadGameScene loadGameMenuScene;
        LoadingScene loadingScene;
        GameScene gameScene;

        RenderTarget2D renderTarget;
        
        Terminal terminal;
        NetworkMap networkMap;
        StatusBar statusBar;
        RemoteView remoteView;
        NotesModule notes;
        
        Computer playerComp;

        Rectangle bgR;
        Texture2D bg, computer, spinner01, spinner02, spinner03, spinner04, spinner05, spinner06, spinner07, spinner08;
        Texture2D[] backgrounds;

        Dictionary<string, Texture2D> NetworkNodeSpinners;
        
        private BloomFilter _bloomFilter;
        private Random _random;

        /// <summary>
        /// Main game constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            GameTitle = String.Format("TerminalGame v{0}.{1}a", version.Major, version.Minor);

            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            GameManager.GetInstance().SetGraphicsDeviceManager(_graphics);
            GameManager.GetInstance().IsGameRunning = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Console.WriteLine("Initializing...");
            KeyboardInput.Initialize(this, 500f, 20);
            Window.Title = GameTitle;
            
            IsMouseVisible = true;
            GameManager.GetInstance().IsGameRunning = false;
            GameManager.GetInstance().IsFullScreen = false;
            GameManager.GetInstance().BloomEnabled = true;

            _stateMachine = new StateMachine(MainMenuState.Instance);

            _random = new Random(DateTime.Now.Millisecond);

            masterVolume = 1.0f;
            musicVolume = 0.2f;
            //audioVolume = 1.0f;

            GameManager.GetInstance().ResolutionW = 1366;
            GameManager.GetInstance().ResolutionH = 768;

            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1366;

            //Set game to fullscreen
            _graphics.HardwareModeSwitch = false;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            Console.WriteLine("Resolution is now " + _graphics.PreferredBackBufferWidth + " x " + _graphics.PreferredBackBufferHeight);
            Drawing.SetBlankTexture(GraphicsDevice);

            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            MediaPlayer.Volume = musicVolume * masterVolume; // 0.5f;
            MediaPlayer.IsRepeating = true;
            base.Initialize();
            Console.WriteLine("Done initializing");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Console.WriteLine("Loading content...");
            base.LoadContent();
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Console.Write("Loading fonts... ");
            _fontXS = Content.Load<SpriteFont>("Fonts/terminalFontXS");
            _fontS = Content.Load<SpriteFont>("Fonts/terminalFontS");
            _font = Content.Load<SpriteFont>("Fonts/terminalFont");
            _fontL = Content.Load<SpriteFont>("Fonts/terminalFontL");
            _fontXL = Content.Load<SpriteFont>("Fonts/terminalFontXL");
            _menuFont = Content.Load<SpriteFont>("Fonts/terminalFontL");
            Console.WriteLine("Done");
            
            Console.Write("Loading music... ");
            bgm_game = Content.Load<Song>("Audio/Music/ambientbgm1_2");
            bgm_menu = Content.Load<Song>("Audio/Music/mainmenu");
            MediaPlayer.Play(bgm_menu);
            Console.WriteLine("Done");

            Console.Write("Loading audio... ");
            networkMapNodeHover = Content.Load<SoundEffect>("Audio/Sounds/interface4");
            networkMapNodeClick = Content.Load<SoundEffect>("Audio/Sounds/click1");
            Console.WriteLine("Done");

            FontManager.SetFonts(_fontXS, _fontS, _font, _fontL, _fontXL);
            
            _load = new LoadingScreen(FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.Small));

            Console.Write("Loading textures... ");
            bgR = new Rectangle(new Point(0, 0), new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            //bg = Content.Load<Texture2D>("Textures/bg");

            backgrounds = new Texture2D[]
            {
                Content.Load<Texture2D>("Textures/bg"),
                Content.Load<Texture2D>("Textures/bg2"),
                Content.Load<Texture2D>("Textures/bg3"),
                Content.Load<Texture2D>("Textures/bg4"),
            };

            computer = Content.Load<Texture2D>("Textures/nmapComputer");

            // Various markers for the networkmap
            spinner01 = Content.Load<Texture2D>("Textures/spinner01");
            spinner02 = Content.Load<Texture2D>("Textures/spinner02");
            spinner03 = Content.Load<Texture2D>("Textures/spinner03");
            spinner04 = Content.Load<Texture2D>("Textures/spinner04");
            spinner05 = Content.Load<Texture2D>("Textures/spinner05");
            spinner06 = Content.Load<Texture2D>("Textures/spinner06");
            spinner07 = Content.Load<Texture2D>("Textures/spinner07");
            spinner08 = Content.Load<Texture2D>("Textures/spinner08");

            NetworkNodeSpinners = new Dictionary<string, Texture2D>()
            {
                { "ConnectedSpinner", spinner01 },
                { "PlayerSpinner", spinner02 },
                { "03", spinner03 },
                { "MissionSpinner", spinner04 },
                { "05", spinner05 },
                { "06", spinner06 },
                { "07", spinner07 },
                { "HoverSpinner", spinner08 },
            };

            bg = backgrounds[_random.Next(0, backgrounds.Length)];
            Console.WriteLine("Done");

            Console.Write("Loading scenes... ");
            mainMenuScene = new MenuScene(GameTitle, Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice, _stateMachine);
            mainMenuScene.ButtonClicked += MainMenu_ButtonClicked;
            loadGameMenuScene = new LoadGameScene(Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice, _stateMachine);
            settingsMenuScene = new SettingsScene(Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice, _stateMachine);
            loadingScene = new LoadingScene(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), Window, GraphicsDevice);
            gameScene = new GameScene(bg, bgR, _stateMachine);

            SceneManager.SetScenes(mainMenuScene, settingsMenuScene, loadGameMenuScene, loadingScene, gameScene);
            Console.WriteLine("Done");
            //Load our Bloomfilter!
            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Small;
            _bloomFilter.BloomStreakLength = 5;
            _bloomFilter.BloomThreshold = -0.1f;
            _bloomFilter.BloomStrengthMultiplier = 0.5f;

            Console.WriteLine("Done loading");
            _stateMachine.Transition(GameState.MainMenu);
        }

        /// <summary>
        /// To happen when Exit has been called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            Console.WriteLine("Exiting...");
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// When a button on the main menu is clicked
        /// </summary>
        /// <param name="e">Contains the text of the pressed button</param>
        private void MainMenu_ButtonClicked(UI.ButtonPressedEventArgs e)
        {
            switch(e.Button)
            {
                case "New Game":
                    {
                        _stateMachine.Transition(GameState.GameLoading);
                        _loadingThread = new Thread(new ThreadStart(StartNewGame));
                        _loadingThread.Start();
                        break;
                    }
                case "Load Game":
                    {
                        _stateMachine.Transition(GameState.LoadMenu);
                        break;
                    }
                case "Settings":
                    {
                        _stateMachine.Transition(GameState.SettingsMenu);
                        break;
                    }
                case "Quit Game":
                    {
                        Exit();
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _bloomFilter.Dispose();
            base.UnloadContent();
        }

        /// <summary>
        /// When game window is focused
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">EventArgs</param>
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
        }

        /// <summary>
        /// When game window loses focus (eg. alt+tab)
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">EventArgs</param>
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
        }

        private void StartNewGame()
        {
            Console.WriteLine("Starting new game...");

            loadingScene.LoadItem = "Generating computers...";

            _os = OS.OS.GetInstance();

            FileSystem playerFS = new FileSystem();
            playerFS.BuildBasicFileSystem();
            playerFS.ChangeDir("bin");
            playerFS.AddFile("sshnuke", "01110011011100110110100001101110011101010110101101100101001000000110000101101100011011000110111101110111011100110010000001111001011011110111010100100000011101000110111100100000011001110110000101101001011011100010000001110101011011100110000101110101011101000110100001101111011100100110100101111010011001010110010000100000011001010110111001110100011100100111100100100000011010010110111001110100011011110010000001110010011001010110110101101111011101000110010100100000011100110111100101110011011101000110010101101101011100110010000001100010011110010010000001100011011000010111010101110011011010010110111001100111001000000110000100100000011000100111010101100110011001100110010101110010001000000110111101110110011001010111001001100110011011000110111101110111001000000110100101101110001000000110000100100000011000110110100001110101011011100110101100100000011011110110011000100000011000110110111101100100011001010010000001100100011001010111001101101001011001110110111001100101011001000010000001110100011011110010000001100111011101010110000101110010011001000010000001100001011001110110000101101001011011100111001101110100001000000110001101110010011110010111000001110100011011110110011101110010011000010111000001101000011010010110001100100000011000010111010001110100011000010110001101101011011100110010000001101111011011100010000001010011010100110100100000100000011101100110010101110010011100110110100101101111011011100010000001101111011011100110010100101110");
            playerFS.AddFile("nmap", "010011100110110101100001011100000010000001101001011100110010000001100001001000000110011001110010011001010110010100100000011000010110111001100100001000000110111101110000011001010110111000101101011100110110111101110101011100100110001101100101001000000111001101100101011000110111010101110010011010010111010001111001001000000111001101100011011000010110111001101110011001010111001000101100001000000110111101110010011010010110011101101001011011100110000101101100011011000111100100100000011101110111001001101001011101000111010001100101011011100010000001100010011110010010000001000111011011110111001001100100011011110110111000100000010011000111100101101111011011100010110000100000011101010111001101100101011001000010000001110100011011110010000001100100011010010111001101100011011011110111011001100101011100100010000001101000011011110111001101110100011100110010000001100001011011100110010000100000011100110110010101110010011101100110100101100011011001010111001100100000011011110110111000100000011000010010000001100011011011110110110101110000011101010111010001100101011100100010000001101110011001010111010001110111011011110111001001101011001011000010000001110100011010000111010101110011001000000110001001110101011010010110110001100100011010010110111001100111001000000110000100100000001000100110110101100001011100000010001000100000011011110110011000100000011101000110100001100101001000000110111001100101011101000111011101101111011100100110101100101110");
            playerFS.ChangeDir("/");

            playerComp = new Computer(Computer.Type.Workstation, "127.0.0.1", "localhost", "pasword", playerFS, new int[] { 10, 21, 22, 23, 25, 53, 67, 69, 80, 110, 123, 143, 443 });
            
            playerComp.SetSpeed(1.0f);

            Console.WriteLine("Setting up computers...");
            Computers.Computers.DoComputers(50);
            Computers.Computers.computerList.Add(playerComp);

            playerComp.GetRoot();
            playerComp.Connect(true);

            Player.GetInstance().PlayersComputer = playerComp;

            loadingScene.LoadItem = "Loading modules...";
            
            int thirdWidth = _graphics.PreferredBackBufferWidth / 3;
            int thirdHeight = _graphics.PreferredBackBufferHeight / 3;
            int tqWidth = Convert.ToInt32(_graphics.PreferredBackBufferWidth * 0.75);


            // TLDR: It's a mess... good luck!
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // MODULE SIZING: (may still need to be properly refined, as border/margin stuff relies heavily on magic numbers (bad)) //
            // Terminal is one third of the screen width minus 2px on either side (1px border + 1px margin)                         //
            // Network map is Full screen width minus terminal width minus 7px                                                      //
            //                    (terminal border/margin + map border/margin + 1px space between modules)                          //
            // Remote view is thee-quarters of full screen width minus terminal width minus border/margin)                          //
            // Notes module is full screen width minus terminal width minus remote view width minus border/margin                   //
            // Status bar is full screen width minus terminal width minus border/margin                                             //
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Console.WriteLine("Loading terminal...");
            terminal = new Terminal(GraphicsDevice,
                new Rectangle(2, 2,
                thirdWidth - 4, _graphics.PreferredBackBufferHeight - 4),
                _font)
            {
                BackgroundColor = Color.Black * 0.9f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Terminal",
                Font = _fontS,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading networkmap...");
            networkMap = new NetworkMap(Window, GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, _graphics.PreferredBackBufferHeight - thirdHeight + 2, 
                _graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, thirdHeight - 4), 
                computer, _fontS, NetworkNodeSpinners)
            {
                BackgroundColor = Color.Black * 0.9f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Network Map",
                Font = _fontS,
                IsActive = true,
                IsVisible = true,
            };
            networkMap.Hover = networkMapNodeHover;
            networkMap.Click = networkMapNodeClick;

            Console.WriteLine("Loading statusbar...");
            statusBar = new StatusBar(GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, 2, 
                _graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, (int)_fontL.MeasureString("A").Y - 4), 
                _fontXS)
            {
                BackgroundColor = Color.MidnightBlue,
                BorderColor = Color.MidnightBlue,
                HeaderColor = Color.MidnightBlue,
                Title = "Status Bar",
                Font = _fontL,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading remoteview...");
            remoteView = new RemoteView(GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, statusBar.Container.Height + 5, 
                tqWidth - terminal.Container.Width - 7, _graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10), 
                _fontL, _font)
            {
                BackgroundColor = Color.Black * 0.9f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Remote System",
                Font = _fontS,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading notes...");
            notes = new NotesModule(GraphicsDevice, 
                new Rectangle(remoteView.Container.X + remoteView.Container.Width + 3, statusBar.Container.Height + 5,
                _graphics.PreferredBackBufferWidth - remoteView.Container.X - remoteView.Container.Width - 5, _graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10), 
                _font)
            {
                BackgroundColor = Color.Black * 0.9f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Friendly neighborhood notepad",
                Font = _fontS,
                IsActive = true,
                IsVisible = true,
            };

            loadingScene.LoadItem = "Initializing...";
            
            _os.Init(terminal, remoteView, networkMap, statusBar, notes);

            terminal.Init();
            Console.WriteLine("Game started");

            GameManager.GetInstance().IsGameRunning = true;

            MediaPlayer.Stop();
            _stateMachine.Transition(GameState.GameRunning);
            MediaPlayer.Play(bgm_game);
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _newKbState = Keyboard.GetState();
            if(_newKbState != _prevKbState)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                    //bloomEnabled = !bloomEnabled;
                }
            }
            _prevKbState = _newKbState;

            KeyboardInput.Update();

            _stateMachine.UpdateState(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (GameManager.GetInstance().BloomEnabled)
            {
                GraphicsDevice.SetRenderTarget(renderTarget);
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                _stateMachine.DrawState(_spriteBatch);
                _spriteBatch.End();
                _bloomFilter.BloomStrengthMultiplier = 1 * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1) * 0.1f + 0.5f);
                Texture2D bloom = _bloomFilter.Draw(renderTarget, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _spriteBatch.Begin(blendState: BlendState.Additive);
                _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(bloom, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                _stateMachine.DrawState(_spriteBatch);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
