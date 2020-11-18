using System;

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
