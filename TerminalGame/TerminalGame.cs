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

        private List<Module> Modules;
        
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
            FontManager.InitializeFonts(Content);

            //TODO: Load this from file
            Globals.SetGlobalFontSize();

            IsMouseVisible = true;
            Modules = new List<Module>()
            {
                new Module(this, new Point(2,2), new Point(200, 400), "Terminal v0.1"),
                new Module(this, new Point(205,2), new Point(500, 400), "SomeWindow v0.1"),
            };
            foreach (Module m in Modules)
            {
                m.Initialize();
            }


            _graphics.HardwareModeSwitch = false;
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * 0.8);
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * 0.8);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
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
            MusicManager._currentMusic = Content.Load<Song>("Audio/Music/ambientbgm1_2");
            MediaPlayer.Play(MusicManager._currentMusic);
            MediaPlayer.Volume = 0;
            MusicManager.IsFadingIn = true;
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

            foreach (Module m in Modules)
            {
                m.Update(gameTime);
            }

            if (MusicManager.IsFadingIn)
                MusicManager.FadeIn(0.01f);
            if (MusicManager.IsFadingOut)
                MusicManager.FadeOut(0.01f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            foreach (Module m in Modules)
            {
                m.Draw(gameTime);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
