namespace TerminalGame.Files.FileSystem.Generator
{
    public static class FileSystemGenerator
    {
        public static FileSystem GenerateDefaultFilesystem()
        {
            File root = new File("");
            root.AddFile(new File("home"));
            root.AddFile(new File("boot"));
            root.AddFile(new File("var"));
            root.AddFile(new File("bin"));
            root.AddFile(new File("etc"));
            root.AddFile(new File("usr"));

            root.GetChild("bin").AddFile(new File("systemd", "1001011011010010000101010011110101100100100", FileType.Binary));
            root.GetChild("boot").AddFile(new File("kernel", "1001010011101110101000110010010011111001001", FileType.Binary));
            root.GetChild("var").AddFile(new File("log"));
            root.GetChild("var").GetChild("log").AddFile(new File("syslog", "syslog file contents", FileType.Text));
            root.GetChild("var").AddFile(new File("mail"));
            root.GetChild("var").GetChild("mail").AddFile(new File("pretendEmail", "pretendEmail file contents", FileType.Text));
            root.GetChild("etc").AddFile(new File("passwd", "passwd file contents", FileType.Text));

            return new FileSystem(root);
        }
    }
}
