using System;
using System.Threading;
using System.Threading.Tasks;
using TerminalGame.UI.Themes;

namespace TerminalGame.Utilities
{
    // TODO: Make singleton rather than static class (maybe?)

    /// <summary>
    /// Command Parser
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="input">The command string to parse</param>
        /// <returns>Deprecated: No longer returns anything (blank string)</returns>
        public static string ParseCommand(string input)
        {
            Player _player = Player.GetInstance();
            OS.OS _os = OS.OS.GetInstance();
            UI.Modules.Terminal _terminal = _os.Terminal;

            var _data = input.Trim(' ').Split();

            var _command = _data[0];

            string[] _args = new string[_data.Length - 1];
            Array.Copy(_data, 1, _args, 0, _data.Length - 1);

            // Prevent arg being null
            if (_args.Length == 0)
                _args = new string[] { "" };

            string noPriv = "\n" + _command + ": Permission denied";

            switch (_command)
            {
                case "theme":
                    {
                        if (_args[0].Length == 0)
                            _terminal.Write("\n" + ThemeManager.GetInstance().CurrentTheme.ThemeName);
                        else
                            ThemeManager.GetInstance().ChangeTheme(_args[0]);
                        break;
                    }
                case "note":
                    {
                        if(_args.Length != 0 && input.Contains("\""))
                            Programs.Note.Execute(_args, input.Split('"')[1]);
                        else
                            Programs.Note.Execute(_args);
                        break;
                    }
                case "login":
                    {
                        _terminal.BeginLogin();
                        break;
                    }
                case "sshnuke":
                    {
                        Task<int> task = Task.Factory.StartNew(() => Programs.SSHNuke.Execute());
                        Console.WriteLine("Starting new task for sshnuke");
                        break;
                    }
                case "shutdown":
                    {
                        var task = Task.Factory.StartNew(() => Programs.Placeholder.Execute());
                        Console.WriteLine("Starting new task for shutdown");
                        break;
                    }
                case "reboot":
                    {
                        var task = Task.Factory.StartNew(() => Programs.Placeholder.Execute());
                        Console.WriteLine("Starting new task for reboot");
                        break;
                    }
                case "":
                    {
                        return "";
                    }
                case "echo":
                    {
                        if (_data.Length > 1 && input.Contains("\""))
                        {
                            string output = input.Split('"')[1];
                            _terminal.Write("\n" + output);
                            return "\n" + output;
                        }
                        return "\n";
                    }
                case "sudo":
                    {
                        if(_data.Length > 1)
                        {
                            if (_args[0] == "su")
                            {

                            }
                        }
                        return "\n" + _command + ": username is not in the sudoers file. This incident will be reported.";
                    }
                case "rm":
                    {
                        if (_player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (_data.Length > 1)
                            {
                                if (_args[0] != null && Player.GetInstance().ConnectedComputer.FileSystem.CurrentDir.Children.Count > 0 && _args[0] != "*")
                                {
                                    if (_args[0] == "-r" && _data.Length > 2)
                                    {
                                        if (_player.ConnectedComputer.FileSystem.TryFindFile(_args[1], true))
                                        {
                                            _player.ConnectedComputer.FileSystem.RemoveFile(_player.ConnectedComputer.FileSystem.FindFile(_args[1], true));
                                        }
                                        else
                                        {
                                            return "\n" + _command + ": missing operand";
                                        }
                                        break;
                                    }
                                    else if (_player.ConnectedComputer.FileSystem.TryFindFile(_args[0], true))
                                    {
                                        return "\n" + _command + ": cannot remove \'" + _args[0] + "\': is a directory";
                                    }
                                    else if (_player.ConnectedComputer.FileSystem.TryFindFile(_args[0], false))
                                    {
                                        _player.ConnectedComputer.FileSystem.RemoveFile(_player.ConnectedComputer.FileSystem.FindFile(_args[0], false));
                                        break;
                                    }
                                    else
                                    {
                                        return "\n" + _command + ": cannot remove \'" + _args[0] + "\': no such file or directory";
                                    }
                                }
                                else
                                {
                                    return "\n" + _command + ": cannot remove \'" + _args[0] + "\': no such file or directory";
                                }
                            }
                            return "\n" + _command + ": missing operand";
                        }
                        else
                        {
                            return noPriv;
                        }
                    }
                case "man":
                    {
                        if (_data.Length > 1)
                        {
                            return "\nNo manual entry for " + _args[0];
                        }
                        return "\nWhat manual page do you want?";
                    }
                case "connect":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Connect.Execute(_args[0]));
                        Console.WriteLine("Starting new task for connect");
                        break;
                    }
                case "dc":
                case "disconnect":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Disconnect.Execute());
                        Console.WriteLine("Starting new task for disconnect");
                        break;
                    }
                case "ipconfig":
                case "ifconfig":
                    {
                        _terminal.Write("\n" + _player.ConnectedComputer.IP);
                        break;
                        //return "\nIP: " + player.ConnectedComputer.IP;
                    }
                case "touch":
                    {
                        if (_player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (_data.Length > 2 && _args[1].Contains("\""))
                            {
                                _player.ConnectedComputer.FileSystem.AddFile(_args[0], input.Split('"')[1]);
                            }
                            else if (_data.Length > 1)
                            {
                                _player.ConnectedComputer.FileSystem.AddFile(_args[0]);
                            }
                            _player.ConnectedComputer.GenerateLog(_player.PlayersComputer, "created file", _player.ConnectedComputer.FileSystem.FindFile(_args[0], false));
                            return "";
                        }
                        else
                        {
                            return noPriv;
                        }
                    }
                case "mkdir":
                    {
                        if (_player.ConnectedComputer.PlayerHasRoot)
                        {
                            if (_data.Length > 1)
                            {
                                _player.ConnectedComputer.FileSystem.AddDir(_args[0]);
                            }
                            return "";
                        }
                        else
                            return noPriv;
                    }
                case "pwd":
                    {
                        if (_player.ConnectedComputer.PlayerHasRoot)
                        {
                            return "\n" + _player.ConnectedComputer.FileSystem.CurrentDir.PrintFullPath();
                        }

                        else
                            return noPriv;
                    }
                case "ls":
                case "dir":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Ls.Execute(_command));
                        Console.WriteLine("Starting new task for ls/dir");
                        break;
                    }
                case "cat":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Cat.Execute(_args[0]));
                        Console.WriteLine("Starting new task for cat");
                        break;
                    }
                case "cd":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Cd.Execute(_args[0]));
                        Console.WriteLine("Starting new task for cd");
                        break;
                    }
                case "nmap":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Nmap.Execute(_args[0]));
                        Console.WriteLine("Starting new task for nmap");
                        break;
                    }
                case "cls":
                case "clear":
                    {
                        _os.Terminal.Clear();
                        return "";
                    }
                case "help":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Help.Execute());
                        Console.WriteLine("Starting new task for help");
                        break;
                    }
                case "quit":
                case "exit":
                    {
                        Task task = Task.Factory.StartNew(() => Programs.Placeholder.Execute());
                        //_terminal.ForceQuit();
                        break;
                    }
                default:
                    {
                        _terminal.Write("\n" + _command + ": command not found");
                        return "\n" + _command + ": command not found";
                    }
            }
            return "";
        }
    }
}
