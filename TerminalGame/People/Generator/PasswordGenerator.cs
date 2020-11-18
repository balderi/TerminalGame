using System;
using System.IO;
using TerminalGame.Utils;

namespace TerminalGame.People.Generator
{
    public class PasswordGenerator
    {
        private static readonly Random _rnd = new Random();

        public static string GeneratePassword(bool random = true)
        {
            if (random)
                return Generators.GeneratePassword(_rnd.Next(4, 12));
            else
            {
                var pw = File.ReadAllLines("Content/Data/passwords.txt");
                return pw[_rnd.Next(pw.Length)];
            }
        }
    }
}
