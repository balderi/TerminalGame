using NUnit.Framework;

namespace TerminalGame.Files.Tests
{
    [TestFixture]
    public class FileTests
    {
        File testFile1, testFile2, testFile3;

        [SetUp]
        public void Setup()
        {
            testFile1 = new File("testDir");
            testFile2 = new File("testFile2", "test2 contents", FileType.Text);
            testFile3 = new File("testFile3", "test3 contents", FileType.Text, 1);
        }

        [Test]
        public void FileTest1()
        {
            Assert.AreEqual(FileType.Directory, testFile1.FileType);
            Assert.AreEqual("testDir", testFile1.Name);
            Assert.AreEqual("testDir is a directory.", testFile1.ToString());
            Assert.AreEqual(-1, testFile1.Size);
        }

        [Test]
        public void FileTest2()
        {
            Assert.AreEqual(FileType.Text, testFile2.FileType);
            Assert.AreEqual("testFile2", testFile2.Name);
            Assert.AreEqual("test2 contents", testFile2.ToString());
            Assert.AreEqual(14, testFile2.Size);
        }

        [Test]
        public void FileTest3()
        {
            Assert.AreEqual(FileType.Text, testFile3.FileType);
            Assert.AreEqual("testFile3", testFile3.Name);
            Assert.AreEqual("test3 contents", testFile3.ToString());
            Assert.AreEqual(1, testFile3.Size);
        }
    }
}