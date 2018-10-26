using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;

namespace TerminalGame.Scenes
{
    public class LoadingScene : Scene
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
            _loading = "Stand by...";
            LoadItem = "";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Drawing.DrawBlankTexture(_graphics), _gameWindow.ClientBounds, Color.Black);
            spriteBatch.DrawString(_largeFont, _loading, new Vector2(_screenCenter.X - _loadCenter.X, _screenCenter.Y - _loadCenter.Y), Color.LightGray);
            spriteBatch.DrawString(_smallFont, LoadItem, new Vector2((int)(_screenCenter.X - _itemCenter.X), (int)(_screenCenter.Y - _itemCenter.Y) + _largeFont.MeasureString(_loading).Y), Color.LightGray);
            Vector2 textMiddlePoint = FontManager.GetFont(FontManager.FontSize.XLarge).MeasureString("Loading") / 2;
            Vector2 position = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15), textMiddlePoint.Y + 15);
            Vector2 position2 = new Vector2((_gameWindow.ClientBounds.Width - textMiddlePoint.X - 15) + TestClass.ShakeStuff(3), textMiddlePoint.Y + 15 + TestClass.ShakeStuff(3));
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.XLarge), "Loading", position2, Color.Green, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFont(FontManager.FontSize.XLarge), "Loading", position, Color.LightGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            _loadCenter = new Vector2(_largeFont.MeasureString(_loading).X / 2, _largeFont.MeasureString(_loading).Y / 2);
            _itemCenter = new Vector2(_smallFont.MeasureString(LoadItem).X / 2, _smallFont.MeasureString(LoadItem).Y / 2);
        }
    }
}
