using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using TerminalGame.Utilities;

namespace TerminalGame.IO
{
    class SaveGame
    {
        public static void CreateBlankSave()
        {
            string saveName = "save_" + DateTime.Now.ToShortDateString();
            GameManager.GetInstance().CurrentSaveName = saveName;
            XmlDocument saveGame = new XmlDocument();
            XmlNode dec = saveGame.CreateXmlDeclaration("1.0", "UTF-8", null);
            saveGame.AppendChild(dec);
            XmlElement root = saveGame.CreateElement("TerminalGameSave");
            root.SetAttribute("version", GameManager.GetInstance().Version);
            root.SetAttribute("saveName", saveName);

            XmlElement player = saveGame.CreateElement("Player");
            player.SetAttribute("name", Player.GetInstance().Name);
            player.SetAttribute("password", Player.GetInstance().Password);
            player.SetAttribute("balance", Player.GetInstance().Balance.ToString());
            root.AppendChild(player);
            saveGame.AppendChild(root);
            saveGame.Save(GameManager.GetInstance().SavePath + "/" + saveName + ".tgs");
            GameManager.GetInstance().CurrentSave = saveGame;
        }

        public static void Save()
        {
            CreateBlankSave();
            foreach(Computers.Computer c in Computers.Computers.GetInstance().ComputerList)
            {
                Parsing.ComputerToXml.Parse(c, GameManager.GetInstance().CurrentSave);
            }
            GameManager.GetInstance().CurrentSave.Save(GameManager.GetInstance().SavePath + "/" + GameManager.GetInstance().CurrentSaveName + ".tgs");
        }
    }
}
