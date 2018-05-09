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
        /// <summary>
        /// Call the COnnect method on the computer with the given IP address
        /// </summary>
        /// <param name="IP"></param>
        public static bool connect(string IP)
        {
            Console.WriteLine("Running connect on " + IP + " ...");
            bool conn = false;
            foreach(Computer c in Computers.Computers.computerList)
            {
                if (c.IP == IP || c.Name == IP)
                {
                    c.Connect(false);
                    conn = true;
                    return true;
                }
            }/*
            if(!conn)
            {*/
                Console.WriteLine("Could not connect to " + IP);
                return false;
            //}

            //return "Could not connect to " + IP;
        }
        /// <summary>
        /// Call the Disconnect method on the computer with the given IP address
        /// </summary>
        public static void disconnect()
        {
            Player.GetInstance().ConnectedComputer.Disconnect(false);
        }
    }
}
