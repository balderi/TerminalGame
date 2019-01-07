﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TerminalGame.UI.Elements;
using TerminalGame.UI.Elements.Modules;
using TerminalGame.Utils;

namespace TerminalGame.States.Screens
{
    class GameRunningScreen : Screen
    {
        private Texture2D _background;
        private Song bgm;


        public GameRunningScreen(Game game) : base(game)
        {

        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            Module terminal = new Module(Game, new Point(2, 2),
                new Point(Globals.GameWidth / 3 - 4, Globals.GameHeight - 4), "Terminal v0.1");

            Module networkMap = new Module(Game, new Point(Globals.GameWidth / 3 + 1, Globals.GameHeight - Globals.GameHeight / 3 + 2),
                new Point(Globals.GameWidth - terminal.Rectangle.Width - 7, Globals.GameHeight / 3 - 4), "NetworkMap v0.1");

            Module statusBar = new Module(Game, new Point(Globals.GameWidth / 3 + 1, 2),
                new Point(Globals.GameWidth - terminal.Rectangle.Width - 7, 50), "status", false);

            Module remoteView = new Module(Game, new Point(terminal.Rectangle.Width + 5, statusBar.Rectangle.Height + 5),
                new Point((int)(Globals.GameWidth * 0.75) - terminal.Rectangle.Width - 7, Globals.GameHeight - statusBar.Rectangle.Height - networkMap.Rectangle.Height - 10), "RemoteView v0.1");

            Module notePad = new Module(Game, new Point(remoteView.Rectangle.X + remoteView.Rectangle.Width + 3, statusBar.Rectangle.Height + 5),
                new Point(Globals.GameWidth - remoteView.Rectangle.X - remoteView.Rectangle.Width - 5, Globals.GameHeight - statusBar.Rectangle.Height - networkMap.Rectangle.Height - 10), "Friendly Neighbourhood Notepad");

            _elements.Add(terminal);
            _elements.Add(networkMap);
            _elements.Add(statusBar);
            _elements.Add(remoteView);
            _elements.Add(notePad);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _background = Content.Load<Texture2D>("Graphics/Textures/Backgrounds/bg");
            bgm = Content.Load<Song>("Audio/Music/ambientbgm1_2");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                foreach (UIElement elem in _elements)
                {
                    elem.FadeOut();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                foreach (UIElement elem in _elements)
                {
                    elem.FadeIn();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                foreach (UIElement elem in _elements)
                {
                    if (elem.MouseIsHovering)
                        elem.Dim();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                foreach (UIElement elem in _elements)
                {
                    if (elem.MouseIsHovering)
                        elem.UnDim();
                }
            }

            base.Update(gameTime); // Handles updating all elements in _elements
        }

        public override void Draw(GameTime gameTime)
        {
            if (_background == null)
                // TODO: Figure out why Draw() gets called before LoadContent() - this is a workaround.
                return;
            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            _spriteBatch.Draw(_background, _rectangle, Color.White);
            base.Draw(gameTime); // Handles drawing all elements in _elements
            _spriteBatch.End();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void SwitchOn()
        {
            base.SwitchOn();
            MusicManager.GetInstance().Start(bgm);
        }
    }
}