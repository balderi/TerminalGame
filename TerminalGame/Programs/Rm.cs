using System;

namespace TerminalGame.Programs
{
    class Rm : Program
    {
        static Rm _instance;

        public static Rm GetInstance()
        {
            if (_instance == null)
                _instance = new Rm();
            return _instance;
        }

        private Rm()
        {

        }

        protected override void Run()
        {
            var comp = World.World.GetInstance().Player.ConnectedComp;

            if (_args.Length < 1)
            {
                Game.Terminal.WriteLine("rm: missing operand");
                Game.Terminal.WriteLine("Try 'rm --help' for more information.");
                Kill();
                return;
            }
            if (_args.Length > 2)
            {
                Game.Terminal.WriteLine("rm: too many operands");
                Game.Terminal.WriteLine("Try 'rm --help' for more information.");
                Kill();
                return;
            }
            if (_args[0] == "--help")
            {
                Game.Terminal.WriteLine("Usage: rm [OPTION]... [FILE]...");
                Game.Terminal.WriteLine("Remove (unlink) the FILE.");
                Game.Terminal.WriteLine("");
                Game.Terminal.WriteLine("  -r, --recursive    remove directories and their contents recursively");
                Game.Terminal.WriteLine("      --help         display this help and exit");
                Game.Terminal.WriteLine("");
                Game.Terminal.WriteLine("By default, rm does not remove directories.  Use the --recursive (-r)");
                Game.Terminal.WriteLine("option to remove each listed directory, too, along with all of its contents.");
                Kill();
                return;
            }
            if (_args[0] == "-r" || _args[0] == "--recursive")
            {
                if (comp.FileSystem.TryFindFile(_args[1], out var file))
                {
                    if (!comp.PlayerHasRoot)
                    {
                        Game.Terminal.WriteLine($"rm: cannot remove '{_args[1]}': Permission denied");
                        Kill();
                        return;
                    }
                    if (file == comp.FileSystem.CurrentDir && comp.FileSystem.CurrentDir != comp.FileSystem.RootDir)
                        comp.FileSystem.ChangeCurrentDir(file.Parent);
                    if (file == comp.FileSystem.RootDir)
                        comp.FileSystem.RootDir = null;
                    file.Purge();
                    Kill();
                    return;
                }
                else
                {
                    Game.Terminal.WriteLine($"rm: cannot remove '{_args[1]}': No such file or directory");
                    Kill();
                    return;
                }

            }
            else
            {
                if (comp.FileSystem.TryFindFile(_args[0], out var file))
                {
                    if (!comp.PlayerHasRoot)
                    {
                        Game.Terminal.WriteLine($"rm: cannot remove '{_args[0]}': Permission denied");
                        Kill();
                        return;
                    }
                    if (file.FileType == Files.FileType.Directory)
                    {
                        Game.Terminal.WriteLine($"rm: cannot remove '{_args[0]}': Is a directory");
                        Kill();
                        return;
                    }
                    file.Purge();
                    Kill();
                    return;
                }
                else
                {
                    Game.Terminal.WriteLine($"rm: cannot remove '{_args[0]}': No such file or directory");
                    Kill();
                    return;
                }
            }
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {

        }
    }
}
