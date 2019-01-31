﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Time
{
    public enum GameSpeed
    {
        Paused,
        RealTime,   // Actual real time
        Single,     // 1x "game speed"
        Double,     // Not actually double or triple speed.
        Triple,     // Just a way to represent ">>" and ">>>".
    }
}
