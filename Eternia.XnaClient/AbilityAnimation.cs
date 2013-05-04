using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Eternia.Game.Actors;
using Microsoft.Xna.Framework.Graphics;
using Eternia.Game;

namespace Eternia.XnaClient
{
    public class AbilityAnimation: SceneNode
    {
        private Actor actor;
        private GraphicsDevice graphicsDevice;
        private Effect billboardEffect;
        private Texture2D texture;
        private VertexPositionTexture[] vertices;

        public AbilityAnimation(Actor actor, GraphicsDevice graphicsDevice, Effect billboardEffect, Texture2D texture)
        {
            this.actor = actor;
            this.graphicsDevice = graphicsDevice;
            this.billboardEffect = billboardEffect;
            this.texture = texture;

            vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
        }

        public override bool IsExpired()
        {
            return !actor.IsAlive;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            if (actor.CurrentOrder != null && actor.CurrentOrder.TargetActor != actor)
            {
                var color = Color.White;
                
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(1f);

                var targetPosition = actor.CurrentOrder.GetTargetLocation();
                var position = actor.Position + (targetPosition - actor.Position) * (1f - (actor.CastingProgress.Current / actor.CastingProgress.Duration));
                var direction = Vector2.Normalize(targetPosition - position);
                var world = Matrix.CreateScale(0.5f) * Matrix.CreateWorld(new Vector3(position.X, 2.0f, position.Y), new Vector3(direction.X, 0, direction.Y), new Vector3(0, 1, 0));

                billboardEffect.Parameters["World"].SetValue(world);
                billboardEffect.Parameters["Diffuse"].SetValue(color.ToVector4());
                billboardEffect.Parameters["Texture"].SetValue(texture);

                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                }
            }

            base.Draw(view, projection);
        }
    }
}
