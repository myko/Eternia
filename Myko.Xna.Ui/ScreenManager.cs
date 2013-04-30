using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace Myko.Xna.Ui
{
    public class ScreenManager: DrawableGameComponent
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }
        public ContentManager ContentManager { get { return Game.Content; } }

        private List<GameScreen> screens;
        private bool isInitialized = false;

        public ScreenManager(Game game) : base(game)
        {
            screens = new List<GameScreen>();
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.Width = Game.Window.ClientBounds.Width;
            screen.Height = Game.Window.ClientBounds.Height;

            if (isInitialized)
            {
                screen.SpriteBatch = SpriteBatch;
                screen.Font = Font;
                screen.LoadContent();
            }

            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (screen.ScreenManager == this)
                screen.ScreenManager = null;

            if (screens.Contains(screen))
                screens.Remove(screen);
        }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Game.Content.Load<SpriteFont>(@"Fonts\Kootenay");

            screens.ForEach(screen =>
            {
                screen.SpriteBatch = SpriteBatch;
                screen.Font = Font;
                screen.LoadContent();
            });

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (screens.Count > 0)
                screens.Last().HandleInputScreen(gameTime);

            screens.ForEach(screen => screen.UpdateScreen(gameTime));
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (screens.Count > 0)
                screens.Last().DrawBackground(gameTime);

            SpriteBatch.End();

            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (screens.Count > 0)
                screens.Last().DrawScreen(gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
