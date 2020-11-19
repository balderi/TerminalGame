using System;
using System.Collections.Generic;
using System.Text;
using TerminalGame.Files;

namespace TerminalGame.Computers.Logging
{
    public class Logger
    {
        public static void CreateLog(Computer computer,string title, string message)
        {
            var time = World.World.GetInstance().CurrentGameTime;

            string logName = $"{time:yyMMdd-HHmmss}_{title}";

            File logFile = new File(logName, message, FileType.Text);

            if(computer.FileSystem.TryFindFileFromPath("var/log", out _, out File logDir))
            {
                logDir.AddFile(logFile);
            }
        }
    }
}
