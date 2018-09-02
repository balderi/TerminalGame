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
    class MenuScene : IScene
    {
        public delegate void ButtonPressedEventHandler(ButtonPressedEventArgs e);

        readonly List<MainMenuButton> buttons;
        public event ButtonPressedEventHandler ButtonClicked;

        private readonly string gameTitle;
        private SpriteFont _titleFont;
        private GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;

        public MenuScene(string GameTitle, GameWindow gameWindow, SpriteFont buttonFont, SpriteFont titleFont, GraphicsDevice graphics)
        {
            gameTitle = GameTitle;
            _titleFont = titleFont;
            _gameWindow = gameWindow;
            _graphics = graphics;

            float leftMargin = 50f;
            float topMargin = 250f;
            float spacing = 20f;
            int width = 500;
            int height = 50;
            int counter = 0;

            MainMenuButton newGame = new MainMenuButton("New Game", width, height, buttonFont, graphics)
            {
            };
            MainMenuButton loadGame = new MainMenuButton("Load Game", width, height, buttonFont, graphics)
            {
            };
            MainMenuButton settings = new MainMenuButton("Settings", width, height, buttonFont, graphics)
            {
            };
            MainMenuButton quit = new MainMenuButton("Quit Game", width, height, buttonFont, graphics)
            {
            };
            buttons = new List<MainMenuButton>()
            {
                newGame,
                loadGame,
                settings,
                quit,
            };
            foreach (MainMenuButton b in buttons)
            {
                b.Position = new Vector2(leftMargin, (topMargin + counter * height + counter * spacing));
                counter++;
            }
            newGame.Click += NewGame_Click;
            loadGame.Click += LoadGame_Click;
            settings.Click += Settings_Click;
            quit.Click += Quit_Click;
        }

        private void Quit_Click(ButtonPressedEventArgs e)
        {
            ButtonClicked?.Invoke(e);
        }

        private void Settings_Click(ButtonPressedEventArgs e)
        {
            ButtonClicked?.Invoke(e);
        }

        private void LoadGame_Click(ButtonPressedEventArgs e)
        {
            ButtonClicked?.Invoke(e);
        }

        private void NewGame_Click(ButtonPressedEventArgs e)
        {
            ButtonClicked?.Invoke(e);
        }

        public void Update(GameTime gameTime)
        {
            foreach (MainMenuButton b in buttons)
            {
                b.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            Vector2 textMiddlePoint = _titleFont.MeasureString(gameTitle) / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(_titleFont, gameTitle, position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(_titleFont, gameTitle, position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            foreach (MainMenuButton b in buttons)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
