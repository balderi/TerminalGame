using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utils
{
    public static class Generators
    {
        private static readonly string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static Random _rnd;

        public static string GeneratePassword(int length = 8)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for(int i = 0; i < length; i++)
            {
                retval += CHARS[_rnd.Next(0, CHARS.Length)];
            }
            return retval;
        }

        public static string GenerateIP()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for (int i = 0; i < 4; i++)
            {
                retval += _rnd.Next(1, 255) + ".";
            }
            return retval.TrimEnd('.');
        }
    }
}
