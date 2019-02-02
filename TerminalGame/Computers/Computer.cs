using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.Utils;
using TerminalGame.Computers.Utils;

namespace TerminalGame.Computers
{
    public class Computer : IComputer
    {
        #region fields
        private readonly int[] _defaultPorts = { 22, 25, 80, 443 };
        private bool _isInitialized;
        #endregion

        #region properties
        public string Name { get; private set; }
        public string IP { get; private set; }
        public string RootPassword { get; private set; }
        public bool IsPlayerConnected { get; private set; }
        public bool PlayerHasRoot { get; private set; }
        public bool IsMissionObjective { get; private set; }
        public bool IsShownOnMap { get; private set; }
        public Dictionary<int, string> OpenPorts { get; private set; }
        public Vector2 Coordinates { get; private set; } // Possibly redundant
        public float MapX { get; set; }
        public float MapY { get; set; }
        public AccessLevel AccessLevel { get; private set; }
        public ComputerType ComputerType { get; private set; }
        #endregion

        public Computer(string name, string ip = "", string rootPassword = "")
        {
            Name = name;

            if (String.IsNullOrWhiteSpace(ip))
                IP = Generators.GenerateIP();
            else
                IP = ip;

            if (String.IsNullOrWhiteSpace(rootPassword))
                RootPassword = Generators.GeneratePassword();
            else
                RootPassword = rootPassword;

            _isInitialized = false;
        }
        
        private void Init()
        {
            if(!_isInitialized)
            {
                IsPlayerConnected = false;
                PlayerHasRoot = false;
                IsMissionObjective = false;
                IsShownOnMap = true;

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Connect the player to this computer
        /// </summary>
        public void Connect()
        {

        }

        /// <summary>
        /// Disconnect the player from this computer
        /// </summary>
        public void Disconnect()
        {

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

        public void Update(GameTime gameTime)
        {

        }
    }
}
