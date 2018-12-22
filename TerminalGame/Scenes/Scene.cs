﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.States;
using TerminalGame.Utils;

namespace TerminalGame.Scenes
{
    abstract class Scene : IScene
    {
        protected StateMachine _stateMachine;
        public Scene()
        {
            _stateMachine = GameManager.GetInstance().StateMachine;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}
