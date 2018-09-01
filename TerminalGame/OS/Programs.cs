using System;
using System.Threading;

namespace TerminalGame.OS
{
    class Programs
    {
        private static Programs _instance;
        private UI.Modules.Terminal _terminal;
        private Player _player = Player.GetInstance();
        private readonly OS _os = OS.GetInstance();
        private Timer _timer;
        private int _count;
        private string[] _textToWrite;

        public static Programs GetInstance()
        {
            if (_instance == null)
                _instance = new Programs();
            return _instance;
        }

        private Programs()
        {
            _terminal = OS.GetInstance().Terminal;
        }

        #region Programs
#pragma warning disable IDE1006 // Naming Styles

        public int ls()
        {
            if (_player.ConnectedComputer.PlayerHasRoot)
            {
                _terminal.Write(_player.ConnectedComputer.FileSystem.ListFiles());
                return 0;
            }
            else
            {
                return NoPriv("ls");
            }
        }

        

        public int echo(string text = null)
        {
            if (!String.IsNullOrEmpty(text))
            {
                _terminal.Write("\n" + text);
            }
            else
            {
                _terminal.Write("\n");
            }
            return 0;
        }


#pragma warning restore IDE1006 // Naming Styles
        #endregion

        #region other methods
        private int NoPriv(string program)
        {
            _terminal.Write("\n" + program + ": Permission denied");
            return 1;
        }

        public void TimedWrite(string[] text, int initialDelay, int tickDelay)
        {
            var autoEvent = new AutoResetEvent(false);
            _count = 0;
            _textToWrite = text;
            _timer = new Timer(GenericTimedWriter, autoEvent, initialDelay, tickDelay);
        }

        private void GenericTimedWriter(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            _terminal.Write(_textToWrite[_count++]);
            if (_count == _textToWrite.Length)
            {
                _count = 0;
                autoEvent.Set();
                _timer.Dispose();
            }
        }
        #endregion
    }
}
