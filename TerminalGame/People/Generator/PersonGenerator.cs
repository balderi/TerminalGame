using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerminalGame.Time;
using TerminalGame.People.Utils;

namespace TerminalGame.People.Generator
{
    public static class PersonGenerator
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);
        private static List<string> _last = File.ReadLines("Content/Data/Names/lastNames.txt").ToList();
        private static List<string> _fNames = File.ReadLines("Content/Data/Names/femaleNames.txt").ToList();
        private static List<string> _mNames = File.ReadLines("Content/Data/Names/maleNames.txt").ToList();

        public static EducationLevel GenerateEducationLevel(int age)
        {
            if (age < 5)
                return EducationLevel.None;
            if (age < 15)
                return (EducationLevel)_rnd.Next(-1, 1);
            if (age < 18)
                return (EducationLevel)_rnd.Next(0, 2);
            if (age >= 18)
                return (EducationLevel)_rnd.Next(0, 3);

            return EducationLevel.None;
        }

        public static string GenerateName(Gender gender)
        {
            string retval = "";
            if(gender == Gender.Female)
            {
                
                retval += _fNames[_rnd.Next(0, _fNames.Count)];
            }
            else
            {
                retval += _mNames[_rnd.Next(0, _mNames.Count)];
            }
            retval += " ";
            retval += _last[_rnd.Next(0, _last.Count)];
            return retval;
        }

        public static DateTime GenerateDOB(int age)
        {
            DateTime retval = GameClock.GameTime.Date;
            retval = retval.AddYears(-(age + 1));
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

            return AgeRange.None;
        }

        public static int GenerateAge(DateTime dob)
        {
            // (Now - DOB) / 10000 (to remove the last four digits (month/day))
            return (int.Parse(GameClock.GameTime.Date.ToString("yyyyMMdd")) - int.Parse(dob.ToString("yyyyMMdd"))) / 10000;
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
