using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Connect
    {
        public static string connect(string IP)
        {
            Console.WriteLine("Running connect on " + IP + " ...");
            foreach(Computer c in Computers.Computers.computerList)
            {
                if (c.IP == IP)
                {
                    return c.Connect();
                }
            }
            Console.WriteLine("Could not connect to " + IP);
            return "Could not connect to " + IP;
        }
        public static string disconnect()
        {
            return Player.GetInstance().ConnectedComputer.Disconnect();
        }
    }
}
