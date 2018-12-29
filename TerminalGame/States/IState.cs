using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.States
{
    public interface IState
    {
        State GetNextState(string state);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
