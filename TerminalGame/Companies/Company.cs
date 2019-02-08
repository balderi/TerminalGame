using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.People;

namespace TerminalGame.Companies
{
    public class Company
    {
        private Random          _rnd;

        public string           Name            { get; private set; }
        public Person           Owner           { get; private set; }
        public User             Admin           { get; private set; }
        public int              NumberOfShares  { get; private set; }
        public int              SharePrice      { get; private set; }
        public int              CompanyValue    { get; private set; }
        public List<Computer>   GetComputers    { get; private set; }

        public Company()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            Name = Generator.Company.GenerateName();
            Owner = new Person(People.Utils.AgeRange.MiddleAged);
            Admin = new User(People.Utils.AgeRange.Adult);
            NumberOfShares = _rnd.Next();
            SharePrice = _rnd.Next();
            CompanyValue = _rnd.Next();
            GetComputers = new List<Computer>
            {
                Generator.Company.GenerateComputer(this, ComputerType.Server),
            };
        }

        public Company(string name)
        {
            Name            = name;
        }

        public Company(string name, int numShares, int sharePrice) : this(name)
        {
            NumberOfShares  = numShares;
            SharePrice      = sharePrice;
        }

        public Company(string name, Person owner, User admin, int numShares, int sharePrice) : this(name, numShares, sharePrice)
        {
            Owner           = owner;
            Admin           = admin;
        }

        public void Tick()
        {

        }
    }
}
