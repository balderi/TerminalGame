using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.People;
using TerminalGame.People.Utils;

namespace TerminalGame.Companies
{
    [DataContract(IsReference = true)]
    public class Company
    {
        private Random  _rnd;

        // TODO: Both owner and admin should be user. Or rework person to have passwords or something.

        // TODO: Company size (int or enum?) determining number of employees, shares, whatevs.

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Person Owner { get; set; }

        [DataMember]
        public Person Admin { get; set; }

        [DataMember]
        public int NumberOfShares { get; set; }

        [DataMember]
        public int SharePrice { get; set; }

        [DataMember]
        public int CompanyValue { get; set; }

        [DataMember]
        public List<Computer> GetComputers { get; set; } = new List<Computer>();

        [DataMember]
        public List<Person> GetPeople { get; set; } = new List<Person>();

        public Company()
        {

        }

        public static Company GetRandomCompany() => new Company(new Person(AgeRange.MiddleAged), new Person(AgeRange.Adult));

        public Company(string name)
        {
            Name = name;
        }

        public Company(Person owner, Person admin)
        {
            _rnd = new Random(DateTime.Now.Millisecond);

            Name = Generator.CompanyGenerator.GenerateName();
            Owner = owner;
            Admin = admin;
            NumberOfShares = _rnd.Next();
            SharePrice = _rnd.Next();
            CompanyValue = _rnd.Next();

            GetPeople.Add(Owner);
            GetPeople.Add(Admin);
        }

        public Company(string name, int numShares, int sharePrice) : this(name)
        {
            NumberOfShares = numShares;
            SharePrice = sharePrice;
        }

        public Company(string name, Person owner, Person admin, int numShares, int sharePrice) : this(name, numShares, sharePrice)
        {
            Owner = owner;
            Admin = admin;
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
    }
}
