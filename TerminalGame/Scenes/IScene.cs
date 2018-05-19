using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.Scenes
{
    public interface IScene
    {
        // TODO: Is an interface the best way to do this?
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
