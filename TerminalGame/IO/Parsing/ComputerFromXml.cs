using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TerminalGame.Computers;
using TerminalGame.Computers.FileSystems;

namespace TerminalGame.IO.Parsing
{
    class ComputerFromXml
    {
        public static Computer Parse(XmlNode xNode)
        {
            Computer retval;
            
            FileSystem fs = new FileSystem();
            RecursiveFsParse(fs, xNode.SelectSingleNode("filesystem"));

            var compAtts = ParseAtts(xNode.Attributes);
            var mappos = ParseAtts(xNode.SelectSingleNode("mapPosition").Attributes);
            var security = ParseAtts(xNode.SelectSingleNode("security").Attributes);
            var misc = ParseAtts(xNode.SelectSingleNode("misc").Attributes);
            var ports = xNode.SelectSingleNode("openPorts").InnerText.Trim().Split(' ');

            int[] openports = new int[ports.Length];
            for(int i = 0; i < ports.Length; i++)
            {
                openports[i] = Convert.ToInt32(ports[i]);
            }

            retval = new Computer((Computer.Type)Convert.ToInt32(compAtts[3]), compAtts[2], compAtts[0], compAtts[1], 
                (float)Convert.ToDecimal(security[1]), fs, openports);

            if (Convert.ToBoolean(misc[0]))
                retval.SetAsObjective();
            if (Convert.ToBoolean(misc[1]))
                retval.ShowOnMap();
            if (Convert.ToBoolean(security[0]))
                retval.GetRoot();

            retval.MapX = (float)Convert.ToDecimal(mappos[0]);
            retval.MapY = (float)Convert.ToDecimal(mappos[1]);

            return retval;
        }

        private static string[] ParseAtts(XmlAttributeCollection atts)
        {
            string[] retval = new string[atts.Count];
            for (int i = 0; i < atts.Count; i++)
            {
                retval[i] = atts[i].Value;
            }
            return retval;
        }

        private static void RecursiveFsParse(FileSystem fs, XmlNode node)
        {
            if (!node.HasChildNodes)
                return;
            foreach(XmlNode cn in node.ChildNodes)
            {
                if(cn.Prefix == "dir")
                {
                    fs.AddDir(cn.Prefix);
                    fs.ChangeDir(cn.Prefix);
                    RecursiveFsParse(fs, cn);
                    fs.ChangeDir("..");
                }
                else
                {
                    var atts = cn.Attributes;
                    string[] deets = new string[atts.Count];
                    for(int i = 0; i < atts.Count; i++)
                    {
                        deets[i] = atts[i].Value;
                    }
                    fs.AddFile(deets[0], deets[1]);
                }
            }
        }
    }
}
