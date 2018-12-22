using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utils
{
    public class MusicManager
    {
        private static float _musicChangeSpeed = 1.0f;
        public static Song _currentMusic;


        public static bool IsFadingOut = false;
        public static bool IsFadingIn = false;

        public static void SetMusicChangeSpeed(float musicChangeSpeed)
        {
            _musicChangeSpeed = musicChangeSpeed;
        }

        public static float GetMusicChangeSpeed()
        {
            return _musicChangeSpeed;
        }

        public static void FadeOut(float delta)
        {
            float volume = MediaPlayer.Volume;
            volume -= delta * _musicChangeSpeed;
            if (volume < 0.0f)
            {
                volume = 0.0f;
                IsFadingOut = false;
            }
            MediaPlayer.Volume = volume;
        }

        public static void FadeIn(float delta)
        {
            float volume = MediaPlayer.Volume;
            volume += delta * _musicChangeSpeed;
            if (volume > 1.0f)
            {
                volume = 1.0f;
                IsFadingIn = false;
            }
            MediaPlayer.Volume = volume;
        }
    }
}
