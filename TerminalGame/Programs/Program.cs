using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Programs
{
    public abstract class Program : IProgram
    {
        #region fields
        protected bool _isKill;
        #endregion

        #region properties
        #endregion

        public abstract void Run();

        public void Kill()
        {
            _isKill = true;
        }

        protected abstract void Init(string optstring = null, string[] args = null);
    }
}
