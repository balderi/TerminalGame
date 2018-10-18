using System.Xml;

namespace TerminalGame.IO.Parsing
{
    class PlayerToXml
    {
        public static void Parse(Player player, XmlDocument xDoc)
        {
            XmlNode root = xDoc.ChildNodes[1];
            XmlElement thePlayer = xDoc.CreateElement("Player");
            thePlayer.SetAttribute("name", player.Name);
            thePlayer.SetAttribute("password", player.Password);
            thePlayer.SetAttribute("balance", player.Balance.ToString());

            XmlElement playerComp = xDoc.CreateElement("compIdent");
            playerComp.SetAttribute("name", Player.GetInstance().PlayersComputer.Name);
            playerComp.SetAttribute("IP", Player.GetInstance().PlayersComputer.IP);
            thePlayer.AppendChild(playerComp);

            root.AppendChild(thePlayer);
        }
    }
}