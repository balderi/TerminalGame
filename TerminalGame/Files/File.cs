using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TerminalGame.People;

namespace TerminalGame.Files
{
    [DataContract(IsReference = true)]
    public class File : IFile
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Contents { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public FileType FileType { get; set; }
        [DataMember]
        public FilePermissionLevel ReadLevel { get; set; }
        [DataMember]
        public FilePermissionLevel WriteLevel { get; set; }
        [DataMember]
        public FilePermissionLevel ExecuteLevel { get; set; }
        [DataMember]
        public File Parent { get; set; }
        [DataMember]
        public List<File> Children { get; set; }
        [DataMember]
        public User Owner { get; set; }

        public File()
        {

        }

        public File(string name, User owner = null, FilePermissionLevel readLevel = 0,
            FilePermissionLevel writeLevel = 0, FilePermissionLevel executeLevel = 0)
        {
            Name = name;
            Contents = "dir";
            FileType = FileType.Directory;
            Size = -1;
            Children = new List<File>();
            ReadLevel = readLevel;
            WriteLevel = writeLevel;
            ExecuteLevel = executeLevel;
            Owner = owner;
        }

        public File(string name, string contents, FileType fileType, User owner = null, FilePermissionLevel readLevel = 0,
            FilePermissionLevel writeLevel = 0, FilePermissionLevel executeLevel = 0)
        {
            if (fileType == FileType.Directory)
                throw new ArgumentException("Directory file type cannot have contents.");

            Name = name;
            Contents = contents;
            FileType = fileType;
            Size = Contents.Length;
            ReadLevel = readLevel;
            WriteLevel = writeLevel;
            ExecuteLevel = executeLevel;
            Owner = owner;
        }

        public File(string name, string contents, FileType fileType, int size, User owner = null, FilePermissionLevel readLevel = 0,
            FilePermissionLevel writeLevel = 0, FilePermissionLevel executeLevel = 0) : this(name, contents, fileType, owner, readLevel, writeLevel, executeLevel)
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
        public void DeleteFile(File file)
        {
            if (FileType != FileType.Directory)
            {
                if (Children.Contains(file))
                    Children.Remove(file);
                else
                    throw new ArgumentException(file.Name + " does not exist.");
            }
            else
                throw new InvalidOperationException(Name + " is a directory.");
        }

        public void DeleteDirectory(File dir, bool recurse)
        {
            if (FileType == FileType.Directory)
            {
                if (dir.Children.Count > 0)
                {
                    if (recurse)
                    {
                        dir.Purge();
                    }
                }
                else
                    Children.Remove(dir);
            }
            else
                Children.Remove(dir);
        }

        public void Purge()
        {
            Contents = null;
            FileType = 0;
            Name = null;
            Size = 0;

            if (Children != null)
            {
                while (Children.Count > 0)
                {
                    var c = Children.FirstOrDefault();
                    if (c != null)
                    {
                        c.Purge();
                        Children.Remove(c);
                    }
                }
            }
            Parent?.Children.Remove(this);
            Parent = null;
        }

        public string GetFullPath()
        {
            string retval = "";
            if (Parent != null)
                retval += Parent.GetFullPath() + "/" + Name;
            return retval;
        }

        public string GetFileDetails(string nameOverride = null)
        {
            StringBuilder sb = new StringBuilder(" ");
            if (FileType == FileType.Directory)
                sb.Append('d');
            else
                sb.Append('-');
            for (int i = 0; i < 3; i++)
            {
                if (ReadLevel >= (FilePermissionLevel)i)
                    sb.Append('r');
                else
                    sb.Append('-');

                if (WriteLevel >= (FilePermissionLevel)i)
                    sb.Append('w');
                else
                    sb.Append('-');

                if (ExecuteLevel >= (FilePermissionLevel)i)
                    sb.Append('x');
                else
                    sb.Append('-');
            }
            if (string.IsNullOrEmpty(nameOverride))
                sb.AppendLine($"  {Name}");
            else
                sb.AppendLine($"  {nameOverride}");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the contents of the file.
        /// </summary>
        /// <returns>File contents as string.</returns>
        public override string ToString()
        {
            if (FileType == FileType.Directory)
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
                    Children.Sort();
                    foreach (var c in Children)
                    {
                        retval += "\n" + c.Name;
                    }
                }
                return retval;
            }
            return Name;
        }

        public string ListChildrenDetails()
        {
            if (FileType == FileType.Directory)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(GetFileDetails("."));
                if (Parent != null)
                    sb.Append(Parent.GetFileDetails(".."));
                if (Children.Count > 0)
                {
                    Children.Sort();
                    foreach (var c in Children)
                    {
                        sb.Append(c.GetFileDetails());
                    }
                }
                return sb.ToString();
            }
            return GetFileDetails();
        }

        public File GetChild(string name)
        {
            if (FileType == FileType.Directory)
            {
                return Children.Find(f => f.Name == name);
            }
            throw new InvalidOperationException("Cannot get child of file.");
        }

        /// <summary>
        /// Allows files to be sorted alphabetically.
        /// </summary>
        /// <param name="other">File to compare to.</param>
        /// <returns>Less than zero if this File precedes <c>other</c> in the sort order.
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

        public bool FileNameExists(string name) => Children.Exists(f => f.Name == name);

        public override bool Equals(object obj)
        {
            return obj is File file &&
                   Name == file.Name &&
                   Contents == file.Contents &&
                   Size == file.Size &&
                   FileType == file.FileType &&
                   EqualityComparer<File>.Default.Equals(Parent, file.Parent) &&
                   EqualityComparer<List<File>>.Default.Equals(Children, file.Children);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Contents, Size, FileType, Parent, Children);
        }
    }
}
