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
        // TODO: Fix multiple external writes to the same line acting as seperate line when removed (breaks terminal)
        // TODO: Add command queue for when input is blocked by running program

        private TextBox terminalInput;
        private Rectangle connAdd, inputViewport, outputViewport;
        private int linesToDraw, currentIndex;
        private string terminalOutput, connectedAddress;
        private List<string> history, output;
        private SpriteFont TerminalFont;
        private bool isMultiLine, isInputBlocked, updateInp;
        private Computer connectedComputer;
        
        public bool IsTakingSpecialInput { get; set; }
        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set; }

        public Terminal(GraphicsDevice graphics, Rectangle container, SpriteFont terminalFont) : base(graphics, container)
        {
            TerminalFont = terminalFont;
            Container = container;
            updateInp = true;
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
            if (!isInputBlocked)
            {
                Console.WriteLine("CMD: " + terminalInput.Text.String);
                history.Insert(0, terminalInput.Text.String);
                string input;
                if (terminalInput.Text.String.Contains("§"))
                {
                    string temp = terminalInput.Text.String.Replace('§', '?');
                    input = InputWrap("\n" + connectedAddress + temp);
                }
                else
                    input = InputWrap("\n" + connectedAddress + terminalInput.Text.String);

                string[] inp = input.Split('§');

                for (int i = 0; i < inp.Length; i++)
                {
                    if (inp[i] != "\n")
                    {
                        output.Add(inp[i]);
                    }
                }
                
                currentIndex = 0;

                if (!string.IsNullOrEmpty(terminalInput.Text.String) && !terminalInput.Text.String.Contains("§"))
                    CommandParser.ParseCommand(terminalInput.Text.String);
                
                UpdateOutput();
                terminalInput.Clear();

                if (isMultiLine)
                    UpdateOutput();

                connectedComputer = Player.GetInstance().ConnectedComputer;
                updateInp = true;
            }
        }

        public void Write(string text)
        {
            string[] formattedOut = Format(TextWrap(text));
            for(int i = 0; i < formattedOut.Length; i++)
            {
                output.Add(formattedOut[i]);
                UpdateOutput();
            }
        }

        private string[] Format(string text)
        {
            return text.Split('§');
        }

        public void BlockInput()
        {
            isInputBlocked = true;
        }

        public void UnblockInput()
        {
            isInputBlocked = false;
        }

        private void TerminalInput_TabDown(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!isInputBlocked)
            {
                // TODO: Make autocomplete work as intended
                terminalInput.Text.String = "Autocomplete command";
                terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
            }
        }

        private void TerminalInput_DnArrow(object sender, KeyboardInput.KeyEventArgs e)
        {
            if (!isInputBlocked)
            {
                if (currentIndex > 0)
                {
                    terminalInput.Text.String = history[--currentIndex];
                }
                terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
                Console.WriteLine("DN :: CI:{0} | HC:{1}", currentIndex, history.Count);
            }
        }

        private void TerminalInput_UpArrow(object sender, KeyboardInput.KeyEventArgs e)
            {
            if (!isInputBlocked)
            {
                if (history.Count > 0 && currentIndex < history.Count)
                {
                    terminalInput.Text.String = history[currentIndex++];
                }
                terminalInput.Cursor.TextCursor = terminalInput.Text.String.Length;
                Console.WriteLine("UP :: CI:{0} | HC:{1}", currentIndex, history.Count);
            }
        }

        /// <summary>
        /// Clear the terminal window
        /// </summary>
        public void Clear()
        {
            UpdateOutput();
            for (int i = 0; i < linesToDraw + 1; i++)
            {
                output.Add("\n");
            }
            output.RemoveAt(0); // Leave room for the next prepended newline
            UpdateOutput();
            Console.WriteLine("TERMINAL CLEAR");
        }

        /// <summary>
        /// Disposes of current textbox and creates a brand new one with the proper dimensions.
        /// </summary>
        /// <returns>A brand new textbox</returns>
        private TextBox TextBox()
        {
            if (terminalInput == null)
            {
                Console.WriteLine("*** CREATE TEXTBOX");
                int maxChars = ((int)(Container.Width - TerminalFont.MeasureString(connectedAddress).Length()) / (int)TerminalFont.MeasureString("_").Length()) + 200;
                TextBox retval = new TextBox(inputViewport, maxChars, "", Graphics, TerminalFont, Color.LightGray, Color.DarkGreen, 30);
                retval.Renderer.Color = Color.LightGray;
                retval.Cursor.Selection = new Color(Color.PeachPuff, .4f);
                retval.Active = true;
                retval.EnterDown += TerminalInput_EnterDown;
                retval.UpArrow += TerminalInput_UpArrow;
                retval.DnArrow += TerminalInput_DnArrow;
                retval.TabDown += TerminalInput_TabDown;
                return retval;
            }
            else
            {
                Console.WriteLine("*** DISPOSE TEXTBOX");
                terminalInput?.Dispose();
                terminalInput = null;
                return TextBox();
            }
        }

        /// <summary>
        /// Initialization method for the terminal
        /// </summary>
        public void Init()
        {
            connectedComputer = Player.GetInstance().ConnectedComputer;
            connectedAddress = "root@127.0.0.1 > ";
            terminalOutput = "";
            IsTakingSpecialInput = false;
            history = new List<string>();
            history.Insert(0, "");
            output = new List<string>();
            connAdd = new Rectangle(Container.X + 3, Container.Height - RenderHeader().Height, 
                (int)(TerminalFont.MeasureString(connectedAddress).X), (int)(TerminalFont.MeasureString(connectedAddress).Y));
            inputViewport = new Rectangle(connAdd.Width, connAdd.Y, Container.Width - connAdd.Width, (int)(TerminalFont.MeasureString("MEASURE ME").Y));

            terminalInput = TextBox();

            outputViewport = new Rectangle(Container.X, Container.Y + RenderHeader().Height + 2, 
                Container.Width, Container.Height - (inputViewport.Height) - RenderHeader().Height);
            
            linesToDraw = (int)(outputViewport.Height / TerminalFont.MeasureString("MEASURE THIS").Y);
            Console.WriteLine("INIT: 0x4C54443D" + linesToDraw);
            Clear();

            foreach(Computer c in Computers.Computers.computerList)
            {
                c.Connected += ConnectedComputer_Connected;
                c.Disonnected += ConnectedComputer_Disonnected;
                c.FileSystem.ChangeDirirectory += FileSystem_ChangeDir;
            }
        }

        private void FileSystem_ChangeDir(object sender, EventArgs e)
        {
            updateInp = true;
        }

        private void ConnectedComputer_Disonnected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** DISCONNECTION EVENT FIRED");
            connectedComputer = Player.GetInstance().ConnectedComputer;
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + ", IS_RT: " + e.IsRoot.ToString());
            UpdateOutput();
            connectedAddress = e.IsRoot ? "root@" + e.ConnectionString + " > " : "user@" + e.ConnectionString + " > ";
            updateInp = true;
        }

        private void ConnectedComputer_Connected(object sender, ConnectEventArgs e)
        {
            Console.WriteLine("*** CONNECTION EVENT FIRED");
            connectedComputer = Player.GetInstance().ConnectedComputer;
            Console.WriteLine("*** CON_STR: " + e.ConnectionString.ToString() + ", IS_RT: " + e.IsRoot.ToString());
            UpdateOutput();
            connectedAddress = e.IsRoot ? "root@" + e.ConnectionString + " > " : "user@" + e.ConnectionString + " > ";
            updateInp = true;
        }

        /// <summary>
        /// We have to re-create the textbox each time its length should change,
        /// otherwise the text will get deformed.
        /// </summary>
        private void UpdateInputSize()
        {
            string text = terminalInput.Text.String;
            int oldWidth = connAdd.Width;
            int newWidth = (int)(TerminalFont.MeasureString(connectedAddress).X);
            if(newWidth != oldWidth)
            {
                connAdd.Width = newWidth;
                inputViewport.X = connAdd.X + connAdd.Width;
                inputViewport.Width = Container.Width - connAdd.Width;
                terminalInput = TextBox();
                terminalInput.Text.String = text;
                terminalInput.Cursor.TextCursor = text.Length;
            }
        }

        /// <summary>
        /// Writes new text to the terminal. Adds any entered commands to the history.
        /// </summary>
        public void UpdateOutput()
        {
            int counter = 0;
            int lines = 0;
            foreach (string s in output)
            {
                if (s.Contains("\n"))
                    lines++;
            }
            while (lines > linesToDraw - 1)
            {
                output.RemoveAt(0);
                lines--;
                counter++;
            }
            while (lines < linesToDraw - 1)
            {
                output.Insert(0, "\n");
                lines++;
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
            if (!IsTakingSpecialInput)
            {
                connectedAddress = connectedComputer.PlayerHasRoot ? "root" : "user";
                connectedAddress += "@" + connectedComputer.IP + connectedComputer.FileSystem.CurrentDir.PrintFullPath() + " > ";
            }

            if (updateInp)
            {
                UpdateInputSize();
                updateInp = false;
            }
            terminalInput.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Drawing.DrawBlankTexture(Graphics);
            spriteBatch.Draw(texture, Container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
            Drawing.DrawBorder(spriteBatch, Container, texture, 1, BorderColor);

            spriteBatch.DrawString(TerminalFont, connectedAddress, new Vector2(connAdd.X, connAdd.Y), Color.LightGray);
            spriteBatch.DrawString(TerminalFont, terminalOutput, new Vector2(outputViewport.X + 3 + TestClass.ShakeStuff(1), outputViewport.Y + TestClass.ShakeStuff(1)), Color.Green);
            spriteBatch.DrawString(TerminalFont, terminalOutput, new Vector2(outputViewport.X + 3, outputViewport.Y), Color.LightGray);
            terminalInput.Draw(spriteBatch);
        }

        public void ForceQuit()
        {
            output.Add("\n\nKernel panic - not syncing: Fatal exception in interrupt\n\n");
            UpdateOutput();
        }

        private string TextWrap(string text)
        {
            Console.WriteLine("TextWrap");
            isMultiLine = false;
            int maxlen = Container.Width / (int)TerminalFont.MeasureString("_").Length() * 2;
            if (text.Length <= maxlen || text.Contains("§"))
                return text;

            isMultiLine = true;
            for (int i = 0; i < (text.Length / maxlen); i++)
            {
                text = text.Insert((i + 1) * maxlen - 1, "§\n");
            }
            return text;
        }

        private string InputWrap(string text)
        {
            Console.WriteLine("InputWrap");
            isMultiLine = false;
            int maxlen = Container.Width / (int)TerminalFont.MeasureString("_").Length() * 2;
            if (text.Length <= maxlen)
                return text;

            isMultiLine = true;
            for(int i = 0; i < (text.Length / maxlen); i++)
            {
                 text = text.Insert((i + 1) * maxlen - 1, "§\n");
            }
            return text;
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
