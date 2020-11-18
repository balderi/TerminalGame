using System;

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
