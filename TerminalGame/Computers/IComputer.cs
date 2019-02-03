using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Computers
{
    public interface IComputer
    {
        bool Connect();
        void Disconnect();
        void Update(GameTime gameTime);
    }
}
