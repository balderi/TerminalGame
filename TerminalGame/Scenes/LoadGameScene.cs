using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.UI;
using TerminalGame.Utilities;
using System.IO;
using System.Threading;
using TerminalGame.IO;

namespace TerminalGame.Scenes
{
    class LoadGameScene : Scene
    {

        private readonly SpriteFont _font;
        private readonly GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;
        private bool _prevKbState, _newKbState;
        private MainMenuButton backButton;
        private List<MainMenuButton> _gameList;

        public LoadGameScene(GameWindow gameWindow, SpriteFont buttonFont, SpriteFont font, GraphicsDevice graphics) : base()
        {
            _font = font;
            _gameWindow = gameWindow;
            _graphics = graphics;
            backButton = new MainMenuButton("< Back", 200, 50, buttonFont, _graphics)
            {
                Position = new Vector2(50, _graphics.Viewport.Height - 50 - 50)
            };
            backButton.Click += OnButtonClick;

            SpriteFont loadButtonFont = FontManager.GetFont(FontManager.FontSize.Medium);
            _gameList = new List<MainMenuButton>();
            int games = 0;
            foreach (var f in Directory.GetFiles(GameManager.GetInstance().SavePath))
            {
                MainMenuButton b = new MainMenuButton(f, (int)loadButtonFont.MeasureString(f).X + 20, 
                    (int)loadButtonFont.MeasureString("A").Y + 20, loadButtonFont, _graphics);
                b.Position = new Vector2(50, games++ * (10 + b.Rectangle.Height) + 200);
                b.Click += OnLoadClick;
                _gameList.Add(b);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            Vector2 textMiddlePoint = _font.MeasureString("Load Game") / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(_font, "Load Game", position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_font, "Load Game", position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            if (_gameList.Count == 0)
                spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.Medium), "No saved games to load :(", new Vector2(50, 200), Color.White);

            foreach(var b in _gameList)
            {
                b.Draw(spriteBatch);
            }
            
            backButton.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            backButton.Update();
            _newKbState = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (_newKbState != _prevKbState)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    _stateMachine.Transition(GameState.MainMenu);
                }
            }
            _prevKbState = _newKbState;

            foreach (var b in _gameList)
            {
                b.Update();
            }
        }

        private void OnButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }

        private void OnLoadClick(ButtonPressedEventArgs e)
        {
            GameManager.GetInstance().CurrentSaveName = e.Text;
            GameManager.GetInstance().StateMachine.Transition(GameState.GameLoading);
            Thread _loadingThread = new Thread(new ThreadStart(WhatTheFuck.GetInstance().StartLoadGame));
            _loadingThread.Start();
        }
    }
}
