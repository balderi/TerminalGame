using System.Xml;
using TerminalGame.IO.Parsing;

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
            Computers.Computers.GetInstance().LinkLoadedComputers();
        }
    }
}
