using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TerminalGame.UI.Elements.Buttons;
using TerminalGame.Utils;

namespace TerminalGame.States.Screens
{
    class MainMenuScreen : Screen
    {
        private readonly List<Button> BUTTONS;
        private Song bgm;

        public MainMenuScreen(Game game) : base(game)
        {
            Button newGame = new Button(game, "New Game", new Point(100, 200), new Point(400, 50));
            Button loadGame = new Button(game, "Load Game", new Point(100, 275), new Point(400, 50));
            Button settings = new Button(game, "Settings", new Point(100, 350), new Point(400, 50));
            Button quit = new Button(game, "Quit", new Point(100, 425), new Point(400, 50));

            newGame.ButtonPressed += NewGame_Clicked;
            loadGame.ButtonPressed += LoadGame_Clicked;
            settings.ButtonPressed += Settings_Clicked;
            quit.ButtonPressed += Quit_Clicked;

            BUTTONS = new List<Button>
            {
                newGame,
                loadGame,
                settings,
                quit
            };
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            bgm = Content.Load<Song>("Audio/Music/mainmenu");
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            MusicManager.GetInstance().Start(bgm);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            base.Draw(gameTime);
            foreach (var b in BUTTONS)
                b.Draw(gameTime);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var b in BUTTONS)
                b.Update(gameTime);
        }

        private void NewGame_Clicked(ButtonPressedEventArgs e)
        {
            Console.WriteLine("New Game clicked");
        }

        private void LoadGame_Clicked(ButtonPressedEventArgs e)
        {
            Console.WriteLine("Load Game clicked");
        }

        private void Settings_Clicked(ButtonPressedEventArgs e)
        {
            Console.WriteLine("Settings clicked");
        }

        private void Quit_Clicked(ButtonPressedEventArgs e)
        {
            Console.WriteLine("Quit clicked");
        }
    }
}
