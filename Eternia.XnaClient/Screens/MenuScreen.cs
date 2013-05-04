using System;
using Eternia.XnaClient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game;
using Microsoft.Xna.Framework.Audio;

namespace EterniaXna.Screens
{
    public class MenuScreen: GameScreen
    {
        private Texture2D cloudsBackground;
        private Texture2D buttonTexture;
        private Texture2D buttonMouseOverTexture;
        private Texture2D borderTexture;
        private ParticleSystem particleSystem;

        public override void LoadContent()
        {
            base.LoadContent();

            cloudsBackground = ContentManager.Load<Texture2D>(@"Interface\purpleclouds");
            buttonTexture = ContentManager.Load<Texture2D>(@"Interface\button1");
            buttonMouseOverTexture = ContentManager.Load<Texture2D>(@"Interface\button1-mouseover");
            borderTexture = ContentManager.Load<Texture2D>(@"Interface\border-1");

            Random random = new Random();

            particleSystem = new ParticleSystem(ContentManager.Load<Effect>(@"Shaders\Particle"), ScreenManager.GraphicsDevice)
            {
                BlendState = BlendState.AlphaBlend,
                Texture = ContentManager.Load<Texture2D>(@"Sprites\star"),
                OpacityFunction = p => p.InverseAgeFraction * p.AgeFraction * 2f,
                SizeFunction = p => p.InverseAgeFraction * 1.0f,
                RotationSpeed = 2f,
                SpawnRate = 0.005f,
                MaxParticles = 600,
                Emitter = () => new Particle()
                {
                    Position = random.NextVector3(-40, 40) + new Vector3(0, 0, -50),
                    Velocity = new Vector3(0, 1, 0) * random.Between(0.9f, 2.5f),
                    Opacity = random.Between(0.4f, 0.8f),
                    LifeSpan = random.Between(3.75f, 7.5f)
                },
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            particleSystem.Update(gameTime, false);
        }

        public override void DrawBackground(GameTime gameTime)
        {
            SpriteBatch.Draw(cloudsBackground, new Rectangle(0, 0, (int)Width, (int)Height), Color.DarkGray, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var aspectRatio = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)ScreenManager.GraphicsDevice.Viewport.Height;
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 10f, 100f);
            particleSystem.Draw(view, projection);
        }
        
        protected Button CreateButton(string text, Vector2 position)
        {
            return new Button
            {
                Content = text,
                Position = position,
                Background = Color.White, // new Color(230, 140, 60),
                BackgroundTexture = buttonTexture,
                MouseOverTexture = buttonMouseOverTexture,
                Width = 150,
                Height = 40,
                ZIndex = 0.1f,
                Sound = ContentManager.Load<SoundEffect>(@"Sounds\157539__nenadsimic__click")
            };
        }

        protected ListBox<T> AddListBox<T>(ControlCollection controls, Vector2 position, float width, float height)
            where T: class
        {
            var listbox = new ListBox<T>
            {
                Width = width - 12,
                Height = height - 12,
                ZIndex = 0.1f,
                Background = Color.Transparent
            }; 

            var border = new Border(listbox)
            {
                Position = position,
                Width = width,
                Height = height,
                ZIndex = 0.1f,
                BorderSize = 2,
                BorderColor = new Color(240, 210, 120),
                BorderTexture = borderTexture,
                Background = new Color(40, 40, 149),
                BackgroundTexture = cloudsBackground,
                Padding = new Vector2(4, 4)
            };

            controls.Add(border);

            return listbox;
        }

        protected BoundListBox<T> AddBoundListBox<T>(ControlCollection controls, Vector2 position, float width, float height)
            where T : class
        {
            var listbox = new BoundListBox<T>
            {
                Width = width - 12,
                Height = height - 12,
                ZIndex = 0.1f,
                Background = Color.Transparent
            };

            var border = new Border(listbox)
            {
                Position = position,
                Width = width,
                Height = height,
                ZIndex = 0.1f,
                BorderSize = 2,
                BorderColor = new Color(160, 140, 120),
                BorderTexture = borderTexture,
                Background = new Color(40, 40, 149),
                BackgroundTexture = cloudsBackground,
                Padding = new Vector2(4, 4)
            };

            controls.Add(border);

            return listbox;
        }
    }
}
