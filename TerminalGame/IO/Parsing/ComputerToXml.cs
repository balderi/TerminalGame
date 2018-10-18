using System.Xml;
using TerminalGame.Computers;

namespace TerminalGame.IO.Parsing
{
    class ComputerToXml
    {
        public static void Parse(Computer computer, XmlDocument xDoc)
        {
            XmlNode root = xDoc.ChildNodes[1];

            XmlElement comp = xDoc.CreateElement("computer");
            comp.SetAttribute("name", computer.Name);
            comp.SetAttribute("password", computer.RootPassword);
            comp.SetAttribute("ip", computer.IP);
            comp.SetAttribute("type", ((int)computer.ComputerType).ToString());
            comp.SetAttribute("access", ((int)computer.Access).ToString());

            XmlElement map = xDoc.CreateElement("mapPosition");
            map.SetAttribute("x", computer.MapX.ToString());
            map.SetAttribute("y", computer.MapY.ToString());
            comp.AppendChild(map);

            XmlElement security = xDoc.CreateElement("security");
            security.SetAttribute("hasRoot", computer.PlayerHasRoot.ToString());
            security.SetAttribute("traceTime", computer.TraceTime.ToString());
            comp.AppendChild(security);

            XmlElement openPorts = xDoc.CreateElement("openPorts");
            openPorts.InnerText = computer.GetPortsString();
            comp.AppendChild(openPorts);

            XmlElement misc = xDoc.CreateElement("misc");
            misc.SetAttribute("IsObjective", computer.IsMissionObjective.ToString());
            misc.SetAttribute("IsShownOnMap", computer.IsShownOnMap.ToString());

            XmlElement links = xDoc.CreateElement("links");
            if(computer.LinkedComputers.Count > 0)
            {
                string linkIndicies = "";
                foreach(Computer c in computer.LinkedComputers)
                {
                    linkIndicies += " " + Computers.Computers.GetInstance().ComputerList.IndexOf(c);
                }
                links.InnerText = linkIndicies;
            }
            misc.AppendChild(links);
            comp.AppendChild(misc);

            XmlElement filesystem = xDoc.CreateElement("filesystem");
            XmlElement rootdir = xDoc.CreateElement("dir");
            rootdir.SetAttribute("name", "/");
            computer.FileSystem.ChangeDir("/");
            foreach(Computers.FileSystems.File f in computer.FileSystem.CurrentDir.Children)
            {
                if(f.IsDirectory)
                {
                    XmlElement dir = xDoc.CreateElement("dir");
                    dir.SetAttribute("name", f.Name);
                    computer.FileSystem.ChangeDir(f.Name);
                    foreach (Computers.FileSystems.File sf in computer.FileSystem.CurrentDir.Children)
                    {
                        if (sf.IsDirectory)
                        {
                            XmlElement subdir = xDoc.CreateElement("dir");
                            subdir.SetAttribute("name", sf.Name);
                            dir.AppendChild(subdir);
                        }
                        else
                        {
                            XmlElement file = xDoc.CreateElement("file");
                            file.SetAttribute("name", sf.Name);
                            file.SetAttribute("contents", sf.Contents);
                            dir.AppendChild(file);
                        }
                    }
                    computer.FileSystem.ChangeDir("..");
                    rootdir.AppendChild(dir);
                }
                else
                {
                    XmlElement file = xDoc.CreateElement("file");
                    file.SetAttribute("name", f.Name);
                    file.SetAttribute("contents", f.Contents);
                    rootdir.AppendChild(file);
                }
            }
            filesystem.AppendChild(rootdir);
            comp.AppendChild(filesystem);
            root.AppendChild(comp);
        }
    }
}
