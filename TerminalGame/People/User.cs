using System;
using TerminalGame.People.Utils;
using TerminalGame.Computers.Utils;
using System.Runtime.Serialization;

namespace TerminalGame.People
{
    public class User : Person
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public AccessLevel AccessLevel { get; set; }

        public User()
        {

        }

        /// <summary>
        /// Generate a random user in a specific age range.
        /// </summary>
        /// <param name="ageRange">The general age range the user should be in.</param>
        public User(AgeRange ageRange) : base(ageRange)
        {
            Username = Generator.UserGenerator.GenerateUsername(Name);
            Password = Generator.PasswordGenerator.GeneratePassword(false);
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
            Username = Generator.UserGenerator.GenerateUsername(Name);
            Password = Generator.PasswordGenerator.GeneratePassword(false);
        }
    }
}
