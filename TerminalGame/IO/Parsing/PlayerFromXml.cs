using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalGame.IO.Parsing
{
    class PlayerFromXml
    {
        public static void Parse(XmlNode xNode)
        {
            var test = xNode.Attributes;
            string[] deets = new string[test.Count];
            for(int i = 0; i < test.Count; i++)
            {
                deets[i] = test[i].Value;
            }
            Player.GetInstance().LoadPlayer(deets[0], deets[1], Convert.ToInt32(deets[2]));
        }
    }
}
