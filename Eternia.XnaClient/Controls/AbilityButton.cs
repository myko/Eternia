using System;
using Eternia.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game.Abilities;
using Eternia.Game.Actors;

namespace EterniaXna
{
    public class AbilityButton: IControlContent
    {
        public Actor Actor { get; set; }
        public Ability Ability { get; set; }

        private ContentControl container;
        private Texture2D texture;
        private Texture2D blankTexture;
        private SpriteFont font;

        public AbilityButton(ContentControl container, Actor actor, Ability ability, Texture2D texture, Texture2D blankTexture, SpriteFont font)
        {
            this.container = container;
            this.Actor = actor;
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
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)container.ActualWidth, (int)container.ActualHeight);
            //if (Actor.CurrentOrder != null && Ability == Actor.CurrentOrder.Ability)
            //    container.SpriteBatch.Draw(texture, bounds, Color.Yellow, container.ZIndex + 0.001f);
            //else
                container.SpriteBatch.Draw(texture, bounds, Color.White, container.ZIndex + 0.001f);
            
            if (!Ability.Cooldown.IsReady)
            {
                var cooldown = ((int)Ability.Cooldown.Current).ToString();
                var textSize = font.MeasureString(cooldown);
                var textPosition = new Vector2((int)(container.ActualWidth / 2 - textSize.X / 2), (int)(container.ActualHeight / 2 - textSize.Y / 2));

                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition, Color.Yellow, container.ZIndex + 0.004f);
            }
        }
    }
}
