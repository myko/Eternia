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
            return !actor.IsAlive;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            if (actor.CurrentOrder != null)
            {
                if (actor.CurrentOrder.Ability.DamageType == DamageTypes.SingleTarget)
                    return;

                if (actor.CurrentOrder.Ability.TargettingType == TargettingTypes.Self)
                    return;

                var color = Color.Salmon;
                if (actor.Faction == Factions.Friend)
                {
                    color = Color.CornflowerBlue;
                    if (actor.CurrentOrder.Ability.Healing.Any())
                        color = Color.LightGreen;
                }
                
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                graphicsDevice.DepthStencilState = DepthStencilState.None;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(0.5f);

                var position = actor.CurrentOrder.GetTargetLocation();
                if (actor.CurrentOrder.Ability.DamageType == DamageTypes.PointBlankArea)
                    position = actor.Position;

                var scale = actor.CurrentOrder.Ability.Area;
                //if (actor.CurrentOrder.Ability.DamageType == DamageTypes.Cleave)
                //    scale = actor.Radius + 1;

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(position.X, 0.04f, position.Y)));
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
