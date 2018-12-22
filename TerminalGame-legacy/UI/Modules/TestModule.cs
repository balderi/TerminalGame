using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerminalGame.Utils;
using Microsoft.Xna.Framework.Audio;

namespace TerminalGame.UI.Modules
{
    class TestModule : Module
    {
        public override SpriteFont Font { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsVisible { get; set; }
        public override string Title { get; set; }
        public override Rectangle Container { get; set; }

        private Button _button;
        
        private readonly SpriteFont _fnt;
        
        private int _counter;
        private string _buttonClicks;

        private SoundEffect _click, _yay;

        public TestModule(GraphicsDevice Graphics, Rectangle Container, SpriteFont font, SoundEffect click, SoundEffect yay) : base(Graphics, Container)
        {
            _yay = yay;
            _click = click;

            _fnt = font;

            if (string.IsNullOrEmpty(Title))
            {
                Title = "!!! UNNAMED WINDOW !!!";
            }

            _counter = 0;
            _buttonClicks = _counter + " button clicks!";

            var width = 200;
            var height = 50;
            var x = Container.X + (Container.Width / 2) - width / 2;
            var y = Container.Y + (Container.Height - height) - 15;
            _button = new Button("CLICK ME!", new Rectangle(x, y, width, height), _fnt, Color.White, Color.Pink, Color.HotPink, Color.Red, Graphics);
            _button.Click += Button_Click;
        }

        private void Button_Click(ButtonPressedEventArgs e)
        {
            _click.Play(0.1f, 0.0f, 0.0f);
            _counter++;
            if (_counter % 100 == 0)
                _yay.Play(0.1f, 0.0f, 0.0f);
            _buttonClicks = _counter + " button clicks!";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Drawing.DrawBlankTexture(_graphics);
            Drawing.DrawBorder(spriteBatch, Container, texture, 1, _themeManager.CurrentTheme.ModuleOutlineColor);
            spriteBatch.Draw(texture, Container, _themeManager.CurrentTheme.ModuleBackgroundColor);
            spriteBatch.Draw(texture, RenderHeader(), _themeManager.CurrentTheme.ModuleHeaderBackgroundColor);
            spriteBatch.DrawString(Font, Title, new Vector2(RenderHeader().X + 5, RenderHeader().Y + 2), _themeManager.CurrentTheme.ModuleHeaderFontColor);
            spriteBatch.DrawString(Font, _buttonClicks, new Vector2(Container.X + (Container.Width / 2) - Font.MeasureString(_buttonClicks).X / 2, Container.Y + (Container.Height / 2) - Font.MeasureString(_buttonClicks).Y / 2), Color.White);
            
            _button.Draw(spriteBatch);
        }

        protected override Rectangle RenderHeader()
        {
            return new Rectangle(Container.X, Container.Y, Container.Width, (int)Font.MeasureString(Title).Y);
        }

        public override void Update(GameTime gameTime)
        {
            _button.Update();
        }
    }
}
