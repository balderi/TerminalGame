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
    class LoadingScene : Scene
    {
        public string LoadItem { get; set; }

        private SpriteFont _largeFont, _smallFont;
        private readonly string _loading;
        private Vector2 _loadCenter, _itemCenter, _screenCenter;
        private GameWindow _gameWindow;
        private readonly GraphicsDevice _graphics;

        public LoadingScene(Vector2 screenCenter, GameWindow gameWindow, GraphicsDevice graphics) : base()
        {
            _screenCenter = screenCenter;
            _graphics = graphics;
            _gameWindow = gameWindow;
            _largeFont = FontManager.GetFont(FontManager.FontSize.Large);
            _smallFont = FontManager.GetFont(FontManager.FontSize.Small);
            _loading = "Loading";
            LoadItem = "";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            spriteBatch.DrawString(_largeFont, _loading, new Vector2(_screenCenter.X - _loadCenter.X, _screenCenter.Y - _loadCenter.Y), Color.LightGray);
            spriteBatch.DrawString(_smallFont, LoadItem, new Vector2((int)(_screenCenter.X - _itemCenter.X), (int)(_screenCenter.Y - _itemCenter.Y) + _largeFont.MeasureString(_loading).Y), Color.LightGray);
        }

        public override void Update(GameTime gameTime)
        {
            _loadCenter = new Vector2(_largeFont.MeasureString(_loading).X / 2, _largeFont.MeasureString(_loading).Y / 2);
            _itemCenter = new Vector2(_smallFont.MeasureString(LoadItem).X / 2, _smallFont.MeasureString(LoadItem).Y / 2);
        }
    }
}
