using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame
{
    class LoadingScreen
    {
        public string LoadItem { get; set; }

        private SpriteFont largeFont, smallFont;
        private readonly string loading;
        private Vector2 loadCenter, itemCenter;
        
        public LoadingScreen(SpriteFont LargeFont, SpriteFont SmallFont)
        {
            largeFont = LargeFont;
            smallFont = SmallFont;
            loading = "Loading...";
            LoadItem = "";
        }

        public void Update(GameTime gameTime)
        {
            loadCenter = new Vector2(largeFont.MeasureString(loading).X / 2, largeFont.MeasureString(loading).Y / 2);
            itemCenter = new Vector2(smallFont.MeasureString(LoadItem).X / 2, smallFont.MeasureString(LoadItem).Y / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 ScreenCenter)
        {
            spriteBatch.DrawString(largeFont, loading, new Vector2((int)(ScreenCenter.X - loadCenter.X), (int)(ScreenCenter.Y - loadCenter.Y)), Color.LightGray);
            spriteBatch.DrawString(smallFont, LoadItem, new Vector2((int)(ScreenCenter.X - itemCenter.X), (int)(ScreenCenter.Y - itemCenter.Y) + largeFont.MeasureString(loading).Y), Color.LightGray);
        }
    }
}
