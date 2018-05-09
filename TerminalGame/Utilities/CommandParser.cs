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
            switch(data[0])
            {
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
                        if(data[1] == "su")
                        {

                        }
                        return data[0] + ": username is not in the sudoers file. This incident will be reported.\n";
                    }
                case "rm":
                    {
                        if (data.Length > 1)
                        {
                            if (Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[1]) != null || Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[2]) != null)
                            {
                                if(data[1] == "-r" && Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[2]) != null)
                                {
                                    Player.GetInstance().ConnectedComputer.FileSystem.RemoveFile(Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[2]));
                                }
                                else if(!Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[1]).IsDirectory)
                                {
                                    Player.GetInstance().ConnectedComputer.FileSystem.RemoveFile(Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[1]));
                                }
                                else
                                {
                                    return data[0] + ": cannot remove \'" + data[1] + "\': is a directory\n";
                                }
                                return "";
                            }
                            else
                                return data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory\n";
                        }
                        return data[0] + ": missing operand\n";
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
                            if (Programs.Connect.connect(data[1]))
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
                            Programs.Connect.disconnect();
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
                        if (data.Length > 1)
                            Player.GetInstance().ConnectedComputer.FileSystem.AddFile(data[1]);
                        return "";
                    }
                case "mkdir":
                    {
                        if (data.Length > 1)
                            Player.GetInstance().ConnectedComputer.FileSystem.AddDir(data[1]);
                        return "";
                    }
                case "pwd":
                    {
                        return Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Name + "\n";
                    }
                case "ls":
                case "dir":
                    {
                        return Player.GetInstance().ConnectedComputer.FileSystem.ListFiles();
                    }
                case "cd":
                    {
                        if (data.Length > 1)
                        {
                            if (Player.GetInstance().ConnectedComputer.FileSystem.FindFile(data[1]) != null)
                            {
                                Player.GetInstance().ConnectedComputer.FileSystem.ChangeDir(data[1]);
                                return "";
                            }
                            return data[0] + ": " + data[1] + ": no such file or directory\n";
                        }
                        return "Usage: " + data[0] + " [Directory]\n";
                    }
                default:
                    return data[0] + ": command not found\n";
            }
        }
    }
}
