using Microsoft.VisualStudio.TestTools.UnitTesting;
using TerminalGame.Computers;

namespace TerminalGame.Utilities.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        //[TestInitialize]
        //public void Init()
        //{
        //    Computer computer = new Computer(Computer.Type.Workstation, "1.1.1.1", "test", "test");
        //    Player player = Player.GetInstance();
        //    computer.GetRoot();
        //    computer.Connect(true);
        //    player.PlayersComputer = computer;
        //    Computers.Computers.DoComputers(10);
        //    Computers.Computers.ComputerList.Add(computer);
        //}

        //[TestMethod]
        //public void Echo() => Assert.AreEqual("\ntest", CommandParser.ParseCommand("echo \"test\""));

        //[TestMethod]
        //public void Blank() => Assert.AreEqual("", CommandParser.ParseCommand(""));

        //[TestMethod]
        //public void Man() => Assert.AreEqual("\nNo manual entry for test", CommandParser.ParseCommand("man test"));

        //[TestMethod]
        //public void Touch() => Assert.AreEqual("", CommandParser.ParseCommand("touch test"));

        //[TestMethod]
        //public void Ls() => Assert.AreEqual("\n    <DIR>    bin§\n    <DIR>    home§\n    <DIR>    logs§\n    <DIR>    sys§\n    <DIR>    usr§", CommandParser.ParseCommand("ls"));

        //[TestMethod]
        //public void TouchedLs()
        //{
        //    CommandParser.ParseCommand("touch test");
        //    Assert.AreEqual("\n    <DIR>    bin§\n    <DIR>    home§\n    <DIR>    logs§\n    <DIR>    sys§\n             test§\n    <DIR>    usr§", CommandParser.ParseCommand("ls"));
        //}

        //[TestMethod]
        //public void ConnectNoArgs() => Assert.AreEqual("\nUsage: connect [IP]", CommandParser.ParseCommand("connect"));

        //[TestMethod]
        //public void ConnectAlreadyConnected()
        //{
        //    Assert.AreEqual("\nCould not connect to test", CommandParser.ParseCommand("connect test"));
        //}

        //[TestMethod]
        //public void ConnectName()
        //{
        //    Assert.AreEqual("\nConnected to Workstation2", CommandParser.ParseCommand("connect Workstation2"));
        //}

        //[TestMethod]
        //public void ConnectIP()
        //{
        //    Assert.AreEqual("\nConnected to 123.123.123.123", CommandParser.ParseCommand("connect 123.123.123.123"));
        //}
    }
}