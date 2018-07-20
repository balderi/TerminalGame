using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class GameScene : IScene
    {
        public Texture2D Background { get; private set; }
        private Rectangle _bgRect;
        public GameScene(Texture2D background, Rectangle bgRect)
        {
            _bgRect = bgRect;
            Background = background;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, _bgRect, Color.White);
            OS.OS.GetInstance().Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            OS.OS.GetInstance().Update(gameTime);
        }
    }
}
