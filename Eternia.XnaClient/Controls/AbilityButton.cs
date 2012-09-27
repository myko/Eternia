using System;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using EterniaGame.Abilities;
using EterniaGame.Actors;

namespace EterniaXna
{
    public class AbilityButton: IControlContent
    {
        public Ability Ability { get; set; }

        private ContentControl container;
        private Actor actor;
        private Texture2D texture;
        private Texture2D blankTexture;
        private SpriteFont font;

        public AbilityButton(ContentControl container, Actor actor, Ability ability, Texture2D texture, Texture2D blankTexture, SpriteFont font)
        {
            this.container = container;
            this.actor = actor;
            this.Ability = ability;
            this.texture = texture;
            this.blankTexture = blankTexture;
            this.font = font;
        }

        public void HandleInput(Vector2 position, GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(Vector2 position, GameTime gameTime)
        {
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)container.Width, (int)container.Height);
            if (Ability == actor.CastingAbility)
                container.SpriteBatch.Draw(texture, bounds, Color.Yellow, container.ZIndex + 0.001f);
            else
                container.SpriteBatch.Draw(texture, bounds, Color.White, container.ZIndex + 0.001f);
            if (!Ability.Enabled)
                container.SpriteBatch.Draw(texture, bounds, new Color(Color.TransparentBlack, 0.5f), container.ZIndex + 0.002f);

            if (!Ability.Cooldown.IsReady)
            {
                var cooldown = ((int)Ability.Cooldown.Current).ToString();
                var textSize = font.MeasureString(cooldown);
                var textPosition = new Vector2((int)(container.Width / 2 - textSize.X / 2), (int)(container.Height / 2 - textSize.Y / 2));

                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition, Color.Yellow, container.ZIndex + 0.004f);
            }
        }
    }
}
