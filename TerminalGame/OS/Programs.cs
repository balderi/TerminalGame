using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TerminalGame.OS
{
    class Programs
    {
        private static Programs instance;
        UI.Modules.Terminal terminal;
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

        public string ls()
        {
            return "";
        }

        public string cd(string folder)
        {
            return "";
        }

        public string rm(string[] args)
        {
            return "";
        }

        public string connect(string ip)
        {
            return "";
        }

        public string disconnect()
        {
            return "";
        }

        public void sshnuke()
        {
            var autoEvent = new AutoResetEvent(false);
            terminal.Write("\nConnecting to " + Player.GetInstance().ConnectedComputer.IP + ":ssh ");
            count = 0;
            textToWrite = new string[]
            {
                ".",
                ".",
                ".",
                " successful.\nAttempting to exploit SSHv1 CRC32 ",
                ".",
                ".",
                ".",
                " successful.",
                "\nResetting root password to \"password\".",
                "\nSystem open: Access level <9>"
            };
            timer = new Timer(write, autoEvent, 1000, 333);
        }

        private void write(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            terminal.Write(textToWrite[count++]);
            if(count == textToWrite.Length)
            {
                Player.GetInstance().ConnectedComputer.GetRoot();
                Player.GetInstance().ConnectedComputer.Connect();
                count = 0;
                autoEvent.Set();
                timer.Dispose();
            }
        }

#pragma warning restore IDE1006 // Naming Styles
        #endregion
    }
}
