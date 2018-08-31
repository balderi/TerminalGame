using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utilities;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class NetworkMap : Module
    {
        //TODO: Make sure the info box stays inside game window

        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set ; }

        List<NetworkNode> nodes;
        Random rnd;
        private readonly SpriteFont SpriteFont;
        Point size;
        Rectangle cont;

        /// <summary>
        /// Constructor for the NetworkMap module. Draws a map of computers on the net.
        /// </summary>
        /// <param name="Graphics">Graphics Device</param>
        /// <param name="Container">Container for the window</param>
        /// <param name="texture">Texture for the window</param>
        /// <param name="font">Font used for the node labels (the infoboxes that pops up when hovering over a node)</param>
        /// <param name="nodeSpinners">Dictionary of \"spinners\" denoting points of interest on the map</param>
        public NetworkMap(GraphicsDevice Graphics, Rectangle Container, Texture2D texture, SpriteFont font, Dictionary<string, Texture2D> nodeSpinners) : base(Graphics, Container)
        {
            size = new Point(32, 32);
            SpriteFont = font;
            rnd = new Random(DateTime.Now.Millisecond);
            nodes = new List<NetworkNode>();
            foreach (Computer c in Computers.Computers.computerList)
            {
                // Prevent the nodes from overlapping on the map
                // More elegant way of doing this?
                bool intersects = true;
                while (intersects)
                {
                    Point position = new Point(rnd.Next(Container.X + 15, Container.X + Container.Width - 115), rnd.Next(Container.Y + 25, Container.Y + Container.Height - 50));
                    cont = new Rectangle(position, size);
                    if (nodes.Count > 0)
                    {
                        intersects = false;
                        foreach (NetworkNode node in nodes)
                        {
                            if (cont.Intersects(node.Container))
                                intersects = true;
                        }
                    }
                    else
                    {
                        intersects = false;
                    }
                }
                NetworkNode n = new NetworkNode(texture, c, cont, new PopUpBox(c.Name + "\n" + c.IP, new Point(0,0), SpriteFont, Color.White, Color.Black * 0.5f, Color.White, Graphics), nodeSpinners);
                nodes.Add(n);
                n.Click += OnNodeClick;
                Thread.Sleep(5);
            }
            IsActive = true;
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(Graphics);
                spriteBatch.Draw(texture, Container, BackgroundColor);

                foreach (NetworkNode node in nodes)
                {
                    if (node.Computer.LinkedComputers.Count != 0)
                    {
                        foreach (NetworkNode node2 in nodes)
                        {
                            if (node.Computer.LinkedComputers.Contains(node2.Computer))
                            {
                                Drawing.DrawLine(spriteBatch, texture, Color.White * 0.33f, node.CenterPosition.ToVector2(), node2.CenterPosition.ToVector2());
                            }
                        }
                    }
                }
                foreach (NetworkNode node in nodes)
                {
                    if (node.Computer != Player.GetInstance().ConnectedComputer && node.Computer != Player.GetInstance().PlayersComputer && !node.IsHovering && !node.Computer.IsMissionObjective)
                        node.Draw(spriteBatch);
                }
                // Makes sure that nodes with spinners are drawn on top, so other nodes don't obtruct them
                foreach (NetworkNode node in nodes)
                {
                    if (node.Computer == Player.GetInstance().ConnectedComputer || node.Computer == Player.GetInstance().PlayersComputer || node.Computer.IsMissionObjective)
                        node.Draw(spriteBatch);
                }

                // Makes sure that the hover-spinner is always on top of other spinners
                foreach (NetworkNode node in nodes)
                {
                    if (node.IsHovering)
                        node.Draw(spriteBatch);
                }

                // Makes sure that infoboxes are drawn on top, so other nodes don't obtruct them
                foreach (NetworkNode node in nodes)
                {
                    if (node.IsHovering)
                        node.InfoBox.Draw(spriteBatch);
                }

                Drawing.DrawBorder(spriteBatch, Container, texture, 1, BorderColor);
                spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
                spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);
            }
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (NetworkNode node in nodes)
            {
                node.Update(gameTime);
            }
        }

        private void OnNodeClick(NodeClickedEventArgs e)
        {
            if(IsActive)
                CommandParser.ParseCommand("connect " + e.IP);
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
