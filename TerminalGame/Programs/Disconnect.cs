﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Programs
{
    class Disconnect : Program
    {

        private static Disconnect _instance;

        public static Disconnect GetInstance()
        {
            if (_instance == null)
                _instance = new Disconnect();
            return _instance;
        }

        private Disconnect()
        {

        }

        protected override void Run()
        {
            _isKill = false;
            if (_args.Length > 1)
            {
                Game.Terminal.WriteLine("Too many arguments: disconnect");
                Kill();
                return;
            }
            if(Player.GetInstance().ConnectedComp == Player.GetInstance().PlayerComp)
            {
                Game.Terminal.WriteLine("Cannot disconnect from gateway");
                Kill();
                return;
            }
            Player.GetInstance().PlayerComp.Connect();
            Game.Terminal.WriteLine("Disconnected");
            Kill();
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}