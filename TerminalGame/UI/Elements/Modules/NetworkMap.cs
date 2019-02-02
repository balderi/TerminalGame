using System;
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
        private Color _nodeColor;

        public NetworkMap(Game game, Point location, Point size, string title, int nodeSize, bool hasHeader = true, bool hasBorder = true) : base(game, location, size, title, hasHeader, hasBorder)
        {
            _nodeSize = new Point(nodeSize);
        }

        public override void Initialize()
        {
            base.Initialize();
            _rnd = new Random(DateTime.Now.Millisecond);
            _world = World.World.GetInstance();
            _networkNodes = new List<NetworkNode>();
            GenerateMapNoOverlap(10);
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
                n.Draw(gameTime);
            }
            base.ScissorDraw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (NetworkNode n in _networkNodes)
                n.Update(gameTime);
        }

        public void GenerateMapNoOverlap(int maxAttempts)
        {
            foreach (Computer c in _world.Computers)
            {
                Rectangle _cont;
                // Prevent the nodes from overlapping on the map
                // More elegant way of doing this?
                int attempts = 0;
                int x = 0;
                int y = 0;
                bool intersects = true;
                while (intersects)
                {
                    x = _rnd.Next(Rectangle.X + _nodeSize.X, Rectangle.X + Rectangle.Width - _nodeSize.X);
                    y = _rnd.Next(Rectangle.Y + _nodeSize.Y, Rectangle.Y + Rectangle.Height - 2 * _nodeSize.Y);

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
                    if (++attempts > maxAttempts)
                    {
                        Console.WriteLine("Node for \"{0}\" intersects after {1} attempts -- ignore overlap and continue.", c.Name, attempts);
                        break;
                    }
                }

                //casts to float are NOT redundant!!!
                c.MapX = (float)x / (float)Rectangle.X;
                c.MapY = (float)y / (float)Rectangle.Y;

                NetworkNode n = new NetworkNode(Game, new Point(x, y), _nodeSize, c, _nodeTexture, false);
                _networkNodes.Add(n);
                n.LeftClick += Node_Click;
                n.MouseHover += Node_Hover;
                n.MouseEnter += Node_Enter;
            }
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
    }
}
