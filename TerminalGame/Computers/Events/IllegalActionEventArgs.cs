using System;

namespace TerminalGame.Computers.Events
{
    public class IllegalActionEventArgs
    {
        public DateTime TimeStamp { get; set; }

        public IllegalActionEventArgs(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
