using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;

namespace TerminalGame.UI.Modules
{
    class Terminal : Module
    {
        private TextBox terminalInput;
        private Rectangle connAdd, inputViewport, outputViewport;
        private int linesToDraw;
        private string terminalOutput, connectedAddress;
        private List<string> history;
        private SpriteFont terminalFont;

        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool isActive { get; set; }
        public override string Title { get; set; }

        public Terminal(GraphicsDevice Graphics, Rectangle Container, SpriteFont TerminalFont) : base(Graphics, Container)
        {
            terminalFont = TerminalFont;

            if (BackgroundColor == null)
            {
                BackgroundColor = Color.LightPink;
            }
            if (BorderColor == null)
            {
                BackgroundColor = Color.Chartreuse;
            }
            if (HeaderColor == null)
            {
                BackgroundColor = Color.Red;
            }
            if (string.IsNullOrEmpty(Title))
            {
                Title = "!!! UNNAMED WINDOW !!!";
            }
        }

        private void TerminalInput_EnterDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            string o = CommandParser.ParseCommand(terminalInput.Text.String);
            history.Add(connectedAddress + terminalInput.Text.String + "\n");
            if (o != "")
                history.Add(o);
            Console.WriteLine("CMD: " + terminalInput.Text.String);
            UpdateOutput();
            terminalInput.Clear();
        }

        public void Init()
        {
            connectedAddress = "root@localhost > ";
            terminalOutput = "";
            history = new List<string>();
            connAdd = new Rectangle(container.X + 3, container.Height - renderHeader().Height, (int)(terminalFont.MeasureString(connectedAddress).X), (int)(terminalFont.MeasureString(connectedAddress).Y));
            inputViewport = new Rectangle(connAdd.Width, connAdd.Y, container.Width - connAdd.Width, (int)(terminalFont.MeasureString("MEASURE ME").Y));
            terminalInput = new TextBox(inputViewport, (76 - connectedAddress.Length), "", graphics, terminalFont, Color.LightGray, Color.DarkGreen, 30);

            outputViewport = new Rectangle(container.X, container.Y + renderHeader().Height + 5, container.Width, container.Height - inputViewport.Height - renderHeader().Height);
            
            linesToDraw = (int)(outputViewport.Height / terminalFont.MeasureString("MEASURE THIS").Y);
            Console.WriteLine("INIT: 0x4C54443D" + linesToDraw);
            for (int i = 0; i < linesToDraw + 1; i++)
            {
                history.Add("\n");
            }
            
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            terminalInput.Active = true;
            terminalInput.EnterDown += TerminalInput_EnterDown;
        }

        public void UpdateOutput()
        {
            int counter = 0;
            while (history.Count > linesToDraw)
            {
                history.RemoveAt(0);
                counter++;
            }
            if (history.Count > 0)
            {
                string holder = "";
                foreach (string s in history)
                {

                    holder += s;
                }
                terminalOutput = holder;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //connAdd.Height = (int)(terminalFont.MeasureString(connectedAddress).Y);
            //connAdd.Width = (int)(terminalFont.MeasureString(connectedAddress).X);

            //inputViewport.X = connAdd.Width;
            //inputViewport.Y = connAdd.Height;
            //inputViewport.Width = container.Width - connAdd.Width;
            //inputViewport.Height = connAdd.Height;

            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, container, texture, 2, BorderColor);
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, renderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(renderHeader().X + 5, renderHeader().Y + 2), Color.White);

            spriteBatch.DrawString(terminalFont, connectedAddress, new Vector2(connAdd.X, connAdd.Y), Color.LightGray);
            spriteBatch.DrawString(terminalFont, terminalOutput, new Vector2(outputViewport.X + 3 + TestClass.ShakeStuff(1), outputViewport.Y + TestClass.ShakeStuff(1)), Color.Green);
            spriteBatch.DrawString(terminalFont, terminalOutput, new Vector2(outputViewport.X + 3, outputViewport.Y), Color.LightGray);
            terminalInput.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);

            terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            terminalInput.Update();
        }

        protected override Rectangle renderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
