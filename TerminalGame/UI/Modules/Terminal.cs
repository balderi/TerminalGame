using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class Terminal : Module
    {

        //TODO: Add ability to take input from outside sources (programs, messages, etc.)

        private TextBox terminalInput;
        private Rectangle connAdd, inputViewport, outputViewport;
        private int linesToDraw;
        private string terminalOutput, connectedAddress;
        private List<string> history, output;
        private SpriteFont terminalFont;
        private bool isMultiLine;
        private Computer connectedComputer;


        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
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
            Console.WriteLine("CMD: " + terminalInput.Text.String);
            string pars = "";
            if (!string.IsNullOrEmpty(terminalInput.Text.String))
                pars = TextWrap(CommandParser.ParseCommand(terminalInput.Text.String));
            string[] o = pars.Split('§');
            output.Add(connectedAddress + terminalInput.Text.String + "\n");
            history.Insert(0, terminalInput.Text.String);
            if (!string.IsNullOrEmpty(o[0]))
            {
                for (int i = 0; i < o.Length; i++)
                {
                    output.Add(o[i]);
                }
            }
            UpdateOutput();
            terminalInput.Clear();
            if (isMultiLine)
                UpdateOutput();
        }

        private void TerminalInput_TabDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            terminalInput.Text.String = "Autocomplete command";
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
        }

        private void TerminalInput_DnArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            terminalInput.Text.String = "Previous Command";
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
        }

        private void TerminalInput_UpArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if(history.Count > 0)
                terminalInput.Text.String = history[0];
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
        }

        public void Init()
        {
            connectedComputer = Player.GetInstance().ConnectedComputer;
            connectedAddress = "root@localhost > ";
            terminalOutput = "";
            history = new List<string>();
            output = new List<string>();
            connAdd = new Rectangle(container.X + 3, container.Height - RenderHeader().Height, (int)(terminalFont.MeasureString(connectedAddress).X), (int)(terminalFont.MeasureString(connectedAddress).Y));
            inputViewport = new Rectangle(connAdd.Width, connAdd.Y, container.Width - connAdd.Width, (int)(terminalFont.MeasureString("MEASURE ME").Y));
            terminalInput = new TextBox(inputViewport, (76 - connectedAddress.Length), "", graphics, terminalFont, Color.LightGray, Color.DarkGreen, 30);

            outputViewport = new Rectangle(container.X, container.Y + RenderHeader().Height + 5, container.Width, container.Height - inputViewport.Height - RenderHeader().Height);
            
            linesToDraw = (int)(outputViewport.Height / terminalFont.MeasureString("MEASURE THIS").Y);
            Console.WriteLine("INIT: 0x4C54443D" + linesToDraw);
            for (int i = 0; i < linesToDraw + 1; i++)
            {
                output.Add("\n");
            }
            
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            terminalInput.Active = true;
            terminalInput.EnterDown += TerminalInput_EnterDown;
            terminalInput.UpArrow += TerminalInput_UpArrow;
            terminalInput.DnArrow += TerminalInput_DnArrow;
            terminalInput.TabDown += TerminalInput_TabDown;
            connectedComputer.Connected += ConnectedComputer_Connected;
        }

        private void ConnectedComputer_Connected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** CONNECTION EVENT FIRED");
            output.Add("[EVENT] Connected to " + e.ConnectionString);
            UpdateOutput();
            connectedAddress = e.IsRoot ? "Root@" + e.ConnectionString : "User@" + e.ConnectionString;
        }

        public void UpdateOutput()
        {
            int counter = 0;
            while (output.Count > linesToDraw)
            {
                output.RemoveAt(0);
                counter++;
            }
            if (output.Count > 0)
            {
                string holder = "";
                foreach (string s in output)
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
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y + 2), Color.White);
            Drawing.DrawBorder(spriteBatch, container, texture, 1, BorderColor);

            spriteBatch.DrawString(terminalFont, connectedAddress, new Vector2(connAdd.X, connAdd.Y), Color.LightGray);
            spriteBatch.DrawString(terminalFont, terminalOutput, new Vector2(outputViewport.X + 3 + TestClass.ShakeStuff(1), outputViewport.Y + TestClass.ShakeStuff(1)), Color.Green);
            spriteBatch.DrawString(terminalFont, terminalOutput, new Vector2(outputViewport.X + 3, outputViewport.Y), Color.LightGray);
            terminalInput.Draw(spriteBatch);
        }

        private string TextWrap(string text)
        {
            isMultiLine = false;
            int maxlen = 77;
            if (text.Length <= maxlen)
                return text;

            isMultiLine = true;
            return text.Insert(maxlen - 1, "\n§");
        }

        public override void Update(GameTime gameTime)
        {
            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);

            terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            terminalInput.Update();
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
