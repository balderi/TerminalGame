using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Computers.FileSystems
{
    class File : IComparable
    {
        public enum FileType
        {
            Directory = 0,
            File,
        }

        public List<File> Children { get; private set; }

        public bool IsDirectory { get; private set; }
        public int Size { get; private set; }
        public FileType Type { get; private set; }
        public string Contents { get; private set; }
        public string Name { get; private set; }
        public File Parent { get; private set; }

        public File(string name)
        {
            Type = FileType.Directory;
            Name = name;
            Children = new List<File>();

            IsDirectory = true;
        }

        public File(string name, string contents = null)
        {
            Type = FileType.File;
            Name = name;
            Children = new List<File>();
            Contents = contents;
            IsDirectory = false;
        }

        /// <summary>
        /// Recursively prints the full directory path from root to current directory
        /// </summary>
        /// <returns>Full, formatted path as string</returns>
        public string PrintFullPath()
        {
            string retval = "";
            if (Parent != this)
            {
                retval += Parent.PrintFullPath() + "/" + Name;
            }
            return retval;
        }

        public void SetParent(File parent)
        {
            Parent = parent;
        }

        public string Execute()
        {
            switch (Type)
            {
                case FileType.Directory:
                    {
                        return String.Format("{0} is a directory.", Name);
                    }
                case FileType.File:
                    {
                        if(Contents != null)
                            return Contents;
                        return "";
                    }
                default:
                    {
                        return String.Format("{0} is an unkown file type.", Name);
                    }
            }
        }

        public void AddFile(string name, string contents = null)
        {
            if (IsDirectory)
            {
                Children.Add(new File(name, contents));
            }
        }

        public void AddFolder(string name)
        {
            if (IsDirectory)
            {
                Children.Add(new File(name));
            }
        }

        public void RemoveFile(File file)
        {
            if (IsDirectory)
            {
                Children.Remove(file);
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is File otherFile)
                return Name.CompareTo(otherFile.Name);
            else
                throw new ArgumentException("Object is not a File");
        }
    }
}
