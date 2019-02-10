using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TerminalGame.States;
using TerminalGame.Utils;

namespace TerminalGame.Screens
{
    class GameLoadingScreen : Screen
    {
        private string _loading, _loadItem;
        private SpriteFont _loadingFont, _loadItemFont;
        private Task _loadingTask;
        private Vector2 _loadPos, _itemPos;

        public GameLoadingScreen(Game game) : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);
            _loading = "Loading";
            _loadItem = "Stand by...";
            _loadingFont = FontManager.GetFont("FontXL");
            _loadItemFont = FontManager.GetFont("FontM");

            _loadPos = new Vector2(Game.Window.ClientBounds.Width / 2 - _loadingFont.MeasureString(_loading).X / 2,
                Game.Window.ClientBounds.Height / 2 - _loadingFont.MeasureString(_loading).Y / 2);
            _itemPos = new Vector2(Game.Window.ClientBounds.Width / 2 - _loadItemFont.MeasureString(_loadItem).X / 2,
                Game.Window.ClientBounds.Height / 2 - _loadItemFont.MeasureString(_loadItem).Y / 2 + _loadingFont.MeasureString(_loading).Y / 2);

            Game.Player = Player.GetInstance();

            Game.Player.CreateNewPlayer("testPlayer", "abc123");

            Console.WriteLine("Creating world");
            Action load = new Action(World.World.GetInstance().CreateWorld);

            _loadingTask = new Task(load);
            _loadingTask.Start();
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_loadingTask.IsCompleted)
            {
                Console.WriteLine("Done");
                StateMachine.GetInstance().ChangeState("gameRunning", new GameRunningScreen(Game));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_loadingFont, _loading, _loadPos, Color.White);
            _spriteBatch.DrawString(_loadItemFont, _loadItem, _itemPos, Color.White);
            _spriteBatch.End();
        }
    }
}
