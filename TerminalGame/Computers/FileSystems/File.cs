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

        public File(FileType type, string name)
        {
            Type = type;
            Name = name;
            Children = new List<File>();

            IsDirectory = type == FileType.Directory;
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
                        return String.Format("{0} is a file.", Name);
                    }
                default:
                    {
                        return String.Format("{0} is an unkown file type.", Name);
                    }
            }
        }

        public void AddFile(string name)
        {
            if (IsDirectory)
            {
                Children.Add(new File(FileType.File, name));
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
            File otherFile = obj as File;
            if (otherFile != null)
                return this.Name.CompareTo(otherFile.Name);
            else
                throw new ArgumentException("Object is not a File");
        }
    }
}
