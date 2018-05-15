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
        public static string ParseCommand(string command)
        {
            var data = command.Split();
            string noPriv = data[0] + ": Permission denied\n";
            switch (data[0])
            {
                case "sshnuke":
                    {
                        Player.GetInstance().ConnectedComputer.GetRoot();
                        Thread.Sleep(10);
                        Player.GetInstance().ConnectedComputer.Connect();
                        return "Connecting to " + Player.GetInstance().ConnectedComputer.IP + ":ssh ... successful.\n§" +
                            "Attempting to exploit SSHv1 CRC32 ... successful.\n§" +
                            "Resetting root password to \"passwd\".\n§" + 
                            "System open: Access level <9>\n";
                    }
                case "shutdown":
                case "reboot":
                case "exit":
                    {
                        return "I'm sorry Dave, I'm afraid I can't do that.\n";
                    }
                case "":
                    {
                        return "";
                    }
                case "echo":
                    {
                        if (data.Length > 1)
                        {
                            return command.Split('"')[1] + "\n";
                        }
                        return "\n";
                    }
                case "sudo":
                    {
                        if(data.Length > 1)
                        {
                            if (data[1] == "su")
                            {

                            }
                        }
                        return data[0] + ": username is not in the sudoers file. This incident will be reported.\n";
                    }
                case "rm":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                if (data[1] != null && Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Children.Count > 0 && data[1] != "*")
                                {
                                    if (data[1] == "-r" && data.Length > 2)
                                    {
                                        if (Player.GetInstance().ConnectedComputer.FileSystem.TryFindFile(data[2], true))
                                            Player.GetInstance().ConnectedComputer.FileSystem.RemoveFile(Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[2], true));
                                        else
                                            return data[0] + ": missing operand\n";
                                    }
                                    else if (Player.GetInstance().ConnectedComputer.FileSystem.TryFindFile(data[1], true))
                                    {
                                        return data[0] + ": cannot remove \'" + data[1] + "\': is a directory\n";
                                    }
                                    else if (Player.GetInstance().ConnectedComputer.FileSystem.TryFindFile(data[1], false))
                                    {
                                        Player.GetInstance().ConnectedComputer.FileSystem.RemoveFile(Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[1], false));
                                    }
                                    else
                                    {
                                        return data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory\n";
                                    }
                                    return "";
                                }
                                else
                                    return data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory\n";
                            }
                            return data[0] + ": missing operand\n";
                        }
                        else
                            return noPriv;
                    }
                case "man":
                    {
                        if (data.Length > 1)
                        {
                            return "No manual entry for " + data[1] + "\n";
                        }
                        return "What manual page do you want?\n";
                    }
                case "connect":
                    {
                        if(data.Length > 1)
                        {
                            if (Programs.Connection.Connect(data[1]))
                                return "Connected to " + data[1] + "\n";
                            else
                                return "Could not connect to " + data[1] + "\n";
                        }
                        return "Usage: " + data[0] + " [IP]\n";
                    }
                case "dc":
                case "disconnect":
                    {
                        if (!Player.GetInstance().PlayersComputer.IsPlayerConnected)
                        {
                            Programs.Connection.Disconnect();
                            return "Disconnected\n";
                        }
                        return "Cannot disconnect from gateway\n";
                    }
                case "ifconfig":
                    {
                        return "IP: " + Player.GetInstance().ConnectedComputer.IP + "\n";
                    }
                case "conch":
                    {
                        return Player.GetInstance().ConnectedComputer.IsPlayerConnected.ToString() + "\n";
                    }
                case "touch":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                                Player.GetInstance().ConnectedComputer.FileSystem.AddFile(data[1]);
                            return "";
                        }
                        else
                            return noPriv;
                    }
                case "mkdir":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                                Player.GetInstance().ConnectedComputer.FileSystem.AddDir(data[1]);
                            return "";
                        }
                        else
                            return noPriv;
                    }
                case "pwd":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            return Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Name + "\n";
                        }

                        else
                            return noPriv;
                    }
                case "ls":
                case "dir":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            return Player.GetInstance().ConnectedComputer.FileSystem.ListFiles();
                        }
                        else
                            return noPriv;
                    }
                case "cd":
                    {
                        if (Player.GetInstance().ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                if (Player.GetInstance().ConnectedComputer.FileSystem.TryFindFile(data[1], true))
                                {
                                    Player.GetInstance().ConnectedComputer.FileSystem.ChangeDir(data[1]);
                                    return "";
                                }
                                return data[0] + ": " + data[1] + ": no such file or directory\n";
                            }
                            return "Usage: " + data[0] + " [Directory]\n";
                        }
                        else
                            return noPriv;
                    }
                case "help":
                    {
                        return
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
                            "  help\n§" +
                            "    Shows this help text\n§";
                    }
                default:
                    return data[0] + ": command not found\n";
            }
        }
    }
}
