using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using TerminalGame.Utilities;
using TerminalGame.Computers;

namespace TerminalGame.UI.Modules
{
    class NetworkMap : Module
    {
        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set; }

        private List<NetworkNode> _nodes;
        private Random _rnd;
        private readonly SpriteFont _spriteFont;
        private Point _nodeSize;
        private Rectangle _cont, _container;
        private readonly GameWindow _gameWindow;
        private SoundEffect _hover, _click;
        private readonly Texture2D _texture;
        private readonly Dictionary<string, Texture2D> _nodeSpinners;

        /// <summary>
        /// Constructor for the NetworkMap module. Draws a map of computers on the net.
        /// </summary>
        /// <param name="graphics">Graphics Device</param>
        /// <param name="container">Container for the window</param>
        /// <param name="texture">Texture for the window</param>
        /// <param name="font">Font used for the node labels (the infoboxes that pops up when hovering over a node)</param>
        /// <param name="nodeSpinners">Dictionary of \"spinners\" denoting points of interest on the map</param>
        public NetworkMap(GameWindow gameWindow, GraphicsDevice graphics, Rectangle container, Texture2D texture, SpriteFont font, Dictionary<string, Texture2D> nodeSpinners) : base(graphics, container)
        {
            _gameWindow = gameWindow;
            _nodeSize = new Point(24);
            _spriteFont = font;
            _rnd = new Random(DateTime.Now.Millisecond);
            _nodes = new List<NetworkNode>();
            _hover = SoundManager.GetInstance().GetSound("networkNodeHover");
            _click = SoundManager.GetInstance().GetSound("networkNodeClick");
            _graphics = graphics;
            _container = container;
            _texture = texture;
            _nodeSpinners = nodeSpinners;

            //for testing
            //GenerateMap(container, texture, graphics, nodeSpinners);
            
            IsActive = true;
        }

        public void GenerateMap()
        {
            foreach (Computer c in Computers.Computers.GetInstance().ComputerList)
            {
                // Prevent the nodes from overlapping on the map
                // More elegant way of doing this?
                int attempts = 0;
                int x = 0;
                int y = 0;
                bool intersects = true;
                while (intersects)
                {
                    x = _rnd.Next(_container.X + _nodeSize.X, _container.X + _container.Width - _nodeSize.Y);
                    y = _rnd.Next(_container.Y + _nodeSize.X, _container.Y + _container.Height - 2 * _nodeSize.Y);

                    Point position = new Point(x, y);
                    _cont = new Rectangle(position, _nodeSize);

                    if (_nodes.Count > 0)
                    {
                        intersects = false;
                        foreach (NetworkNode node in _nodes)
                        {
                            if (_cont.Intersects(node.Container))
                                intersects = true;
                        }
                    }
                    else
                    {
                        intersects = false;
                    }
                    if (++attempts > 29)
                    {
                        Console.WriteLine("Node for \"{0}\" intersects after {1} attempts -- ignore overlap and continue.", c.Name, attempts);
                        break;
                    }
                }

                //casts to float are NOT redundant!!!
                c.MapX = (float)x / (float)_container.X;
                c.MapY = (float)y / (float)_container.Y;

                NetworkNode n = new NetworkNode(_texture, c, _cont, new PopUpBox(c.Name + " x:" + c.MapX + ", y:" + c.MapY + "\n" + c.IP, 
                    new Point(_cont.X + _cont.Width + 10, _cont.Y - 5), _spriteFont, Color.White, Color.Black * 0.5f, Color.White, _graphics), 
                    _nodeSpinners);

                _nodes.Add(n);
                n.Click += OnNodeClick;
                n.Hover += OnNodeHover;
                n.Enter += OnMouseEnter;
                Thread.Sleep(5);
            }
        }

