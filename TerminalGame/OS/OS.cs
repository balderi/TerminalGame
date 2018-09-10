using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Modules;

namespace TerminalGame.OS
{
    class OS
    {
        private static OS _instance;

        public Terminal Terminal { get; private set; }
        public RemoteView RemoteView { get; private set; }
        public NetworkMap NetworkMap { get; private set; }
        public StatusBar StatusBar { get; private set; }
        public NotesModule Notes { get; private set; }
        public List<Module> Modules { get; private set; }
        
        private OS()
        {
            System.Console.WriteLine("Starting OS");
        }

        public void Init(Terminal terminal, RemoteView remoteView, NetworkMap networkmap, StatusBar statusBar, NotesModule notes)
        {
            Terminal = terminal;
            RemoteView = remoteView;
            NetworkMap = networkmap;
            StatusBar = statusBar;
            Notes = notes;

            Modules = new List<Module>()
            {
                Terminal,
                RemoteView,
                NetworkMap,
                StatusBar,
                Notes
            };
        }

        public static OS GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OS();
            }
            return _instance;
        }

        public void EnableModule(Module module)
        {
            foreach(Module m in Modules)
            {
                if (module.Equals(m) && !m.IsActive)
                    m.IsActive = true;
            }
        }

        public void DisableModule(Module module)
        {
            foreach (Module m in Modules)
            {
                if (module.Equals(m) && m.IsActive)
                    m.IsActive = false;
            }
        }

        public void Write(string text)
        {
            Terminal.Write(text);
        }

        public void HideModule(Module module)
        {
            foreach(Module m in Modules)
            {
                if(m.Equals(module))
                {
                    m.IsActive = false;
                }
            }
        }

        public void ShowModule(Module module)
        {
            foreach (Module m in Modules)
            {
                if (m.Equals(module))
                {
                    m.IsActive = true;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Module m in Modules)
            {
                m.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Module m in Modules)
            {
                m.Draw(spriteBatch);
            }
        }
    }
}
