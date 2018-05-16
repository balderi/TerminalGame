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

        public static Programs GetInstance()
        {
            if (instance == null)
                instance = new Programs();
            return instance;
        }

        private Programs()
        {

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
            OS.GetInstance().Terminal.Write("Connecting to " + Player.GetInstance().ConnectedComputer.IP + ":ssh ");
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write(" successful.\n");


            OS.GetInstance().Terminal.Write("Attempting to exploit SSHv1 CRC32 ");
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write("."); Thread.Sleep(333);
            OS.GetInstance().Terminal.Write(" successful.\n"); Thread.Sleep(200);


            OS.GetInstance().Terminal.Write("Resetting root password to \"password\".\n");
            Player.GetInstance().ConnectedComputer.GetRoot(); Thread.Sleep(200);
            Player.GetInstance().ConnectedComputer.Connect();
            OS.GetInstance().Terminal.Write("System open: Access level <9>\n");
        }

#pragma warning restore IDE1006 // Naming Styles
        #endregion
    }
}
