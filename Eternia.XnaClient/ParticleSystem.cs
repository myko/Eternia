using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eternia.Game;

namespace Eternia.XnaClient
{
    public class Particle
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        
        public float Age { get; set; }
        public float LifeSpan { get; set; }

        public float Alpha { get; set; }
        public float Scale { get; set; }
        public float Angle { get; set; }

        public float InverseAge { get { return LifeSpan - Age; } }
        public float AgeFraction { get { return LifeSpan > 0 ? Age / LifeSpan : 0; } }
        public float InverseAgeFraction { get { return 1 - AgeFraction; } }

        public Particle()
        {
            Scale = 1f;
            Alpha = 1f;
            LifeSpan = 1f;
        }
    }

    public class ParticleSystem: SceneNode
    {
        private readonly Effect effect;
        private readonly GraphicsDevice graphicsDevice;

        public static Random random = new Random();

        public float Age { get; set; }
        public float LifeSpan { get; set; }
        public bool IsAlive { get; set; }
        public Vector3 Position { get; set; }
        public List<Particle> Particles { get; set; }

        public Texture2D Texture { get; set; }
        public BlendState BlendState { get; set; }

        public Func<Particle> Emitter { get; set; }
        public int MaxParticles { get; set; }
        public float SpawnRate { get; set; }
        
        public float RotationSpeed { get; set; }
        public List<Vector3> Forces { get; set; }

        public Func<Particle, float> AlphaFunc;
        public Func<Particle, float> ScaleFunc;
        public Func<Particle, float> AngleFunc;

        private float spawn;

        public ParticleSystem(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.effect = effect;
            this.graphicsDevice = graphicsDevice;

            IsAlive = true;
            Particles = new List<Particle>();

            BlendState = BlendState.AlphaBlend;

            Emitter = () => new Particle() { Position = Position };
            MaxParticles = 1000;
            SpawnRate = 1.0f;
            spawn = 0;

            Age = 0;
            LifeSpan = float.PositiveInfinity;

            AlphaFunc = x => x.Alpha;
            ScaleFunc = x => x.Scale;
            AngleFunc = x => x.Angle;

            Forces = new List<Vector3>();
        }

        public override bool IsExpired()
        {
            return !IsAlive && !Particles.Any();
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            while (IsAlive && spawn <= 0f && Particles.Count < MaxParticles)
            {
                Particles.Add(Emitter());
                spawn = SpawnRate;
            }

            if (!isPaused)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Age += deltaTime;
                IsAlive = Age < LifeSpan;

                foreach (var particle in Particles)
                {
                    particle.Age += deltaTime;
                    particle.Position = particle.Position + particle.Velocity * deltaTime;
                    if (Forces.Any())
                        particle.Velocity += Forces.Aggregate((s, x) => x + s) * deltaTime;

                    //if (particle.Position.Y < 0 && particle.Velocity.Y < 0)
                    //    particle.Velocity = new Vector3(particle.Velocity.X, -particle.Velocity.Y, particle.Velocity.Z);

                    particle.Alpha = AlphaFunc(particle); // particle.Opacity - deltaTime * DecayRate;
                    particle.Scale = ScaleFunc(particle); // particle.Size + deltaTime * GrowthRate;
                    particle.Angle = AngleFunc(particle);
                }

                Particles.RemoveAll(p => p.Age > p.LifeSpan);

                spawn -= deltaTime;
            }

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            if (Particles.Any())
            {
                var vertices = new VertexPositionColorTexture[6 * Particles.Count];
                for (int i = 0; i < Particles.Count; i++)
                {
                    var s = Particles[i].Scale;
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

                effect.Parameters["World"].SetValue(Matrix.Identity);
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["Texture"].SetValue(Texture);
                effect.Parameters["Alpha"].SetValue(1);
                effect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

                graphicsDevice.DepthStencilState = DepthStencilState.None;
                graphicsDevice.BlendState = BlendState;
                graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, 2 * Particles.Count);
                }
            }

            base.Draw(view, projection);
        }
    }
}
