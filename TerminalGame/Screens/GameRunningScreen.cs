using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.Time;
using TerminalGame.UI.Elements;
using TerminalGame.UI.Elements.Modules;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class GameRunningScreen : Screen
    {
        private Texture2D _background;
        private KeyboardState _prevState, _newState;

        public GameRunningScreen(Game game) : base(game)
        {

        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            if (_isInitialized)
                return;

            Console.WriteLine("gameRunning is not initialized!");

            base.Initialize();

            World.World.GetInstance().Player.ConnectedComp = World.World.GetInstance().Player.PlayerComp;


            Terminal terminal = new Terminal(Game, new Point(2, 2),
                new Point(Globals.Settings.GameWidth / 3 - 4, Globals.Settings.GameHeight - 4), "Terminal v0.1");

            NetworkMap networkMap = new NetworkMap(Game, new Point(Globals.Settings.GameWidth / 3 + 1, Globals.Settings.GameHeight - Globals.Settings.GameHeight / 3 + 2),
                new Point(Globals.Settings.GameWidth - terminal.Rectangle.Width - 7, Globals.Settings.GameHeight / 3 - 4), "NetworkMap v0.1", 24);

            StatusBar statusBar = new StatusBar(Game, new Point(Globals.Settings.GameWidth / 3, 1),
                new Point(Globals.Settings.GameWidth - terminal.Rectangle.Width - 5, 52), "status", false, false);
            
            RemoteView remoteView = new RemoteView(Game, new Point(terminal.Rectangle.Width + 5, statusBar.Rectangle.Height + 3),
                new Point((int)(Globals.Settings.GameWidth * 0.75) - terminal.Rectangle.Width - 7, Globals.Settings.GameHeight - statusBar.Rectangle.Height - networkMap.Rectangle.Height - 8), "RemoteView v0.1");

            Module notePad = new Module(Game, new Point(remoteView.Rectangle.X + remoteView.Rectangle.Width + 3, statusBar.Rectangle.Height + 3),
                new Point(Globals.Settings.GameWidth - remoteView.Rectangle.X - remoteView.Rectangle.Width - 5, Globals.Settings.GameHeight - statusBar.Rectangle.Height - networkMap.Rectangle.Height - 8), "Friendly Neighbourhood Notepad");

            _elements.Add(terminal);
            _elements.Add(networkMap);
            _elements.Add(statusBar);
            _elements.Add(remoteView);
            _elements.Add(notePad);

            foreach (var m in _elements)
                m.Initialize();

            Game.CurrentGameSpeed = GameSpeed.Single;
            Game.Terminal = terminal;
        }

        protected override void LoadContent()
        {
            _background = Content.Load<Texture2D>("Graphics/Textures/Backgrounds/bg");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            GameClock.Tick(Game.CurrentGameSpeed);
            World.World.GetInstance().Tick();

            _newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                foreach (UIElement elem in _elements)
                {
                    elem.FadeOut();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                foreach (UIElement elem in _elements)
                {
                    elem.FadeIn();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                foreach (UIElement elem in _elements)
                {
                    if (elem.MouseIsHovering)
                        elem.Dim();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                foreach (UIElement elem in _elements)
                {
                    if (elem.MouseIsHovering)
                        elem.UnDim();
                }
            }

            if(_prevState.IsKeyDown(Keys.Escape) && _newState.IsKeyUp(Keys.Escape))
            {
                ScreenManager.GetInstance().ChangeScreen("mainMenu");
            }

            _prevState = _newState;
            base.Update(gameTime); // Handles updating all elements in _elements
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            _spriteBatch.Draw(_background, _rectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime); // Handles drawing all elements in _elements
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }

        protected override void UnloadContent()
        {
            foreach (var e in _elements)
                e.Dispose();
            base.UnloadContent();
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            MusicManager.GetInstance().Start("gameBgm");
        }
    }
}
