using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TerminalGame.Parsing
{
    public class CommandParser
    {
        public CommandToken Tokenize(string command)
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

        public CommandToken TokenizeTextArgs(string command)
        {
            var cmd = command.Split(' ')[0];
            string[] args = Regex.Split(command, "\"[a-zA-Z0-9 \\-_]+\"");
            return new CommandToken { Command = cmd, Args = args };
        }

        public bool TryTokenize(string command, bool textArg, out CommandToken token)
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

        public void Parse(CommandToken token)
        { 
            switch (token.Command)
            {
                case "echo":
                    {
                        break;
                    }
                case "sudo":
                    {
                        break;
                    }
                case "man":
                    {
                        break;
                    }
                case "ip":
                    {
                        break;
                    }
                case "ifconfig":
                case "ipconfig":
                    {
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
                        break;
                    }
                case "connect":
                    {
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
                        break;
                    }
                case "ls":
                case "dir":
                    {
                        break;
                    }
                case "cd":
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
