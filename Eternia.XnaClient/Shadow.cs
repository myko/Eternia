using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Eternia.Game.Actors;

namespace Eternia.XnaClient
{
    public class Shadow: SceneNode
    {
        private GraphicsDevice graphicsDevice;
        private Effect effect;
        private Texture2D texture;
        private Actor actor;
        private VertexPositionTexture[] vertices;

        public Shadow(GraphicsDevice graphicsDevice, Effect effect, Texture2D texture, Actor actor)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
            this.texture = texture;
            this.actor = actor;

            vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            effect.Parameters["World"].SetValue(Matrix.CreateScale(actor.Radius * 1.5f) * Matrix.CreateTranslation(new Vector3(actor.Position.X, 0.03f, actor.Position.Y + 0.5f)));
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["Alpha"].SetValue(0.5f);
            effect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());
            effect.Parameters["Texture"].SetValue(texture);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
            }

            base.Draw(view, projection);
        }
    }
}
