using Microsoft.Xna.Framework;
using System;
using TerminalGame.Computers.FileSystems;
using System.Collections.Generic;
using TerminalGame.Utils;

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
        public string LinksToLoad { get; set; }
        public Dictionary<int, string> OpenPorts { get; private set; }
        public RemoteUI RemoteUI { get; private set; }
        public float MapX { get; set; }
        public float MapY { get; set; }
        public float TraceTime { get; private set; }
        public ActiveTracer Tracer { get; private set; }

        public event EventHandler<ConnectEventArgs> Connected;
        public event EventHandler<ConnectEventArgs> Disonnected;
        
        private readonly int[] _defaultPorts = { 22, 25, 80, 443 };
        private Random _random;

        public Computer(Type type, string ip, string name, string rootPassword, float traceTime)
        {
            ComputerType = type;
            IP = ip;
            Name = name;
            RootPassword = rootPassword;
            TraceTime = traceTime;
            RemoteUI = CreateRemoteUI();
            LinkedComputers = new List<Computer>();
            Initialize();
        }

        public Computer(Type type, string ip, string name, string rootPassword, float traceTime, RemoteUI remoteUI) : this(type, ip, name, rootPassword, traceTime)
        {
            ComputerType = type;
            IP = ip;
            Name = name;
            RootPassword = rootPassword;
            RemoteUI = remoteUI;
            LinkedComputers = new List<Computer>();
            Initialize();
        }

        public Computer(Type type, string ip, string name, string rootPassword, float traceTime, FileSystem fileSystem) : this(type, ip, name, rootPassword, traceTime)
        {
            FileSystem = fileSystem;
        }

        public Computer(Type type, string ip, string name, string rootPassword, float traceTime, FileSystem fileSystem, int[] openPorts) : this(type, ip, name, rootPassword, traceTime, fileSystem)
        {
            OpenPorts = BuildPorts(openPorts);
        }

        private void Initialize()
        {
            IsShownOnMap = true;
            _random = new Random(DateTime.Now.Millisecond);
            if(OpenPorts == null)
                OpenPorts = BuildPorts(_defaultPorts);
            if(FileSystem == null)
            {
                FileSystem fs = new FileSystem();
                fs.BuildBasicFileSystem();
                FileSystem = fs;
            }
            Tracer = new ActiveTracer(TraceTime);
        }

        public void LoadLinks()
        {
            if(!String.IsNullOrEmpty(LinksToLoad))
            {
                string[] temp = LinksToLoad.Trim().Split(' ');
                foreach(string s in temp)
                {
                    Link(Computers.GetInstance().ComputerList[Convert.ToInt32(s)]);
                }
            }
        }

        public void AbortTrace()
        {
            Tracer.StopTrace();
        }

        /// <summary>
        /// Sets the player's current connection to this computer
        /// </summary>
        /// <param name="GoingHome">Don't disconnect before connecting. Only used during game start, as there is no computer to disconnect from</param>
        public void Connect(bool GoingHome = false)
        {
            if (GoingHome)
            {
                Player.GetInstance().ConnectedComputer = this;
                IsPlayerConnected = true;
            }
            else
            {
                Player.GetInstance().ConnectedComputer.Disconnect(true);
                Player.GetInstance().ConnectedComputer = this;
                IsPlayerConnected = true;
            }
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
            Tracer.StopTrace();
            Disonnected?.Invoke(null, new ConnectEventArgs(IP, PlayerHasRoot));
            Console.WriteLine("DISC: Disconnecting from " + IP);
            if (!reconnect)
            {
                Player.GetInstance().PlayersComputer.Connect(true);
            }
        }

        public void DoOffensiveAction()
        {
            if (!PlayerHasRoot)
            {
                Tracer.StartTrace();
            }
        }

        public bool Login(string user, string pass)
        {
            if((user == "root" || user == "admin") && pass == RootPassword)
            {
                GetRoot();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Toggles whether the computer is visible on the network map or not.
        /// </summary>
        public void ToggleShowOnMap() => IsShownOnMap = !IsShownOnMap;

        /// <summary>
        /// Makes the computer visible on the network map.
        /// </summary>
        public void ShowOnMap() => IsShownOnMap = true;

        /// <summary>
        /// Hides the computer on the network map.
        /// </summary>
        public void HideOnMap() => IsShownOnMap = false;

        /// <summary>
        /// Make the computer show up on the network map as a mission objective (does not change the visibility).
        /// </summary>
        public void SetAsObjective() => IsMissionObjective = true;

        /// <summary>
        /// Makes the computer show up on the network map as a regular computer (does not change the visibility).
        /// </summary>
        public void RemoveAsObjective() => IsMissionObjective = false;

        /// <summary>
        /// Get a list of open ports on the computer as a <c>int, string</c> dictionary.
        /// </summary>
        /// <returns>A list of open ports as a <c>int, string</c> dictionary.</returns>
        public Dictionary<int, string> GetOpenPorts() => OpenPorts;

        /// <summary>
        /// Checks whether or nat a given port is open on the computer.
        /// </summary>
        /// <param name="port">The port number as <c>int</c></param>
        /// <returns><c>true</c> if the port is open, otherwise <c>false</c>.</returns>
        public bool CheckPortOpen(int port)
        {
            foreach(int key in OpenPorts.Keys)
            {
                if (key == port)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Build the dictionary of open ports from array.
        /// </summary>
        /// <param name="ports"><c>int array</c> of open ports.</param>
        /// <returns>A list of open ports as a <c>int, string</c> dictionary.</returns>
        /// <remarks>This should probably be private.</remarks>
        public Dictionary<int,string> BuildPorts(int[] ports)
        {
            var retval = new Dictionary<int, string>();
            foreach(int port in ports)
            {
                retval.Add(port, Enum.IsDefined(typeof(KnownPorts), port) ? ((KnownPorts)port).ToString() : "Unknown");
            }
            return retval;
        }

        public int[] GetPortsArray()
        {
            int length = OpenPorts.Count;
            var retval = new int[length];
            int i = 0;
            foreach(KeyValuePair<int, string> port in OpenPorts)
            {
                retval[i++] = port.Key;
            }
            return retval;
        }

        public string GetPortsString()
        {
            string retval = "";
            var ports = GetPortsArray();
            int length = ports.Length;
            for(int i = 0; i < length; i++)
            {
                retval += " " + ports[i];
            }

            return retval;
        }

        private RemoteUI CreateRemoteUI()
        {
            return new RemoteUI(this);
        }

        /// <summary>
        /// Set the speed of the computer.
        /// </summary>
        /// <param name="speed">The new computer speed.</param>
        /// <remarks>This is not fully implemented yet.</remarks>
        public void SetSpeed(float speed) => Speed = speed;

        /// <summary>
        /// Change the speed of the comuter by a percentage.
        /// </summary>
        /// <param name="changePercentage">The percentage to change the speed by.</param>
        public void ChangeSpeed(float changePercentage) => Speed *= (1 + changePercentage);

        /// <summary>
        /// Set a new password.
        /// </summary>
        /// <param name="password">The new password.</param>
        public void ChangePassword(string password) => RootPassword = password;

        /// <summary>
        /// Link this computer to a different computer.
        /// Also creates a visual link in the for of a line on the network map.
        /// </summary>
        /// <param name="computer">The computer to link this to.</param>
        /// <remarks>Not fully implemented.</remarks>
        public void Link(Computer computer)
        {
            if (!LinkedComputers.Contains(computer))
                LinkedComputers.Add(computer);
        }

        /// <summary>
        /// For generic actions
        /// </summary>
        /// <param name="Source">Source system</param>
        /// <param name="Action">The action performed e.g. gained root</param>
        public void GenerateLog(Computer Source, string Action)
        {
            FileSystem.AddFileToDir("log", String.Format("{0} {1} {2}", DateTime.Now.ToShortTimeString(), Source.IP, Action).Replace(' ', '_'), String.Format("{0} {1}", DateTime.Now.ToShortTimeString(), Source.IP, Action));
        }

        /// <summary>
        /// For actions on files
        /// </summary>
        /// <param name="Source">Source system</param>
        /// <param name="Action">The action performed e.g. deleted file</param>
        /// <param name="AffectedFile">The affected file</param>
        public void GenerateLog(Computer Source, string Action, File AffectedFile)
        {
            FileSystem.AddFileToDir("log", String.Format("{0} {1} {2}", DateTime.Now.ToShortTimeString(), Source.IP, Action, AffectedFile.Name).Replace(' ', '_'), String.Format("{0} {1}", DateTime.Now.ToShortTimeString(), Source.IP, Action, AffectedFile.Name));
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
