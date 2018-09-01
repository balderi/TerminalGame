using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalGame.Computers;

namespace TerminalGame.Programs
{
    class Nmap
    {
        static Player player = Player.GetInstance();
        static OS.OS os = OS.OS.GetInstance();
        static Computer playerComp = Player.GetInstance().PlayersComputer;
        static Computer remoteComp;
        static UI.Modules.Terminal terminal = os.Terminal;
        static string[] textToWrite;
        static Random rnd;

        public static int Execute(string ip = null)
        {
            rnd = new Random(DateTime.Now.Millisecond);
            // TODO: Make latency dependent on distance from player computer. Maybe make this a property of computers.
            int latency = rnd.Next(10, 99);
            terminal.BlockInput();
            player.PlayersComputer.FileSystem.ChangeDir("/");
            player.PlayersComputer.FileSystem.ChangeDir("bin");
            if (player.PlayersComputer.FileSystem.TryFindFile("nmap", false))
            {
                player.PlayersComputer.FileSystem.ChangeDir("/");
                if (String.IsNullOrEmpty(ip))
                {
                    Console.WriteLine("** WARN: nmap IP was NULL or EMPTY");
                    remoteComp = player.ConnectedComputer;
                    ip = remoteComp.Name;
                }

                terminal.Write("\nStarting Nmap scan for " + ip);
                Thread.Sleep(20 * latency);
                if (HostExists(ip))
                {
                    textToWrite = new string[]
                    {
                        "\nNmap scan report for " + remoteComp.Name + " (" + remoteComp.IP + ")",
                        "\nHost is up (0." + latency + "s latency).",
                        "\nNot shown: " + (1000 - remoteComp.OpenPorts.Count) + " filtered ports",
                        "\n",
                        "\nPORT   STATE  SERVICE",
                        "\n",
                        "\n",
                        "\nDevice type: " + remoteComp.ComputerType,
                        "\nNo exact OS matches for host (test conditions non-ideal).",
                        "\n",
                        "\nNmap done: 1 IP address (1 host up) scanned",
                        "\n",
                    };

                    for (int i = 0; i < textToWrite.Length; i++)
                    {
                        if (i == 5)
                        {
                            foreach (var port in remoteComp.OpenPorts)
                            {
                                terminal.Write("\n" + PrettyPrintPorts(port.Key, port.Value));
                                Thread.Sleep(5 * latency);
                            }
                        }
                        else
                        {
                            if (textToWrite[i].Contains("\n"))
                                terminal.Write(textToWrite[i]);
                            else
                                terminal.WritePartialLine(textToWrite[i]);
                            Thread.Sleep((int)(latency * playerComp.Speed));
                        }
                    }
                }
                else
                {
                    Thread.Sleep(20 * latency);
                    terminal.Write("\nNo response from host " + ip + ".");
                }

                terminal.UnblockInput();
                return 0;
            }
            else
            {
                terminal.Write("\nThe program \'nmap\' is currently not installed");
                terminal.UnblockInput();
                player.PlayersComputer.FileSystem.ChangeDir("/");
                return 1;
            }
        }

        static bool HostExists(string ip)
        {
            foreach(var comp in Computers.Computers.computerList)
            {
                if (comp.IP == ip || comp.Name == ip)
                {
                    remoteComp = comp;
                    return true;
                }
            }
            return false;
        }

        static string PrettyPrintPorts(int port, string service)
        {
            string retval = "";
            retval += port + Spaces(port.ToString(), 7);
            retval += "open" + Spaces("open", 7);
            retval += service.ToLower();
            return retval;
        }

        static string Spaces(string text, int length)
        {
            string retval = "";
            for(int i = 0; i < (length - text.Length); i++)
            {
                retval += " ";
            }
            return retval;
        }
    }
}
