using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class NotesModule : Module
    {
        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }
        public List<string> Notes { get; set; }
        public int ID { get; private set; }

        private readonly SpriteFont NoteFont;
        string noteRender, divider, id, bar;
        

        public NotesModule(GraphicsDevice Graphics, Rectangle Container, SpriteFont noteFont) : base(Graphics, Container)
        {
            ID = 0;
            Notes = new List<string>();
            NoteFont = noteFont;
            string sample = "This is a sample note.\nPlease ignore.";
            string sample2 = "This is also a sample note.\nPlease ignore this too.";
            Notes.Add(sample);
            Notes.Add(sample2);
            noteRender = "";
            bar = "-----------------------------";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, container, texture, 1, BorderColor);
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
            spriteBatch.DrawString(NoteFont, noteRender, new Vector2(container.X + 10, container.Y + Font.MeasureString("A").Y + 10), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            noteRender = "";
            ID = 0;
            foreach(string s in Notes)
            {
                id = "<Note" + ID.ToString() + ">";
                divider = "--" + id + bar.Substring(0, bar.Length - id.Length);
                ID++;
                noteRender += String.Format("{0}\n{1}\n",divider,s);
            }
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
