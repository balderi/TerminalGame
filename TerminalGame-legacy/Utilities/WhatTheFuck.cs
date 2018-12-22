using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.Computers;
using TerminalGame.Computers.FileSystems;
using TerminalGame.IO;
using TerminalGame.Scenes;
using TerminalGame.States;
using TerminalGame.UI.Modules;
using TerminalGame.UI.Themes;

namespace TerminalGame.Utils
{
    //Ah, fuck! I can't believe I've done this!

    public class WhatTheFuck
    {
        Terminal terminal;
        NetworkMap networkMap;
        StatusBar statusBar;
        RemoteView remoteView;
        NotesModule notes;

        Computer playerComp;

        LoadingScene loadingScene;
        
        //Thread _loadingThread;

        OS.OS _os;

        GraphicsDeviceManager _graphics;

        GameWindow _window;

        Dictionary<string, Texture2D> NetworkNodeSpinners;

        Texture2D computer;

        private static WhatTheFuck _instance;

        public static WhatTheFuck GetInstance()
        {
            if (_instance == null)
                _instance = new WhatTheFuck();
            return _instance;
        }

        private WhatTheFuck()
        {
        }

        public void SetupWTF(GraphicsDeviceManager graphics, GameWindow window, Dictionary<string, Texture2D> networkNodeSpinners, Texture2D computer)
        {
            _graphics = graphics;
            _window = window;
            NetworkNodeSpinners = networkNodeSpinners;
            this.computer = computer;
            loadingScene = new LoadingScene(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), _window, _graphics.GraphicsDevice);
        }

        public void StartNewGame()
        {
            Console.WriteLine("Starting new game...");

            loadingScene.LoadItem = "Generating computers...";
            Console.WriteLine("Setting up computers...");

            _os = OS.OS.GetInstance();

            FileSystem playerFS = new FileSystem();
            playerFS.BuildBasicFileSystem();
            playerFS.AddFileToDir("bin", "sshnuke", "01110011011100110110100001101110011101010110101101100101001000000110000101101100011011000110111101110111011100110010000001111001011011110111010100100000011101000110111100100000011001110110000101101001011011100010000001110101011011100110000101110101011101000110100001101111011100100110100101111010011001010110010000100000011001010110111001110100011100100111100100100000011010010110111001110100011011110010000001110010011001010110110101101111011101000110010100100000011100110111100101110011011101000110010101101101011100110010000001100010011110010010000001100011011000010111010101110011011010010110111001100111001000000110000100100000011000100111010101100110011001100110010101110010001000000110111101110110011001010111001001100110011011000110111101110111001000000110100101101110001000000110000100100000011000110110100001110101011011100110101100100000011011110110011000100000011000110110111101100100011001010010000001100100011001010111001101101001011001110110111001100101011001000010000001110100011011110010000001100111011101010110000101110010011001000010000001100001011001110110000101101001011011100111001101110100001000000110001101110010011110010111000001110100011011110110011101110010011000010111000001101000011010010110001100100000011000010111010001110100011000010110001101101011011100110010000001101111011011100010000001010011010100110100100000100000011101100110010101110010011100110110100101101111011011100010000001101111011011100110010100101110", File.FileType.Binary);
            playerFS.AddFileToDir("bin", "nmap", "010011100110110101100001011100000010000001101001011100110010000001100001001000000110011001110010011001010110010100100000011000010110111001100100001000000110111101110000011001010110111000101101011100110110111101110101011100100110001101100101001000000111001101100101011000110111010101110010011010010111010001111001001000000111001101100011011000010110111001101110011001010111001000101100001000000110111101110010011010010110011101101001011011100110000101101100011011000111100100100000011101110111001001101001011101000111010001100101011011100010000001100010011110010010000001000111011011110111001001100100011011110110111000100000010011000111100101101111011011100010110000100000011101010111001101100101011001000010000001110100011011110010000001100100011010010111001101100011011011110111011001100101011100100010000001101000011011110111001101110100011100110010000001100001011011100110010000100000011100110110010101110010011101100110100101100011011001010111001100100000011011110110111000100000011000010010000001100011011011110110110101110000011101010111010001100101011100100010000001101110011001010111010001110111011011110111001001101011001011000010000001110100011010000111010101110011001000000110001001110101011010010110110001100100011010010110111001100111001000000110000100100000001000100110110101100001011100000010001000100000011011110110011000100000011101000110100001100101001000000110111001100101011101000111011101101111011100100110101100101110", File.FileType.Binary);

            Player.GetInstance().CreateNewPlayer("testPlayer", "P@ssw0rd");

            playerComp = new Computer(Computer.Type.Workstation, "127.0.0.1", "localhost", Player.GetInstance().Password, 0.0f, playerFS, new int[] { 10, 21, 22, 23, 25, 53, 67, 69, 80, 110, 123, 143, 443 });

            playerComp.SetSpeed(1.0f);

            Computers.Computers.GetInstance().ComputerList.Clear();

            //we add the player's computer first, to make sure it is the first computer on the list.
            Computers.Computers.GetInstance().ComputerList.Add(playerComp);
            //then we add the rest
            Computers.Computers.GetInstance().DoComputers(100);

            playerComp.GetRoot();
            playerComp.Connect(true);

            Player.GetInstance().PlayersComputer = playerComp;

            loadingScene.LoadItem = "Loading themes...";

            Console.WriteLine("Loading themes...");
            var themeManager = ThemeManager.GetInstance();
            Theme test = new Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);
            themeManager.AddTheme(test);
            themeManager.ChangeTheme("test");

            DoModules();

            loadingScene.LoadItem = "Building map...";

