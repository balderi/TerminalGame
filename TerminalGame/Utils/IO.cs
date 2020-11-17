using System.IO;

namespace TerminalGame.Utils
{
    public static class IO
    {
        public static bool CheckAndCreateDirectory(string path)
        {
            if(!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public static string CheckSaveName(string playerName)
        {
            int i = 1;
            string fileNameBase = $"Saves/save_{playerName}";
            string tryName = fileNameBase;
            while(File.Exists($"{tryName}.tgs"))
            {
                tryName = $"{fileNameBase}({i++})";
            }
            return tryName + ".tgs";
        }
    }
}
