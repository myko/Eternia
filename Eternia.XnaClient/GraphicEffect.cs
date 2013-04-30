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
        public float Scale { get; set; }
        public float Alpha { get; set; }
        public float Age { get; set; }
        public Texture2D Texture { get; set; }

        public GraphicEffect(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
        }

        public override bool IsExpired()
        {
            return Alpha <= 0.05f;
        }

        public override void Update(GameTime time, bool isPaused)
        {
            Age += (float)time.ElapsedGameTime.TotalSeconds;
            Scale = 1f + 10f * Age;
            Alpha = 1f - 4f * Age;
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

            //graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements);
            //graphicsDevice.RenderState.AlphaBlendEnable = true;
            //graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //graphicsDevice.RenderState.DestinationBlend = Blend.One;
            //graphicsDevice.RenderState.DepthBufferWriteEnable = false;
            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            //effect.Begin();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                //pass.Begin();
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                //pass.End();
            }
            //effect.End();

            //graphicsDevice.RenderState.AlphaBlendEnable = true;
            //graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            //graphicsDevice.RenderState.DepthBufferWriteEnable = true;
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;


            base.Draw(view, projection);
        }
    }
}
