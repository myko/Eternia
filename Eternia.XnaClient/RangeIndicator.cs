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
    public class RangeIndicator: SceneNode
    {
        private Actor actor;
        private GraphicsDevice graphicsDevice;
        private Effect billboardEffect;
        private Texture2D texture;
        private VertexPositionTexture[] vertices;

        public RangeIndicator(Actor actor, GraphicsDevice graphicsDevice, Effect billboardEffect, Texture2D texture)
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
            return false;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            if (actor.CurrentOrder != null && (actor.CurrentOrder.Ability.DamageType == DamageTypes.Cleave || actor.CurrentOrder.Ability.DamageType == DamageTypes.PointBlankArea))
            {
                var color = Color.Red;
                if (actor.Faction == Factions.Friend)
                {
                    color = Color.Blue;
                    if (actor.CurrentOrder.Ability.Healing.Any())
                        color = Color.Green;
                }
                
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                graphicsDevice.DepthStencilState = DepthStencilState.None;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(0.5f);

                var position = new Vector3(actor.Position.X, 0.04f, actor.Position.Y);
                if (actor.CurrentOrder.Ability.DamageType == DamageTypes.Cleave)
                    position = new Vector3(actor.Targets.Peek().Position.X, 0.04f, actor.Targets.Peek().Position.Y);

                var scale = actor.CurrentOrder.Ability.Range.Maximum;
                if (actor.CurrentOrder.Ability.DamageType == DamageTypes.Cleave)
                    scale = actor.Radius + 1;

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
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
