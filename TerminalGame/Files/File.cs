using System;
using System.Collections.Generic;

namespace TerminalGame.Files
{
    public class File : IFile
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
            Children = new List<File>();
        }

        public File(string name, string contents, FileType fileType)
        {
            if (fileType == FileType.Directory)
                throw new ArgumentException("Directory file type cannot have contents.");

            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = Contents.Length;
        }

        public File(string name, string contents, FileType fileType, int size) : this(name, contents, fileType)
        {
            Size = size;
        }

        /// <summary>
        /// Rename the file.
        /// </summary>
        /// <param name="name">New file name.</param>
        public void Rename(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add a file to the list of children.
        /// </summary>
        /// <param name="file">File to add.</param>
        public void AddFile(File file)
        {
            if (FileType == FileType.Directory)
            {
                if (Children.Contains(file))
                    throw new ArgumentException(file.Name + " already exists.");
                else
                {
                    Children.Add(file);
                    file.Parent = this;
                }
            }
            else
                throw new InvalidOperationException(Name + " is not a directory.");
        }

        /// <summary>
        /// Remove a file from the list of children.
        /// </summary>
        /// <param name="file">File to remove.</param>
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

        public string GetFullPath()
        {
            string retval = "";
            if (Parent != null)
                retval += Parent.GetFullPath() + "/" + Name;
            return retval;
        }

        /// <summary>
        /// Returns the contents of the file.
        /// </summary>
        /// <returns>File contents as string.</returns>
        public override string ToString()
        {
            if(FileType == FileType.Directory)
            {
                return Name + " is a directory.";
            }
            return Contents;
        }

        /// <summary>
        /// Returns a list of children, or file name if file.
        /// </summary>
        /// <returns>List of children or file name as string.</returns>
        public string ListChildren()
        {
            if (FileType == FileType.Directory)
            {
                string retval = Parent == null ? "." : ".\n..";
                if (Children.Count > 0)
                {
                    foreach (var c in Children)
                    {
                        retval += "\n" + c.Name;
                    }
                }
                return retval;
            }
            return Name;
        }

        public File GetChild(string name)
        {
            if(FileType == FileType.Directory)
            {
                bool findFile(File f)
                {
                    return f.Name == name;
                }
                return Children.Find(findFile);
            }
            throw new InvalidOperationException("Cannot get child of file.");
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

        public bool FileNameExists(string name)
        {
            foreach(File f in Children)
            {
                if (f.Name == name)
                    return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            var file = obj as File;
            return file != null &&
                   Name == file.Name &&
                   Contents == file.Contents &&
                   Size == file.Size &&
                   FileType == file.FileType &&
                   EqualityComparer<File>.Default.Equals(Parent, file.Parent) &&
                   EqualityComparer<List<File>>.Default.Equals(Children, file.Children);
        }

        public override int GetHashCode()
        {
            var hashCode = 1411017911;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Contents);
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            hashCode = hashCode * -1521134295 + FileType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<File>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<File>>.Default.GetHashCode(Children);
            return hashCode;
        }
    }
}
