using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Companies;
using TerminalGame.Computers;

namespace TerminalGame.Missions
{
    public class Mission
    {
        public MissionType Type { get; set; }
        public int Payout { get; set; }
        public Company Client { get; set; }
        public Company TargetCompany { get; set; }
        public Computer TargetComputer { get; set; }
        public bool IsActive { get; set; }
        public bool IsComplete { get; set; }
    }
}
