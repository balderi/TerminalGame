using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TerminalGame.IO.Parsing;
using TerminalGame.States;
using TerminalGame.Utilities;

namespace TerminalGame.IO
{
    class LoadGame
    {
        public static void Load(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path); 
            XmlNode player = xDoc.ChildNodes[1].SelectSingleNode("Player");
            PlayerFromXml.Parse(player);
            XmlNodeList computers = xDoc.ChildNodes[1].SelectNodes("computer");
            foreach(XmlNode n in computers)
            {
                Computers.Computers.GetInstance().ComputerList.Add(ComputerFromXml.Parse(n));
            }
        }
    }
}
