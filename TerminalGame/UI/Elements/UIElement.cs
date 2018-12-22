using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.UI.Elements
{
    class UIElement
    {
        #region fields
        #endregion

        #region properties
        public Rectangle Rectangle { get; private set; }
        #endregion

        #region events
        public event EventHandler MouseHover;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Click;
        #endregion

        public UIElement(Point location, Point size)
        {
            Rectangle = new Rectangle(location, size);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
        }

        protected virtual void OnMouseHover(object sender, EventArgs e)
        {
            MouseHover?.Invoke(this, e);
        }

        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            MouseEnter?.Invoke(this, e);
        }

        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            MouseLeave?.Invoke(this, e);
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
