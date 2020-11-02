using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TerminalGame.Utils
{
    public static class IO
    {
        public static bool CheckAndCreateDirectory(string path)
        {
            if(!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}
