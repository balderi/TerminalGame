using System;
using System.Collections.Generic;
using System.Threading;
using TerminalGame.Utilities;

namespace TerminalGame.Computers
{
    class Computers
    {
        public static List<Computer> computerList;

        private static Computers _instance;
        private static Random _rnd;
        public static Computers GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Computers();
            }
            return _instance;
        }

        /// <summary>
        /// Generates fixed and random computers and adds them to the computerList.
        /// </summary>
        public static void DoComputers(int amount)
        {
            computerList = new List<Computer>();
            Computer c1 = new Computer(Computer.Type.Workstation, "123.123.123.123", "TestComputer", "abc123");
            Computer c2 = new Computer(Computer.Type.Server, "100.100.100.100", "TestServer", "abc123");
            Computer c3 = new Computer(Computer.Type.Server, "1.12.123.123", "TestServer With A Pretty Long Name Just To Check Dat InfoBox", "abc123");
            Computer c4 = new Computer(Computer.Type.Server, "111.111.111.111", "Intraware Technology Internal Services Machine", "abc123");
            
            computerList.Add(c1);
            computerList.Add(c2);
            computerList.Add(c3);
            computerList.Add(c4);

            c1.Link(c2);
            c2.Link(c3);
            c3.Link(c4);

            for (int i = 0; i < amount; i++)
            {
                Computer c = new Computer(Computer.Type.Workstation, IPgen(), "Workstation" + i, Passwords.GeneratePassword());
                computerList.Add(c);
            }
        }
        
        /// <summary>
        /// Generate a random IP address.
        /// </summary>
        /// <returns></returns>
        private static string IPgen()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for (int i = 0; i < 4; i++)
            {
                //Make sure each computer gets a different IP. Less than 5ms might work. More than 5ms might be needed.
                Thread.Sleep(5);
                retval += _rnd.Next(1, 254) + ".";
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
