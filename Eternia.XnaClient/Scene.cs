using System;
using System.Collections.Generic;
using System.Linq;
using Eternia.Game;
using Eternia.Game.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Eternia.XnaClient
{
    public class Scene
    {
        private readonly GraphicsDevice graphicsDevice;

        public List<SceneNode> Nodes { get; private set; }

        Effect billboardEffect;
        Texture2D selectionTexture;

        Vector2 cameraPosition;
        float cameraDistance = 15f;
        Matrix view;
        Matrix projection;

        public Scene(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            Nodes = new List<SceneNode>();

            cameraPosition = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager contentManager)
        {
            billboardEffect = contentManager.Load<Effect>(@"Shaders\Billboard");
            selectionTexture = contentManager.Load<Texture2D>(@"Interface\selection");
        }

        public void HandleInput(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            cameraDistance = Math.Min(60f, Math.Max(10f, 25f - mouseState.ScrollWheelValue * 0.01f));

            if (keyboardState.IsKeyDown(Keys.Left))
                cameraPosition.X = Math.Max(-20, cameraPosition.X - 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X = Math.Min(20, cameraPosition.X + 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Up))
                cameraPosition.Y = Math.Max(-18, cameraPosition.Y - 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Down))
                cameraPosition.Y = Math.Min(30, cameraPosition.Y + 20f * deltaTime);
        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            Nodes.RemoveAll(x => x.IsExpired());
            Nodes.ForEach(x => x.Update(gameTime, isPaused));
        }

        public void Draw()
        {
            float aspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

            var cameraWorldPosition = new Vector3(cameraPosition.X, cameraDistance, cameraPosition.Y);
            var cameraWorldTarget = new Vector3(cameraPosition.X, 0, cameraPosition.Y);
            view = Matrix.CreateLookAt(cameraWorldPosition,
                                       cameraWorldTarget,
                                       Vector3.Cross(Vector3.Normalize(cameraWorldTarget - cameraWorldPosition), new Vector3(-1, 0, 0)));

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    0.1f,
                                                                    1000f);

            //projection = Matrix.CreateOrthographic(54, 30, 0.1f, 1000f);

            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            Nodes.ForEach(x => x.Draw(view, projection));
        }
        
        public void DrawHelperBox(Vector3 position, Color color, ContentManager contentManager)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            var helperModel = contentManager.Load<Model>(@"Models\Objects\helper");
            foreach (ModelMesh mesh in helperModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    (effect as BasicEffect).TextureEnabled = false;
                    effect.DiffuseColor = color.ToVector3();
                    effect.Parameters["World"].SetValue(Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(position));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();
            }
        }

        public void DrawBillboard(Vector3 position, Texture2D texture, float scale)
        {
            var vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));

            var color = Color.Salmon;

            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.DepthStencilState = DepthStencilState.None;

            billboardEffect.Parameters["View"].SetValue(view);
            billboardEffect.Parameters["Projection"].SetValue(projection);
            billboardEffect.Parameters["Alpha"].SetValue(1);
            
            billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
            billboardEffect.Parameters["Diffuse"].SetValue(color.ToVector4());
            billboardEffect.Parameters["Texture"].SetValue(texture);

            foreach (var pass in billboardEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }

        public Vector2 Unproject(MouseState mouseState)
        {
            var near = graphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 0), projection, view, Matrix.Identity);
            var far = graphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 1), projection, view, Matrix.Identity);
            var ray = Vector3.Normalize(far - near);

            var v = near + ray * -near.Y / ray.Y;
            return new Vector2(v.X, v.Z);
        }

        public Vector2 Project(Vector2 v)
        {
            var r = graphicsDevice.Viewport.Project(new Vector3(v.X, 0, v.Y), projection, view, Matrix.Identity);
            return new Vector2(r.X, r.Y);
        }

        public int CountNodes()
        {
            int result = 0;
            Stack<SceneNode> nodesToCount = new Stack<SceneNode>(Nodes);

            while (nodesToCount.Any())
            {
                var currentNode = nodesToCount.Pop();
                foreach (var child in currentNode.Nodes)
                {
                    nodesToCount.Push(child);
                }
                result++;
            }

            return result;
        }
    }
}
