﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.UI.Elements;

namespace TerminalGame.Screens
{
    public partial class Screen : DrawableGameComponent
    {
        protected ContentManager Content;
        protected List<UIElement> _elements;
        protected SpriteBatch _spriteBatch;
        protected Rectangle _rectangle;
        protected float _opacity;
        protected bool _isInitialized;
        public new TerminalGame Game;

        public Screen(Game game, bool fadeIn = false) : base(game)
        {
            Game = game as TerminalGame;
            _rectangle = new Rectangle(0, 0, Globals.Settings.GameWidth, Globals.Settings.GameHeight);
            _elements = new List<UIElement>();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Content = Game.Content;
            _isInitialized = false;
            _opacity = fadeIn ? 0.0f : 1.0f;
        }

        public virtual void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize();
            _isInitialized = true;
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
        /// </summary>
        public virtual void SwitchOn()
        {
            Console.WriteLine("screen turn on");
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
