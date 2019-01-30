using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Files
{
    class File : IFile
    {
        private readonly Programs.Program _program;
        
        public string Name { get; private set; }
        public string Contents { get; private set; }
        public int Size { get; private set; }
        public FileType FileType { get; private set; }
        public List<File> Children { get; private set; }

        public File(string name)
        {
            Name = name;
            Contents = "";
            FileType = FileType.Directory;
            Size = -1;
        }

        public File(string name, string contents, FileType fileType)
        {
            if (fileType == FileType.Binary)
                throw new ArgumentException("Binary files need to be instantiated with a program.");
            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = Contents.Length;
        }

        public File(string name, string contents, FileType fileType, int size) : this(name, contents, fileType)
        {
            Size = size;
        }

        public File(string name, Programs.Program program, int size)
        {
            Name = name;
            Contents = "10101010101"; // TODO: Make a generator to generate random binary. Possibly some random easter eggs?
            FileType = FileType.Binary;
            Size = size;
        }

        public void Rename(string name)
        {
            Name = name;
        }

        public void AddFile(File file)
        {
            if (FileType == FileType.Directory)
            {
                if (Children.Contains(file))
                    throw new ArgumentException(file.Name + " already exists.");
                else
                    Children.Add(file);
            }
            else
                throw new InvalidOperationException(Name + " is not a directory.");
        }

        public void RemoveFile(File file)
        {
            if (FileType == FileType.Directory)
            {
                if (Children.Contains(file))
                    Children.Remove(file);
                else
                    throw new ArgumentException(file.Name + " does not exist.");
            }
            else
                throw new InvalidOperationException(Name + " is not a directory.");
        }

        public override string ToString()
        {
            return Contents;
        }

        public int CompareTo(File other)
        {
            if (other == null)
                return 1;
            if (other is File otherFile)
                return Name.CompareTo(otherFile.Name);
            else
                throw new ArgumentException("Object is not a File");
        }
    }
}
