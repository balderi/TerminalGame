using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Files.FileSystem
{
    public class FileSystem
    {
        public File RootDir { get; set; }
        public File CurrentDir { get; set; }
        public File LastDir { get; set; }

        public FileSystem()
        {

        }

        public FileSystem(File root)
        {
            if (root.FileType is FileType.Directory)
            {
                RootDir = root;
                CurrentDir = RootDir;
            }
            else throw new ArgumentException("Root file has to be a directory.");
        }

        public bool TryFindFile(string name, out File result)
        {
            if (name == ".")
            {
                result = CurrentDir;
                return true;
            }
            if(name == "..")
            {
                if (CurrentDir.Parent == null) // we're at root (/), nowhere else to go
                    result = CurrentDir;
                else
                    result = CurrentDir.Parent;
                return true;
            }

            result = CurrentDir.Children.Find(f => f.Name == name);
            return result != null;
        }

        public bool TryFindFileFromPath(string filePath, out string path, out string file)
        {
            // E.g. cat some/dir/with/file, where file is a valid file

            path = "";
            file = "";
            return false;
        }

        public bool TryFindFilePath(string name, out string path)
        {
            bool getPath(File file, out string pPath)
            {
                pPath = "";
                if (file.Name == name)
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
                            pPath += file.Name + "/" + tempPath;
                            return true;
                        }
                    }
                }
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

        public void ChangeCurrentDirFromPath(string path)
        {
            var splitPath = path.Split('/');
            var currentDir = CurrentDir;
            var lastDir = LastDir;

            foreach(var dir in splitPath)
            {
                if(TryFindFile(dir, out File directory))
                {
                    if(directory.FileType == FileType.Directory)
                    {
                        ChangeCurrentDir(directory);
                    }
                    else
                    {
                        CurrentDir = currentDir;
                        throw new ArgumentException("Invalid path.");
                    }
                }
            }
            LastDir = currentDir;
        }

        public void ChangeCurrentDir(File directory)
        {
            if (directory == null)
                return;
            if (directory.FileType == FileType.Directory)
            {
                LastDir = CurrentDir;
                CurrentDir = directory;
            }
            else throw new ArgumentException($"{directory} is not a directory.");
        }

        public override string ToString()
        {
            // TODO: something...?
            return base.ToString();
        }
    }
}
