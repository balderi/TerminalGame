using System;
using System.Threading;

namespace TerminalGame.OS
{
    class Programs
    {
        private static Programs instance;
        UI.Modules.Terminal terminal;
        Player player = Player.GetInstance();
        readonly OS os = OS.GetInstance();
        Timer timer;
        int count;
        string[] textToWrite;

        public static Programs GetInstance()
        {
            if (instance == null)
                instance = new Programs();
            return instance;
        }

        private Programs()
        {
            terminal = OS.GetInstance().Terminal;
        }

        #region Programs
#pragma warning disable IDE1006 // Naming Styles

        public int ls()
        {
            if (player.ConnectedComputer.PlayerHasRoot)
            {
                terminal.Write(player.ConnectedComputer.FileSystem.ListFiles());
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
                terminal.Write("\n" + text);
            }
            else
            {
                terminal.Write("\n");
            }
            return 0;
        }


#pragma warning restore IDE1006 // Naming Styles
        #endregion

        #region other methods
        private int NoPriv(string program)
        {
            terminal.Write("\n" + program + ": Permission denied");
            return 1;
        }

        public void TimedWrite(string[] text, int initialDelay, int tickDelay)
        {
            var autoEvent = new AutoResetEvent(false);
            count = 0;
            textToWrite = text;
            timer = new Timer(GenericTimedWriter, autoEvent, initialDelay, tickDelay);
        }

        private void GenericTimedWriter(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            terminal.Write(textToWrite[count++]);
            if (count == textToWrite.Length)
            {
                count = 0;
                autoEvent.Set();
                timer.Dispose();
            }
        }
        #endregion
    }
}
