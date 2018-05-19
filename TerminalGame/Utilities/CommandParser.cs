using System;

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
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ParseCommand(string input)
        {
            Player player = Player.GetInstance();
            OS.OS os = OS.OS.GetInstance();
            UI.Modules.Terminal terminal = os.Terminal;
            OS.Programs programs = OS.Programs.GetInstance();

            var data = input.Trim(' ').Split();

            var command = data[0];

            string[] args = new string[data.Length - 1];
            Array.Copy(data, 1, args, 0, data.Length - 1);

            string noPriv = "\n" + command + ": Permission denied";

            switch (command)
            {
                case "note":
                    {
                        if(args.Length != 0 && input.Contains("\""))
                            Programs.Note.Execute(args, input.Split('"')[1]);
                        else
                            Programs.Note.Execute(args);
                        break;
                    }
                case "sshnuke":
                    {
                        Programs.SSHNuke.Execute();
                        break;
                    }
                case "shutdown":
                    {
                        return "\nI'm sorry Dave, I'm afraid I can't do that.";
                    }
                case "reboot":
                    {
                        return "\nI'm sorry Dave, I'm afraid I can't do that.";
                    }
                case "exit":
                    {
                        return "\nI'm sorry Dave, I'm afraid I can't do that.";
                    }
                case "":
                    {
                        return "";
                    }
                case "echo":
                    {
                        if (data.Length > 1 && input.Contains("\""))
                        {
                            string output = input.Split('"')[1];
                            programs.echo(output);
                            return "\n" + output;
                        }
                        return "\n";
                    }
                case "sudo":
                    {
                        if(data.Length > 1)
                        {
                            if (args[0] == "su")
                            {

                            }
                        }
                        return "\n" + command + ": username is not in the sudoers file. This incident will be reported.";
                    }
                case "rm":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                if (args[0] != null && Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Children.Count > 0 && args[0] != "*")
                                {
                                    if (args[0] == "-r" && data.Length > 2)
                                    {
                                        if (player.ConnectedComputer.FileSystem.TryFindFile(args[1], true))
                                        {
                                            player.ConnectedComputer.FileSystem.RemoveFile(player.ConnectedComputer.FileSystem.FindFile(args[1], true));
                                        }
                                        else
                                        {
                                            return "\n" + command + ": missing operand";
                                        }
                                        break;
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(args[0], true))
                                    {
                                        return "\n" + command + ": cannot remove \'" + args[0] + "\': is a directory";
                                    }
                                    else if (player.ConnectedComputer.FileSystem.TryFindFile(args[0], false))
                                    {
                                        player.ConnectedComputer.FileSystem.RemoveFile(player.ConnectedComputer.FileSystem.FindFile(args[0], false));
                                        break;
                                    }
                                    else
                                    {
                                        return "\n" + command + ": cannot remove \'" + args[0] + "\': no such file or directory";
                                    }
                                }
                                else
                                {
                                    return "\n" + command + ": cannot remove \'" + args[0] + "\': no such file or directory";
                                }
                            }
                            return "\n" + command + ": missing operand";
                        }
                        else
                        {
                            return noPriv;
                        }
                    }
                case "man":
                    {
                        if (data.Length > 1)
                        {
                            return "\nNo manual entry for " + args[0];
                        }
                        return "\nWhat manual page do you want?";
                    }
                case "connect":
                    {
                        if (args.Length != 0)
                            Programs.Connect.Execute(args[0]);
                        else
                            Programs.Connect.Execute();
                        break;
                    }
                case "dc":
                case "disconnect":
                    {
                        Programs.Disconnect.Execute();
                        break;
                    }
                case "ipconfig":
                case "ifconfig":
                    {
                        return "\nIP: " + player.ConnectedComputer.IP;
                    }
                case "touch":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 2 && args[1].Contains("\""))
                            {
                                player.ConnectedComputer.FileSystem.AddFile(args[0], input.Split('"')[1]);
                            }
                            else if (data.Length > 1)
                            {
                                player.ConnectedComputer.FileSystem.AddFile(args[0]);
                            }
                            player.ConnectedComputer.GenerateLog(player.PlayersComputer, "created file", player.ConnectedComputer.FileSystem.FindFile(args[0], false));
                            return "";
                        }
                        else
                        {
                            return noPriv;
                        }
                    }
                case "mkdir":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (data.Length > 1)
                            {
                                player.ConnectedComputer.FileSystem.AddDir(args[0]);
                            }
                            return "";
                        }
                        else
                            return noPriv;
                    }
                case "pwd":
                    {
                        if (player.ConnectedComputer.PlayerHasRoot)
                        {
                            return "\n" + player.ConnectedComputer.FileSystem.CurrentDir.PrintFullPath();
                        }

                        else
                            return noPriv;
                    }
                case "ls":
                case "dir":
                    {
                        Programs.Ls.Execute(command);
                        break;
                    }
                case "cat":
                    {
                        if(args.Length != 0)
                            Programs.Cat.Execute(args[0]);
                        else
                            Programs.Cat.Execute();
                        break;
                    }
                case "cd":
                    {
                        if (args.Length != 0)
                            Programs.Cd.Execute(args[0]);
                        else
                            Programs.Cd.Execute();
                        break;
                    }
                case "cls":
                case "clear":
                    {
                        os.Terminal.Clear();
                        return "";
                    }
                case "help":
                    {
                        Programs.Help.Execute();
                        break;
                    }
                default:
                    {
                        terminal.Write("\n" + command + ": command not found");
                        return "\n" + command + ": command not found";
                    }
            }
            return "";
        }
    }
}
