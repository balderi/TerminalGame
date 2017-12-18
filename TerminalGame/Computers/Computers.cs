using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Computers
{
    class Computers
    {
        public static List<Computer> computerList;

        private static Computers instance;
        public static Computers GetInstance()
        {
            if (instance == null)
            {
                instance = new Computers();
            }
            return instance;
        }

        public static void DoComputers()
        {
            computerList = new List<Computer>();
            Computer c1 = new Computer(Computer.Type.Workstation, "123.123.123.123", "TestComputer", "abc123");
            Computer c2 = new Computer(Computer.Type.Server, "100.100.100.100", "TestServer", "abc123");
            computerList.Add(c1);
            computerList.Add(c2);
        }
        
    }
}
