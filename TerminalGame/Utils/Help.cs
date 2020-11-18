namespace TerminalGame.Utils
{
    public class Help
    {
        public static string GetHelp()
        {
            var retval = "TerminalGame Help\n" +
                " System commands:\n" +
                "  ls :  lists all files in the current directory\n" +
                "  cd <dir> :  change directory\n" +
                "  cat <file> :  display a file's contents\n" +
                "  touch <name> :  create a file\n" +
                "  mkdir <name> :  create a directory\n" +
                "  rm <opt> <file> :  remove a file\n" +
                "  connect <ip> :  connect to a remote system\n" +
                "  disconnect :  disconnect from a remote system\n" +
                "  dc :  synonym for <disconnect>\n" +
                " Tools:\n" +
                "  nmap <ip (optional)> :  scan a system for open ports\n" +
                "  sshnuke :  exploit ssh to gain elevated privileges\n" +
                " Game commands:\n" +
                "  save :  save the game";
            return retval;
        }
    }
}
