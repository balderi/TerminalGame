using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    class GameScene : IScene
    {
        public Texture2D Background { get; private set; }
        private Rectangle _bgRect;
        private Color _bgColor;
        readonly StateMachine _stateMachine;

        public GameScene(Texture2D background, Rectangle bgRect, StateMachine stateMachine)
        {
            _bgRect = bgRect;
            Background = background;
            _bgColor = Color.White;
            _stateMachine = stateMachine;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, _bgRect, _bgColor);
            OS.OS.GetInstance().Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            //_bgColor = Color.White * ((float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1) * 0.5f + 0.75f);
            OS.OS.GetInstance().Update(gameTime);
        }
    }
}
