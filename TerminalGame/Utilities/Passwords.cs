using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    class Passwords
    {
        static Random rnd;

        static readonly string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GeneratePassword(int length = 8)
        {
            rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for(int i = 0; i < length; i++)
            {
                retval += chars[i];
            }
            return retval;
        }
    }
}
