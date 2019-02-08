using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.People.Utils;
using TerminalGame.Computers.Utils;

namespace TerminalGame.People
{
    public class User : Person
    {
        public string       Username        { get; protected set; }
        public string       Password        { get; protected set; }
        public AccessLevel  AccessLevel     { get; protected set; }

        /// <summary>
        /// Generate a random user.
        /// </summary>
        public User() : base()
        {
            // TODO: Generate user.
        }

        /// <summary>
        /// Generate a random user in a specific age range.
        /// </summary>
        /// <param name="ageRange">The general age range the user should be in.</param>
        public User(AgeRange ageRange) : base(ageRange)
        {
            // TODO: Generate user.
        }

        /// <summary>
        /// Generate a specific user.
        /// </summary>
        /// <param name="name">User's name.</param>
        /// <param name="dob">User's date of birth (will automatically determine age and age range).</param>
        /// <param name="gender">User's gender.</param>
        /// <param name="education">User's level of education.</param>
        public User(string name, DateTime dob, Gender gender, EducationLevel education) : base(name, dob, gender, education)
        {
            Name        = name;
            DOB         = dob;
            Gender      = gender;
            Education   = education;

            // TODO: Calculate age.
            // TODO: Calculate age range.
        }
    }
}
