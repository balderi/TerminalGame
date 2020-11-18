using System;

namespace TerminalGame.Time
{
    public static class GameClock
    {                                               // 1 real second =
        private const double REAL_TIME = 0.016;     // 1 sec
        private const double SINGLE = 0.016;     // 1 min
        private const double DOUBLE = 0.528;     // 33 min
        private const double TRIPLE = 5.280;     // 333 min

        private static DayChangeEventArgs _dayChange;

        public delegate void DayChangeEventHandler(DayChangeEventArgs e);
        public static event DayChangeEventHandler DayChange;

        public static DateTime GameTime;

        public static void Initialize(string dateString = "2000-01-01T00:00:00.0000000Z")
        {
            GameTime = DateTime.Parse(dateString);
            _dayChange = new DayChangeEventArgs();
        }

        public static void Load(string dateString)
        {
            GameTime = DateTime.Parse(dateString);
        }

        public static void Tick(GameSpeed gameSpeed)
        {
            int _currentDay = GameTime.Day;
            switch (gameSpeed)
            {
                case GameSpeed.Paused:
                    {
                        break;
                    }
                case GameSpeed.RealTime:
                    {
                        GameTime = GameTime.AddSeconds(REAL_TIME);
                        break;
                    }
                case GameSpeed.Single:
                    {
                        GameTime = GameTime.AddMinutes(SINGLE);
                        break;
                    }
                case GameSpeed.Double:
                    {
                        GameTime = GameTime.AddMinutes(DOUBLE);
                        break;
                    }
                case GameSpeed.Triple:
                    {
                        GameTime = GameTime.AddMinutes(TRIPLE);
                        break;
                    }
                default:
                    {
                        GameTime = GameTime.AddMinutes(SINGLE);
                        break;
                    }
            }
            if (_currentDay != GameTime.Day)
                DayChange?.Invoke(_dayChange);
        }
    }

    public class DayChangeEventArgs : EventArgs
    {

    }
}
