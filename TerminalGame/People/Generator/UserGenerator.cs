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
            switch (_rnd.Next(4))
            {
                case 0:
                    return sname[0][0] + sname[1]; // First initial, last name
                case 1:
                    return sname[0] + sname[1][0]; // First name, last initial
                case 2:
                    return sname[0]; // First name
                case 3:
                    return sname[1]; // Last name
                default:
                    throw new ArgumentOutOfRangeException("Error generating username");
            }
        }
    }
}
