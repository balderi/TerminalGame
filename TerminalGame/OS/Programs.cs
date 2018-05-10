using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.OS
{
    class Programs
    {
        private static Programs instance;

        public static Programs GetInstance()
        {
            if (instance == null)
                instance = new Programs();
            return instance;
        }

        private Programs()
        {

        }

        #region Programs
#pragma warning disable IDE1006 // Naming Styles

        public string ls()
        {
            return "";
        }

        public string cd(string folder)
        {
            return "";
        }

        public string rm(string[] args)
        {
            return "";
        }

        public string connect(string ip)
        {
            return "";
        }

        public string disconnect()
        {
            return "";
        }

#pragma warning restore IDE1006 // Naming Styles
        #endregion
    }
}
