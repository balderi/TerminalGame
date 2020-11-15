using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalGame.Computers.Events
{
    public class DisconnectedEventArgs
    {
        public DateTime TimeStamp { get; set; }

        public DisconnectedEventArgs(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
