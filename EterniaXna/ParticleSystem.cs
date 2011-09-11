using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EterniaGame;

namespace EterniaXna
{
    public class Particle
    {
        public Vector3 Position { get; set; }
        public float Alpha { get; set; }
        public float Size { get; set; }
        public float Angle { get; set; }
    }

    public class ParticleSystem
    {
        public static Random random = new Random();

        public bool IsAlive { get; set; }
        public Vector3 Position { get; set; }
        public Texture2D Texture { get; set; }

        public List<Particle> Particles { get; set; }
        private float spawn = 1.0f;

        public ParticleSystem()
        {
            IsAlive = true;
            Particles = new List<Particle>();
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var particle in Particles)
            {
                particle.Position = particle.Position + new Vector3(random.Between(-0.5f, 0.5f), random.Between(-0.5f, 0.5f), random.Between(0.5f, 2.5f)) * deltaTime;
                particle.Alpha = particle.Alpha - deltaTime * 1f;
                particle.Size = particle.Size + deltaTime * 1f;
                particle.Angle = particle.Angle + deltaTime;
            }

            Particles.RemoveAll(p => p.Alpha < 0);

            spawn -= deltaTime * 100f;

            if (IsAlive && spawn <= 0f)
            {
                Particles.Add(new Particle() { Position = Position, Alpha = random.Between(0.8f, 1f), Size = random.Between(0.1f, 0.3f), Angle = random.Between(0f, 6f) });
                spawn = 1f;
            }
        }

        public void Draw(Effect billboardEffect, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            if (!Particles.Any())
                return;

            var vertices = new VertexPositionColorTexture[6 * Particles.Count];
            for (int i = 0; i < Particles.Count; i++)
            {
                var s = Particles[i].Size;
                var p = Particles[i].Position;
                var color = new Color(Particles[i].Alpha, Particles[i].Alpha, Particles[i].Alpha, Particles[i].Alpha);
                var invView = Matrix.CreateRotationZ(Particles[i].Angle) * Matrix.Invert(view);
                vertices[i * 6 + 0] = new VertexPositionColorTexture(p + invView.Left * s, color, new Vector2(0, 0));
                vertices[i * 6 + 1] = new VertexPositionColorTexture(p + invView.Up * s, color, new Vector2(1, 0));
                vertices[i * 6 + 2] = new VertexPositionColorTexture(p + invView.Down * s, color, new Vector2(0, 1));
                vertices[i * 6 + 3] = new VertexPositionColorTexture(p + invView.Up * s, color, new Vector2(1, 0));
                vertices[i * 6 + 4] = new VertexPositionColorTexture(p + invView.Right * s, color, new Vector2(1, 1));
                vertices[i * 6 + 5] = new VertexPositionColorTexture(p + invView.Down * s, color, new Vector2(0, 1));
            }

            billboardEffect.Parameters["World"].SetValue(Matrix.Identity);
            billboardEffect.Parameters["View"].SetValue(view);
            billboardEffect.Parameters["Projection"].SetValue(projection);
            billboardEffect.Parameters["Texture"].SetValue(Texture);
            billboardEffect.Parameters["Alpha"].SetValue(0.5f);
            billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColorTexture.VertexElements);
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphicsDevice.RenderState.DepthBufferWriteEnable = false;

            billboardEffect.Begin();
            foreach (var pass in billboardEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, 2 * Particles.Count);
                pass.End();
            }
            billboardEffect.End();

            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphicsDevice.RenderState.DepthBufferWriteEnable = true;
        }
    }
}
