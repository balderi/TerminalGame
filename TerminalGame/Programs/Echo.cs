using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Files;

namespace TerminalGame.Programs
{
    class Echo : Program
    {
        private static Echo _instance;

        public static Echo GetInstance()
        {
            if (_instance == null)
                _instance = new Echo();
            return _instance;
        }

        private Echo()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length < 1)
            {
                Game.Terminal.WriteLine("");
                Kill();
                return;
            }
            else
            {
                Game.Terminal.WriteLine("");
                foreach (string word in _args)
                {
                    Game.Terminal.Write(word + " ");
                }
            }
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
