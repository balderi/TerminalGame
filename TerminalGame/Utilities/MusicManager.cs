﻿using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    public class MusicManager
    {
        private static MusicManager _instance;

        public List<Song> Songs { get; private set; }

        private MusicManager()
        {
            if (Songs == null)
                Songs = new List<Song>();
        }

        public static MusicManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MusicManager();
            }
            return _instance;
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }
    }
}
