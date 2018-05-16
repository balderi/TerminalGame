using System;
using System.Threading;

namespace TerminalGame.Utilities
{
    /// <summary>
    /// Command Parser
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static void ParseCommand(string command)
        {
            Player player = Player.GetInstance();
            OS.OS os = OS.OS.GetInstance();
            UI.Modules.Terminal terminal = os.Terminal;

            var data = command.Split();
            string noPriv = data[0] + ": Permission denied\n";
            switch (data[0])
            {
                case "note":
                    {
                        if(data.Length > 1)
                        {
                            if(data[1] == "-r" && data.Length > 2)
                            {
                                if(data[2] == "*")
                                {
                                    os.Notes.Clear();
                                    terminal.Write("All notes removed");
                                    break;
                                }
                                if(Int32.TryParse(data[2], out int id))
                                {
                                    if(os.Notes.RemoveNote(id))
                                    {
                                        terminal.Write("Note removed\n");
                                        break;
                                    }
                                }
                                terminal.Write("No note with id \'" + data[2] + "\'\n");
                                break;
                            }
                            else if (data[1] == "-r" && data.Length < 3)
                            {
                                terminal.Write("Usage: " + data[0] + " [OPTIONS] [NOTE OR NOTE ID]\n");
                                break;
                            }
                            else
                            {
                                try
                                {
                                    os.Notes.AddNote(command.Split('"')[1]);
                                }
                                catch
                                {
                                    os.Notes.AddNote(data[1]);
                                }
                                terminal.Write("Note added\n");
                                break;
                            }
                        }
                        terminal.Write("Usage: " + data[0] + " [OPTIONS] [NOTE OR NOTE ID]\n");
                        break;
                    }
                case "sshnuke":
                    {
                        OS.Programs.GetInstance().sshnuke();
                        break;
                        //Player.GetInstance().ConnectedComputer.GetRoot();
                        //Thread.Sleep(10);
                        //Player.GetInstance().ConnectedComputer.Connect();
                        //return "Connecting to " + Player.GetInstance().ConnectedComputer.IP + ":ssh ... successful.\n§" +
                        //    "Attempting to exploit SSHv1 CRC32 ... successful.\n§" +
                        //    "Resetting root password to \"passwd\".\n§" + 
                        //    "System open: Access level <9>\n";
                    }
                case "shutdown":
                case "reboot":
                case "exit":
                    {
                        terminal.Write("I'm sorry Dave, I'm afraid I can't do that.\n");
                        break;
                    }
                case "":
                    {
                        terminal.Write("");
                        break;
                    }
                case "echo":
                    {
                        if (data.Length > 1)
                        {
                            terminal.Write(command.Split('"')[1] + "\n");
                            break;
                        }
                        terminal.Write("\n");
                        break;
                    }
                case "sudo":
                    {
                        if(data.Length > 1)
                        {
                            if (data[1] == "su")
                            {

                            }
                        }
                        terminal.Write(data[0] + ": username is not in the sudoers file. This incident will be reported.\n");
                        break;
                    }
                case "rm":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                if (data[1] != null && Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Children.Count > 0 && data[1] != "*")
                                {
                                    if (data[1] == "-r" && data.Length > 2)
                                    {
                                        if (player.ConnectedComputer.FileSystem.TryFindFile(data[2], true))
                                        {
                                            player.ConnectedComputer.FileSystem.RemoveFile(player.ConnectedComputer.FileSystem.FindFile(data[2], true));
                                        }
                                        else
                                        {
                                            terminal.Write(data[0] + ": missing operand\n");
                                        }
                                        break;
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(data[1], true))
                                    {
                                        terminal.Write(data[0] + ": cannot remove \'" + data[1] + "\': is a directory\n");
                                        break;
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(data[1], false))
                                    {
                                        player.ConnectedComputer.FileSystem.RemoveFile(player.ConnectedComputer.FileSystem.FindFile(data[1], false));
                                        break;
                                    }
                                    else
                                    {
                                        terminal.Write(data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory\n");
                                        break;
                                    }
                                }
                                else
                                {
                                    terminal.Write(data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory\n");
                                    break;
                                }
                            }
                            terminal.Write(data[0] + ": missing operand\n");
                            break;
                        }
                        else
                        {
                            terminal.Write(noPriv);
                            break;
                        }
                    }
                case "man":
                    {
                        if (data.Length > 1)
                        {
                            terminal.Write("No manual entry for " + data[1] + "\n");
                            break;
                        }
                        terminal.Write("What manual page do you want?\n");
                        break;
                    }
                case "connect":
                    {
                        if(data.Length > 1)
                        {
                            if (Programs.Connection.Connect(data[1]))
                            {
                                terminal.Write("Connected to " + data[1] + "\n");
                                break;
                            }
                            else
                            {
                                terminal.Write("Could not connect to " + data[1] + "\n");
                                break;
                            }
                        }
                        terminal.Write("Usage: " + data[0] + " [IP]\n");
                        break;
                    }
                case "dc":
                case "disconnect":
                    {
                        if (!player.PlayersComputer.IsPlayerConnected)
                        {
                            Programs.Connection.Disconnect();
                            terminal.Write("Disconnected\n");
                            break;
                        }
                        terminal.Write("Cannot disconnect from gateway\n");
                        break;
                    }
                case "ifconfig":
                    {
                        terminal.Write("IP: " + player.ConnectedComputer.IP + "\n");
                        break;
                    }
                case "conch":
                    {
                        terminal.Write(player.ConnectedComputer.IsPlayerConnected.ToString() + "\n");
                        break;
                    }
                case "touch":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                                player.ConnectedComputer.FileSystem.AddFile(data[1]);
                            break;
                        }
                        else
                        {
                            terminal.Write(noPriv);
                            break;
                        }
                    }
                case "mkdir":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                                player.ConnectedComputer.FileSystem.AddDir(data[1]);
                            break;
                        }
                        else
                            terminal.Write(noPriv);
                        break;
                    }
                case "pwd":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            terminal.Write(player.ConnectedComputer.FileSystem.CurrentDir.Name + "\n");
                            break;
                        }

                        else
                            terminal.Write(noPriv);
                        break;
                    }
                case "ls":
                case "dir":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            terminal.Write(player.ConnectedComputer.FileSystem.ListFiles());
                            break;
                        }
                        else
                            terminal.Write(noPriv);
                        break;
                    }
                case "cd":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                if (player.ConnectedComputer.FileSystem.TryFindFile(data[1], true))
                                {
                                    player.ConnectedComputer.FileSystem.ChangeDir(data[1]);
                                    break;
                                }
                                terminal.Write(data[0] + ": " + data[1] + ": no such file or directory\n");
                                break;
                            }
                            terminal.Write("Usage: " + data[0] + " [Directory]\n");
                            break;
                        }
                        else
                            terminal.Write( noPriv);
                        break;
                    }
                case "clear":
                    {
                        OS.OS.GetInstance().Terminal.Clear();
                        break;
                    }
                case "help":
                    {
                        terminal.Write(
                            "  cd [DIRECTORY]\n§" +
                            "    Changes directory\n§" +
                            "  rm [OPTION] [FILE]\n§" +
                            "    Removes a file or directory\n§" +
                            "  ls / dir\n§" +
                            "    Lists all files/folders in the current directory\n§" +
                            "  pwd\n§" +
                            "    Lists the current directory\n§" +
                            "  touch [NAME]\n§" +
                            "    Creates a file\n§" +
                            "  mkdir [NAME]\n§" +
                            "    Creates a directory\n§" +
                            "  connect [IP]\n§" +
                            "    Connects to another computer\n§" +
                            "  disconnect / dc\n§" +
                            "    Disconnects from computer\n§" +
                            "  ifconfig\n§" +
                            "    Shows the IP address of the computer\n§" +
                            "  sshnuke\n§" +
                            "    Gain root access on another system\n§" +
                            "  help\n§" +
                            "    Shows this help text\n§");
                        break;
                    }
                default:
                    terminal.Write(data[0] + ": command not found\n");
                    break;
            }
        }
    }
}
