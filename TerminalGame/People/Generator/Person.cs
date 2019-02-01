using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Time;

namespace TerminalGame.People.Generator
{
    public static class Person
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);
        
        public static EducationLevel GenerateEducationLevel(int age)
        {
            if (age < 5)
                return EducationLevel.None;
            if (age < 15)
                return (EducationLevel)_rnd.Next(0, 2);
            if (age < 18)
                return (EducationLevel)_rnd.Next(0, 3);
            if (age >= 18)
                return (EducationLevel)_rnd.Next(0, 4);

            // Impossible!
            return EducationLevel.None;
        }

        public static string GenerateName(Gender gender)
        {
            string retval = "";
            string[] last = File.ReadAllLines("Content/Data/Names/lastNames.txt");
            if(gender == Gender.Female)
            {
                string[] names = File.ReadAllLines("Content/Data/Names/femaleNames.txt");
                retval += names[_rnd.Next(0, names.Length)];
            }
            else
            {
                string[] names = File.ReadAllLines("Content/Data/Names/maleNames.txt");
                retval += names[_rnd.Next(0, names.Length)];
            }
            retval += " ";
            retval += last[_rnd.Next(0, last.Length)];
            return retval;
        }

        public static DateTime GenerateDOB(int age)
        {
            DateTime retval = GameClock.GameTime.Date;
            retval = retval.AddYears(-age);
            retval = retval.AddDays(_rnd.Next(0, 365));
            return retval;
        }

        public static AgeRange GenerateAgeRange(DateTime dob)
        {
            return GenerateAgeRange(GenerateAge(dob));
        }

        public static AgeRange GenerateAgeRange(int age)
        {
            if (age < 0)
                return AgeRange.None;
            if (age <= 1)
                return AgeRange.Infant;
            if (age <= 3)
                return AgeRange.Toddler;
            if (age <= 9)
                return AgeRange.Kid;
            if (age <= 12)
                return AgeRange.Preteen;
            if (age <= 19)
                return AgeRange.Teen;
            if (age <= 44)
                return AgeRange.Adult;
            if (age <= 65)
                return AgeRange.MiddleAged;
            if (age > 65)
                return AgeRange.Senior;

            // Impossible!
            return AgeRange.None;
        }

        public static int GenerateAge(DateTime dob)
        {
            return (int)GameClock.GameTime.Date.Subtract(dob.Date).TotalDays / 365;
        }

        public static int GenerateAge(AgeRange ageRange)
        {
            switch(ageRange)
            {
                case AgeRange.Infant:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(0, 2));
                    }
                case AgeRange.Toddler:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(1, 4));
                    }
                case AgeRange.Kid:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(3, 9));
                    }
                case AgeRange.Preteen:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(9, 13));
                    }
                case AgeRange.Teen:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(13, 20));
                    }
                case AgeRange.Adult:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(20, 45));
                    }
                case AgeRange.MiddleAged:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(45, 65));
                    }
                case AgeRange.Senior:
                    {
                        return GameClock.GameTime.Year - (GameClock.GameTime.Year - _rnd.Next(65, 101));
                    }
                default:
                    return -1;
            }
        }
    }
}
