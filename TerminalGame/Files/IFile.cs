using System;

namespace TerminalGame.Files
{
    interface IFile : IComparable<File>
    {
        void Rename(string name);
        void AddFile(File file);
        void DeleteFile(File file);
    }
}
