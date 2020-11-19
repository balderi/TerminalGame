using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TerminalGame.Companies;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.Files;
using TerminalGame.Files.FileSystem;
using TerminalGame.Files.FileSystem.Generator;
using TerminalGame.People;
using TerminalGame.People.Utils;
using TerminalGame.Time;

namespace TerminalGame.World
{
    [DataContract]
    public class World
    {
        [DataMember]
        public DateTime CurrentGameTime { get; set; }

        [DataMember]
        public Player Player { get { return Player.GetInstance(); } set { Player = value; } }

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
        public TerminalGame Game { get; set; }

        private static World _instance;

        public static World GetInstance(string loadFromFile = "")
        {
            if (!string.IsNullOrWhiteSpace(loadFromFile))
            {
                _instance = Load(loadFromFile);
                _instance.FixWorld();
            }
            if (_instance == null)
            {
                _instance = new World();
            }
            return _instance;
        }

        private World()
        {
        }

        private bool CheckIfNameExists(string name)
        {
            foreach (Company c in CompanyList)
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
        public void CreateWorld(TerminalGame game, int companies)
        {
            DateTime beginWorld = DateTime.Now;
            Game = game;
            GameClock.Initialize();
            CurrentGameTime = GameClock.GameTime;
            //Player.GetInstance().CreateNewPlayer("testPlayer", "abc123");
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
            var bin = new Files.File("bin");
            bin.AddFile(new Files.File("terminal", "terminal executable", FileType.Binary, 48374));
            bin.AddFile(new Files.File("notepad", "notepad executable", FileType.Binary, 7522));
            bin.AddFile(new Files.File("remoteview", "remoteview executable", FileType.Binary, 61325));
            bin.AddFile(new Files.File("netmap", "netmap executable", FileType.Binary, 879820));
            pfs.RootDir.AddFile(bin);

            Company mCorp = new Company("Mission Corp");
            Computer mComp = new Computer("Mission Hub", new int[] { 80 }, ComputerType.Server, mCorp, "123.123.123.123", "swordfish", FileSystemGenerator.GenerateDefaultFilesystem());
            mCorp.GetComputers.Add(mComp);
            CompanyList.Add(mCorp);

            Company pc = new Company("Unknown", new Person("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), new Person("Unknown", DateTime.Parse("1970-01-01"), (Gender)(-1), (EducationLevel)(-1)), 0, 0); ;

            Computer PlayerComp = new Computer("localhost", new int[] { 69, 1337 }, ComputerType.Workstation, pc, "127.0.0.1", Player.Password, pfs)
            {
                Game = game
            };

            Computers.Add(PlayerComp);

            DateTime beginCompanies = DateTime.Now;
            for (int i = 0; i < companies; i++)
            {
                Console.Write($"Creating company {i}/{companies}...");
                Company comp = Company.GetRandomCompany(game);
                int whoops = 0;
                while (CheckIfNameExists(comp.Name))
                {
                    comp.Name = Companies.Generator.CompanyGenerator.GenerateName();
                    whoops++;
                }
                comp.GenerateComputers();
                Console.WriteLine($"Done: {comp.Name}, whoopses: {whoops}.");
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

            foreach (string s in test)
                Console.WriteLine(s);

            foreach (Computer c in Computers)
            {
                c.Init(Game);
                if (c.Owner == CompanyList[35])
                {
                    c.IsMissionObjective = true;
                }
            }

            PlayerComp.PlayerHasRoot = true;
            PlayerComp.AccessLevel = AccessLevel.Root;

            Player.GetInstance().PlayerComp = PlayerComp;

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
        /// Sets the public names of computers, which is usually done during creation.
        /// </summary>
        public void FixWorld()
        {
            foreach (Computer c in Computers)
            {
                c.SetPublicName();
                c.Game = Game; // just in case...
            }
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
            var fileName = $"Saves/save_{Player.Name}.tgs";
            Console.WriteLine($"*** Writing to file {fileName}...");
            StreamWriter writer = new StreamWriter(fileName);
            JsonSerializer jsonSerializer = new JsonSerializer
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };
            jsonSerializer.Serialize(writer, this);
            writer.Flush();
            writer.Close();
            Console.WriteLine("*** Done writing to file!");
        }
    }
}
