using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
    public class World
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
        public List<Company> CompanyList { get; private set; }

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
            foreach(Company c in CompanyList)
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
            CompanyList = new List<Company>();

            Files.File testFile = new Files.File("testFile", "this is a test", FileType.Text);
            Files.File testSubDir = new Files.File("test2");
            testSubDir.AddFile(testFile);
            Files.File testDir = new Files.File("test");
            testDir.AddFile(testSubDir);
            Files.File testRoot = new Files.File("");
            testRoot.AddFile(testDir);
            Files.File testFile2 = new Files.File("testFile2", "test 2", FileType.Text);
            testRoot.AddFile(testFile2);
            FileSystem pfs = new FileSystem(testRoot);

            Company pc = new Company("Unknown", new Person("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), new Person("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), 0, 0); ;

            Computer PlayerComp = new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, pc, "127.0.0.1", Player.GetInstance().Password, pfs); //new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, "127.0.0.1", Player.GetInstance().Password, pfs);

            Player.GetInstance().PlayerComp = PlayerComp;

            Computers.Add(PlayerComp);

            //for (int i = 0; i < 200; i++)
            //{
            //    Computers.Add(new Computer("Workstation" + i));
            //}
            //Console.WriteLine("Generated {0} computers in {1} seconds.", Computers.Count, (DateTime.Now.Subtract(beginC).TotalSeconds).ToString("N4"));
            DateTime beginCompanies = DateTime.Now;
            for (int i = 0; i < 500; i++)
            {
                Company comp = new Company();
                int whoops = 0;
                while (CheckName(comp.Name))
                {
                    comp.Name = Companies.Generator.CompanyGenerator.GenerateName();
                    whoops++;
                }
                Console.WriteLine(comp.Name + " whoopses: " + whoops.ToString());
                CompanyList.Add(comp);
            }
            Console.WriteLine("Generated {0} companies in {1} seconds.", CompanyList.Count, (DateTime.Now.Subtract(beginCompanies).TotalSeconds).ToString("N4"));

            List<string> names = new List<string>();

            foreach (Company c in CompanyList)
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
            foreach (var person in People)
                person.Tick();
            foreach (var computer in Computers)
                computer.Tick();
            foreach (var company in CompanyList)
                company.Tick();
        }

        /// <summary>
        /// Fixes relationship between companies, computers and files
        /// which are stripped when saving, to prevent circular dependencies
        /// </summary>
        public void FixWorld()
        {
            if(CompanyList.Count > 0)
            {
                foreach(Company c in CompanyList)
                {
                    c.FixComputers();
                }
            }
        }

        /// <summary>
        /// Load a previously saved world from file.
        /// </summary>
        /// <param name="fileName">Path to the save file.</param>
        public static World Load(string fileName)
        {
            TextReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(World));
            World w = (World)serializer.Deserialize(reader);
            w.FixWorld();
            return w;
        }

        /// <summary>
        /// Save the current world to file.
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");
            TextWriter writer = new StreamWriter(@"Saves\save_" + Player.GetInstance().Name + ".tgs");
            XmlSerializer serializer = new XmlSerializer(typeof(World));
            serializer.Serialize(writer, this);
        }
    }
}
