using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.People;

namespace TerminalGame.Companies
{
    class Company
    {
        public string   Name            { get; private set; }
        public Person   Owner           { get; private set; }
        public User     Admin           { get; private set; }
        public int      NumberOfShares  { get; private set; }
        public int      SharePrice      { get; private set; }

        public Company()
        {

        }

        public Company(string name)
        {
            Name            = name;
        }

        public Company(string name, int numShares, int sharePrice)
        {
            Name            = name;
            NumberOfShares  = numShares;
            SharePrice      = sharePrice;
        }

        public Company(string name, Person owner, User admin, int numShares, int sharePrice)
        {
            Name            = name;
            Owner           = owner;
            Admin           = admin;
            NumberOfShares  = numShares;
            SharePrice      = sharePrice;
        }
    }
}
