using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Connection
    {
        /// <summary>
        /// Call the COnnect method on the computer with the given IP address
        /// </summary>
        /// <param name="IP"></param>
        public static bool Connect(string IP)
        {
            Console.WriteLine("Running connect on " + IP + " ...");
            foreach(Computer c in Computers.Computers.computerList)
            {
                if(IP == Player.GetInstance().ConnectedComputer.IP || IP == Player.GetInstance().ConnectedComputer.Name)
                {
                    Console.WriteLine("Already connected to " + IP);
                    return false;
                }
                if (c.IP == IP || c.Name == IP)
                {
                    c.Connect(false);
                    return true;
                }
            }
            Console.WriteLine("Could not connect to " + IP);
            return false;
        }

        /// <summary>
        /// Call the Disconnect method on the computer with the given IP address
        /// </summary>
        public static void Disconnect()
        {
            Player.GetInstance().ConnectedComputer.Disconnect(false);
        }
    }
}
