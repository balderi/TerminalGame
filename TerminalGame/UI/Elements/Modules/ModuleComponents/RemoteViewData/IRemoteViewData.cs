using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData
{
    public interface IRemoteViewData
    {
        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch, Rectangle remoteViewRectangle, float opacity);
    }
}
