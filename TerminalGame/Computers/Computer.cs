using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TerminalGame.Utils;
using TerminalGame.Computers.Utils;
using TerminalGame.Companies;
using TerminalGame.Files.FileSystem;

namespace TerminalGame.Computers
{
    public class Computer : IComputer
    {
        #region fields
        private readonly int[] _defaultPorts = { 22, 25, 80, 443 };
        private bool _isInitialized;
        private string _publicName;
        #endregion

        #region properties
        /// <summary>
        /// Returns computer name with split-identifier.
        /// To get the proper name, use the <c>GetPublicName</c> method instead.
        /// </summary>
        public string Name { get; private set; }
        public string IP { get; private set; }
        public string RootPassword { get; private set; }
        public bool IsPlayerConnected { get; private set; }
        public bool PlayerHasRoot { get; private set; }
        public bool IsMissionObjective { get; private set; }
        public bool IsShownOnMap { get; private set; }
        public bool IsOnline { get; private set; }
        public Dictionary<int, string> OpenPorts { get; private set; }
        public Vector2 Coordinates { get; private set; } // Possibly redundant
        public float MapX { get; set; }
        public float MapY { get; set; }
        public AccessLevel AccessLevel { get; private set; }
        public ComputerType ComputerType { get; private set; }
        public Company Owner { get; private set; }
        public FileSystem FileSystem { get; private set; }
        #endregion

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

        public void Init()
        {
            if(!_isInitialized)
            {
                IsPlayerConnected = false;
                PlayerHasRoot = false;
                IsMissionObjective = false;
                IsShownOnMap = true;
                IsOnline = true;
                if(Name.Contains("§¤§"))
                    _publicName = Name.Replace("§¤§", "\n");
                else
                    _publicName = Name + "\n" + ComputerType.ToString();

                _isInitialized = true;
            }
        }

        public string GetPublicName()
        {
            return _publicName;
        }

        /// <summary>
        /// Connect the player to this computer
        /// </summary>
        /// <returns><c>true</c> if connection sucessful. <c>false</c> otherwise.</returns>
        public bool Connect()
        {
            if (IsOnline)
            {
                Player.GetInstance().ConnectedComp.Disconnect();
                Player.GetInstance().ConnectedComp = this;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disconnect the player from this computer
        /// </summary>
        public void Disconnect()
        {
            Player.GetInstance().ConnectedComp = Player.GetInstance().PlayerComp;
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
            foreach (int key in OpenPorts.Keys)
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
        public Dictionary<int, string> BuildPorts(int[] ports)
        {
            var retval = new Dictionary<int, string>();
            foreach (int port in ports)
            {
                retval.Add(port, Enum.IsDefined(typeof(KnownPorts), port) ? ((KnownPorts)port).ToString() : "Unknown");
            }
            return retval;
        }

        public override string ToString()
        {
            return _publicName + "\n" + IP;
        }

        public void Tick()
        {
            // TODO: update computer if necessary
        }

        public static Computer Load()
        {
            var c = new Computer("temp", "0.0.0.0", "temp"); // TEMP
            c.Init(); // TEMP
            return c; // TEMP
        }

        public void Save(string fileName)
        {
            // TODO: save computer
        }
    }
}
