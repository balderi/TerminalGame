using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using TerminalGame.Utils;
using TerminalGame.UI.Themes;
using System.Threading.Tasks;

namespace TerminalGame.Tracers
{
    class ActiveTracer
    {
        public float TraceSpeed { get; set; }
        public bool IsActive { get; set; }
        public int Counter { get; set; }
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

            _delay = (int)(5000 * TraceSpeed);
            if (_delay < 10)
                _delay = 10;

            _timer = new System.Timers.Timer(_delay);
            _timer.Elapsed += Timer_tick;
        }

        public async Task StartTrace()
        {
            Console.WriteLine("Trace started!");
            IsActive = true;
            _timer.Start();
            await DoTrace();
            //t.Start();
        }

        public void StopTrace()
        {
            Console.WriteLine("Active trace stopped");

            if (Counter > 99)
            {
                // TODO: Fire an event to determine what happens when the trace completes
                Screens.ScreenManager.GetInstance().ChangeScreen("gameOver");
            }
            IsActive = false;
            _timer.Stop();
            Counter = 0;
        }

        private void Timer_tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Active trace at {0}%", Counter);
            if (Counter < 100)
                Counter++;
            else
                StopTrace();
        }

        private async Task DoTrace()
        {
            while (Counter < 100 && IsActive)
            {
                try
                {
                    SoundManager.GetInstance().GetSound("traceWarn").Play();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                ThemeManager.GetInstance().CurrentTheme.Flash();
                await Task.Delay(50 * (110 - Counter));
            }
            StopTrace();
        }
    }
}
