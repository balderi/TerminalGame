using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Files.FileSystem.Generator
{
    public static class FileSystemGenerator
    {
        public static FileSystem GenerateDefaultFilesystem()
        {
            File root = new File("");
            root.AddFile(new File("home"));
            root.AddFile(new File("boot"));
            root.AddFile(new File("var"));
            root.AddFile(new File("bin"));
            root.AddFile(new File("etc"));
            root.AddFile(new File("usr"));

            return new FileSystem(root);
        }
    }
}
