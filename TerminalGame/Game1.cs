using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TerminalGame.UI.Modules;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.Computers;
using System.Reflection;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Menu mainMenu;

        LoadingScreen load;

        OS.OS os;

        private SpriteFont font, fontL, fontXL, menuFont, fontS, fontXS;
        private Song bgm_game, bgm_menu;
        private readonly string GameTitle;

        enum GameState { Menu, Game, Loading }

        GameState gameState;

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

            //graphics.PreferredBackBufferHeight = 960;
            //graphics.PreferredBackBufferWidth = 1440;

            //Set game to fullscreen
            graphics.HardwareModeSwitch = false;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            
            MediaPlayer.Volume = 0.2f;
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

            mainMenu = new Menu(GameTitle, spriteBatch, menuFont, GraphicsDevice);

            mainMenu.ButtonClicked += MainMenu_ButtonClicked;

            load = new LoadingScreen(fontL, fontS);

            Console.WriteLine("Loading textures");
            bg = Content.Load<Texture2D>("Textures/bg");
            computer = Content.Load<Texture2D>("Textures/nmapComputer");

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

            Console.WriteLine("Done loading");
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
                        gameState = GameState.Loading;
                        StartNewGame();
                        break;
                    }
                case "Load Game":
                    {
                        break;
                    }
                case "Settings":
                    {
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
            
            os = OS.OS.GetInstance();

            playerComp = new Computer(Computer.Type.Workstation, "127.0.0.1", "localhost", "pasword");
            playerComp.FileSystem.ChangeDir("bin");
            playerComp.FileSystem.AddFile("sshnuke", "01110011011100110110100001101110011101010110101101100101001000000110000101101100011011000110111101110111011100110010000001111001011011110111010100100000011101000110111100100000011001110110000101101001011011100010000001110101011011100110000101110101011101000110100001101111011100100110100101111010011001010110010000100000011001010110111001110100011100100111100100100000011010010110111001110100011011110010000001110010011001010110110101101111011101000110010100100000011100110111100101110011011101000110010101101101011100110010000001100010011110010010000001100011011000010111010101110011011010010110111001100111001000000110000100100000011000100111010101100110011001100110010101110010001000000110111101110110011001010111001001100110011011000110111101110111001000000110100101101110001000000110000100100000011000110110100001110101011011100110101100100000011011110110011000100000011000110110111101100100011001010010000001100100011001010111001101101001011001110110111001100101011001000010000001110100011011110010000001100111011101010110000101110010011001000010000001100001011001110110000101101001011011100111001101110100001000000110001101110010011110010111000001110100011011110110011101110010011000010111000001101000011010010110001100100000011000010111010001110100011000010110001101101011011100110010000001101111011011100010000001010011010100110100100000100000011101100110010101110010011100110110100101101111011011100010000001101111011011100110010100101110");
            playerComp.FileSystem.ChangeDir("/");

            Console.WriteLine("Setting up computers...");
            Computers.Computers.DoComputers();
            Computers.Computers.computerList.Add(playerComp);

            playerComp.GetRoot();
            playerComp.Connect(true);

            Player.GetInstance().PlayersComputer = playerComp;

            bgR = new Rectangle(new Point(0, 0), new Point(bg.Width, bg.Height));
            
            Console.WriteLine("Loading terminal...");
            terminal = new Terminal(GraphicsDevice, new Rectangle(2, 1, 700, graphics.PreferredBackBufferHeight - 2), font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Terminal",
                Font = fontS,
            };

            Console.WriteLine("Loading networkmap...");
            networkMap = new NetworkMap(GraphicsDevice, new Rectangle(705, graphics.PreferredBackBufferHeight - 299, graphics.PreferredBackBufferWidth - 706, 298), computer, fontS, NetworkNodeSpinners)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Network Map",
                Font = fontS,
            };

            Console.WriteLine("Loading statusbar...");
            statusBar = new StatusBar(GraphicsDevice, new Rectangle(705, 1, graphics.PreferredBackBufferWidth - 706, (int)fontL.MeasureString("A").Y - 1), fontXS)
            {
                BackgroundColor = Color.MidnightBlue,
                BorderColor = Color.MidnightBlue,
                HeaderColor = Color.MidnightBlue,
                Title = "Status Bar",
                Font = fontL,
            };

            Console.WriteLine("Loading remoteview...");
            remoteView = new RemoteView(GraphicsDevice, new Rectangle(705, (int)fontL.MeasureString("A").Y + 4, graphics.PreferredBackBufferWidth - 706 - 304, graphics.PreferredBackBufferHeight - 304 - (int)fontL.MeasureString("A").Y - 2), fontL, font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Remote System",
                Font = fontS,
            };

            Console.WriteLine("Loading notes...");
            notes = new NotesModule(GraphicsDevice, new Rectangle(704 + graphics.PreferredBackBufferWidth - 706 - 300, (int)fontL.MeasureString("A").Y + 4, 301, graphics.PreferredBackBufferHeight - 304 - (int)fontL.MeasureString("A").Y - 2), font)
            {
                BackgroundColor = Color.Black * 0.75f,
                BorderColor = Color.RoyalBlue,
                HeaderColor = Color.RoyalBlue,
                Title = "Friendly neighborhood notepad",
                Font = fontS,
            };

            os.Init(terminal, remoteView, networkMap, statusBar, notes);

            Console.WriteLine("INIT: Name:" + playerComp.Name);
            Console.WriteLine("INIT: Connect: " + playerComp.IsPlayerConnected);
            Console.WriteLine("CHK: Connect: " + (Player.GetInstance().PlayersComputer != null).ToString());
            terminal.Init();
            Console.WriteLine("Game started");

            gameState = GameState.Game;

            MediaPlayer.Stop();
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
                MediaPlayer.Stop();
                if(gameState > 0)
                    terminal.ForceQuit();
                Exit();
            }
            
            KeyboardInput.Update();
            switch((int)gameState)
            {
                case 0:
                    {
                        mainMenu.Update();
                        break;
                    }
                case 1:
                    {
                        os.Update(gameTime);
                        break;
                    }
                case 2:
                    {
                        load.Update(gameTime);
                        break;
                    }
                default:
                    break;
            }
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
            switch ((int)gameState)
            {
                case 0:
                    {
                        mainMenu.Draw(spriteBatch, Window, fontXL);
                        break;
                    }
                case 1:
                    {
                        spriteBatch.Draw(bg, bgR, Color.White);
                        os.Draw(spriteBatch);
                        break;
                    }
                case 2:
                    {
                        load.Draw(spriteBatch, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2));
                        break;
                    }
                default:
                        break;
            }
            spriteBatch.End();
        }
    }
}
