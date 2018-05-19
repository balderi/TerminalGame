using System;
using System.Threading;

namespace TerminalGame.Programs
{
    class Connect
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;
        static Timer timer;
        static int count;
        static string[] textToWrite;
        static string IP;

        public static void Execute(string ip = null)
        {
            if (!String.IsNullOrEmpty(ip))
            {
                IP = ip;

                if (ip == player.ConnectedComputer.IP || ip == player.ConnectedComputer.Name)
                {
                    terminal.Write("\nYou are already connected to " + player.ConnectedComputer.Name + "@" + player.ConnectedComputer.IP);
                    return;
                }

                var autoEvent = new AutoResetEvent(false);
                count = 0;

                textToWrite = new string[]
                {
                    "\nEstablishing connection to " + ip,
                    ".",
                    ".",
                    ".§"
                };

                os.NetworkMap.IsActive = false;

                timer = new Timer(ConnectionWriter, autoEvent, 100, 500);
            }
            else
            {
                terminal.Write("\nUsage: connect [IP]");
                return;
            }
        }

        private static void ConnectionWriter(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            terminal.Write(textToWrite[count++]);
            if (count == textToWrite.Length)
            {
                bool conn = false;
                foreach (Computers.Computer c in Computers.Computers.computerList)
                {
                    if (IP == c.IP || IP == c.Name)
                    {
                        conn = true;
                        c.Connect(false);
                        terminal.Write("\nConnection established");
                        os.NetworkMap.IsActive = true;
                    }
                }
                if(!conn)
                    terminal.Write("\nCould not connect to " + IP);

                count = 0;
                autoEvent.Set();
                timer.Dispose();
            }
        }
    }
}
