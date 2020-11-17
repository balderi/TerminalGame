using System.Collections.Generic;
using TerminalGame.Utils;
using TerminalGame.Computers.Utils;
using TerminalGame.Companies;
using TerminalGame.Files.FileSystem;
using System.Runtime.Serialization;
using TerminalGame.Tracers;
using TerminalGame.Computers.Events;
using System;
using System.Linq;
using TerminalGame.People;

namespace TerminalGame.Computers
{
    [DataContract(IsReference = true)]
    public class Computer : IComputer
    {
        // TODO: Value determining difficulty to break in/get traced/whatevs.

        #region fields
        private readonly int[] _defaultPorts = { 22, 25, 80, 443 };
        private bool _isInitialized;
        private string _publicName;
        private Random _rnd = new Random(); // TEMP
        #endregion

        #region properties
        /// <summary>
        /// Returns computer name with split-identifier.
        /// To get the proper name, use the <c>GetPublicName</c> method instead.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string RootPassword { get; set; }

        [DataMember]
        public bool IsPlayerConnected { get; set; }

        [DataMember]
        public bool PlayerHasRoot { get; set; }

        [DataMember]
        public bool IsMissionObjective { get; set; }

        [DataMember]
        public bool IsShownOnMap { get; set; }

        [DataMember]
        public bool IsOnline { get; set; }

        [DataMember]
        public List<int> OpenPorts { get; set; }

        [DataMember]
        public float MapX { get; set; }

        [DataMember]
        public float MapY { get; set; }

        [DataMember]
        public AccessLevel AccessLevel { get; set; }

        [DataMember]
        public ComputerType ComputerType { get; set; }

        [DataMember]
        public Company Owner { get; set; }

        [DataMember]
        public FileSystem FileSystem { get; set; }

        [DataMember]
        public double TraceTime { get; set; }

        [DataMember]
        public List<User> Users { get; set; }

        public TerminalGame Game { get; set; }
        #endregion

        public event EventHandler<ConnectedEventArgs> OnConnected;
        public event EventHandler<DisconnectedEventArgs> OnDisconnected;
        public event EventHandler<IllegalActionEventArgs> OnIllegalAction;

        public Computer()
        {
            
        }

        public Computer(string name, string ip = "", string rootPassword = "", FileSystem fileSystem = null)
        {
            Name = name;

            if (string.IsNullOrWhiteSpace(ip))
                IP = Generators.GenerateIP();
            else
                IP = ip;

            if (string.IsNullOrWhiteSpace(rootPassword))
                RootPassword = Generators.GeneratePassword();
            else
                RootPassword = rootPassword;

            FileSystem = fileSystem;

            TraceTime = _rnd.NextDouble();

            _isInitialized = false;
        }

        public Computer(string name, int[] ports, ComputerType type, string ip = "", string rootPassword = "", FileSystem fileSystem = null) : this(name, ip, rootPassword, fileSystem)
        {
            OpenPorts = BuildPorts(ports);
            ComputerType = type;
        }

        public Computer (string name, int[] ports, ComputerType type, Company owner, string ip = "", string rootPassword = "", FileSystem fileSystem = null) : this(name, ip, rootPassword, fileSystem)
        {
            OpenPorts = BuildPorts(ports);
            ComputerType = type;
            Owner = owner;
        }

        public void Init(TerminalGame game)
        {
            if(!_isInitialized)
            {
                IsPlayerConnected = false;
                PlayerHasRoot = true;
                IsMissionObjective = false;
                IsShownOnMap = true;
                IsOnline = true;
                Game = game;
                SetPublicName();
                _isInitialized = true;
            }
        }

        public string GetPublicName()
        {
            return _publicName;
        }

        public void SetPublicName()
        {
            _publicName = Name.Contains("§¤§") ? Name.Replace("§¤§", "\n") : Name + "\n" + ComputerType.ToString();
        }

        /// <summary>
        /// Connect the player to this computer
        /// </summary>
        /// <returns><c>true</c> if connection sucessful. <c>false</c> otherwise.</returns>
        public bool Connect()
        {
            if (IsOnline)
            {
                World.World.GetInstance().Player.ConnectedComp.Disconnect();
                World.World.GetInstance().Player.ConnectedComp = this;
                IsPlayerConnected = true;
                OnConnected?.Invoke(this, new ConnectedEventArgs(World.World.GetInstance().CurrentGameTime));
                PerformIllegalAction(); // TEMP
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disconnect the player from this computer
        /// </summary>
        public void Disconnect(bool forced = false)
        {
            World.World.GetInstance().Player.ConnectedComp = World.World.GetInstance().Player.PlayerComp;
            IsPlayerConnected = false;
            if(ActiveTracer.GetInstance().IsActive)
                ActiveTracer.GetInstance().StopTrace();
            OnDisconnected?.Invoke(this, new DisconnectedEventArgs(World.World.GetInstance().CurrentGameTime));
            if(forced)
            {
                Game.Terminal.WriteLine("Connection closed by remote host");
                MusicManager.GetInstance().ChangeSong("gameBgm", 0.5f);
            }
        }

        /// <summary>
        /// Attempt to log in on this computer
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="pass">Password</param>
        /// <returns><c>true</c> if user/pass combination is correct, <c>false</c> otherwise</returns>
        public bool Login(string user, string pass)
        {
            return true;
        }

        /// <summary>
        /// To be called when a user performs an illegal action on this computer.
        /// </summary>
        public void PerformIllegalAction()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            if(this != World.World.GetInstance().Player.PlayerComp)
                ActiveTracer.GetInstance().StartTrace(TraceTime); // TODO: base tracetime on computer difficulty or something
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            OnIllegalAction?.Invoke(this, new IllegalActionEventArgs(World.World.GetInstance().CurrentGameTime));
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
        /// Get a list of open ports on the computer.
        /// </summary>
        /// <returns>A list of open ports.</returns>
        public List<int> GetOpenPorts() => OpenPorts;

        /// <summary>
        /// Checks whether or nat a given port is open on the computer.
        /// </summary>
        /// <param name="port">The port number as <c>int</c></param>
        /// <returns><c>true</c> if the port is open, otherwise <c>false</c>.</returns>
        public bool CheckPortOpen(int port) => OpenPorts.Exists(x => x == port);

        /// <summary>
        /// Build a list of open ports from array.
        /// </summary>
        /// <param name="ports"><c>int array</c> of open ports.</param>
        /// <returns>A list of open ports.</returns>
        public List<int> BuildPorts(int[] ports) =>  ports.ToList();

        public override string ToString()
        {
            return _publicName + "\n" + IP;
        }

        public void Tick()
        {
            if(FileSystem.RootDir == null)
            {
                if(IsPlayerConnected)
                {
                    Disconnect(true);
                }
                IsOnline = false;
            }
        }
    }
}
