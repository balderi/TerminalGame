using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.States;
using System.Reflection;
using TerminalGame.Utilities;
using TerminalGame.Scenes;
using System.Threading;
using TerminalGame.UI.Shaders;
using TerminalGame.UI.Themes;
using TerminalGame.IO;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        KeyboardState _prevKbState, _newKbState;

        Thread _loadingThread;

        LoadingScreen _load;

        private SpriteFont _font, _fontL, _fontXL, _menuFont, _fontS, _fontXS;
        private readonly string GameTitle;
        private float musicVolume, /*audioVolume,*/ masterVolume;

        MenuScene mainMenuScene;
        SettingsScene settingsMenuScene;
        LoadGameScene loadGameMenuScene;
        LoadingScene loadingScene;
        GameScene gameRunningScene;
        GameOverScene gameOverScene;
        NewGameScene newGameScene;

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

            IsFixedTimeStep = true;
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
            Console.WriteLine("Initializing...");
            KeyboardInput.Initialize(this, 500f, 20);
            Window.Title = GameTitle;
            IsMouseVisible = true;
            
            _random = new Random(DateTime.Now.Millisecond);

            masterVolume = 1.0f;
            musicVolume = 0.2f;
            //audioVolume = 1.0f;

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
            MusicManager.GetInstance().AddSong(Content.Load<Song>("Audio/Music/ambientbgm1_2"));
            MusicManager.GetInstance().AddSong(Content.Load<Song>("Audio/Music/mainmenu"));
            MusicManager.GetInstance().AddSong(Content.Load<Song>("Audio/Music/dynamicsTest0"));
            MusicManager.GetInstance().AddSong(Content.Load<Song>("Audio/Music/dynamicsTest1"));
            MusicManager.GetInstance().AddSong(Content.Load<Song>("Audio/Music/dynamicsTest2"));
            Console.WriteLine("Done");

            Console.Write("Loading audio... ");
            SoundManager.GetInstance().LoadSound("networkNodeHover", 
                Content.Load<SoundEffect>("Audio/Sounds/interface4"));
            SoundManager.GetInstance().LoadSound("networkNodeClick", 
                Content.Load<SoundEffect>("Audio/Sounds/click1"));
            SoundManager.GetInstance().LoadSound("traceWarning", 
                Content.Load<SoundEffect>("Audio/Sounds/trace_warn"));
            Console.WriteLine("Done");

            /* BEGIN GAME MANAGER STUFF */
            
            if (!Validation.IsSaveGamesFolderValid())
            {
                throw new Exception("Save game folder could not be validated.");
            }

            GameManager.GetInstance().SetGraphicsDeviceManager(_graphics);
            GameManager.GetInstance().IsGameRunning = false;

            GameManager.GetInstance().IsGameRunning = false;
            GameManager.GetInstance().IsFullScreen = false;
            GameManager.GetInstance().BloomEnabled = true;

            GameManager.GetInstance().ChangeResolution(1366, 768);
            //GameManager.GetInstance().ChangeResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);

            //Set game to fullscreen
            _graphics.HardwareModeSwitch = false;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Console.WriteLine("Resolution is now " + _graphics.PreferredBackBufferWidth + " x " + _graphics.PreferredBackBufferHeight);
            Drawing.SetBlankTexture(GraphicsDevice);
            
            /* END GAME MANAGER STUFF */

            FontManager.SetFonts(_fontXS, _fontS, _font, _fontL, _fontXL);
            
            _load = new LoadingScreen(FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.Small));

            Console.Write("Loading textures... ");
            bgR = new Rectangle(new Point(0, 0), new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            backgrounds = new Texture2D[]
            {
                Content.Load<Texture2D>("Textures/bg"),
                Content.Load<Texture2D>("Textures/bg2"),
                Content.Load<Texture2D>("Textures/bg3"),
                Content.Load<Texture2D>("Textures/bg4"),
            };

            computer = Content.Load<Texture2D>("Textures/nmapComputer-2");

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
                { "RootSpinner", spinner04 },
                { "MissionSpinner", spinner05 },
                { "06", spinner06 },
                { "07", spinner07 },
                { "HoverSpinner", spinner08 },
            };

            bg = backgrounds[_random.Next(0, backgrounds.Length)];
            Console.WriteLine("Done");

            Console.Write("Loading scenes... ");
            mainMenuScene = new MenuScene(GameTitle, Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice);
            mainMenuScene.ButtonClicked += MainMenu_ButtonClicked;
            loadGameMenuScene = new LoadGameScene(Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice);
            settingsMenuScene = new SettingsScene(Window, FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice);
            loadingScene = new LoadingScene(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), Window, GraphicsDevice);
            gameRunningScene = new GameScene(bg, bgR);
            gameOverScene = new GameOverScene(new Vector2(_graphics.PreferredBackBufferWidth / 2, 
                _graphics.PreferredBackBufferHeight / 2), Window, 
                FontManager.GetFont(FontManager.FontSize.Large), GraphicsDevice);
            newGameScene = new NewGameScene(Window, FontManager.GetFont(FontManager.FontSize.Large),
                FontManager.GetFont(FontManager.FontSize.XLarge), GraphicsDevice);


            SceneManager.SetScenes(mainMenuScene, settingsMenuScene, loadGameMenuScene, 
                loadingScene, gameRunningScene, gameOverScene, newGameScene);
            Console.WriteLine("Done");
            //Load our Bloomfilter!
            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Small;
            _bloomFilter.BloomStreakLength = 5;
            _bloomFilter.BloomThreshold = -0.1f;
            _bloomFilter.BloomStrengthMultiplier = 0.5f;

            WhatTheFuck.GetInstance().SetupWTF(_graphics, Window, NetworkNodeSpinners, computer);

            Console.WriteLine("Done loading");

            GameManager.GetInstance().StateMachine.Transition(GameState.MainMenu);
        }

        /// <summary>
        /// To happen when Exit has been called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            try
            {
                Player.GetInstance().ConnectedComputer.AbortTrace();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if(GameManager.GetInstance().IsGameRunning)
                SaveGame.Save();
            Console.WriteLine("Exiting...");
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// When a button on the main menu is clicked
        /// </summary>
        /// <param name="e">Contains the text of the pressed button</param>
        private void MainMenu_ButtonClicked(UI.ButtonPressedEventArgs e)
        {
            switch(e.Text)
            {
                case "New Game":
                    {
                        GameManager.GetInstance().StateMachine.Transition(GameState.GameLoading);
                        _loadingThread = new Thread(new ThreadStart(WhatTheFuck.GetInstance().StartNewGame));
                        _loadingThread.Start();
                        break;
                    }
                case "Load Game":
                    {
                        GameManager.GetInstance().StateMachine.Transition(GameState.LoadMenu);
                        break;
                    }
                case "Settings":
                    {
                        GameManager.GetInstance().StateMachine.Transition(GameState.SettingsMenu);
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

            ThemeManager.GetInstance().Update(gameTime);
            GameManager.GetInstance().StateMachine.UpdateState(gameTime);
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
                GraphicsDevice.SetRenderTarget(GameManager.GetInstance().GetRenderTarget());
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                GameManager.GetInstance().StateMachine.DrawState(_spriteBatch);
                _spriteBatch.End();
                _bloomFilter.BloomStrengthMultiplier = 1 * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1) * 0.1f + 0.5f);
                Texture2D bloom = _bloomFilter.Draw(GameManager.GetInstance().GetRenderTarget(), _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _spriteBatch.Begin(blendState: BlendState.Additive);
                _spriteBatch.Draw(GameManager.GetInstance().GetRenderTarget(), new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(bloom, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                GameManager.GetInstance().StateMachine.DrawState(_spriteBatch);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
