using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.People
{
    class Person
    {
        protected Random _rnd;

        public string Name { get; protected set; }
        public DateTime DOB { get; protected set; }
        public int Age { get; protected set; }
        public Gender Gender { get; protected set; }
        public EducationLevel Education { get; protected set; }
        public AgeRange AgeRange { get; protected set; }
        public string Email { get; protected set; } // TODO: Email provider.
        public int Phone { get; protected set; } // Ehhh, maybe not?

        /// <summary>
        /// Generate a random person.
        /// </summary>
        public Person()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            Gender = (Gender)_rnd.Next(0, 2);
            AgeRange = (AgeRange)_rnd.Next(0, 8);
            Age = Generator.Person.GenerateAge(AgeRange);
            DOB = Generator.Person.GenerateDOB(Age);
            Name = Generator.Person.GenerateName(Gender);

            Email = "";
            Phone = 0;
        }

        /// <summary>
        /// Generate a random person in a specific age range.
        /// </summary>
        /// <param name="ageRange">The general age range the person should be in.</param>
        public Person(AgeRange ageRange)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            Gender = (Gender)_rnd.Next(0, 2);
            AgeRange = ageRange;
            Age = Generator.Person.GenerateAge(AgeRange);
            DOB = Generator.Person.GenerateDOB(Age);
            Name = Generator.Person.GenerateName(Gender);

            Email = "";
            Phone = 0;
        }

        /// <summary>
        /// Generate a specific person.
        /// </summary>
        /// <param name="name">Person's name.</param>
        /// <param name="dob">Person's date of birth (will automatically determine age and age range).</param>
        /// <param name="gender">Person's gender.</param>
        /// <param name="education">Person's level of education.</param>
        public Person(string name, DateTime dob, Gender gender, EducationLevel education)
        {
            Name = name;
            DOB = dob;
            Gender = gender;
            Education = education;
            Age = Generator.Person.GenerateAge(dob);
            AgeRange = Generator.Person.GenerateAgeRange(Age);

            Email = "";
            Phone = 0;
        }
    }
}
