using System;
using System.Text.RegularExpressions;
using TerminalGame.Programs;
using TerminalGame.Utils;

namespace TerminalGame.Parsing
{
    public static class CommandParser
    {
        public static CommandToken Tokenize(string command)
        {
            var splitCmd = command.Split(' ');
            var cmd = splitCmd[0];
            string[] args;
            if (splitCmd.Length > 1)
            {
                args = new string[splitCmd.Length - 1];
                Array.ConstrainedCopy(splitCmd, 1, args, 0, args.Length);
            }
            else
                args = new string[0];
            return new CommandToken { Command = cmd, Args = args };
        }

        public static CommandToken TokenizeTextArgs(string command)
        {
            var cmd = command.Split(' ')[0];
            string[] args = new string[] { Regex.Match(command.Replace(cmd + " ", ""), "\"[^\"]+\"").Value.Replace("\"", "") };
            return new CommandToken { Command = cmd, Args = args };
        }

        public static bool TryTokenize(string command, bool textArg, out CommandToken token)
        {
            if (textArg)
            {
                if(!command.Contains("\""))
                {
                    token = new CommandToken { Command = "error", Args = new string[0] };
                    return false;
                }
                token = TokenizeTextArgs(command);
                return true;
            }
            token = Tokenize(command);
            return true;
        }

        public static void Parse(CommandToken token, TerminalGame game)
        {
            Console.WriteLine("Command: {0}", token.Command);
            switch (token.Command)
            {
                case "":
                    {
                        game.Terminal.Write("");
                        break;
                    }
                case "echo":
                    {
                        Echo.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "sudo":
                    {
                        game.Terminal.WriteLine("user is not in the sudoers file.  This incidient will be reported.");
                        break;
                    }
                case "man":
                    {
                        break;
                    }
                case "ip":
                    {
                        Ip.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "ifconfig":
                case "ipconfig":
                    {
                        Ifconfig.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "pwd":
                    {
                        break;
                    }
                case "cls":
                case "clear":
                    {
                        break;
                    }
                case "help":
                    {
                        break;
                    }
                case "reboot":
                    {
                        break;
                    }
                case "shutdown":
                    {
                        break;
                    }
                case "quit":
                case "exit":
                    {
                        break;
                    }
                case "login":
                    {
                        break;
                    }
                case "disconnect":
                case "dc":
                    {
                        Disconnect.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "connect":
                    {
                        Connect.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "rm":
                    {
                        break;
                    }
                case "rmdir":
                    {
                        break;
                    }
                case "mkdir":
                    {
                        break;
                    }
                case "touch":
                    {
                        break;
                    }
                case "cat":
                    {
                        Cat.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "ls":
                case "dir":
                    {
                        Ls.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "cd":
                    {
                        Cd.GetInstance().Init(game, null, token.Args);
                        break;
                    }
                case "save":
                    {
                        World.World.GetInstance().Save();
                        game.Terminal.WriteLine("Game saved.");
                        break;
                    }
                default:
                    {
                        game.Terminal.WriteLine(token.Command + " is not a recognized command");
                        break;
                    }
            }
        }
    }
}
