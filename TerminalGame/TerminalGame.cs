using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using TerminalGame.Utils;
using TerminalGame.UI.Elements.Modules;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace TerminalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TerminalGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Module> _modules;

        private MusicManager _musicManager;

        private Texture2D bg; //temp

        public TerminalGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            base.Initialize();
            FontManager.InitializeFonts(Content);

            //TODO: Load this from file
            Globals.SetGlobalFontSize();

            IsMouseVisible = true;

            // for testing

            _graphics.HardwareModeSwitch = false;
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * 0.8);
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * 0.8);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            UI.Themes.Theme test = new UI.Themes.Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);

            UI.Themes.ThemeManager.GetInstance().AddTheme(test);
            UI.Themes.ThemeManager.GetInstance().ChangeTheme("test");

            Module terminal = new Module(this, new Point(2, 2), new Point(_graphics.PreferredBackBufferWidth / 3 - 4, _graphics.PreferredBackBufferHeight - 4), "Terminal v0.1");
            Module networkMap = new Module(this, new Point(_graphics.PreferredBackBufferWidth / 3 + 1, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight / 3 + 2), new Point(_graphics.PreferredBackBufferWidth - terminal.Rectangle.Width - 7, _graphics.PreferredBackBufferHeight / 3 - 4), "NetworkMap v0.1");
            Module remoteView = new Module(this, new Point(terminal.Rectangle.Width + 5, 2), new Point((int)(_graphics.PreferredBackBufferWidth * 0.75) - terminal.Rectangle.Width - 7, _graphics.PreferredBackBufferHeight - networkMap.Rectangle.Height - 7), "RemoteView v0.1");
            Module notePad = new Module(this, new Point(remoteView.Rectangle.X + remoteView.Rectangle.Width + 3, 2), new Point(_graphics.PreferredBackBufferWidth - remoteView.Rectangle.X - remoteView.Rectangle.Width - 5, _graphics.PreferredBackBufferHeight - networkMap.Rectangle.Height - 7), "Friendly Neighbourhood Notepad");

            _modules = new List<Module>()
            {
                terminal,
                networkMap,
                remoteView,
                notePad,
            };
            foreach (Module m in _modules)
            {
                m.Initialize();
            }

            // end for testing

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
            _musicManager.Start(Content.Load<Song>("Audio/Music/ambientbgm1_2"));
            bg = Content.Load<Texture2D>("Graphics/Textures/Backgrounds/bg");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                _musicManager.FadeOut();
                foreach (Module m in _modules)
                {
                    m.FadeOut();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                _musicManager.FadeIn();
                foreach (Module m in _modules)
                {
                    m.FadeIn();
                }
            }

            foreach (Module m in _modules)
            {
                m.Update(gameTime);
            }

            UI.Themes.ThemeManager.GetInstance().Update(gameTime);

            _musicManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);
            _spriteBatch.Draw(bg, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.End();

            foreach (Module m in _modules)
            {
                m.Draw(gameTime);
            }
        }
    }
}
