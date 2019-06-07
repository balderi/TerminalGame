using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace TerminalGame.Utils
{
    public class MusicManager
    {
        private Song _currentSong, _nextSong;
        private float _musicChangeSpeed, _delta;
        private bool _isFadingOut, _isFadingIn, _isPlaying;

        private static MusicManager _instance;

        private Dictionary<string, Song> _songs;

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
            _songs = new Dictionary<string, Song>();
        }

        public void AddSong(string key, Song song)
        {
            _songs.Add(key, song);
        }

        public void Start(string song, bool isRepeating = true)
        {
            if (_songs.TryGetValue(song, out _currentSong))
            {
                MediaPlayer.Volume = 0;
                MediaPlayer.IsRepeating = isRepeating;
                MediaPlayer.Play(_currentSong);
                _isFadingIn = true;
                _isPlaying = true;
                _delta = 0.01f;
            }
            else
                throw new ArgumentException(song + " does not exist on songs");
        }

        public void ChangeSong(string nextSong, float delta = 0.1f)
        {
            if (_songs.TryGetValue(nextSong, out _nextSong))
            {
                if (_currentSong == _nextSong)
                    return;
                
                _delta = delta;
                FadeOut(delta);
            }
            else
                throw new ArgumentException(nextSong + " does not exist on songs");
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
