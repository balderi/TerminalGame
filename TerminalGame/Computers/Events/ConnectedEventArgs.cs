using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalGame.Computers.Events
{
    public class ConnectedEventArgs
    {
        public DateTime TimeStamp { get; set; }

        public ConnectedEventArgs(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
