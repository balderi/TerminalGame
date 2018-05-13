﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Modules;

namespace TerminalGame.OS
{
    class OS
    {
        //TODO: This should be a singleton module manager, for better interop
        private static OS instance;

        public List<Module> Modules { get; private set; }
        
        private OS()
        {
            Modules = new List<Module>();
        }

        public static OS GetInstance()
        {
            if (instance == null)
            {
                instance = new OS();
            }
            return instance;
        }

        public void AddModule(Module module)
        {
            foreach(Module m in Modules)
            {
                if (module.Equals(m))
                    return;
            }
            Modules.Add(module);
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

        public void HideModule(Module module)
        {

        }

        public void ShowModule(Module module)
        {

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
