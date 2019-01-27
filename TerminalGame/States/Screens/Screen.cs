using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.UI.Elements;
using TerminalGame.Utils;

namespace TerminalGame.States.Screens
{
    public partial class Screen : DrawableGameComponent
    {
        protected ContentManager Content;
        protected List<UIElement> _elements;
        protected SpriteBatch _spriteBatch;
        protected Rectangle _rectangle;
        public new TerminalGame Game;

        public Screen(Game game) : base(game)
        {
            Game = game as TerminalGame;
            _rectangle = new Rectangle(0, 0, Globals.GameWidth, Globals.GameHeight);
            _elements = new List<UIElement>();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Content = Game.Content;
        }

        public virtual void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (UIElement elem in _elements)
                elem.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (UIElement elem in _elements)
                elem.Draw(gameTime);
        }


        /// <summary>
        /// Should be called when this screen is switched to.
        /// Will handle starting the correct music, etc.
        /// </summary>
        public virtual void SwitchOn()
        {

        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }
    }
}
