using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TerminalGame
{
    class TestClass
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static string[] strings = new string[] { "Test", "Hello", "WATMAN", "shaky text lol" };

        //Legacy - not used!
        public static string PrintStuff()
        {
            return strings[rnd.Next(0, strings.Length)];
        }

        //Legacy - still used!
        public static int ShakeStuff(int Interval)
        {
            return rnd.Next(-Interval, Interval + 1);
        }
    }
}
