using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.XnaClient
{
    public class GraphicEffect: SceneNode
    {
        private readonly Effect effect;
        private readonly GraphicsDevice graphicsDevice;

        public Vector2 Position { get; set; }
        public float Size { get; set; }
        public float Scale { get; private set; }
        public float Alpha { get; set; }
        public float Age { get; set; }
        public float LifeTime { get; set; }
        public Texture2D Texture { get; set; }

        public GraphicEffect(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;

            LifeTime = 0.5f;
        }

        public override bool IsExpired()
        {
            return Age > LifeTime;
        }

        public override void Update(GameTime time, bool isPaused)
        {
            if (!isPaused)
            {
                Age += (float)time.ElapsedGameTime.TotalSeconds;
                Scale = Size * (Age / LifeTime);
                Alpha = 1f - (Age / LifeTime);
            }
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            //var v = Project(Position);
            float s = Scale;

            var p = new Vector3(Position.X, 1f, Position.Y);
            var vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(p + new Vector3(-1, 0, 1) * s, new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(p + new Vector3(1, 0, 1) * s, new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(p + new Vector3(-1, 0, -1) * s, new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(p + new Vector3(1, 0, 1) * s, new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(p + new Vector3(1, 0, -1) * s, new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(p + new Vector3(-1, 0, -1) * s, new Vector2(0, 1));

            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["Texture"].SetValue(Texture);
            effect.Parameters["Alpha"].SetValue(Alpha);
            effect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
            }

            base.Draw(view, projection);
        }
    }
}
