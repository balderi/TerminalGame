using Microsoft.Xna.Framework;
using System;
using TerminalGame.Computers.FileSystems;
using System.Collections.Generic;
using TerminalGame.Utilities;

namespace TerminalGame.Computers
{
    class Computer
    {
        public enum Type { Workstation, Server, Mainframe, Laptop }
        public enum AccessLevel { root, user }
        public Type ComputerType { get; private set; }
        public AccessLevel Access { get; private set; }
        public float Speed { get; private set; }
        public string IP { get; private set; }
        public string Name { get; private set; }
        public string RootPassword { get; private set; }
        public bool IsPlayerConnected { get; private set; }
        public bool PlayerHasRoot { get; private set; }
        public bool IsMissionObjective { get; private set; }
        public bool IsShownOnMap { get; private set; }
        public FileSystem FileSystem { get; private set; }
        public List<Computer> LinkedComputers { get; private set; }
        public Dictionary<int, string> OpenPorts { get; private set; }

        public event EventHandler<ConnectEventArgs> Connected;
        public event EventHandler<ConnectEventArgs> Disonnected;

        private enum KnownPorts
        {
            FTP = 21,
            SSH = 22,
            Telnet = 23,
            SMTP = 25,
            DNS = 53,
            DHCP = 67,
            TFTP = 69,
            HTTP = 80,
            POP = 110,
            NTP = 123,
            IMAP = 143,
            HTTPS = 443
        };

        private readonly int[] _defaultPorts = { 22, 25, 80, 443 };

        public Computer(Type type, string ip, string name, string rootPassword)
        {
            ComputerType = type;
            IP = ip;
            Name = name;
            RootPassword = rootPassword;
            LinkedComputers = new List<Computer>();
            Initialize();
        }

        public Computer(Type type, string ip, string name, string rootPassword, FileSystem fileSystem) : this (type, ip, name, rootPassword)
        {
            FileSystem = fileSystem;
        }

        public Computer(Type type, string ip, string name, string rootPassword, FileSystem fileSystem, int[] openPorts) : this(type, ip, name, rootPassword, fileSystem)
        {
            OpenPorts = BuildPorts(openPorts);
        }

        private void Initialize()
        {
            if(OpenPorts == null)
                OpenPorts = BuildPorts(_defaultPorts);
            if(FileSystem == null)
            {
                FileSystem fs = new FileSystem();
                fs.BuildBasicFileSystem();
                this.FileSystem = fs;
            }
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

        public void ToggleShowOnMap() => IsShownOnMap = !IsShownOnMap;

        public void SetAsObjective() => IsMissionObjective = true;

        public void RemoveAsObjective() => IsMissionObjective = false;

        public Dictionary<int, string> GetOpenPorts() => OpenPorts;

        public bool CheckPortOpen(int port)
        {
            foreach(int key in OpenPorts.Keys)
            {
                if (key == port)
                    return true;
            }
            return false;
        }

        public Dictionary<int,string> BuildPorts(int[] ports)
        {
            var retval = new Dictionary<int, string>();
            int p = ports.Length;
            string service;
            for(int i = 0; i < p; i++)
            {
                service = Enum.IsDefined(typeof(KnownPorts), ports[i]) ? ((KnownPorts)ports[i]).ToString() : "Unknown";
                retval.Add(ports[i], service);
            }
            return retval;
        }

        public void BuildBasicFileSystem()
        {
            FileSystem = new FileSystem();
            string[] baseDirs = { "bin", "usr", "home", "sys", "logs" };
            for (int i = 0; i < baseDirs.Length; i++)
            {
                FileSystem.AddDir(baseDirs[i]);
            }
        }

        public void SetSpeed(float speed) => Speed = speed;

        public void ChangeSpeed(float changePercentage) => Speed *= (1 + changePercentage);

        public void ChangePassword(string password) => RootPassword = password;

        public void Link(Computer computer)
        {
            if (!LinkedComputers.Contains(computer))
                LinkedComputers.Add(computer);
            //if (!computer.LinkedComputers.Contains(this))
            //    computer.Link(this);
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
        public void GetRoot() => PlayerHasRoot = true;

        public void Update(GameTime gameTime) => Access = PlayerHasRoot ? AccessLevel.root : AccessLevel.user;
    }
}
