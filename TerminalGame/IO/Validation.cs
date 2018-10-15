﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TerminalGame.Utilities;

namespace TerminalGame.IO
{
    class Validation
    {
        public static bool IsSaveGamesFolderValid()
        {
            string basePath = GameManager.GetInstance().UserFilePath;
            string savePath = GameManager.GetInstance().SavePath;

            Console.WriteLine("Validating required folders...");
            Console.WriteLine("Checking path {0}...", savePath);
            if(!Directory.Exists(savePath))
            {
                Console.WriteLine("{0} does not exist.", savePath);
                Console.WriteLine("Checking path {0}...", basePath);
                if (!Directory.Exists(basePath))
                {
                    Console.WriteLine("{0} does not exist.", basePath);
                    Console.Write("Creating {0}...", basePath);
                    Directory.CreateDirectory(basePath);
                    Console.WriteLine("Done");
                }
                Console.WriteLine("{0} validated.", basePath);
                Console.Write("Creating {0}...", savePath);
                Directory.CreateDirectory(savePath);
                Console.WriteLine("Done");
                Console.WriteLine("{0} validated.", savePath);
            }
            Console.WriteLine("Folder validation completed successfully.");
            return true;
        }

        public static bool IsSaveFileValid()
        {
            return true;
        }
    }
}