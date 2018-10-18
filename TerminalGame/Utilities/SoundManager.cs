using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace TerminalGame.Utilities
{
    class SoundManager
    {
        private static SoundManager _instance;
        private SoundManager()
        {
            Sounds = new Dictionary<string, SoundEffect>();
        }

        public Dictionary<string, SoundEffect> Sounds { get; private set; }

        public static SoundManager GetInstance()
        {
            if(_instance == null)
            {
                _instance = new SoundManager();
            }
            return _instance;
        }

        public void LoadSound(string name, SoundEffect sound)
        {
            Sounds.Add(name, sound);
        }

        public SoundEffect GetSound(string name)
        {
            if (!Sounds.TryGetValue(name, out SoundEffect retval))
            {
                Console.WriteLine("Tried playing sound '{0}', but it does not exist.", name);
                return Sounds["traceWarning"];
            }
            return retval;
        }
    }
}
