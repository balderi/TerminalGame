using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalGame.Companies;
using TerminalGame.Computers;
using TerminalGame.People;

namespace TerminalGame.World
{
    class World
    {
        private Random _rnd;

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

        /// <summary>
        /// Generate a new, random world.
        /// </summary>
        public void CreateWorld()
        {
            DateTime beginC = DateTime.Now;
            _rnd = new Random(DateTime.Now.Millisecond);
            Computers = new List<Computer>();
            People = new List<Person>();
            Companies = new List<Company>();

            Computer PlayerComp = new Computer("localhost", "127.0.0.1", Player.GetInstance().Password);

            Player.GetInstance().PlayerComp = PlayerComp;

            Computers.Add(PlayerComp);

            for (int i = 0; i < 200; i++)
            {
                Computers.Add(new Computer("Workstation" + i));
            }
            Console.WriteLine("Generated {0} computers in {1} seconds.", Computers.Count, (DateTime.Now.Subtract(beginC).TotalSeconds).ToString("N4"));

            foreach (Computer c in Computers)
            {
                c.Init();
            }
            
            //DateTime beginP = DateTime.Now;
            ////for (int i = 0; i < 1000; i++)
            ////    People.Add(new Person());
            //Parallel.For(0, 5, index =>
            //{
            //    for (int i = 0; i < 200; i++)
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

        }

        /// <summary>
        /// Load a previously saved world from file.
        /// </summary>
        /// <param name="fileName">Path to the save file.</param>
        public void Load(string fileName)
        {

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
