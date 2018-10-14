using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.UI.Themes;
using TerminalGame.Utilities;

namespace TerminalGame.UI.Modules
{
    class NotesModule : Module
    {
        // TODO: Word-wrapping on notes

        // TODO: Limit notes to length of window (or implement some kind of scrolling?)

        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public List<string> Notes { get; private set; }
        public override Rectangle Container { get; set; }

        private readonly SpriteFont _noteFont;
        private string _noteRender, _divider, _id, _bar;
        

        public NotesModule(GraphicsDevice graphics, Rectangle container, SpriteFont noteFont) : base(graphics, container)
        {
            Notes = new List<string>();
            _noteFont = noteFont;
            string sample = "Use\n  note [TEXT]\nto add a note.\nE.g. note \"this is a test\"\n(Note the quotes!)";
            string sample2 = "Use\n  note -r [ID]\nto remove a note.\nE.g. note -r 1\nwould remove this note (Note1).";
            string sample3 = "Or\n  note -r *\nto remove all notes.";
            Notes.Add(sample);
            Notes.Add(sample2);
            Notes.Add(sample3);
            _noteRender = "";
            _bar = "";
            for(int i = 0; i < (container.Width/noteFont.MeasureString("_").X - 1); i++)
            {
                _bar += "-";
            }
        }

        public bool AddNote(string text)
        {
            Notes.Add(text);
            return true;
        }

        public bool RemoveNote(int id)
        {
            try
            {
                Notes.RemoveAt(id);
                return true;
            }
            catch (ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }
            return false;
        }

        public void Clear()
        {
            Notes.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);
                Drawing.DrawBorder(spriteBatch, Container, texture, 1, _themeManager.CurrentTheme.ModuleOutlineColor);
                spriteBatch.Draw(texture, Container, _themeManager.CurrentTheme.ModuleBackgroundColor);
                spriteBatch.Draw(texture, RenderHeader(), _themeManager.CurrentTheme.ModuleHeaderBackgroundColor);
                spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), _themeManager.CurrentTheme.ModuleHeaderFontColor);
                spriteBatch.DrawString(_noteFont, _noteRender, new Vector2(Container.X + 10, Container.Y + Font.MeasureString("A").Y + 10), _themeManager.CurrentTheme.ModuleFontColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _noteRender = "";
            int counter = 0;
            foreach(string s in Notes)
            {
                _id = "<Note" + counter++ + ">";
                _divider = _bar.Substring(0, 2) + _id + _bar.Substring(0, _bar.Length - _id.Length - 3);
                _noteRender += String.Format("{0}\n{1}\n",_divider,s);
            }
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
