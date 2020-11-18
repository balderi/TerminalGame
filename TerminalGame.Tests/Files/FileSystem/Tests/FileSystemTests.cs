using NUnit.Framework;
using System;
using TerminalGame.Files;

namespace TerminalGameTest.Files.FileSystem.Tests
{
    [TestFixture]
    class FileSystem_NewFileSystem
    {
        TerminalGame.Files.FileSystem.FileSystem fs1;
        TerminalGame.Files.FileSystem.FileSystem fs2;
        TerminalGame.Files.FileSystem.FileSystem fs3;


        [SetUp]
        public void Setup()
        {
            fs1 = new TerminalGame.Files.FileSystem.FileSystem();
            File root = new File("");
            fs2 = new TerminalGame.Files.FileSystem.FileSystem(root);
            root = new File("");
            File dir = new File("dir");
            dir.AddFile(new File("d1file1", "dir1 file1 contents", FileType.Text));
            dir.AddFile(new File("d1file2", "dir1 file2 contents", FileType.Text));
            File dir2 = new File("dir2");
            dir2.AddFile(new File("d2file1", "dir2 file1 contents", FileType.Text));
            dir2.AddFile(new File("d2file1", "dir2 file1 contents", FileType.Text));
            dir.AddFile(dir2);
            root.AddFile(dir);
            fs3 = new TerminalGame.Files.FileSystem.FileSystem(root);
        }

        [Test]
        public void TestBlankFs()
        {
            Assert.IsNull(fs1.RootDir);
            Assert.Throws<NullReferenceException>(() => fs1.RootDir.ListChildren());
            Assert.IsNull(fs1.CurrentDir);
            Assert.Throws<NullReferenceException>(() => fs1.CurrentDir.ListChildren());
        }

        [Test]
        public void TestBaseFs()
        {
            Assert.IsNotNull(fs2.RootDir);
            Assert.IsTrue(fs2.RootDir.ListChildren() == ".");
            Assert.IsNotNull(fs2.CurrentDir);
            Assert.IsTrue(fs2.CurrentDir.ListChildren() == ".");
        }

        [Test]
        public void ComplexFsTest()
        {
            Assert.IsNotNull(fs3.RootDir);
            Assert.IsTrue(fs3.RootDir.ListChildren() == ".\ndir");
            Assert.IsTrue(fs3.CurrentDir.ListChildren() == ".\ndir");
            fs3.ChangeCurrentDirFromPath("dir");
            Assert.IsNotNull(fs3.CurrentDir);
            Assert.AreEqual("dir", fs3.CurrentDir.Name);
            Assert.IsTrue(fs3.CurrentDir.ListChildren() == ".\n..\nd1file1\nd1file2\ndir2");
            fs3.ChangeCurrentDirFromPath("..");
            Assert.IsTrue(fs3.CurrentDir.ListChildren() == ".\ndir");
        }

        [Test]
        public void TestTryFindFile1()
        {
            fs3.TryFindFile("dir", out var r1);
            Assert.AreEqual("dir", r1.Name);
        }

        [Test]
        public void TestTryFindFile2()
        {
            fs3.TryFindFile(".", out var r1);
            Assert.AreEqual("", r1.Name);
            fs3.TryFindFile("..", out var r2);
            Assert.AreEqual("", r2.Name);
            fs3.TryFindFile("/", out var r3);
            Assert.AreEqual("", r3.Name);
        }

        [Test]
        public void TestTryFindFilePath1()
        {
            Assert.IsTrue(fs3.TryFindFilePath("d1file1", out var path));
            Assert.AreEqual("/dir/", path);
        }

        [Test]
        public void TestTryFindFilePath2()
        {
            Assert.IsTrue(fs3.TryFindFilePath("d2file1", out var path));
            Assert.AreEqual("/dir/dir2/", path);
        }

        [Test]
        public void TestTryFindFileFromPath1()
        {
            Assert.IsTrue(fs3.TryFindFileFromPath("dir/d1file1", out var path, out var file));
            Assert.AreEqual("/dir/", path);
            Assert.AreEqual("d1file1", file);
        }

        [Test]
        public void TestTryFindFileFromPath2()
        {
            Assert.IsTrue(fs3.TryFindFileFromPath("dir/dir2/d2file1", out var path, out var file));
            Assert.AreEqual("/dir/dir2/", path);
            Assert.AreEqual("d2file1", file);
        }

        [Test]
        public void TestTryFindFileFromPath3()
        {
            Assert.IsTrue(fs3.TryFindFileFromPath("dir/dir2", out var path, out var file));
            Assert.AreEqual("/dir/", path);
            Assert.AreEqual("dir2", file);
        }

        [Test]
        public void TestMisc()
        {
            File f = new File("notADir", "some text", FileType.Text);

            Assert.Throws<ArgumentException>(() => new TerminalGame.Files.FileSystem.FileSystem(f));
        }
    }
}
