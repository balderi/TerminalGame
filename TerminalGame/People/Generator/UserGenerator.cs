using System;

namespace TerminalGame.People.Generator
{
    public static class UserGenerator
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);

        public static string GenerateUsername(string name)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            string[] sname = name.Split(' ');
            return (_rnd.Next(4)) switch
            {
                0 => sname[0][0] + sname[1],// First initial, last name
                1 => sname[0] + sname[1][0],// First name, last initial
                2 => sname[0],// First name
                3 => sname[1],// Last name
                _ => throw new ArgumentOutOfRangeException("Error generating username"),
            };
        }
    }
}
