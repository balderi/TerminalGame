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
    class NetworkMap : Module
    {
        //TODO: Make sure the info box stays inside game window

        public override SpriteFont Font { get; set; }
        public override Color BackgroundColor { get; set; }
        public override Color BorderColor { get; set; }
        public override Color HeaderColor { get; set; }
        public override bool IsActive { get; set; }
        public override string Title { get; set; }
        
        List<NetworkNode> nodes;
        Random rnd;
        private readonly SpriteFont SpriteFont;
        Point size;
        Rectangle cont;

        public NetworkMap(GraphicsDevice Graphics, Rectangle Container, Texture2D texture, SpriteFont font) : base(Graphics, Container)
        {
            size = new Point(25, 25);
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
                NetworkNode n = new NetworkNode(texture, c, cont, new PopUpBox(c.Name + "\n" + c.IP, new Point(0,0), SpriteFont, Color.White, Color.Black * 0.5f, Color.White, graphics));
                nodes.Add(n);
                n.Click += OnNodeClick;
                Thread.Sleep(5);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Drawing.DrawBlankTexture(graphics);
            Drawing.DrawBorder(spriteBatch, container, texture, 1, BorderColor);
            spriteBatch.Draw(texture, container, BackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), HeaderColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), Color.White);

            foreach (NetworkNode node in nodes)
            {
                node.Draw(spriteBatch);
            }
            foreach (NetworkNode node in nodes)
            {
                if(node.IsHovering)
                    node.InfoBox.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (NetworkNode node in nodes)
            {
                node.Update(gameTime);
            }
        }

        private void OnNodeClick(NodeClickedEventArgs e)
        {
            CommandParser.ParseCommand("connect " + e.IP);
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(container.X, container.Y, container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
