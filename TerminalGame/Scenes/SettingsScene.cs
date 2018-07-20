using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class SettingsScene : IScene
    {
        
        private SpriteFont _font;
        private GameWindow _gameWindow;
        private GraphicsDevice _graphics;

        public SettingsScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics)
        {
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "SettingsScene", new Vector2(10, 10), Color.White);

        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
