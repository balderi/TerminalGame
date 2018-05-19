using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;

namespace TerminalGame.UI.Modules
{
    class NotesModule : Module
    {
        // TODO: Word-wrapping on notes

        // TODO: Limit notes to length of window (or implement some kind of scrolling?)

        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }
        public List<string> Notes { get; private set; }
        public override Rectangle Container { get; set; }

        private readonly SpriteFont NoteFont;
        string noteRender, divider, id, bar;
        

        public NotesModule(GraphicsDevice Graphics, Rectangle Container, SpriteFont noteFont) : base(Graphics, Container)
        {
            Notes = new List<string>();
            NoteFont = noteFont;
            string sample = "Use\n  note [TEXT]\nto add a note.\nE.g. note \"this is a test\"\n(Note the quotes!)";
            string sample2 = "Use\n  note -r [ID]\nto remove a note.\nE.g. note -r 1\nwould remove this note (Note1).";
            string sample3 = "Or\n  note -r *\nto remove all notes.";
            Notes.Add(sample);
            Notes.Add(sample2);
            Notes.Add(sample3);
            noteRender = "";
            bar = "-------------------------------";
        }

        public void AddNote(string text)
        {
            Notes.Add(text);
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
            Texture2D texture = Drawing.DrawBlankTexture(Graphics);
            Drawing.DrawBorder(spriteBatch, Container, texture, 1, BorderColor);
            spriteBatch.Draw(texture, Container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
            spriteBatch.DrawString(NoteFont, noteRender, new Vector2(Container.X + 10, Container.Y + Font.MeasureString("A").Y + 10), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            noteRender = "";
            foreach(string s in Notes)
            {
                id = "<Note" + Notes.IndexOf(s) + ">";
                divider = bar.Substring(0, 2) + id + bar.Substring(0, bar.Length - id.Length - 2);
                noteRender += String.Format("{0}\n{1}\n",divider,s);
            }
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
