using System;
using System.Collections.Generic;

namespace TerminalGame.Computers.FileSystems
{
    class FileSystem
    {
        public event EventHandler ChangeDirirectory;
        public List<File> Children;
        public File CurrentDir { get; private set; }

        public FileSystem()
        {
            File root = new File("/");
            root.SetParent(root);
            CurrentDir = root;
            Children = new List<File>() { root };
        }

        public void BuildBasicFileSystem()
        {
            string[] baseDirs = { "bin", "usr", "home", "sys", "logs" };
            for (int i = 0; i < baseDirs.Length; i++)
            {
                AddDir(baseDirs[i]);
            }
        }

        private File FindRoot(File sourceDir)
        {
            while (CurrentDir.Parent.Parent != CurrentDir.Parent)
                FindRoot(CurrentDir.Parent);
            return sourceDir.Parent;
        }

        public File FindFile(string name, bool isDir, bool fromRoot = false)
        {
            if (name == "..")
                return CurrentDir.Parent;

            if (name == "/")
                return FindRoot(CurrentDir);

            bool findFile(File f)
            {
                if(isDir)
                    return f.Name == name && f.IsDirectory;
                return f.Name == name;
            }
            if (fromRoot)
            {
                return FindRoot(CurrentDir).Children.Find(findFile);
            }
            else
            {
                return CurrentDir.Children.Find(findFile);
            }
        }

        public bool TryFindFile(string name, bool isDir, bool fromRoot = false)
        {
            if (name == "..")
                return true;

            if (name == "/")
                return true;

            if (fromRoot)
            {
                foreach (File f in FindRoot(CurrentDir).Children)
                {
                    if (isDir)
                    {
                        if (f.Name == name && f.IsDirectory)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (f.Name == name && !f.IsDirectory)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                foreach (File f in CurrentDir.Children)
                {
                    if (isDir)
                    {
                        if (f.Name == name && f.IsDirectory)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (f.Name == name && !f.IsDirectory)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeDir(string name)
        {
            if (name == "..")
            {
                CurrentDir = CurrentDir.Parent;
            }
            else if (name == "/")
            {
                CurrentDir = FindRoot(CurrentDir);
            }
            else if (FindFile(name, true) != null)
            {
                bool findFile(File f)
                { return f.Name == name && f.IsDirectory && !f.Equals(CurrentDir); }
                CurrentDir = CurrentDir.Children.Find(findFile);
                ChangeDirirectory?.Invoke(this, new EventArgs());
            }
        }

        public void AddFile(string name, string contents = null)
        {
            File f = new File(name, contents);
            f.SetParent(CurrentDir);
            CurrentDir.Children.Add(f);
        }

        // TODO: Add ability to take full paths as input, e.g. /bin/subfolder/file
        public void AddFileToDir(string directoryName, string name, string contents = null)
        {
            if (TryFindFile(directoryName, true))
            {
                File f = new File(name, contents);
                f.SetParent(FindFile(directoryName, true));
                FindFile(directoryName, true).Children.Add(f);
            }
            else if (TryFindFile(directoryName, true, true))
            {
                File f = new File(name, contents);
                f.SetParent(FindFile(directoryName, true, true));
                FindFile(directoryName, true, true).Children.Add(f);
            }
            //else
            //    throw new Exception(directoryName + " is not a directory.");
        }

        public void RemoveFile(File file)
        {
            if (CurrentDir.Children.Remove(file))
            {
                // if file is removed
            }
            else
            {

            }
        }

        public void AddDir(string name)
        {
            File f = new File(name);
            f.SetParent(CurrentDir);
            CurrentDir.Children.Add(f);
        }

        public string ListFiles()
        {
            return ListFiles(CurrentDir);
        }

        private string ListFiles(File file)
        {
            //Sorts alphabetically
            file.Children.Sort();
            string retval = "";
            if (file.Parent != CurrentDir)
            {
                retval += "\n    <DIR>    .§";
                retval += "\n    <DIR>    ..§";
            }
            
            foreach (File f in file.Children)
            {
                if (f.IsDirectory)
                    retval += "\n    <DIR>    " + f.Name + "§";
                else
                    retval += "\n             " + f.Name + "§";
            }
            return retval;
        }
    }
}
