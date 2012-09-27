using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

namespace EterniaXna.Screens
{
    public class MenuScreen: GameScreen
    {
        private Texture2D cloudsBackground;
        private Texture2D buttonTexture;
        private Texture2D buttonMouseOverTexture;
        private Texture2D borderTexture;

        public override void LoadContent()
        {
            base.LoadContent();

            cloudsBackground = ContentManager.Load<Texture2D>(@"Interface\purpleclouds");
            buttonTexture = ContentManager.Load<Texture2D>(@"Interface\button2");
            buttonMouseOverTexture = ContentManager.Load<Texture2D>(@"Interface\button2-mouseover");
            borderTexture = ContentManager.Load<Texture2D>(@"Interface\border-2");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Draw(cloudsBackground, new Rectangle(0, 0, (int)Width, (int)Height), Color.Gray, 0);
        }

        protected Button CreateButton(string text, Vector2 position)
        {
            return new Button
            {
                Content = text,
                Position = position,
                Background = new Color(230, 140, 60),
                BackgroundTexture = buttonTexture,
                MouseOverTexture = buttonMouseOverTexture,
                Width = 150,
                Height = 40,
                ZIndex = 0.1f,
            };
        }

        protected ListBox<T> AddListBox<T>(ControlCollection controls, Vector2 position, float width, float height)
            where T: class
        {
            var listbox = new ListBox<T>
            {
                Width = width - 32,
                Height = height - 32,
                ZIndex = 0.1f,
                Background = Color.TransparentBlack
            }; 

            var border = new Border(listbox)
            {
                Position = position,
                Width = width,
                Height = height,
                ZIndex = 0.1f,
                BorderSize = 16,
                BorderColor = new Color(240, 210, 120),
                BorderTexture = borderTexture,
                Background = new Color(40, 40, 149),
                BackgroundTexture = cloudsBackground
            };

            controls.Add(border);

            return listbox;
        }

        protected BoundListBox<T> AddBoundListBox<T>(ControlCollection controls, Vector2 position, float width, float height)
            where T : class
        {
            var listbox = new BoundListBox<T>
            {
                Width = width - 32,
                Height = height - 32,
                ZIndex = 0.1f,
                Background = Color.TransparentBlack
            };

            var border = new Border(listbox)
            {
                Position = position,
                Width = width,
                Height = height,
                ZIndex = 0.1f,
                BorderSize = 16,
                BorderColor = new Color(160, 140, 120),
                BorderTexture = borderTexture,
                Background = new Color(40, 40, 149),
                BackgroundTexture = cloudsBackground
            };

            controls.Add(border);

            return listbox;
        }
    }
}
