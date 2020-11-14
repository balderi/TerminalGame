using System;
using System.Runtime.Serialization;
using TerminalGame.People.Utils;

namespace TerminalGame.People
{
    [DataContract]
    public class Person
    {
        protected Random        _rnd;

        [DataMember]
        public string           Name        { get; set; }
        [DataMember]
        public DateTime         DOB         { get; set; }
        [DataMember]
        public int              Age         { get; set; }
        [DataMember]
        public Gender           Gender      { get; set; }
        [DataMember]
        public EducationLevel   Education   { get; set; }
        [DataMember]
        public AgeRange         AgeRange    { get; set; }
        [DataMember]
        public string           Email       { get; set; } // TODO: Email provider.
        [DataMember]
        public int              Phone       { get; set; } // Ehhh, maybe not?

        public Person()
        {

        }

        /// <summary>
        /// Generate a random person.
        /// </summary>
        public Person GetRandomPerson()
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            var retval = new Person
            {
                Gender = (Gender)_rnd.Next(0, 2),
                AgeRange = (AgeRange)_rnd.Next(4, 8),
                Age = Generator.PersonGenerator.GenerateAge(AgeRange),
                Education = Generator.PersonGenerator.GenerateEducationLevel(Age),
                DOB = Generator.PersonGenerator.GenerateDOB(Age),
                Name = Generator.PersonGenerator.GenerateName(Gender),

                Email = "",
                Phone = 0
            };
            return retval;
        }

        /// <summary>
        /// Generate a random person in a specific age range.
        /// </summary>
        /// <param name="ageRange">The general age range the person should be in.</param>
        public Person(AgeRange ageRange)
        {
            _rnd        = new Random(DateTime.Now.Millisecond);
            Gender      = (Gender)_rnd.Next(0, 2);
            AgeRange    = ageRange;
            Age         = Generator.PersonGenerator.GenerateAge(AgeRange);
            Education   = Generator.PersonGenerator.GenerateEducationLevel(Age);
            DOB         = Generator.PersonGenerator.GenerateDOB(Age);
            Name        = Generator.PersonGenerator.GenerateName(Gender);

            Email       = "";
            Phone       = 0;
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
            Name        = name;
            DOB         = dob;
            Gender      = gender;
            Education   = education;
            Age         = Generator.PersonGenerator.GenerateAge(dob);
            AgeRange    = Generator.PersonGenerator.GenerateAgeRange(Age);

            Email       = "";
            Phone       = 0;
        }

        public int GetCurrentAge() => Generator.PersonGenerator.GenerateAge(DOB);
        public AgeRange GetCurrentAgeRange() => Generator.PersonGenerator.GenerateAgeRange(DOB);

        public override string ToString()
        {
            return Name + "\n"
                + Gender.ToString() + ", " + GetCurrentAge().ToString() + "\n"
                + DOB.ToShortDateString() + "\n"
                + Education.ToString();
        }

        public void Tick()
        {

        }
    }
}
