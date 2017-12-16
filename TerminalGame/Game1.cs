using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using System;
using System.Threading;

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

        private TextBox terminalInput;
        private SpriteFont font, fontXL, testfont, menuFont;
        private Rectangle connAdd;
        private Rectangle inputViewport;
        private Rectangle outputViewport;
        private Song bgm_game, bgm_menu;
        private int linesToDraw;
        private string GameTitle, testString, terminalOutput, connectedAddress, outPrepend;
        private List<string> history;
        private bool isExiting;

        enum GameState { Menu, Game }

        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameTitle = "TerminalGame v0.1a";
            this.IsFixedTimeStep = true;
            history = new List<string>();
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
            // TODO: Add your initialization logic here
            KeyboardInput.Initialize(this, 500f, 20);
            Window.Title = GameTitle;
            outPrepend = connectedAddress = "root@localhost > ";
            outPrepend = "> ";
            terminalOutput = "";

            //Set game to fullscreen
            graphics.HardwareModeSwitch = false;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            
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

            font = Content.Load<SpriteFont>("Fonts/terminalFont");
            menuFont = Content.Load<SpriteFont>("Fonts/terminalFontL");
            fontXL = Content.Load<SpriteFont>("Fonts/terminalFontXL");
            testfont = Content.Load<SpriteFont>("Fonts/terminalFontXS");

            connAdd = new Rectangle(5, graphics.PreferredBackBufferHeight - 30, (int)(font.MeasureString(connectedAddress).X), (int)(font.MeasureString("MEASURE ME").Y * 1.1));
            inputViewport = new Rectangle(connAdd.Width, graphics.PreferredBackBufferHeight - 30, 400, (int)(font.MeasureString("MEASURE ME").Y * 1.1));
            terminalInput = new TextBox(inputViewport, 44, "", GraphicsDevice, font, Color.LightGray, Color.DarkGreen, 30);

            outputViewport = new Rectangle(3, 5, 600, graphics.PreferredBackBufferHeight - (inputViewport.Height + 3));
            
            testString = TestClass.PrintStuff();

            linesToDraw = (int)(outputViewport.Height / font.MeasureString("MEASURE THIS").Y);

            Console.WriteLine("INIT: 0x000000" + linesToDraw);

            for(int i = 0; i < linesToDraw + 1; i++)
            {
                history.Add("\n");
            }

            bgm_game = Content.Load<Song>("Audio/Music/bgm");
            bgm_menu = Content.Load<Song>("Audio/Music/mainmenu");
            MediaPlayer.Play(bgm_menu);

            mainMenu = new Menu(GameTitle, spriteBatch, menuFont, GraphicsDevice);

            terminalInput.EnterDown += OnEnterDown;
            mainMenu.ButtonClicked += MainMenu_ButtonClicked;
            
            float margin = 3;
            terminalInput.Area = new Rectangle((int)(inputViewport.X + margin), inputViewport.Y, (int)(inputViewport.Width - margin), inputViewport.Height);
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            
            terminalInput.Active = true;
            
            Console.WriteLine("Done loading");
        }

        public new void Exit()
        {
            Console.WriteLine("Exiting...");
            base.Exit();
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
                        MediaPlayer.Stop();
                        gameState = GameState.Game;
                        MediaPlayer.Play(bgm_game);
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
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// When game window is focused
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">EventArgs</param>
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            history.Add("Window regained focus\n");
            UpdateOutput();
        }

        /// <summary>
        /// When game window loses focus (eg. alt+tab)
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">EventArgs</param>
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            history.Add("Window lost focus\n");
            UpdateOutput();
        }

        /// <summary>
        /// Called when user hits the Enter key
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs</param>
        void OnEnterDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            string o = CommandParser.ParseCommand(terminalInput.Text.String);
            history.Add(connectedAddress + terminalInput.Text.String + "\n");
            if (o != "")
                history.Add(o);
            Console.WriteLine("CMD: " + terminalInput.Text.String);

            UpdateOutput();

            terminalInput.Clear();
        }

        public void UpdateOutput()
        {
            int counter = 0;
            while (history.Count > linesToDraw)
            {
                history.RemoveAt(0);
                counter++;
            }
            if (history.Count > 0)
            {
                string holder = "";
                foreach (string s in history)
                {

                    holder += s;
                }
                terminalOutput = holder;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (isExiting)
            {
                Exit();
                Thread.Sleep(2000);
            }
            base.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                history.Add("Kernel panic - not syncing: Fatal exception in interrupt\n");
                UpdateOutput();
                isExiting = true;
            }
            
            KeyboardInput.Update();
            switch((int)gameState)
            {
                case 0:
                    {
                        IsMouseVisible = true;
                        mainMenu.Update();
                        break;
                    }
                case 1:
                    {
                        IsMouseVisible = false;
                        float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
                        terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

                        terminalInput.Update();

                        testString = terminalInput.Text.String;
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
            switch ((int)gameState)
            {
                case 0:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        spriteBatch.Begin();
                        mainMenu.Draw(spriteBatch, Window, fontXL);
                        spriteBatch.End();
                        break;
                    }
                case 1:
                        {
                        GraphicsDevice.Clear(Color.Black);

                        spriteBatch.Begin();

                        Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                        Vector2 textMiddlePoint = fontXL.MeasureString(testString) / 2;
                        Vector2 position2 = new Vector2(Window.ClientBounds.Width / 2 + TestClass.ShakeStuff(3), Window.ClientBounds.Height / 2 + TestClass.ShakeStuff(3));
                        Vector2 caretPos = new Vector2(8, graphics.PreferredBackBufferHeight - 20);
                        spriteBatch.DrawString(fontXL, testString, position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                        spriteBatch.DrawString(fontXL, testString, position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

                        spriteBatch.DrawString(font, connectedAddress, new Vector2(connAdd.X, connAdd.Y), Color.LightGray);

                        spriteBatch.DrawString(font, terminalOutput, new Vector2(outputViewport.X + 3 + TestClass.ShakeStuff(1), outputViewport.Y + TestClass.ShakeStuff(1)), Color.Green);
                        spriteBatch.DrawString(font, terminalOutput, new Vector2(outputViewport.X + 3, outputViewport.Y), Color.LightGray);

                        terminalInput.Draw(spriteBatch);
                        spriteBatch.End();

                        base.Draw(gameTime);
                        break;
                    }
                default:
                        break;
        }
        }
    }
}
