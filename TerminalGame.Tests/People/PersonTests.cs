using NUnit.Framework;
using System;
using TerminalGame.People.Utils;

namespace TerminalGame.People.Tests
{
    [TestFixture]
    public class PersonTests
    {
        Person testPerson1, testPerson2, testPerson3;

        [SetUp]
        public void Setup()
        {
            Time.GameClock.Initialize();

            testPerson1 = new Person();
            testPerson2 = new Person("Test Person", DateTime.Parse("1987/03/08"), Gender.Female, EducationLevel.Secondary);
            testPerson3 = new Person(AgeRange.Adult);
        }

        [Test]
        public void PersonTest1()
        {
            Assert.IsNull(testPerson1.Name);
            Assert.AreEqual(0, testPerson1.Age);
            Assert.AreEqual((EducationLevel)0, testPerson1.Education);
            Assert.AreEqual((Gender)0, testPerson1.Gender);
            Assert.AreEqual((AgeRange)0, testPerson1.AgeRange);
        }

        [Test]
        public void PersonTest2()
        {
            Assert.AreEqual("Test Person", testPerson2.Name);
            Assert.AreEqual(Gender.Female, testPerson2.Gender);
            Assert.AreEqual(AgeRange.Preteen, testPerson2.AgeRange);
        }

        [Test]
        public void PersonTest3()
        {
            Assert.IsTrue(testPerson3.Age >= 20);
            Assert.IsTrue(testPerson3.Age <= 44);
            Assert.IsNotNull(testPerson3.Name);
            Assert.IsNotNull(testPerson3.DOB);
        }
    }
}