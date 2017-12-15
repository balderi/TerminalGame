﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch outputSprites;

        private TextBox terminalInput;
        //private TextBox terminalOutput;
        private SpriteFont font, fontXL, testfont;
        private Rectangle inputViewport;
        private Rectangle outputViewport;

        private Song bgm;

        TestParser parser;

        int linesToDraw;
        
        string GameTitle, testString, terminalOutput;

        string outPrepend;

        List<string> history;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameTitle = "Terminal Game";
            this.IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            System.Console.WriteLine("Initializing...");
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            KeyboardInput.Initialize(this, 500f, 20);
            Window.Title = GameTitle;
            parser = new TestParser(this);
            outPrepend = "root@localhost > ";
            outPrepend = "> ";
            terminalOutput = "";

            //Set game to fullscreen
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

            base.Initialize();
            System.Console.WriteLine("Done initializing");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            System.Console.WriteLine("Loading content...");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            outputSprites = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/terminalFont");
            fontXL = Content.Load<SpriteFont>("Fonts/terminalFontXL");
            testfont = Content.Load<SpriteFont>("Fonts/terminalFontXS");

            inputViewport = new Rectangle(19, graphics.PreferredBackBufferHeight - 20, 400, (int)(font.MeasureString("MEASURE ME").Y * 1.1));
            terminalInput = new TextBox(inputViewport, 44, "", GraphicsDevice, font, Color.LightGray, Color.DarkGreen, 30);

            outputViewport = new Rectangle(5, 5, 600, graphics.PreferredBackBufferHeight - inputViewport.Height);
            
            testString = TestClass.PrintStuff();

            linesToDraw = (int)(outputViewport.Height / font.MeasureString("MEASURE THIS").Y);

            System.Console.WriteLine("INIT: 0x000000" + linesToDraw);

            history = new List<string>();
            for(int i = 0; i < linesToDraw + 1; i++)
            {
                history.Add("\n");
            }

            bgm = Content.Load<Song>("Audio/Music/bgm");
            MediaPlayer.Play(bgm);

            terminalInput.EnterDown += OnEnterDown;

            //terminalInput.Text.String = "root@localhost";


            float margin = 3;
            terminalInput.Area = new Rectangle((int)(inputViewport.X + margin), inputViewport.Y, (int)(inputViewport.Width - margin), inputViewport.Height);
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            
            terminalInput.Active = true;
            
            // TODO: use this.Content to load your game content here
            System.Console.WriteLine("Done loading");
        }
        
        void OnEnterDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            string o = CommandParser.ParseCommand(terminalInput.Text.String);
            history.Add(outPrepend + terminalInput.Text.String + "\n");
            if(o != "")
                history.Add(o);
            System.Console.WriteLine("CMD: " + terminalInput.Text.String);
            //if (history.Count > linesToDraw)
            //{
            //    history.RemoveAt(0);
            //}
            int counter = 0;
            while(history.Count > linesToDraw)
            {
                history.RemoveAt(0);
                counter++;
            }
            System.Console.WriteLine("Purged " + counter + " lines from history");
            if (history.Count > 0)
            {
                string holder = "";
                foreach (string s in history)
                {
                    holder += s;
                }
                terminalOutput = holder;
            }
            terminalInput.Clear();
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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            KeyboardInput.Update();
            
            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
            terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            terminalInput.Update();

            testString = terminalInput.Text.String;
                        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            
            Vector2 position = new Vector2(Window.ClientBounds.Width / 2 + TestClass.ShakeStuff(1), Window.ClientBounds.Height / 2 + TestClass.ShakeStuff(1));
            Vector2 textMiddlePoint = fontXL.MeasureString(testString) / 2;
            Vector2 position2 = new Vector2(Window.ClientBounds.Width / 2 + TestClass.ShakeStuff(3), Window.ClientBounds.Height / 2 + TestClass.ShakeStuff(3));
            Vector2 caretPos = new Vector2(8, graphics.PreferredBackBufferHeight - 20);
            spriteBatch.DrawString(fontXL, testString, position2, Color.ForestGreen, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(fontXL, testString, position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            
            spriteBatch.DrawString(font, "> ", caretPos, Color.LightGray, 0, new Vector2(0f), 1.0f, SpriteEffects.None, 0.5f);

            terminalInput.Draw(spriteBatch);
            spriteBatch.End();

            outputSprites.Begin();
            outputSprites.DrawString(font, terminalOutput, new Vector2(outputViewport.X + 3, outputViewport.Y), Color.LightGray);
            outputSprites.End();

            base.Draw(gameTime);
        }
    }
}
