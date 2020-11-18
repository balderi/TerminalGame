using System;
using System.Threading.Tasks;
using System.Timers;
using TerminalGame.UI.Themes;
using TerminalGame.Utils;

namespace TerminalGame.Tracers
{
    class ActiveTracer
    {
        public double TraceSpeed { get; set; }
        public bool IsActive { get; set; }
        public int Counter { get; set; }
        private int _delay;
        private System.Timers.Timer _timer;

        private static ActiveTracer _instance;

        public static ActiveTracer GetInstance()
        {
            if (_instance == null)
                _instance = new ActiveTracer();
            return _instance;
        }

        private ActiveTracer()
        {

        }

        public string GetTracePercentage()
        {
            if (IsActive)
            {
                return $"{(float)Counter / 10:N1}%";
            }
            else
            {
                return "no trace";
            }
        }

        public async Task StartTrace(double traceSpeed)
        {
            Counter = 0;
            if (traceSpeed > 1.0)
                TraceSpeed = 1.0;
            else if (traceSpeed < 0.0)
                TraceSpeed = 0.0;
            else
                TraceSpeed = traceSpeed;

            _delay = (int)(500 * TraceSpeed);
            if (_delay < 10)
                _delay = 10;
            _timer?.Dispose();
            _timer = new System.Timers.Timer(_delay);
            _timer.Elapsed += Timer_tick;

            Console.WriteLine($"Trace started! (delay: {_delay}ms)");
            IsActive = true;
            _timer.Start();
            await DoTrace();
            //t.Start();
        }

        public void StopTrace()
        {
            if (!IsActive)
                return;

            IsActive = false;
            _timer.Stop();

            if (Counter > 999)
            {
                // TODO: Fire an event to determine what happens when the trace completes
                //Screens.ScreenManager.GetInstance().ChangeScreen("gameOver");
                World.World.GetInstance().Player.ConnectedComp.Disconnect(true);
            }

            Counter = 0;
            Console.WriteLine($"Active trace stopped (counter: {Counter})");
        }

        private void Timer_tick(object sender, ElapsedEventArgs e)
        {
            if (Counter < 1000)
                Counter++;
        }

        private async Task DoTrace()
        {
            await Task.Delay(1000);
            while (Counter < 1000 && IsActive)
            {
                try
                {
                    SoundManager.GetInstance().GetSound("traceWarn").Play();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine($"Active trace at {GetTracePercentage()}");
                ThemeManager.GetInstance().CurrentTheme.Flash();
                await Task.Delay(5 * (1100 - Counter));
            }
            if (IsActive)
                StopTrace();
        }
    }
}
