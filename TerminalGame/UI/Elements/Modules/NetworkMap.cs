﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Computers;
using TerminalGame.UI.Elements.Modules.ModuleComponents;

namespace TerminalGame.UI.Elements.Modules
{
    class NetworkMap : Module
    {
        private World.World _world;
        private List<NetworkNode> _networkNodes;
        private Texture2D _nodeTexture;
        private Random _rnd;
        private Point _nodeSize;
        private Dictionary<string, Texture2D> _networkNodeSpinners;

        public NetworkNode PlayerCompNode { get; set; }
        public NetworkNode HoverNode { get; set; }
        public NetworkNode ConnectedNode { get; set; }

        public NetworkMap(Game game, Point location, Point size, string title, int nodeSize, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {
            _nodeSize = new Point(nodeSize);
        }

        public override void Initialize()
        {
            base.Initialize();

            // Various markers for the networkmap
            Texture2D spinner01 = Content.Load<Texture2D>("Graphics/Textures/spinner01");
            Texture2D spinner02 = Content.Load<Texture2D>("Graphics/Textures/spinner02");
            Texture2D spinner03 = Content.Load<Texture2D>("Graphics/Textures/spinner03");
            Texture2D spinner04 = Content.Load<Texture2D>("Graphics/Textures/spinner04");
            Texture2D spinner05 = Content.Load<Texture2D>("Graphics/Textures/spinner05");
            Texture2D spinner06 = Content.Load<Texture2D>("Graphics/Textures/spinner06");
            Texture2D spinner07 = Content.Load<Texture2D>("Graphics/Textures/spinner07");
            Texture2D spinner08 = Content.Load<Texture2D>("Graphics/Textures/spinner08");

            _networkNodeSpinners = new Dictionary<string, Texture2D>()
            {
                { "ConnectedSpinner", spinner01 },
                { "PlayerSpinner", spinner02 },
                { "03", spinner03 },
                { "MissionSpinner", spinner04 },
                { "05", spinner05 },
                { "06", spinner06 },
                { "07", spinner07 },
                { "HoverSpinner", spinner08 },
            };

            _rnd = new Random(DateTime.Now.Millisecond);
            _world = World.World.GetInstance();
            _networkNodes = new List<NetworkNode>();
            GenerateMapNoOverlap(30);
            foreach (NetworkNode n in _networkNodes)
                n.Initialize();
        }

        protected override void LoadContent()
        {
            _nodeTexture = Content.Load<Texture2D>("Graphics/Textures/nmapComputer-2");
            base.LoadContent();
        }

        public override void ScissorDraw(GameTime gameTime)
        {
            foreach (NetworkNode n in _networkNodes)
            {
                if(n != PlayerCompNode && n != HoverNode && n != ConnectedNode)
                    n.Draw(gameTime);
            }
            PlayerCompNode.Draw(gameTime);
            ConnectedNode.Draw(gameTime);
            if(HoverNode != null)
                HoverNode.Draw(gameTime);
            base.ScissorDraw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (NetworkNode n in _networkNodes)
            {
                n.Update(gameTime);
            }
        }

        public void GenerateMapNoOverlap(int maxAttempts)
        {
            DateTime begin = DateTime.Now;
            foreach (Computer c in _world.Computers)
            {
                Rectangle _cont;
                int attempts = 0;
                int x = 0;
                int y = 0;
                bool intersects = true;
                // Prevent the nodes from overlapping on the map
                // More elegant way of doing this?
                while (intersects)
                {
                    x = _rnd.Next(Rectangle.X + (_nodeSize.X / 2), Rectangle.X + Rectangle.Width - _nodeSize.X);
                    y = _rnd.Next(Rectangle.Y + _nodeSize.Y, Rectangle.Y + Rectangle.Height - _nodeSize.Y);

                    Point position = new Point(x, y);
                    _cont = new Rectangle(position, _nodeSize);

                    if (_networkNodes.Count > 0)
                    {
                        intersects = false;
                        foreach (NetworkNode node in _networkNodes)
                        {
                            if (_cont.Intersects(node.Rectangle))
                                intersects = true;
                        }
                    }
                    else
                    {
                        intersects = false;
                    }
                    if (++attempts > maxAttempts - 1)
                    {
                        Console.WriteLine("Node for \"{0}\" intersects after {1} attempts -- ignore overlap and continue.", c.Name, attempts);
                        break;
                    }
                }

                //casts to float are NOT redundant!!!
                c.MapX = (float)x / (float)Rectangle.X;
                c.MapY = (float)y / (float)Rectangle.Y;

                NetworkNode n = new NetworkNode(Game, this, new Point(x, y), _nodeSize, c, _nodeTexture, _networkNodeSpinners, false);
                _networkNodes.Add(n);
                n.LeftClick += Node_Click;
                n.MouseHover += Node_Hover;
                n.MouseEnter += Node_Enter;
            }
            TimeSpan donzo = DateTime.Now.Subtract(begin);
            Console.WriteLine("Generated {0} nodes in {1} seconds.", _networkNodes.Count, (donzo.TotalSeconds).ToString("N4"));
        }

        private void Node_Click(object sender, MouseEventArgs e)
        {

        }

        private void Node_Hover(object sender, MouseEventArgs e)
        {

        }

        private void Node_Enter(object sender, MouseEventArgs e)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
