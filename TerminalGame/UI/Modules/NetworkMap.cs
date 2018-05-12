﻿using System;
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

        Point offset;
        List<NetworkNode> nodes;
        Random rnd;
        PopUpBox infoBox;
        SpriteFont SpriteFont;
        Point size;
        Rectangle cont;

        public NetworkMap(GraphicsDevice Graphics, Rectangle Container, Texture2D texture, SpriteFont font) : base(Graphics, Container)
        {
            offset = new Point(30, -10);
            size = new Point(25, 25);
            SpriteFont = font;
            //infoBox = new PopUpBox("info\nbox", new Rectangle(0, 0, 150, 50), Drawing.DrawBlankTexture(graphics), SpriteFont, Color.White, Color.Black * 0.5f, Color.White, graphics);
            rnd = new Random(DateTime.Now.Millisecond);
            nodes = new List<NetworkNode>();
            foreach (Computer c in Computers.Computers.computerList)
            {
                bool intersects = true;
                //Rectangle container;
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
                NetworkNode n = new NetworkNode(texture, c, cont, new PopUpBox("info\nbox", new Rectangle(0, 0, 150, 50), SpriteFont, Color.White, Color.Black * 0.5f, Color.White, graphics));
                nodes.Add(n);
                //n.Hover += OnNodeHover;
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
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y + 2), Color.White);

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
