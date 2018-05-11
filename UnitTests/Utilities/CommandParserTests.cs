using Microsoft.VisualStudio.TestTools.UnitTesting;
using TerminalGame;
using TerminalGame.Utilities;
using TerminalGame.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        [TestInitialize]
        public void Init()
        {
            Computer computer = new Computer(Computer.Type.Workstation, "1.1.1.1", "test", "test");
            Player player = Player.GetInstance();
            computer.GetRoot();
            computer.Connect(true);
            player.PlayersComputer = computer;
            Computers.Computers.DoComputers();
            Computers.Computers.computerList.Add(computer);
        }

        [TestMethod]
        public void Echo() => Assert.AreEqual("test\n", CommandParser.ParseCommand("echo \"test\""));

        [TestMethod]
        public void Blank() => Assert.AreEqual("", CommandParser.ParseCommand(""));

        [TestMethod]
        public void Man() => Assert.AreEqual("No manual entry for test\n", CommandParser.ParseCommand("man test"));

        [TestMethod]
        public void Touch() => Assert.AreEqual("", CommandParser.ParseCommand("touch test"));

        [TestMethod]
        public void Ls() => Assert.AreEqual("    <DIR>    bin\n§    <DIR>    usr\n§    <DIR>    home\n§    <DIR>    sys\n§", CommandParser.ParseCommand("ls"));

        [TestMethod]
        public void TouchedLs()
        {
            CommandParser.ParseCommand("touch test");
            Assert.AreEqual("    <DIR>    bin\n§    <DIR>    usr\n§    <DIR>    home\n§    <DIR>    sys\n§             test\n§", CommandParser.ParseCommand("ls"));
        }

        [TestMethod]
        public void ConnectNoArgs() => Assert.AreEqual("Usage: connect [IP]\n", CommandParser.ParseCommand("connect"));

        [TestMethod]
        public void ConnectName1()
        {
            Assert.AreEqual("Connected to test\n", CommandParser.ParseCommand("connect test"));
        }

        [TestMethod]
        public void ConnectName2()
        {
            Assert.AreEqual("Connected to Workstation2\n", CommandParser.ParseCommand("connect Workstation2"));
        }

        [TestMethod]
        public void ConnectIP()
        {
            Assert.AreEqual("Connected to 123.123.123.123\n", CommandParser.ParseCommand("connect 123.123.123.123"));
        }
    }
}