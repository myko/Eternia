using System;
using System.Linq;
using Eternia.Game.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eternia.Game.Events;
using Myko.Xna.Ui;

namespace Eternia.XnaClient
{
    public class ScrollingTextSystem : SceneNode, IEventHandler<ActorTookDamage>, IEventHandler<ActorDodged>, IEventHandler<ActorMissed>, IEventHandler<ActorWasHealed>
    {
        private readonly Scene scene;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont smallFont;
        private readonly SpriteFont largeFont;

        public ScrollingTextSystem(Scene scene, SpriteBatch spriteBatch, SpriteFont smallFont, SpriteFont largeFont)
        {
            this.scene = scene;
            this.spriteBatch = spriteBatch;
            this.smallFont = smallFont;
            this.largeFont = largeFont;

            Event.Subscribe<ActorTookDamage>(this);
        }

        public void Handle(ActorTookDamage ev)
        {
            var font = ev.IsCrit ? largeFont : smallFont;

            Nodes.Add(new ScrollingText(spriteBatch, ev.Damage.ToString("0"), font)
            {
                Target = ev.Actor,
                Color = ev.Actor.Faction == Factions.Enemy ? Color.Yellow : Color.Salmon
            });
        }

        public void Handle(ActorWasHealed ev)
        {
            var font = ev.IsCrit ? largeFont : smallFont;

            Nodes.Add(new ScrollingText(spriteBatch, ev.Healing.ToString("0"), font)
            {
                Target = ev.Actor,
                Color = Color.LightGreen
            });
        }

        public void Handle(ActorDodged ev)
        {
            Nodes.Add(new ScrollingText(spriteBatch, "Dodge", smallFont)
            {
                Target = ev.Actor,
                Color = ev.Actor.Faction == Factions.Enemy ? Color.Yellow : Color.Salmon
            });
        }

        public void Handle(ActorMissed ev)
        {
            Nodes.Add(new ScrollingText(spriteBatch, "Miss", smallFont)
            {
                Target = ev.Actor,
                Color = ev.Actor.Faction == Factions.Enemy ? Color.Yellow : Color.Salmon
            });
        }

        public override bool IsExpired()
        {
            return false;
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            base.Update(gameTime, isPaused);

            foreach (var text in Nodes.OfType<ScrollingText>())
            {
                var position = scene.Project(text.Target.Position)
                    + new Vector2((int)(-text.Width * 0.5f), (int)(-(text.Target.Radius + 75f)))
                    + new Vector2(0, (int)((2f - text.Life) * -text.Speed));

                bool intersects = true;
                while (intersects)
                {
                    var bounds = new Rectangle((int)position.X, (int)position.Y, (int)text.Width, (int)text.Height);
                    bounds.Inflate(5, 5);

                    if (Nodes.OfType<ScrollingText>().Where(x => x != text).Any(x =>
                    {
                        var otherBounds = new Rectangle((int)x.Position.X, (int)x.Position.Y, (int)x.Width, (int)x.Height);
                        return bounds.Intersects(otherBounds);
                    }))
                    {
                        position += new Vector2(-6, 0);
                        intersects = true;
                    }
                    else
                    {
                        intersects = false;
                    }
                }

                text.Position = position;
            }
        }
    }

    public class ScrollingText: SceneNode
    {
        private readonly SpriteBatch spriteBatch;

        public string Text { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public SpriteFont Font { get; private set; }

        public Actor Target { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; }
        public float Speed { get; set; }
        public float Life { get; set; }

        public ScrollingText(SpriteBatch spriteBatch, string text, SpriteFont font)
        {
            this.spriteBatch = spriteBatch;
            this.Text = text;
            this.Font = font;

            var size = font.MeasureString(text);
            Width = size.X;
            Height = size.Y;

            Alpha = 1;
            Speed = 45;
            Life = 2;
        }

        public override bool IsExpired()
        {
            return Life <= 0f;
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            Life -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Position = Position + new Vector2(0, (float)-gameTime.ElapsedGameTime.TotalSeconds * Speed);
            Alpha = Math.Min(1, Life);

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            //if (selectedActors.Contains(st.Source) || selectedActors.Contains(st.Target))
            
            spriteBatch.DrawString(Font, Text, Position, new Color(Color, Alpha));

            spriteBatch.DrawString(Font, Text, Position + new Vector2(-1, -1), new Color(Color.Black, Alpha));
            //spriteBatch.DrawString(Font, Text, Position + new Vector2(1, -1), new Color(Color.Black, Alpha));
            //spriteBatch.DrawString(Font, Text, Position + new Vector2(-1, 1), new Color(Color.Black, Alpha));
            spriteBatch.DrawString(Font, Text, Position + new Vector2(1, 1), new Color(Color.Black, Alpha));
            spriteBatch.DrawString(Font, Text, Position, new Color(Color, Alpha), 0.02f);

            base.Draw(view, projection);
        }
    }
}
