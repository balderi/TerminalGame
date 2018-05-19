namespace TerminalGame.Programs
{
    class Help
    {
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;

        public static void Execute()
        {
            terminal.Write("\n  cd [DIRECTORY]§" +
                            "\n    Change directory§" +
                            "\n  rm [OPTIONS] [FILE]§" +
                            "\n    Remove a file or directory§" +
                            "\n  ls / dir§" +
                            "\n    List all files/folders in the current directory§" +
                            "\n  pwd§" +
                            "\n    List the current directory§" +
                            "\n  touch [NAME]§" +
                            "\n    Create a file§" +
                            "\n  mkdir [NAME]§" +
                            "\n    Create a directory§" +
                            "\n  connect [IP]§" +
                            "\n    Connect to another computer§" +
                            "\n  disconnect / dc§" +
                            "\n    Disconnect from computer§" +
                            "\n  ifconfig§" +
                            "\n    Show the IP address of the computer§" +
                            "\n  clear / cls§" +
                            "\n    Clear the terminal§" +
                            "\n  sshnuke§" +
                            "\n    Gain root access on another system§" +
                            "\n  note [OPTIONS] [TEXT OR NOTE ID]§" +
                            "\n    Add or remove note§" +
                            "\n  help§" +
                            "\n    Shows this help text§");
        }
    }
}
