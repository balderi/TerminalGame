using System;
using System.Xml;
using TerminalGame.Utilities;

namespace TerminalGame.IO
{
    class SaveGame
    {
        public static void CreateBlankSave()
        {
            string saveName = Player.GetInstance().Name + "_save";
            GameManager.GetInstance().CurrentSaveName = saveName;
            XmlDocument saveGame = new XmlDocument();
            XmlNode dec = saveGame.CreateXmlDeclaration("1.0", "UTF-8", null);
            saveGame.AppendChild(dec);
            XmlElement root = saveGame.CreateElement("TerminalGameSave");
            root.SetAttribute("version", GameManager.GetInstance().Version);
            root.SetAttribute("saveName", saveName);
            saveGame.AppendChild(root);

            saveGame.Save(GameManager.GetInstance().SavePath + "/" + saveName + ".tgs");
            GameManager.GetInstance().CurrentSave = saveGame;
        }

        public static void Save()
        {
            Console.WriteLine("Starting save...");
            CreateBlankSave();

            Parsing.PlayerToXml.Parse(Player.GetInstance(), GameManager.GetInstance().CurrentSave);

            foreach(Computers.Computer c in Computers.Computers.GetInstance().ComputerList)
            {
                Parsing.ComputerToXml.Parse(c, GameManager.GetInstance().CurrentSave);
            }

            GameManager.GetInstance().CurrentSave.Save(GameManager.GetInstance().SavePath + "/" + GameManager.GetInstance().CurrentSaveName + ".tgs");
            Console.WriteLine("Done");
        }
    }
}
