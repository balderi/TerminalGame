using System;
using System.Collections.Generic;
using System.Linq;
using TerminalGame.Companies;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.Files;
using TerminalGame.Files.FileSystem;
using TerminalGame.People;
using TerminalGame.People.Utils;
using TerminalGame.Time;

namespace TerminalGame.World
{
    class World
    {
        //private Random _rnd;

        /// <summary>
        /// A list of all computers in the world.
        /// </summary>
        public List<Computer> Computers { get; private set; }

        /// <summary>
        /// A list of all people in the world.
        /// </summary>
        public List<Person> People { get; private set; }

        /// <summary>
        /// A list of all companies in the world.
        /// </summary>
        public List<Company> Companies { get; private set; }

        private static World _instance;

        public static World GetInstance()
        {
            if (_instance == null)
                _instance = new World();
            return _instance;
        }

        private World()
        {
        }

        private bool CheckName(string name)
        {
            foreach(Company c in Companies)
            {
                if (c.Name == name)
                    return true;
            }
            return false;
        }

        static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = e.OrderByDescending(s => s).ThenBy(r => r.Length);
            return sorted.Reverse();
        }

        /// <summary>
        /// Generate a new, random world.
        /// </summary>
        public void CreateWorld()
        {
            GameClock.Initialize();
            //_rnd = new Random(DateTime.Now.Millisecond);
            Computers = new List<Computer>();
            People = new List<Person>();
            Companies = new List<Company>();

            File testFile = new File("testFile", "this is a test", FileType.Text);
            File testSubDir = new File("test2");
            testSubDir.AddFile(testFile);
            File testDir = new File("test");
            testDir.AddFile(testSubDir);
            File testRoot = new File("");
            testRoot.AddFile(testDir);
            File testFile2 = new File("testFile2", "test 2", FileType.Text);
            testRoot.AddFile(testFile2);
            FileSystem pfs = new FileSystem(testRoot);

            Company pc = new Company("Unknown", new Person("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), new User("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), 0, 0); ;

            Computer PlayerComp = new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, pc, "127.0.0.1", Player.GetInstance().Password, pfs); //new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, "127.0.0.1", Player.GetInstance().Password, pfs);

            Player.GetInstance().PlayerComp = PlayerComp;

            Computers.Add(PlayerComp);

            //for (int i = 0; i < 200; i++)
            //{
            //    Computers.Add(new Computer("Workstation" + i));
            //}
            //Console.WriteLine("Generated {0} computers in {1} seconds.", Computers.Count, (DateTime.Now.Subtract(beginC).TotalSeconds).ToString("N4"));
            DateTime beginCompanies = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                Company comp = new Company();

                while (CheckName(comp.Name))
                    comp = new Company();
                
                Companies.Add(comp);
            }
            Console.WriteLine("Generated {0} companies in {1} seconds.", Companies.Count, (DateTime.Now.Subtract(beginCompanies).TotalSeconds).ToString("N4"));

            List<string> names = new List<string>();

            foreach (Company c in Companies)
            {
                Computers.AddRange(c.GetComputers);
                names.Add(c.Name);
            }

            List<string> test = SortByLength(names.AsEnumerable()).ToList();

            foreach(string s in test)
                Console.WriteLine(s);

            foreach (Computer c in Computers)
            {
                c.Init();
            }
            //DateTime beginP = DateTime.Now;
            ////for (int i = 0; i < 1000; i++)
            ////    People.Add(new Person());
            //Parallel.For(0, 5, index =>
            //{
            //    for (int i = 0; i < 2000; i++)
            //    {
            //        People.Add(new Person());
            //    }
            //    Console.WriteLine(index);
            //});
            //Console.WriteLine("Generated {0} people in {1} seconds.", People.Count, (DateTime.Now.Subtract(beginP).TotalSeconds).ToString("N4"));
        }

        /// <summary>
        /// Update all entities in the current world.
        /// </summary>
        public void Tick()
        {
            foreach (var computer in Computers)
                computer.Tick();
            foreach (var person in People)
                person.Tick();
            foreach (var company in Companies)
                company.Tick();
        }

        /// <summary>
        /// Load a previously saved world from file.
        /// </summary>
        /// <param name="fileName">Path to the save file.</param>
        public static World Load(string fileName)
        {
            return new World(); // TEMP
        }

        /// <summary>
        /// Save the current world to file.
        /// </summary>
        /// <param name="fileName">Path to the save file.</param>
        public void Save(string fileName)
        {

        }
    }
}
