using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.People
{
    class Person
    {
        public string Name { get; protected set; }
        public DateTime DOB { get; protected set; }
        public int Age { get; protected set; }
        public Gender Gender { get; protected set; }
        public EducationLevel Education { get; protected set; }
        public AgeRange AgeRange { get; protected set; }
        public string Email { get; protected set; }
        public int Phone { get; protected set; }

        /// <summary>
        /// Generate a random person.
        /// </summary>
        public Person()
        {
            // TODO: Generate person.
        }

        /// <summary>
        /// Generate a random person in a specific age range.
        /// </summary>
        /// <param name="ageRange">The general age range the person should be in.</param>
        public Person(AgeRange ageRange)
        {
            // TODO: Generate person.
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

            // TODO: Calculate age.
            // TODO: Calculate age range.
        }
    }
}
