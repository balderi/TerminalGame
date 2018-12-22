using System;
using System.IO;

namespace TerminalGame.Utils
{
    class Settings
    {
        private static readonly string _folderPath = Environment.SpecialFolder.ApplicationData + "/TerminalGame";
        private static readonly string _filePath = _folderPath + "/settings.xml";

        private static Settings _instance;
        public static Settings GetInstance()
        {
            if(_instance == null)
            {
                _instance = new Settings();
            }
            return _instance;
        }

        private Settings()
        {
            //this space intentionally left blank
        }

        public void Load()
        {
            if(!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            else
            {

            }
        }

        public void Save()
        {
            if(!File.Exists(_filePath))
            {

            }
            else
            {

            }
        }
    }
}
