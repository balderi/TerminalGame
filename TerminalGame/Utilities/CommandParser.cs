using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    public static class CommandParser
    {
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
                case "ls":
                    {
                        return "";
                    }
                case "cat":
                case "cd":
                    {
                        if(data.Length > 1)
                        {
                            return data[0] + ": " + data[1] + ": no such file or directory\n";
                        }
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
                        return data[0] + ": username is not in the sudoers file. This incident will be reported.\n";
                    }
                case "rm":
                    {
                        if (data.Length > 1)
                        {
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
                default:
                    return data[0] + ": command not found\n";
            }
        }
    }
}
