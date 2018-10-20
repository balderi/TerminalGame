using System;
using System.Threading;
using System.Timers;
using TerminalGame.UI.Themes;

namespace TerminalGame.Utilities
{
    class ActiveTracer
    {
        public float TraceSpeed { get; private set; }
        public bool IsActive { get; private set; }
        public int Counter { get; private set; }
        private readonly int _delay;
        private System.Timers.Timer _timer;

        public ActiveTracer(float traceSpeed)
        {
            Counter = 0;
            if (traceSpeed > 1f)
                TraceSpeed = 1f;
            else if (traceSpeed < 0f)
                TraceSpeed = 0f;
            else
                TraceSpeed = traceSpeed;

            _delay = (int)(2000 * TraceSpeed);
            if (_delay < 10)
                _delay = 10;

            _timer = new System.Timers.Timer(_delay);
            _timer.Elapsed += Timer_tick;
        }

        public void StartTrace()
        {
            Console.WriteLine("Trace started!");
            IsActive = true;
            _timer.Start();
            Thread thread = new Thread(new ThreadStart(DoTrace));
            thread.Start();
        }

        public void StopTrace()
        {
            if(Counter > 99)
            {
                GameManager.GetInstance().IsGameRunning = false;
            }
            IsActive = false;
            _timer.Stop();
            Counter = 0;
        }

        private void Timer_tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("trace at {0}%", Counter);
            if (Counter < 100)
                Counter++;
            else
                StopTrace();
        }

        private void DoTrace()
        {
            while(Counter < 100 && IsActive)
            {
                try
                {
                    SoundManager.GetInstance().GetSound("traceWarning").Play();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                ThemeManager.GetInstance().CurrentTheme.Flash();
                Thread.Sleep(/*(_delay / 10) */ 50 * (110 - Counter));
            }
            StopTrace();
        }
    }
}