        public void BuildLoadedMap()
        {
            foreach (Computer c in Computers.Computers.GetInstance().ComputerList)
            {
                int x = (int)(c.MapX * Container.X);
                int y = (int)(c.MapY * Container.Y);

                Point position = new Point(x, y);
                _cont = new Rectangle(position, _nodeSize);

                NetworkNode n = new NetworkNode(_texture, c, _cont, new PopUpBox(c.Name + " x:" + c.MapX + ", y:" + c.MapY + "\n" + c.IP,
                    new Point(_cont.X + _cont.Width + 10, _cont.Y - 5), _spriteFont, Color.White, Color.Black * 0.5f, Color.White, _graphics),
                    _nodeSpinners);

                _nodes.Add(n);
                n.Click += OnNodeClick;
                n.Hover += OnNodeHover;
                n.Enter += OnMouseEnter;
            }
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Texture2D texture = Drawing.DrawBlankTexture(_graphics);
                spriteBatch.Draw(texture, Container, _themeManager.CurrentTheme.ModuleBackgroundColor);

                foreach (NetworkNode node in _nodes)
                {
                    if (node.Computer.LinkedComputers.Count != 0)
                    {
                        foreach (NetworkNode node2 in _nodes)
                        {
                            if (node.Computer.LinkedComputers.Contains(node2.Computer))
                            {
                                Drawing.DrawLine(spriteBatch, texture, Color.White * 0.33f, node.CenterPosition.ToVector2(), node2.CenterPosition.ToVector2());
                            }
                        }
                    }
                }
                foreach (NetworkNode node in _nodes)
                {
                    if (node.Computer != Player.GetInstance().ConnectedComputer && node.Computer != Player.GetInstance().PlayersComputer && !node.IsHovering && !node.Computer.IsMissionObjective)
                        node.Draw(spriteBatch);
                }
                // Makes sure that nodes with spinners are drawn on top, so other nodes don't obtruct them
                foreach (NetworkNode node in _nodes)
                {
                    if (node.Computer == Player.GetInstance().ConnectedComputer || node.Computer == Player.GetInstance().PlayersComputer || node.Computer.IsMissionObjective)
                        node.Draw(spriteBatch);
                }

                // Makes sure that the hover-spinner is always on top of other spinners
                foreach (NetworkNode node in _nodes)
                {
                    if (node.IsHovering)
                    { 
                        node.Draw(spriteBatch);
                        node.InfoBox.Draw(spriteBatch);
                    }
                }

                // Makes sure that infoboxes are drawn on top, so other nodes don't obtruct them
                //foreach (NetworkNode node in _nodes)
                //{
                //    if (node.IsHovering)
                //    {
                //        node.InfoBox.Draw(spriteBatch);
                //    }
                //}

                Drawing.DrawBorder(spriteBatch, Container, texture, 1, _themeManager.CurrentTheme.ModuleOutlineColor);
                spriteBatch.Draw(texture, RenderHeader(), _themeManager.CurrentTheme.ModuleHeaderBackgroundColor);
                spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y), _themeManager.CurrentTheme.ModuleHeaderFontColor);
            }
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (NetworkNode node in _nodes)
            {
                if (node.InfoBox.Container.X + node.InfoBox.Container.Width >= _gameWindow.ClientBounds.Width - 10)
                {
                    node.InfoBox.ChangeLocation(new Point(node.Position.X - node.InfoBox.Container.Width - 10, node.Position.Y));
                }
                node.Update(gameTime);
            }
        }

        private void OnMouseEnter(MouseEventArgs e)
        {
            try
            {
                _hover.Play(0.25f, 1f, 0f);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnNodeClick(NodeClickedEventArgs e)
        {
            if(IsActive)
            {
                if (e.IP == Player.GetInstance().PlayersComputer.IP)
                    CommandParser.ParseCommand("dc");
                else
                    CommandParser.ParseCommand("connect " + e.IP);
            }

            try
            {
                _click.Play(0.25f, 1f, 0f);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnNodeHover(NodeHoverEventArgs e)
        {
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }
    }
}
