using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Files
{
    interface IFile : IComparable<File>
    {
        void Rename(string name);
    }
}
