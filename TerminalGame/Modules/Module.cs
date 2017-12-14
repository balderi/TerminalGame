using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.Modules
{
    public abstract class Module
    {
        private SpriteFont Font;
        public Vector2 Position { get; private set; }
        public Color BackgroundColor { get; private set; }
        
        public Module(Vector2 Position, Color BackgroundColor, SpriteFont Font)
        {
            this.Font = Font;
            this.Position = Position;
            this.BackgroundColor = BackgroundColor;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