            networkMap.GenerateMap();

            loadingScene.LoadItem = "Initializing...";

            _os.Init(terminal, remoteView, networkMap, statusBar, notes);

            terminal.Init();
            Console.WriteLine("Game started");

            GameManager.GetInstance().IsGameRunning = true;

            GameManager.GetInstance().StateMachine.Transition(GameState.GameRunning);

            SaveGame.Save();
        }

        public void StartLoadGame()
        {
            Console.WriteLine("Loading game...");

            loadingScene.LoadItem = "Loading computers...";
            Console.WriteLine("Loading computers...");

            _os = OS.OS.GetInstance();

            Computers.Computers.GetInstance().ComputerList.Clear();

            LoadGame.Load(GameManager.GetInstance().CurrentSaveName);

            playerComp = Computers.Computers.GetInstance().ComputerList[0];

            playerComp.SetSpeed(1.0f);

            playerComp.GetRoot();
            playerComp.Connect(true);

            Player.GetInstance().PlayersComputer = playerComp;

            loadingScene.LoadItem = "Loading themes...";

            Console.WriteLine("Loading themes...");

            var themeManager = ThemeManager.GetInstance();
            Theme test = new Theme("test", new Color(51, 51, 55), Color.Black * 0.75f,
                Color.LightGray, new Color(63, 63, 63), Color.White, new Color(80, 80, 80),
                Color.RoyalBlue, Color.Blue, Color.DarkOrange, Color.Green, Color.Red);
            themeManager.AddTheme(test);
            themeManager.ChangeTheme("test");

            DoModules();

            loadingScene.LoadItem = "Building map...";

            networkMap.BuildLoadedMap();

            loadingScene.LoadItem = "Initializing...";

            _os.Init(terminal, remoteView, networkMap, statusBar, notes);

            terminal.Init();
            Console.WriteLine("Game started");

            GameManager.GetInstance().IsGameRunning = true;

            GameManager.GetInstance().StateMachine.Transition(GameState.GameRunning);

            SaveGame.Save();
        }

        public void DoModules()
        {
            loadingScene.LoadItem = "Loading modules...";

            int thirdWidth = _graphics.PreferredBackBufferWidth / 3;
            int thirdHeight = _graphics.PreferredBackBufferHeight / 3;
            int tqWidth = Convert.ToInt32(_graphics.PreferredBackBufferWidth * 0.75);


            // TLDR: It's a mess... good luck!
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // MODULE SIZING: (may still need to be properly refined, as border/margin stuff relies heavily on magic numbers (bad)) //
            // Terminal is one third of the screen width minus 2px on either side (1px border + 1px margin)                         //
            // Network map is Full screen width minus terminal width minus 7px                                                      //
            //                    (terminal border/margin + map border/margin + 1px space between modules)                          //
            // Remote view is thee-quarters of full screen width minus terminal width minus border/margin)                          //
            // Notes module is full screen width minus terminal width minus remote view width minus border/margin                   //
            // Status bar is full screen width minus terminal width minus border/margin                                             //
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Console.WriteLine("Loading terminal...");
            terminal = new Terminal(_graphics.GraphicsDevice,
                new Rectangle(2, 2,
                thirdWidth - 4, _graphics.PreferredBackBufferHeight - 4),
                FontManager.GetFont(FontManager.FontSize.Medium))
            {
                Title = "Terminal",
                Font = FontManager.GetFont(FontManager.FontSize.Small),
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading networkmap...");
            networkMap = new NetworkMap(_window, _graphics.GraphicsDevice,
                new Rectangle(terminal.Container.Width + 5, _graphics.PreferredBackBufferHeight - thirdHeight + 2,
                _graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, thirdHeight - 4),
                computer, FontManager.GetFont(FontManager.FontSize.Small), NetworkNodeSpinners)
            {
                Title = "Network Map",
                Font = FontManager.GetFont(FontManager.FontSize.Small),
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading statusbar...");
            statusBar = new StatusBar(_graphics.GraphicsDevice,
                new Rectangle(terminal.Container.Width + 5, 2,
                _graphics.PreferredBackBufferWidth - terminal.Container.Width - 7, (int)FontManager.GetFont(FontManager.FontSize.Large).MeasureString("A").Y - 4),
                FontManager.GetFont(FontManager.FontSize.XSmall))
            {
                Title = "Status Bar",
                Font = FontManager.GetFont(FontManager.FontSize.Large),
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading remoteview...");
            remoteView = new RemoteView(_graphics.GraphicsDevice,
                new Rectangle(terminal.Container.Width + 5, statusBar.Container.Height + 5,
                tqWidth - terminal.Container.Width - 7, _graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10),
                FontManager.GetFont(FontManager.FontSize.Large), FontManager.GetFont(FontManager.FontSize.Medium))
            {
                Title = "Remote System",
                Font = FontManager.GetFont(FontManager.FontSize.Small),
                IsActive = true,
                IsVisible = true,
            };

            Console.WriteLine("Loading notes...");
            notes = new NotesModule(_graphics.GraphicsDevice,
                new Rectangle(remoteView.Container.X + remoteView.Container.Width + 3, statusBar.Container.Height + 5,
                _graphics.PreferredBackBufferWidth - remoteView.Container.X - remoteView.Container.Width - 5, _graphics.PreferredBackBufferHeight - networkMap.Container.Height - statusBar.Container.Height - 10),
                FontManager.GetFont(FontManager.FontSize.Medium))
            {
                Title = "Friendly neighborhood notepad",
                Font = FontManager.GetFont(FontManager.FontSize.Small),
                IsActive = true,
                IsVisible = true,
            };
        }
    }
}
