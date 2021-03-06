﻿using System;
using System.Threading;
using TerminalGame.Computers;
using TerminalGame.Computers.Utils;
using TerminalGame.Files.FileSystem;

namespace TerminalGame.Companies.Generator
{
    public static class CompanyGenerator
    {
        private static readonly string[] aName = new string[] { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Lambda", "Omicron", "Sigma", "Omega", "Zeta", "Holo", "Crypto", "Advanced", "Adaptive", "Air", "Aero", "Applied", "Auto", "Automated", "Automatic", "Bank", "Best", "Capital", "City", "Chem", "Com", "Commercial", "Compu", "Creative", "Cyber", "Dyn", "Dyna", "Dyno", "East", "Eastern", "Energy", "Elite", "Equity", "Excellent", "Excellency", "Express", "External", "Extreme", "Gen", "General", "Green", "Health", "Home", "Ini", "Initial", "International", "Key", "Max", "Mac", "Med", "Media", "Medical", "Mega", "Micro", "National", "North", "Northern", "Office", "Omni", "Online", "Pharma", "Poly", "Proto", "Quantum", "Regional", "Safe", "Sci", "SIM", "Simulated", "South", "Southern", "Super", "Synthetic", "Synth", "Virtual", "Void", "West", "Western", "Worldwide" };
        private static readonly string[] bName = new string[] { " Associates", "help", "Help", " Help", " Automatics", " Computers", " Computer", " Computers", " Service", " Services", " Shipping", " Logistics", " Renovation", "Consult", " Consulting", " Construction", " Estates", " Power", " Electric", " Services", " Computer Services", " Technologies", " Tech", " Technology", " Energy", "field", "Field", "dyne", "Dyne", "tyne", "Tyne", " Energy", " Pharmaceuticals", " Pharma", "Pharm", " Financial", " Devices", " International", " Industial Technologies", "land", " Labs", "vox", "Vox", "vision", "Vision", "tel", "Tel", "Nation", "net", "Net", " Group", " Products", "Zone", " Homes", "Sim", " Simulations", " Software", "soft", "Soft", "point", " Point", " Worldwide", "Star", "ware", " Brands", "Air", " Air", " Airlines", " Airways", "Auto", " Auto", " Foods", " Healthcare" };
        private static readonly string[] suffix = new string[] { ", Inc.", " Corp.", " Corporation", " Incorporated", ", Ltd.", " Company", " Co.", " LLC", " Intl.", " International" };

        private static Random _rnd;

        public static FileSystem GenerateFileSystem { get; private set; }

        public static string Lengths = $"{aName.Length}, {bName.Length}, {suffix.Length}";

        public static Computer GenerateComputer(Company company, TerminalGame game, ComputerType type)
        {
            string fullType;
            switch (type)
            {
                case ComputerType.Laptop:
                    {
                        fullType = "Mobile Workstation";
                        break;
                    }
                case ComputerType.Mainframe:
                    {
                        fullType = "Central Mainframe";
                        break;
                    }
                case ComputerType.Server:
                    {
                        fullType = "Public Accses Server";
                        break;
                    }
                case ComputerType.Workstation:
                    {
                        fullType = "Internal Workstation " + _rnd.Next(1, 101);
                        break;
                    }
                default:
                    {
                        fullType = type.ToString();
                        break;
                    }
            }
            FileSystem gfs = Files.FileSystem.Generator.FileSystemGenerator.GenerateDefaultFilesystem();
            return new Computer(company.Name + "§¤§" + fullType, new int[] { 21, 22, 67, 80, 110, 443, 27015 }, type, company, fileSystem: gfs)
            {
                Game = game
            };
        }

        public static string GenerateName()
        {
            Thread.Sleep(2);
            _rnd = new Random(DateTime.Now.Millisecond);
            switch (_rnd.Next(0, 8))
            {
                case 0:
                case 3:
                case 5:
                case 7:
                    {
                        return aName[_rnd.Next(0, aName.Length)] + bName[_rnd.Next(0, bName.Length)] + suffix[_rnd.Next(0, suffix.Length)];
                    };
                case 1:
                    {
                        return aName[_rnd.Next(0, aName.Length)] + bName[_rnd.Next(0, bName.Length)];
                    };
                case 2:
                case 4:
                case 6:
                    {
                        return aName[_rnd.Next(0, aName.Length)] + suffix[_rnd.Next(0, suffix.Length)];
                    };
                default:
                    {
                        return "EvilCorp";
                    };
            }
        }
    }
}
