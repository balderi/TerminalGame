using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using System;

namespace TerminalGame.UI.Modules
{
    public abstract class Module
    {
        public abstract SpriteFont Font { get; set; }
        public abstract Rectangle Rectangle { get; set; }
        public abstract Color BackgroundColor { get; set; }
        public abstract Color BorderColor { get; set; }
        public abstract Color HeaderColor { get; set; }
        public abstract bool isActive { get; set; }
        public abstract string Title { get; set; }
        
        protected GraphicsDevice graphics;

        protected Module(GraphicsDevice Graphics)
        {
            graphics = Graphics;
        }

        protected abstract Rectangle renderHeader();
        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
