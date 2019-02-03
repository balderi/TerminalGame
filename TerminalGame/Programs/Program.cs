using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TerminalGame.Programs
{
    public abstract class Program : IProgram
    {
        #region fields
        protected bool _isKill;
        protected string _opString;
        protected string[] _args;
        protected bool _isInitialized;
        protected Timer _timer;
        protected TerminalGame Game;
        #endregion

        #region properties
        public int PID { get; protected set; }
        #endregion

        protected abstract void Run();

        public void Kill()
        {
            _isKill = true;
            _opString = null;
            _args = null;
            _isInitialized = false;
        }

        public virtual void Init(TerminalGame game, string optstring = null, string[] args = null)
        {
            if (!_isInitialized)
            {
                Game = game;
                _timer = new Timer();
                _timer.Elapsed += Timer_Tick;
                _opString = optstring;
                _args = args;
                _isInitialized = true;
                Run();
            }
        }

        protected abstract void Timer_Tick(object sender, EventArgs e);
    }
}
