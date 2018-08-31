using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateMachine stateMachine;

        Thread loadingThread;

        LoadingScreen load;

        OS.OS os;

        private SpriteFont font, fontL, fontXL, menuFont, fontS, fontXS;
        private Song bgm_game, bgm_menu;
        private readonly string GameTitle;
        private float musicVolume, /*audioVolume,*/ masterVolume;
        private bool gameStarted;

        MenuScene menuScene;
        SettingsScene settingsScene;
        LoadingScene loadingScene;
        GameScene gameScene;


        Terminal terminal;
        NetworkMap networkMap;
        StatusBar statusBar;
        RemoteView remoteView;
        NotesModule notes;
        
        Computer playerComp;

        Rectangle bgR;
        Texture2D bg, computer, spinner01, spinner02, spinner03, spinner04, spinner05, spinner06, spinner07, spinner08;

        Dictionary<string, Texture2D> NetworkNodeSpinners;

        /// <summary>
        /// Main game constructor
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            GameTitle = String.Format("TerminalGame v{0}.{1}a", version.Major, version.Minor);
            IsFixedTimeStep = true;
            gameStarted = false;
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
            
            stateMachine = new StateMachine(GameMenuState.Instance);

            masterVolume = 1.0f;
            musicVolume = 0.2f;
            //audioVolume = 1.0f;

            //graphics.PreferredBackBufferHeight = 768;
            //graphics.PreferredBackBufferWidth = 1366;

            //Set game to fullscreen
            graphics.HardwareModeSwitch = false;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Console.WriteLine("Resolution is now " + graphics.PreferredBackBufferWidth + " x " + graphics.PreferredBackBufferHeight);

            Drawing.SetBlankTexture(GraphicsDevice);

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Console.Write("Loading fonts");
            fontXS = Content.Load<SpriteFont>("Fonts/terminalFontXS");
            Console.Write(".");
            fontS = Content.Load<SpriteFont>("Fonts/terminalFontS");
            Console.Write(".");
            font = Content.Load<SpriteFont>("Fonts/terminalFont");
            Console.Write(".");
            fontL = Content.Load<SpriteFont>("Fonts/terminalFontL");
            Console.Write(".");
            fontXL = Content.Load<SpriteFont>("Fonts/terminalFontXL");
            Console.Write(".");
            menuFont = Content.Load<SpriteFont>("Fonts/terminalFontL");
            Console.Write(".");
            bgm_game = Content.Load<Song>("Audio/Music/ambientbgm1_2");
            Console.Write(".");
            bgm_menu = Content.Load<Song>("Audio/Music/mainmenu");
            Console.WriteLine("Done");
            MediaPlayer.Play(bgm_menu);

            FontManager.SetFonts(fontXS, fontS, font, fontL, fontXL);
            
            Window.TextInput += Window_TextInput;

            load = new LoadingScreen(fontL, fontS);

            Console.WriteLine("Loading textures");
            bgR = new Rectangle(new Point(0, 0), new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            bg = Content.Load<Texture2D>("Textures/bg");

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

            Console.WriteLine("Loading scenes...");
            menuScene = new MenuScene(GameTitle, Window, fontL, fontXL, GraphicsDevice);
            menuScene.ButtonClicked += MainMenu_ButtonClicked;
            settingsScene = new SettingsScene(Window, font, font, GraphicsDevice);
            loadingScene = new LoadingScene(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Window, GraphicsDevice);
            gameScene = new GameScene(bg, bgR);

            SceneManager.SetScenes(menuScene, settingsScene, loadingScene, gameScene);

            Console.WriteLine("Done loading");
            stateMachine.Transition(Keys.A);
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if(e.Key == Keys.Escape && gameStarted)
            {
                stateMachine.Transition(e.Key);
            }
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
            Console.WriteLine("e: " + e.Button);
            switch(e.Button)
            {
                case "New Game":
                    {
                        stateMachine.Transition(Keys.Attn);
                        loadingThread = new Thread(new ThreadStart(StartNewGame));
                        loadingThread.Start();
                        break;
                    }
                case "Load Game":
                    {
                        break;
                    }
                case "Settings":
                    {
                        stateMachine.Transition(Keys.Apps);
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

            os = OS.OS.GetInstance();

            FileSystem playerFS = new FileSystem();
            playerFS.BuildBasicFileSystem();
            playerFS.ChangeDir("bin");
            playerFS.AddFile("sshnuke", "01110011011100110110100001101110011101010110101101100101001000000110000101101100011011000110111101110111011100110010000001111001011011110111010100100000011101000110111100100000011001110110000101101001011011100010000001110101011011100110000101110101011101000110100001101111011100100110100101111010011001010110010000100000011001010110111001110100011100100111100100100000011010010110111001110100011011110010000001110010011001010110110101101111011101000110010100100000011100110111100101110011011101000110010101101101011100110010000001100010011110010010000001100011011000010111010101110011011010010110111001100111001000000110000100100000011000100111010101100110011001100110010101110010001000000110111101110110011001010111001001100110011011000110111101110111001000000110100101101110001000000110000100100000011000110110100001110101011011100110101100100000011011110110011000100000011000110110111101100100011001010010000001100100011001010111001101101001011001110110111001100101011001000010000001110100011011110010000001100111011101010110000101110010011001000010000001100001011001110110000101101001011011100111001101110100001000000110001101110010011110010111000001110100011011110110011101110010011000010111000001101000011010010110001100100000011000010111010001110100011000010110001101101011011100110010000001101111011011100010000001010011010100110100100000100000011101100110010101110010011100110110100101101111011011100010000001101111011011100110010100101110");
            playerFS.AddFile("nmap", "010011100110110101100001011100000010000001101001011100110010000001100001001000000110011001110010011001010110010100100000011000010110111001100100001000000110111101110000011001010110111000101101011100110110111101110101011100100110001101100101001000000111001101100101011000110111010101110010011010010111010001111001001000000111001101100011011000010110111001101110011001010111001000101100001000000110111101110010011010010110011101101001011011100110000101101100011011000111100100100000011101110111001001101001011101000111010001100101011011100010000001100010011110010010000001000111011011110111001001100100011011110110111000100000010011000111100101101111011011100010110000100000011101010111001101100101011001000010000001110100011011110010000001100100011010010111001101100011011011110111011001100101011100100010000001101000011011110111001101110100011100110010000001100001011011100110010000100000011100110110010101110010011101100110100101100011011001010111001100100000011011110110111000100000011000010010000001100011011011110110110101110000011101010111010001100101011100100010000001101110011001010111010001110111011011110111001001101011001011000010000001110100011010000111010101110011001000000110001001110101011010010110110001100100011010010110111001100111001000000110000100100000001000100110110101100001011100000010001000100000011011110110011000100000011101000110100001100101001000000110111001100101011101000111011101101111011100100110101100101110");
            playerFS.ChangeDir("/");

            playerComp = new Computer(Computer.Type.Workstation, "127.0.0.1", "localhost", "pasword", playerFS, new int[] { 10, 21, 22, 23, 25, 53, 67, 69, 80, 110, 123, 143, 443 });
            
            playerComp.SetSpeed(1.0f);

            Console.WriteLine("Setting up computers...");
            Computers.Computers.DoComputers();
            Computers.Computers.computerList.Add(playerComp);

            playerComp.GetRoot();
            playerComp.Connect(true);

            Player.GetInstance().PlayersComputer = playerComp;

            loadingScene.LoadItem = "Loading modules...";
            
            int thirdWidth = graphics.PreferredBackBufferWidth / 3;
            int thirdHeight = graphics.PreferredBackBufferHeight / 3;
            int tqWidth = Convert.ToInt32(graphics.PreferredBackBufferWidth * 0.75);

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
                thirdWidth - 4, graphics.PreferredBackBufferHeight - 4),
                font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Terminal",
                Font = fontS,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading networkmap...");
            networkMap = new NetworkMap(GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, graphics.PreferredBackBufferHeight - thirdHeight + 2, 
                graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, thirdHeight - 4), 
                computer, fontS, NetworkNodeSpinners)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Network Map",
                Font = fontS,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading statusbar...");
            statusBar = new StatusBar(GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, 2, 
                graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, (int)fontL.MeasureString("A").Y - 4), 
                fontXS)
            {
                BackgroundColor = Color.MidnightBlue,
                BorderColor = Color.MidnightBlue,
                HeaderColor = Color.MidnightBlue,
                Title = "Status Bar",
                Font = fontL,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading remoteview...");
            remoteView = new RemoteView(GraphicsDevice, 
                new Rectangle(terminal.Container.Width + 5, statusBar.Container.Height + 5, 
                tqWidth - terminal.Container.Width - 7, graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10), 
                fontL, font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Remote System",
                Font = fontS,
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading notes...");
            notes = new NotesModule(GraphicsDevice, 
                new Rectangle(remoteView.Container.X + remoteView.Container.Width + 3, statusBar.Container.Height + 5,
                graphics.PreferredBackBufferWidth - remoteView.Container.X - remoteView.Container.Width - 5, graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10), 
                font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Friendly neighborhood notepad",
                Font = fontS,
                IsActive = true,
                IsVisible = true,
            };

            loadingScene.LoadItem = "Initializing...";

            os.Init(terminal, remoteView, networkMap, statusBar, notes);

            Console.WriteLine("INIT: Name:" + playerComp.Name);
            Console.WriteLine("INIT: Connect: " + playerComp.IsPlayerConnected);
            Console.WriteLine("CHK: Connect: " + (Player.GetInstance().PlayersComputer != null).ToString());
            terminal.Init();
            Console.WriteLine("Game started");

            gameStarted = true;

            MediaPlayer.Stop();
            stateMachine.Transition(Keys.Attn);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //MediaPlayer.Stop();
                //if(gameState > 0)
                //    terminal.ForceQuit();
                //Exit();
            }

            KeyboardInput.Update();

            stateMachine.UpdateState(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);

            //spriteBatch.Draw(bg, bgR, Color.White);

            stateMachine.DrawState(spriteBatch);

            spriteBatch.End();
        }
    }
}
