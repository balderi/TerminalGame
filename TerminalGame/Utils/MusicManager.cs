using Microsoft.Xna.Framework;
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
        private Song _currentSong, _nextSong;
        private float _musicChangeSpeed, _delta;
        private bool _isFadingOut, _isFadingIn, _isPlaying;

        private static MusicManager _instance;

        public static MusicManager GetInstance()
        {
            if (_instance == null)
                _instance = new MusicManager();
            return _instance;
        }

        private MusicManager()
        {
            _isFadingOut = false;
            _isFadingIn = false;
            _musicChangeSpeed = 1.0f;
        }

        public void Start(Song song)
        {
            _currentSong = song;
            MediaPlayer.Volume = 0;
            MediaPlayer.Play(_currentSong);
            _isFadingIn = true;
            _isPlaying = true;
            _delta = 0.1f;
        }

        public void ChangeSong(Song nextSong, float delta = 0.1f)
        {
            if (_currentSong == nextSong)
                return;

            _nextSong = nextSong;
            _delta = delta;
            FadeOut(delta);
        }

        public void SetMusicChangeSpeed(float musicChangeSpeed)
        {
            _musicChangeSpeed = musicChangeSpeed;
        }

        public float GetMusicChangeSpeed()
        {
            return _musicChangeSpeed;
        }

        public void FadeOut(float delta = 0.01f)
        {
            _delta = delta;
            _isFadingOut = true;
            _isFadingIn = false;
        }

        public void FadeIn(float delta = 0.01f)
        {
            _delta = delta;
            _isFadingOut = false;
            _isFadingIn = true;
        }

        public void Update(GameTime gameTime)
        {
            if(_isFadingOut)
            {
                float volume = MediaPlayer.Volume;
                volume -= _delta * _musicChangeSpeed;
                if (volume < 0.0f)
                {
                    volume = 0.0f;
                    _isFadingOut = false;
                    if(_nextSong != null)
                    {
                        MediaPlayer.Stop();
                        _currentSong = _nextSong;
                        _nextSong = null;
                        MediaPlayer.Play(_currentSong);
                        _isFadingIn = true;
                    }
                    if(!_isPlaying)
                    {
                        MediaPlayer.Stop();
                    }
                }
                MediaPlayer.Volume = volume;
            }

            if(_isFadingIn)
            {
                float volume = MediaPlayer.Volume;
                volume += _delta * _musicChangeSpeed;
                if (volume > 1.0f)
                {
                    volume = 1.0f;
                    _isFadingIn = false;
                }
                MediaPlayer.Volume = volume;
            }
        }
    }
}
