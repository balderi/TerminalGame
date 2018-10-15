using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TerminalGame.Utilities;

namespace TerminalGame.IO
{
    class LoadGame
    {
        public static void Load()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(GameManager.GetInstance().SavePath + "/" + GameManager.GetInstance().CurrentSaveName + ".tgs");
            XmlNode player = xDoc.ChildNodes[1].SelectSingleNode("Player");
            Parsing.PlayerFromXml.Parse(player);
        }
    }
}
