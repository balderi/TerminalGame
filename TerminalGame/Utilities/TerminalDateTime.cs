using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    class TerminalDateTime
    {
        private DateTime _now;
        private DateTimeOffset lastChange;

        private TerminalDateTime _instance;

        public TerminalDateTime GetInstance()
        {
            if (_instance == null)
                _instance = new TerminalDateTime();
            return _instance;
        }

        private TerminalDateTime()
        {

        }

        public void SetTime(DateTime startTime)
        {
            _now = startTime;
            lastChange = _now;
        }

        public DateTime Now()
        {
            return _now;
        }

        public void Update(GameTime gameTime)
        {
            if (_now - lastChange >= TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalMilliseconds * 10))
            {
                _now += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalMilliseconds * 10);
            }

            lastChange = _now + TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalMilliseconds * 10);
        }
    }
}
