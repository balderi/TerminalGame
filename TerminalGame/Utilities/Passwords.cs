using System;

namespace TerminalGame.Utilities
{
    class Passwords
    {
        private static Random _rnd;

        private static readonly string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GeneratePassword(int length = 8)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            string retval = "";
            for(int i = 0; i < length; i++)
            {
                retval += _chars[i];
            }
            return retval;
        }
    }
}
