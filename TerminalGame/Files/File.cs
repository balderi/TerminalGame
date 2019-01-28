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
        public string Name { get; private set; }
        public string Contents { get; private set; }
        public int Size { get; private set; }
        public FileType FileType { get; private set; }

        public File(string name, string contents, FileType fileType)
        {
            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = Contents.Length;
        }

        public File(string name, string contents, FileType fileType, int size)
        {
            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = size;
        }

        public void Rename(string name)
        {
            Name = name;
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
