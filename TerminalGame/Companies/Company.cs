using System;
using System.Collections.Generic;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.People;
using TerminalGame.People.Utils;

namespace TerminalGame.Companies
{
    public class Company
    {
        private Random          _rnd;

        // TODO: Both owner and admin should be user. Or rework person to have passwords or something.

        // TODO: Company size (int or enum?) determining number of employees, shares, whatevs.

        public string           Name            { get; set; }
        public Person           Owner           { get; set; }
        public Person           Admin           { get; set; }
        public int              NumberOfShares  { get; set; }
        public int              SharePrice      { get; set; }
        public int              CompanyValue    { get; set; }
        public List<Computer>   GetComputers    { get; set; }

        public Company()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            Name = Generator.CompanyGenerator.GenerateName();
            Owner = new Person(AgeRange.MiddleAged);
            NumberOfShares = _rnd.Next();
            SharePrice = _rnd.Next();
            CompanyValue = _rnd.Next();
            Admin = new Person(AgeRange.Adult);
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

        public Company(string name, Person owner, Person admin, int numShares, int sharePrice) : this(name, numShares, sharePrice)
        {
            Owner           = owner;
            Admin           = admin;
        }

        public void Tick()
        {

        }

        public void GenerateComputers()
        {
            GetComputers = new List<Computer>
            {
                Generator.CompanyGenerator.GenerateComputer(this, ComputerType.Server),
            };
        }

        /// <summary>
        /// Fixes relationship between companies and computers
        /// which are stripped when saving, to prevent circular dependencies
        /// </summary>
        public void FixComputers()
        {
            if(GetComputers.Count > 0)
            {
                foreach(Computer c in GetComputers)
                {
                    c.Owner = this;
                    c.FileSystem.RootDir.FixFile();
                }
            }
        }
    }
}
