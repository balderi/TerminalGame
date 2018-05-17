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
            string noPriv = "\n" + data[0] + ": Permission denied";
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
                                    terminal.Write("\nAll notes removed");
                                    break;
                                }
                                if(Int32.TryParse(data[2], out int id))
                                {
                                    if(os.Notes.RemoveNote(id))
                                    {
                                        terminal.Write("\nNote removed");
                                        break;
                                    }
                                }
                                terminal.Write("\nNo note with id \'" + data[2] + "\'");
                                break;
                            }
                            else if (data[1] == "-r" && data.Length < 3)
                            {
                                terminal.Write("\n" + data[0] + ": missing operand");
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
                                terminal.Write("\nNote added");
                                break;
                            }
                        }
                        terminal.Write("\nUsage: " + data[0] + " [OPTIONS] [NOTE OR NOTE ID]");
                        break;
                    }
                case "sshnuke":
                    {
                        OS.Programs.GetInstance().sshnuke();
                        break;
                    }
                case "shutdown":
                case "reboot":
                case "exit":
                    {
                        terminal.Write("\nI'm sorry Dave, I'm afraid I can't do that.");
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
                            terminal.Write("\n" + command.Split('"')[1]);
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
                        terminal.Write("\n" + data[0] + ": username is not in the sudoers file. This incident will be reported.");
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
                                            terminal.Write("\n" + data[0] + ": missing operand");
                                        }
                                        break;
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(data[1], true))
                                    {
                                        terminal.Write("\n" + data[0] + ": cannot remove \'" + data[1] + "\': is a directory");
                                        break;
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(data[1], false))
                                    {
                                        player.ConnectedComputer.FileSystem.RemoveFile(player.ConnectedComputer.FileSystem.FindFile(data[1], false));
                                        break;
                                    }
                                    else
                                    {
                                        terminal.Write("\n" + data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory");
                                        break;
                                    }
                                }
                                else
                                {
                                    terminal.Write("\n" + data[0] + ": cannot remove \'" + data[1] + "\': no such file or directory");
                                    break;
                                }
                            }
                            terminal.Write("\n" + data[0] + ": missing operand");
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
                            terminal.Write("\nNo manual entry for " + data[1]);
                            break;
                        }
                        terminal.Write("\nWhat manual page do you want?");
                        break;
                    }
                case "connect":
                    {
                        if(data.Length > 1)
                        {
                            terminal.Write("\nConnecting to " + data[1] + "...");
                            if (Programs.Connection.Connect(data[1]))
                            {
                                terminal.Write("\nConnected to " + data[1]);
                                break;
                            }
                            else
                            {
                                terminal.Write("\nCould not connect to " + data[1]);
                                break;
                            }
                        }
                        terminal.Write("\nUsage: " + data[0] + " [IP]");
                        break;
                    }
                case "dc":
                case "disconnect":
                    {
                        if (!player.PlayersComputer.IsPlayerConnected)
                        {
                            Programs.Connection.Disconnect();
                            terminal.Write("\nDisconnected");
                            break;
                        }
                        terminal.Write("\nCannot disconnect from gateway");
                        break;
                    }
                case "ifconfig":
                    {
                        terminal.Write("\nIP: " + player.ConnectedComputer.IP);
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
                            terminal.Write("\n" + player.ConnectedComputer.FileSystem.CurrentDir.PrintFullPath());
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
                                terminal.Write("\n" + data[0] + ": " + data[1] + ": no such file or directory");
                                break;
                            }
                            terminal.Write("\nUsage: " + data[0] + " [DIRECTORY]");
                            break;
                        }
                        else
                            terminal.Write( noPriv);
                        break;
                    }
                case "cls":
                case "clear":
                    {
                        OS.OS.GetInstance().Terminal.Clear();
                        break;
                    }
                case "help":
                    {
                        terminal.Write(
                            "\n  cd [DIRECTORY]§" +
                            "\n    Changes directory§" +
                            "\n  rm [OPTIONS] [FILE]§" +
                            "\n    Removes a file or directory§" +
                            "\n  ls / dir§" +
                            "\n    Lists all files/folders in the current directory§" +
                            "\n  pwd§" +
                            "\n    Lists the current directory§" +
                            "\n  touch [NAME]§" +
                            "\n    Creates a file§" +
                            "\n  mkdir [NAME]§" +
                            "\n    Creates a directory§" +
                            "\n  connect [IP]§" +
                            "\n    Connects to another computer§" +
                            "\n  disconnect / dc§" +
                            "\n    Disconnects from computer§" +
                            "\n  ifconfig§" +
                            "\n    Shows the IP address of the computer§" +
                            "\n  clear / cls§" +
                            "\n    Clears the terminal§" +
                            "\n  sshnuke§" +
                            "\n    Gain root access on another system§" +
                            "\n  note [OPTIONS] [TEXT OR NOTE ID]§" +
                            "\n    Add or remove note§" +
                            "\n  help§" +
                            "\n    Shows this help text§");
                        break;
                    }
                default:
                    terminal.Write("\n" + data[0] + ": command not found");
                    break;
            }
        }
    }
}
