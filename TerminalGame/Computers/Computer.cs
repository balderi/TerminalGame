using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Computers
{
    class Computer
    {
        public enum Type { Workstation, Server, Mainframe, Laptop }
        Type type;
        public enum AccessLevel { root, user }
        AccessLevel access;

        public string IP { get; private set; }
        public string Name { get; private set; }
        public string RootPassword { get; private set; }
        public bool IsPlayerConnected { get; private set; }
        public bool PlayerHasRoot { get; private set; }
        public event EventHandler<ConnectEventArgs> Connected;
        public event EventHandler<ConnectEventArgs> Disonnected;

        public Computer(Type type, string IP, string Name, string RootPassword)
        {
            this.type = type;
            this.IP = IP;
            this.Name = Name;
        }

        public string Connect()
        {
            Player.GetInstance().ConnectedComputer = this;
            IsPlayerConnected = true;
            Connected?.Invoke(null, new ConnectEventArgs(IP, PlayerHasRoot));
            Console.WriteLine("Connected to " + IP);
            return "Connected to " + IP;
        }

        public string Disconnect()
        {
            Player.GetInstance().ConnectedComputer = Player.GetInstance().PlayersComputer;
            IsPlayerConnected = false;
            Console.WriteLine("Disconnected from " + IP);
            return "Disconnected";
        }

        public void Update(GameTime gameTime)
        {
            access = PlayerHasRoot ? AccessLevel.root : AccessLevel.user;
        }
    }
    public class ConnectEventArgs : EventArgs
    {
        public string ConnectionString { get; private set; }
        public bool IsRoot { get; private set; }

        public ConnectEventArgs(string connectionString, bool isRoot)
        {
            ConnectionString = connectionString;
            IsRoot = isRoot;
        }
    }
}
