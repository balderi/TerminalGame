using Microsoft.Xna.Framework;
using System;
using TerminalGame.Computers.FileSystems;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TerminalGame.Computers
{
    class Computer
    {
        public enum Type { Workstation, Server, Mainframe, Laptop }

        readonly Type type;
        public enum AccessLevel { root, user }
        AccessLevel access;

        public string IP { get; private set; }
        public string Name { get; private set; }
        public string RootPassword { get; private set; }
        public bool IsPlayerConnected { get; private set; }
        public bool PlayerHasRoot { get; private set; }
        public FileSystem FileSystem { get; private set; }
        public List<Computer> LinkedComputers { get; private set; }

        public event EventHandler<ConnectEventArgs> Connected;
        public event EventHandler<ConnectEventArgs> Disonnected;
        
        public Computer(Type type, string IP, string Name, string RootPassword, FileSystem FileSystem)
        {
            this.type = type;
            this.IP = IP;
            this.Name = Name;
            this.RootPassword = RootPassword;
            this.FileSystem = FileSystem;
            LinkedComputers = new List<Computer>();
        }

        public Computer(Type type, string IP, string Name, string RootPassword)
        {
            this.type = type;
            this.IP = IP;
            this.Name = Name;
            this.RootPassword = RootPassword;
            BuildBasicFileSystem();
            LinkedComputers = new List<Computer>();
        }

        /// <summary>
        /// Sets the player's current connection to this computer
        /// </summary>
        /// <param name="GoingHome">Don't disconnect before connecting. Only used during game start, as there is no computer to disconnect from</param>
        public void Connect(bool GoingHome = false)
        {
            if (GoingHome)
            {
                Console.WriteLine("*** CONN: GOING HOME");
                Player.GetInstance().ConnectedComputer = this;
                IsPlayerConnected = true;
            }
            else
            {
                Player.GetInstance().ConnectedComputer.Disconnect(true);
                Player.GetInstance().ConnectedComputer = this;
                IsPlayerConnected = true;
            }
            Console.WriteLine("CONN: Calling Connected?.Invoke with IP:" + IP + " and PHR: " + PlayerHasRoot.ToString());
            Connected?.Invoke(null, new ConnectEventArgs(IP, PlayerHasRoot));
            Console.WriteLine("CONN: Connected to " + IP);
        }

        /// <summary>
        /// Disconnects the player from this computer.
        /// </summary>
        /// <param name="reconnect">Only set to true when called from Connect.</param>
        public void Disconnect(bool reconnect = false)
        {
            IsPlayerConnected = false;

            Console.WriteLine("DISC: Calling Disconnected?.Invoke");
            Disonnected?.Invoke(null, new ConnectEventArgs(IP, PlayerHasRoot));
            Console.WriteLine("DISC: Disconnecting from " + IP);
            if (reconnect)
            {
                Console.WriteLine("DISC: RECONNECT");
            }
            else
            {
                Console.WriteLine("*** DISC: GOING HOME");
                Player.GetInstance().PlayersComputer.Connect(true);
            }
        }

        private void BuildBasicFileSystem()
        {
            FileSystem = new FileSystem();
            string[] baseDirs = { "bin", "usr", "home", "sys", "logs" };
            for (int i = 0; i < baseDirs.Length; i++)
            {
                FileSystem.AddDir(baseDirs[i]);
            }
        }

        public void Link(Computer computer)
        {
            if (!LinkedComputers.Contains(computer))
                LinkedComputers.Add(computer);
            if (!computer.LinkedComputers.Contains(this))
                computer.Link(this);
        }

        /// <summary>
        /// For generic actions
        /// </summary>
        /// <param name="Source">Source system</param>
        /// <param name="Action">The action performed e.g. gained root</param>
        public void GenerateLog(Computer Source, string Action)
        {
            FileSystem.AddFileToDir("logs", String.Format("{0} {1} {2}", DateTime.Now.ToShortTimeString(), Source.IP, Action).Replace(' ', '_'), String.Format("{0} {1}", DateTime.Now.ToShortTimeString(), Source.IP, Action));
        }

        /// <summary>
        /// For actions on files
        /// </summary>
        /// <param name="Source">Source system</param>
        /// <param name="Action">The action performed e.g. deleted file</param>
        /// <param name="AffectedFile">The affected file</param>
        public void GenerateLog(Computer Source, string Action, File AffectedFile)
        {
            FileSystem.AddFileToDir("logs", String.Format("{0} {1} {2}", DateTime.Now.ToShortTimeString(), Source.IP, Action, AffectedFile.Name).Replace(' ', '_'), String.Format("{0} {1}", DateTime.Now.ToShortTimeString(), Source.IP, Action, AffectedFile.Name));
        }

        /// <summary>
        /// For traffic routing logs, when e.g. used as proxy
        /// </summary>
        /// <param name="Source">Source system</param>
        /// <param name="Dest">Destination system</param>
        public void GenerateLog(Computer Source, Computer Dest)
        {

        }

        /// <summary>
        /// Grant the player eleveted (root) permission on this computer.
        /// </summary>
        public void GetRoot()
        {
            PlayerHasRoot = true;
        }

        public void Update(GameTime gameTime)
        {
            access = PlayerHasRoot ? AccessLevel.root : AccessLevel.user;
        }
    }
}
