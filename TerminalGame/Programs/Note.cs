using System;

namespace TerminalGame.Programs
{
    class Note
    {
        private static OS.OS _os = OS.OS.GetInstance();
        private static UI.Modules.Terminal _terminal = _os.Terminal;

        public static void Execute(string[] args, string text = null)
        {
            if (args.Length != 0)
            {
                if (args[0] == "-r" && args.Length > 1)
                {
                    if (args[1] == "*")
                    {
                        _os.Notes.Clear();
                        _terminal.Write("\nAll notes removed");
                        return;
                    }
                    if (Int32.TryParse(args[1], out int id))
                    {
                        if (_os.Notes.RemoveNote(id))
                        {
                            _terminal.Write("\nNote removed");
                            return;
                        }
                    }
                    _terminal.Write("\nNo note with id \'" + args[1] + "\'");
                    return;
                }
                else if (args[0] == "-r" && args.Length < 2)
                {
                    _terminal.Write("\nnote: missing operand");
                    return;
                }
                else
                {
                    if(!String.IsNullOrEmpty(text))
                    {
                        if (_os.Notes.AddNote(text))
                            _terminal.Write("\nNote added");
                        else
                            _terminal.Write("\nNote already exists.");
                    }
                    else
                    {
                        if (_os.Notes.AddNote(args[0]))
                            _terminal.Write("\nNote added");
                        else
                            _terminal.Write("\nNote already exists.");
                    }
                    return;
                }
            }
            _terminal.Write("\nUsage: note [OPTIONS] [NOTE OR NOTE ID]");
        }
    }
}
