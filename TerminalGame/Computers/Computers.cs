using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TerminalGame.Computers
{
    class Computers
    {
        public static List<Computer> computerList;

        private static Computers instance;
        private static Random rnd;
        public static Computers GetInstance()
        {
            if (instance == null)
            {
                instance = new Computers();
            }
            return instance;
        }

        /// <summary>
        /// Generates fixed and random computers and adds them to the computerList.
        /// </summary>
        public static void DoComputers()
        {
            computerList = new List<Computer>();
            //FileSystems.FileSystem fs = new FileSystems.FileSystem();
            Computer c1 = new Computer(Computer.Type.Workstation, "123.123.123.123", "TestComputer", "abc123");
            Computer c2 = new Computer(Computer.Type.Server, "100.100.100.100", "TestServer", "abc123");
            computerList.Add(c1);
            computerList.Add(c2);

            for(int i = 0; i < 10; i++)
            {
                Computer c = new Computer(Computer.Type.Workstation, IPgen(), "Workstation" + i, "password");
                computerList.Add(c);
            }
        }
        
        /// <summary>
        /// Generate a random IP address.
        /// </summary>
        /// <returns></returns>
        private static string IPgen()
        {
            rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for (int i = 0; i < 4; i++)
            {
                //Make sure each computer gets a different IP. Less than 5ms might work. More than 5ms might be needed.
                Thread.Sleep(5);
                retval += rnd.Next(1, 254) + ".";
            }
            return retval.TrimEnd('.');
        }
    }
    /// <summary>
    /// Custom EventArgs for Computers
    /// </summary>
    public class ConnectEventArgs : EventArgs
    {
        /// <summary>
        /// IP passed through the event
        /// </summary>
        public string ConnectionString { get; private set; }
        /// <summary>
        /// Is player root? Passed through event
        /// </summary>
        public bool IsRoot { get; private set; }
        /// <summary>
        /// The actual EventArgs
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="isRoot"></param>
        public ConnectEventArgs(string connectionString, bool isRoot)
        {
            ConnectionString = connectionString;
            IsRoot = isRoot;
        }
    }
}
