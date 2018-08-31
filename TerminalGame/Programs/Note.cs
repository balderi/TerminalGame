using System;

namespace TerminalGame.Programs
{
    class Note
    {
        static OS.OS os = OS.OS.GetInstance();
        static UI.Modules.Terminal terminal = os.Terminal;

        public static void Execute(string[] args, string text = null)
        {
            if (args.Length != 0)
            {
                if (args[0] == "-r" && args.Length > 1)
                {
                    if (args[1] == "*")
                    {
                        os.Notes.Clear();
                        terminal.Write("\nAll notes removed");
                        return;
                    }
                    if (Int32.TryParse(args[1], out int id))
                    {
                        if (os.Notes.RemoveNote(id))
                        {
                            terminal.Write("\nNote removed");
                            return;
                        }
                    }
                    terminal.Write("\nNo note with id \'" + args[1] + "\'");
                    return;
                }
                else if (args[0] == "-r" && args.Length < 2)
                {
                    terminal.Write("\nnote: missing operand");
                    return;
                }
                else
                {
                    if(!String.IsNullOrEmpty(text))
                    {
                        if (os.Notes.AddNote(text))
                            terminal.Write("\nNote added");
                        else
                            terminal.Write("\nNote already exists.");
                    }
                    else
                    {
                        if (os.Notes.AddNote(args[0]))
                            terminal.Write("\nNote added");
                        else
                            terminal.Write("\nNote already exists.");
                    }
                    return;
                }
            }
            terminal.Write("\nUsage: note [OPTIONS] [NOTE OR NOTE ID]");
        }
    }
}
