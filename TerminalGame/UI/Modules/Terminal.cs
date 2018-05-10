using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Collections.Generic;
using TerminalGame.Utilities;
using TerminalGame.Utilities.TextHandler;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class Terminal : Module
    {
        //TODO: Make singleton, and add the instance to the player, instead of the game itself
        //TODO: Add ability to take input from outside sources (programs, messages, etc.)

        private TextBox terminalInput;
        private Rectangle connAdd, inputViewport, outputViewport;
        private int linesToDraw, currentIndex;
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
        
        /// <summary>
        /// Called when user hits the Enter key.
        /// Calls the Command Parser with the input text and then handles the output
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs</param>
        private void TerminalInput_EnterDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            Console.WriteLine("CMD: " + terminalInput.Text.String);
            output.Add(connectedAddress + terminalInput.Text.String + "\n");
            string pars = "";
            currentIndex = 0;

            if (!string.IsNullOrEmpty(terminalInput.Text.String))
                pars = TextWrap(CommandParser.ParseCommand(terminalInput.Text.String));

            string[] o = pars.Split('§');
            history.Insert(0, terminalInput.Text.String);

            if (!string.IsNullOrEmpty(o[0]))
            {
                for (int i = 0; i < o.Length; i++)
                {
                    if (!string.IsNullOrEmpty(o[i]) && o[i] != "\n")
                    {
                        output.Add(o[i]);
                    }
                }
            }

            if (terminalInput.Text.String == "clear")
                Clear();
            if(terminalInput.Text.String == "hist")
            {
                foreach(string s in history)
                {
                    output.Add(s);
                }
            }

            UpdateOutput();
            terminalInput.Clear();
        }

        private void TerminalInput_TabDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            terminalInput.Text.String = "Autocomplete command";
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
        }

        private void TerminalInput_DnArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (currentIndex > 0)
            {
                terminalInput.Text.String = history[--currentIndex];
            }
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
            Console.WriteLine("DN :: CI:{0} | HC:{1}", currentIndex, history.Count);
        }

        private void TerminalInput_UpArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (history.Count > 0 && currentIndex < history.Count)
            {
                terminalInput.Text.String = history[currentIndex++];
            }
            terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
            Console.WriteLine("UP :: CI:{0} | HC:{1}", currentIndex, history.Count);
        }

        /// <summary>
        /// Clear the terminal window
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < linesToDraw + 1; i++)
            {
                output.Add("\n");
            }
        }

        /// <summary>
        /// Initialization method for the terminal
        /// </summary>
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

            outputViewport = new Rectangle(container.X, container.Y + RenderHeader().Height + 2, container.Width, container.Height - (inputViewport.Height) - RenderHeader().Height);
            
            linesToDraw = (int)(outputViewport.Height / terminalFont.MeasureString("MEASURE THIS").Y);
            Console.WriteLine("INIT: 0x4C54443D" + linesToDraw);
            Clear();
            
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            terminalInput.Active = true;
            terminalInput.EnterDown += TerminalInput_EnterDown;
            terminalInput.UpArrow += TerminalInput_UpArrow;
            terminalInput.DnArrow += TerminalInput_DnArrow;
            terminalInput.TabDown += TerminalInput_TabDown;
            foreach(Computer c in Computers.Computers.computerList)
            {
                c.Connected += ConnectedComputer_Connected;
                c.Disonnected += ConnectedComputer_Disonnected;
                Console.WriteLine("Subscribed to " + c.IP + "'s CONN/DISC Events");
            }
        }

        private void ConnectedComputer_Disonnected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** DISCONNECTION EVENT FIRED");
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + ", IS_RT: " + e.IsRoot.ToString());
            //output.Add("Disconnected\n");
            UpdateOutput();
            connectedAddress = e.IsRoot ? "root@" + e.ConnectionString + " > " : "user@" + e.ConnectionString + " > ";
            UpdateInputSize();
        }

        private void ConnectedComputer_Connected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** CONNECTION EVENT FIRED");
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + ", IS_RT: " + e.IsRoot.ToString());
            output.Add("Connecting to " + e.ConnectionString + "\n");
            UpdateOutput();
            connectedAddress = e.IsRoot ? "root@" + e.ConnectionString + " > " : "user@" + e.ConnectionString + " > ";
            UpdateInputSize();
        }
        
        private void UpdateInputSize()
        {
            connAdd.Width = (int)(terminalFont.MeasureString(connectedAddress).X);

            inputViewport.X = connAdd.X + connAdd.Width;
            inputViewport.Width = container.Width - connAdd.Width;
            terminalInput = new TextBox(inputViewport, (76 - connectedAddress.Length), "", graphics, terminalFont, Color.LightGray, Color.DarkGreen, 30);
            terminalInput.Renderer.Color = Color.LightGray;
            terminalInput.Cursor.Selection = new Color(Color.PeachPuff, .4f);
            terminalInput.Active = true;
        }

        /// <summary>
        /// Writes new text to the terminal. Adds any entered commands to the history.
        /// </summary>
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

        public override void Update(GameTime gameTime)
        {
            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);

            terminalInput.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            terminalInput.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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

        public void ForceQuit()
        {
            output.Add("\n\nKernel panic - not syncing: Fatal exception in interrupt\n\n");
            UpdateOutput();
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

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
