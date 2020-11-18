using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TerminalGame.Computers;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData
{
    public interface IRemoteViewData
    {
        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch, Rectangle remoteViewRectangle);
    }
}
