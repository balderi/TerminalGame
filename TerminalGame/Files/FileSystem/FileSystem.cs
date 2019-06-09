using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Files.FileSystem
{
    public class FileSystem
    {
        public File RootDir { get; private set; }
        public File CurrentDir { get; private set; }

        public FileSystem(File root)
        {
            if (root.FileType is FileType.Directory)
            {
                RootDir = root;
                CurrentDir = RootDir;
            }
            else throw new ArgumentException("Root file has to be a directory.");
        }

        public bool TryFindFile(string name, out List<File> result)
        {
            List<File> retval = new List<File>();
            result = retval;
            return true;
        }

        public bool TryFindFilePath(string name, out string path)
        {
            bool getPath(File file, out string pPath)
            {
                if(file.Name == name)
                {
                    pPath = file.Name;
                    return true;
                }
                if(file.FileType == FileType.Directory)
                {
                    foreach(var f in file.Children)
                    {
                        if (getPath(f, out string tempPath))
                        {
                            pPath += "\\" + tempPath;
                            return true;
                        }
                    }
                }
                pPath = "";
                return false;
            }
            if(getPath(RootDir, out string outPath))
            {
                path = outPath;
                return true;
            }
            path = "";
            return false;
        }

        public void ChangeCurrentDir(File directory)
        {
            if (directory.FileType == FileType.Directory)
                CurrentDir = directory;
            else throw new ArgumentException("File is not a directory.");
        }

        public override string ToString()
        {
            // TODO: something...?
            return base.ToString();
        }
    }
}
