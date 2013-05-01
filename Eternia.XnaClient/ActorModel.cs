using Eternia.Game.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.XnaClient
{
    public class ActorModel: SceneNode
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly Effect effect;

        private readonly Texture2D texture;

        public Actor Actor { get; private set; }

        public bool IsMouseOver { get; set; }
        public bool IsSelected { get; set; }
        public bool IsTargeted { get; set; }
        public bool HasSelection { get; set; }

        public ActorModel(Actor actor, Texture2D texture, ContentManager contentManager, GraphicsDevice graphicsDevice, Effect effect)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
            this.texture = texture;

            this.Actor = actor;
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
            {
            }

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            var color = Color.LightGray;
            var outlineColor = Color.White;

            if (IsMouseOver)
            {
                color = Color.White;
                if (Actor.Faction == Factions.Friend)
                    outlineColor = Color.Green;
                else
                    outlineColor = Color.Red;
            }

            if (HasSelection)
            {
                color = Color.Gray;
                if (IsMouseOver)
                    color = Color.LightGray;
                if (IsTargeted)
                    color = Color.Salmon;
                if (IsSelected)
                    color = Color.White;
            }

            Matrix world = Matrix.CreateScale(Actor.Diameter) * Matrix.CreateTranslation(new Vector3(Actor.Position.X, 0, Actor.Position.Y));

            if (!Actor.IsAlive)
            {
                world = Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.ToRadians(90f)) * Matrix.CreateScale(new Vector3(1, 1, 0.6f)) * world;
                color = Color.DarkGray;
            }

            var vertices = new VertexPositionColorTexture[6];

            vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 0.8f, -1),color, new Vector2(0, 0));
            vertices[1] = new VertexPositionColorTexture(new Vector3(1, 0.8f, -1), color, new Vector2(1, 0));
            vertices[2] = new VertexPositionColorTexture(new Vector3(-1, 0.2f, 1), color, new Vector2(0, 1));
            vertices[3] = new VertexPositionColorTexture(new Vector3(1, 0.8f, -1), color, new Vector2(1, 0));
            vertices[4] = new VertexPositionColorTexture(new Vector3(1, 0.2f, 1), color, new Vector2(1, 1));
            vertices[5] = new VertexPositionColorTexture(new Vector3(-1, 0.2f, 1), color, new Vector2(0, 1));

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["Texture"].SetValue(texture);
            effect.Parameters["Alpha"].SetValue(1);
            effect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }
    }
}
