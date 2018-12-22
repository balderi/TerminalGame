using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerminalGame.States;
using TerminalGame.UI;
using TerminalGame.Utils;
using System.IO;
using System.Threading;

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
        private SpriteFont _loadButtonFont;

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

            _loadButtonFont = FontManager.GetFont(FontManager.FontSize.Medium);
            _gameList = new List<MainMenuButton>();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            _gameList.Clear();
            int games = 0;
            foreach (var f in Directory.GetFiles(GameManager.GetInstance().SavePath))
            {
                string title = f.Split('\\').Last().Split('/').Last();
                MainMenuButton b = new MainMenuButton(title, (int)_loadButtonFont.MeasureString("'MaximumPlayerAccountNameLength'").X + 20,
                    (int)_loadButtonFont.MeasureString("A").Y + 20, _loadButtonFont, _graphics);
                b.Position = new Vector2(50, games++ * (10 + b.Rectangle.Height) + 200);
                b.Value = f;
                b.Click += OnLoadClick;
                _gameList.Add(b);

                MainMenuButton d = new MainMenuButton(" X ", (int)_loadButtonFont.MeasureString("----").X + 20,
                    (int)_loadButtonFont.MeasureString("A").Y + 20, _loadButtonFont, _graphics)
                {
                    Position = new Vector2(b.Position.X + b.Rectangle.Width + 10, b.Position.Y),
                    Value = f
                };
                d.Click += OnDeleteClick;
                _gameList.Add(d);
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

            //yeah, I know... it works, though... ¯\_(ツ)_/¯
            try
            {
                foreach (var b in _gameList)
                {
                    b.Update();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnButtonClick(ButtonPressedEventArgs e)
        {
            _stateMachine.Transition(GameState.MainMenu);
        }

        private void OnLoadClick(ButtonPressedEventArgs e)
        {
            GameManager.GetInstance().CurrentSaveName = e.Value;
            GameManager.GetInstance().StateMachine.Transition(GameState.GameLoading);
            Thread _loadingThread = new Thread(new ThreadStart(WhatTheFuck.GetInstance().StartLoadGame));
            _loadingThread.Start();
        }

        private void OnDeleteClick(ButtonPressedEventArgs e)
        {
            File.Delete(e.Value);
            RefreshButtons();
        }
    }
}
