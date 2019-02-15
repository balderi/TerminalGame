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
        public File Parent { get; private set; }
        public List<File> Children { get; private set; }

        public File(string name)
        {
            Name = name;
            Contents = "dir";
            FileType = FileType.Directory;
            Size = -1;
        }

        public File(string name, string contents, FileType fileType)
        {
            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = Contents.Length;
        }

        public File(string name, string contents, FileType fileType, int size) : this(name, contents, fileType)
        {
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

        /// <summary>
        /// Allows files to be sorted alphabetically.
        /// </summary>
        /// <param name="other">File to compare to.</param>
        /// <returns>Less than zero if this FIle precedes <c>other</c> in the sort order.
        /// Zero if this instance occurs in the same position in the sort order as <c>other</c>.
        /// Greater than zero if this instance follows <c>other</c> in the sort order.</returns>
        public int CompareTo(File other)
        {
            if (other == null)
                return 1;
            if (other is File otherFile)
                return Name.CompareTo(otherFile.Name);
            else
                throw new ArgumentException("Object is not a File"); //impossible!
        }
    }
}
