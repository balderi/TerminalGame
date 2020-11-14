using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
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
    [DataContract]
    public class World
    {
        private Random _rnd = new Random();

        [DataMember]
        public DateTime CurrentGameTime { get; set; }

        [DataMember]
        public Player Player { get; set; }

        /// <summary>
        /// A list of all people in the world.
        /// </summary>
        [DataMember]
        public List<Person> People { get; set; }

        /// <summary>
        /// A list of all computers in the world.
        /// </summary>
        [DataMember]
        public List<Computer> Computers { get; set; }

        /// <summary>
        /// A list of all companies in the world.
        /// </summary>
        [DataMember]
        public List<Company> CompanyList { get; set; }

        private static World _instance;

        public static World GetInstance(string loadFromFile = "")
        {
            if (_instance == null)
            {
                if (!string.IsNullOrWhiteSpace(loadFromFile))
                {
                    _instance = Load(loadFromFile);
                    _instance.FixWorld();
                }
                else
                    _instance = new World();
            }
            return _instance;
        }

        private World()
        {
        }

        private bool CheckIfNameExists(string name)
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
            DateTime beginWorld = DateTime.Now;
            GameClock.Initialize();
            CurrentGameTime = GameClock.GameTime;
            Player = new Player();
            Player.CreateNewPlayer("testPlayer", "abc123");
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
            //CompanyList.Add(pc);

            Computer PlayerComp = new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, pc, "127.0.0.1", Player.Password, pfs); //new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, "127.0.0.1", World.World.GetInstance().Player.Password, pfs);
            //pc.GetComputers.Add(PlayerComp);

            Player.PlayerComp = PlayerComp;

            Computers.Add(PlayerComp);

            DateTime beginCompanies = DateTime.Now;
            for (int i = 0; i < 500; i++)
            {
                Company comp = Company.GetRandomCompany();
                int whoops = 0;
                while (CheckIfNameExists(comp.Name))
                {
                    comp.Name = Companies.Generator.CompanyGenerator.GenerateName();
                    whoops++;
                }
                comp.GenerateComputers();
                Console.WriteLine(comp.Name + " whoopses: " + whoops.ToString());
                CompanyList.Add(comp);
            }
            Console.WriteLine("Generated {0} companies in {1} seconds.", CompanyList.Count, (DateTime.Now.Subtract(beginCompanies).TotalSeconds).ToString("N4"));

            List<string> names = new List<string>();

            foreach (Company c in CompanyList)
            {
                Computers.AddRange(c.GetComputers);
                People.AddRange(c.GetPeople);
                names.Add(c.Name);
            }

            List<string> test = SortByLength(names.AsEnumerable()).ToList();

            foreach(string s in test)
                Console.WriteLine(s);

            foreach (Computer c in Computers)
            {
                c.Init();
            }
            Console.WriteLine("Generated world in {0} seconds.", (DateTime.Now.Subtract(beginWorld).TotalSeconds).ToString("N4"));
        }

        public bool TryGetComputerByIp(string ip, out Computer computer) => (computer = Computers.Find(x => x.IP == ip)) != null;

        public bool TryGetComputerByName(string name, out Computer computer) => (computer = Computers.Find(x => x.Name == name)) != null;

        public bool TryGetPersonByName(string name, out Person person) => (person = People.Find(x => x.Name == name)) != null;

        public bool TryGetPersonByEmail(string email, out Person person) => (person = People.Find(x => x.Email == email)) != null;

        /// <summary>
        /// Update all entities in the current world.
        /// </summary>
        public void Tick()
        {
            CurrentGameTime = GameClock.GameTime;
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
            foreach (Computer c in Computers)
                c.SetPublicName();
        }

        /// <summary>
        /// Load a previously saved world from file.
        /// </summary>
        /// <param name="fileName">Path to the save file.</param>
        public static World Load(string fileName)
        {
            //GameClock.Initialize();

            StreamReader reader = new StreamReader(fileName);
            JsonSerializer jsonSerializer = new JsonSerializer
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                //Formatting = Newtonsoft.Json.Formatting.Indented
            };
            World w = (World)jsonSerializer.Deserialize(reader, typeof(World));
            GameClock.Initialize(w.CurrentGameTime.ToString());
            w.FixWorld();
            return w;
        }

        /// <summary>
        /// Save the current world to file.
        /// </summary>
        public void Save()
        {
            Utils.IO.CheckAndCreateDirectory("Saves");

            StreamWriter writer = new StreamWriter(@"Saves\save_" + Player.Name + ".tgs");
            JsonSerializer jsonSerializer = new JsonSerializer
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                //Formatting = Newtonsoft.Json.Formatting.Indented
            };
            jsonSerializer.Serialize(writer, this);
            writer.Flush();
            writer.Close();
            Console.WriteLine("*** Done writing to file!");
        }
    }
}
