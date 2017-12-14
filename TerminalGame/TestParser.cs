using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame
{
    class TestParser
    {
        Utilities.KeyboardManager keyMan;
        StringBuilder sb;
        List<string> parseOut;
        KeyboardState previousState;
        public TestParser(Game game)
        {
            sb = new StringBuilder();
            parseOut = new List<string>();
            previousState = Keyboard.GetState();
            keyMan = new Utilities.KeyboardManager(game);
        }

        public List<string> Parse(KeyboardState state)
        {
            foreach(var key in state.GetPressedKeys())
            {
                if(key == Keys.Enter && sb.Length > 0)
                {
                    parseOut.Add(sb.ToString());
                    sb.Clear();
                }
                else if(key != Keys.Enter && !previousState.IsKeyDown(key))
                {
                    //Console.WriteLine(key.ToString());
                    sb.Append(key);
                }
            }
            previousState = state;
            return parseOut;
        }

        public List<string> ParseString(string input)
        {
            if(input != "")
            {
                parseOut.Add(input);
            }
            return parseOut;
        }

        public bool ParseReturn(KeyboardState state)
        {
            foreach (var key in state.GetPressedKeys())
            {
                if (key == Keys.Enter && sb.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
