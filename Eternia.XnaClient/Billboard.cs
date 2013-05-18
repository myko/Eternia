using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.XnaClient
{
    public class Billboard: SceneNode
    {
        private readonly Effect effect;
        private readonly GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertices = new VertexPositionTexture[6];

        public Texture2D Texture { get; set; }
        public float LifeTime { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Alpha { get; set; }
        public float Angle { get; set; }
        public Color Diffuse { get; set; }

        public float Age { get; private set; }

        public Func<float, Vector2> PositionFunc { get; set; }
        public Func<float, float> AlphaFunc { get; set; }
        public Func<float, float> ScaleFunc { get; set; }
        public Func<float, float> AngleFunc { get; set; }
        public Func<float, Color> DiffuseFunc { get; set; }

        public Billboard(GraphicsDevice graphicsDevice, Effect effect)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;

            LifeTime = 1f;
            Scale = 1f;
            Alpha = 1f;
            Angle = 0f;
            Diffuse = Color.White;
            Age = 0f;

            PositionFunc = x => Position;
            AlphaFunc = x => Alpha;
            ScaleFunc = x => Scale;
            AngleFunc = x => Angle;
            DiffuseFunc = x => Diffuse;

            vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 1));
        }

        public override bool IsExpired()
        {
            return Age >= LifeTime;
        }

        public override void Update(GameTime time, bool isPaused)
        {
            if (!isPaused)
            {
                Age += (float)time.ElapsedGameTime.TotalSeconds;
                var age = Age / LifeTime;

                Position = PositionFunc(age);
                Scale = ScaleFunc(age);
                Alpha = AlphaFunc(age);
                Angle = AngleFunc(age);
                Diffuse = DiffuseFunc(age);
            }
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            var position = new Vector3(Position.X, 1f, Position.Y);

            effect.Parameters["World"].SetValue(Matrix.CreateScale(Scale) * Matrix.CreateRotationY(Angle) * Matrix.CreateTranslation(position));
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["Texture"].SetValue(Texture);
            effect.Parameters["Alpha"].SetValue(Alpha);
            effect.Parameters["Diffuse"].SetValue(Diffuse.ToVector4());

            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.BlendState = BlendState.Additive;
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
