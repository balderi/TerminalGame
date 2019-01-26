using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using TerminalGame.Utils;
using TerminalGame.UI.Elements.Modules;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using TerminalGame.States;
using TerminalGame.States.Screens;
using System;
using System.Reflection;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TerminalGame : Game
    {
        private readonly GraphicsDeviceManager GRAPHICS;
        private SpriteBatch _spriteBatch;

        private MusicManager _musicManager;

        private StateMachine stateMachine;

        public readonly string GAME_TITLE;
        public Version version;

        public string Title { get; private set; }
        public string Version { get; private set; }

        public TerminalGame()
        {
            GRAPHICS = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            version = Assembly.GetEntryAssembly().GetName().Version;
            Title = Assembly.GetEntryAssembly().GetName().Name;
            Version = String.Format("v{0}.{1}a", version.Major, version.Minor);
            GAME_TITLE = String.Format("{0} v{1}.{2}a", Title, version.Major, version.Minor);
            IsFixedTimeStep = true;
            GRAPHICS.SynchronizeWithVerticalRetrace = true;
            GRAPHICS.GraphicsProfile = GraphicsProfile.HiDef;
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

            GRAPHICS.HardwareModeSwitch = false;
            GRAPHICS.PreferredBackBufferHeight = 768;// (int)(GraphicsDevice.DisplayMode.Height * 0.8);
            GRAPHICS.PreferredBackBufferWidth = 1366;// (int)(GraphicsDevice.DisplayMode.Width * 0.8);
            GRAPHICS.IsFullScreen = false;
            GRAPHICS.ApplyChanges();

            Globals.GameHeight = GRAPHICS.PreferredBackBufferHeight;
            Globals.GameWidth = GRAPHICS.PreferredBackBufferWidth;
            FontManager.InitializeFonts(Content);

            Window.Title = GAME_TITLE;

            //TODO: Load this from file
            Globals.SetGlobalFontSize();
            Globals.GenerateDummyTexture(GraphicsDevice);
            IsMouseVisible = true;

            stateMachine = StateMachine.GetInstance();
            stateMachine.Initialize(SplashState.GetInstance(), GRAPHICS, new SplashScreen(this), this);
            
            UI.Themes.Theme test = new UI.Themes.Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);

            UI.Themes.ThemeManager.GetInstance().AddTheme(test);
            UI.Themes.ThemeManager.GetInstance().ChangeTheme("test");
            
            System.Console.WriteLine("init done");
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
            System.Console.WriteLine("load done");
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
            if (Keyboard.GetState().IsKeyDown(Keys.M) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                _musicManager.FadeOut();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                _musicManager.FadeIn();
            }

            UI.Themes.ThemeManager.GetInstance().Update(gameTime);

            _musicManager.Update(gameTime);

            base.Update(gameTime);

            stateMachine.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);
            stateMachine.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
