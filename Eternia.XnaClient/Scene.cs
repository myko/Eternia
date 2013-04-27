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
        float cameraDistance = 10f;
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

            cameraDistance = Math.Min(40f, Math.Max(5f, 25f - mouseState.ScrollWheelValue * 0.01f));

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
            Nodes.RemoveAll(st => st.IsExpired());
            Nodes.ForEach(st => st.Update(gameTime, isPaused));
        }

        public void DrawHelperBox(Vector3 position, Color color, ContentManager contentManager)
        {
            graphicsDevice.RenderState.DepthBufferEnable = true;
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

        public void Draw(IEnumerable<Actor> selectedActors, Actor mouseOverActor, Battle battle, ContentManager contentManager)
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

            projection = Matrix.CreateOrthographic(54, 30, 0.1f, 1000f);

            graphicsDevice.RenderState.DepthBufferEnable = true;

            foreach (var actorModel in Nodes.OfType<ActorModel>().OrderBy(a => a.Actor.Position.Y))
            {
                actorModel.IsMouseOver = actorModel.Actor == mouseOverActor;
                actorModel.IsTargeted = selectedActors.Any(x => x.Targets.FirstOrDefault() == actorModel.Actor);
                actorModel.HasSelection = selectedActors.Any();
                actorModel.IsSelected = selectedActors.Contains(actorModel.Actor);
            }

            Nodes.ForEach(x => x.Draw(view, projection));
            
            foreach (var selectedActor in selectedActors)
            {
                var vertices = new VertexPositionTexture[6];
                vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
                vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
                vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
                vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));

                graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements);
                graphicsDevice.RenderState.AlphaBlendEnable = true;
                graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                graphicsDevice.RenderState.DepthBufferWriteEnable = true;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(1f);

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Position.X, 0.06f, selectedActor.Position.Y)));
                if (selectedActor.Faction == Factions.Friend)
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Green.ToVector4());
                else
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Red.ToVector4());
                billboardEffect.Parameters["Texture"].SetValue(selectionTexture);

                billboardEffect.Begin();
                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                    pass.End();
                }
                billboardEffect.End();

                if (selectedActor.Destination.HasValue || selectedActor.OrderedDestination.HasValue)
                {
                    if (selectedActor.Destination.HasValue)
                        billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Destination.Value.X, 0.05f, selectedActor.Destination.Value.Y)));
                    if (selectedActor.OrderedDestination.HasValue)
                        billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.OrderedDestination.Value.X, 0.05f, selectedActor.OrderedDestination.Value.Y)));
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());
                    if (selectedActor.OrderedDestination.HasValue)
                        billboardEffect.Parameters["Texture"].SetValue(contentManager.Load<Texture2D>(@"Interface\destination2"));
                    else
                        billboardEffect.Parameters["Texture"].SetValue(contentManager.Load<Texture2D>(@"Interface\destination"));

                    billboardEffect.Begin();
                    foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                        pass.End();
                    }
                    billboardEffect.End();
                }

                graphicsDevice.RenderState.DepthBufferWriteEnable = true;
            }

            DrawShadows(battle, contentManager);

            foreach (var actor in battle.Actors.Where(x => x.Faction == Factions.Enemy && x.IsAlive && x.CurrentOrder != null))
            {
                if (actor.CurrentOrder.Ability.DamageType == DamageTypes.PointBlankArea || actor.CurrentOrder.Ability.DamageType == DamageTypes.Cleave)
                {
                    var vertices = new VertexPositionTexture[6];
                    vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
                    vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                    vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
                    vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                    vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
                    vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));

                    graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements);
                    graphicsDevice.RenderState.AlphaBlendEnable = true;
                    graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                    graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                    graphicsDevice.RenderState.DepthBufferWriteEnable = false;

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
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Red.ToVector4());
                    billboardEffect.Parameters["Texture"].SetValue(contentManager.Load<Texture2D>(@"Interface\circlearea"));

                    billboardEffect.Begin();
                    foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                        pass.End();
                    }
                    billboardEffect.End();

                    graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                }
            }
        }

        private void DrawShadows(Battle battle, ContentManager contentManager)
        {
            foreach (var actor in battle.Actors)
            {
                var vertices = new VertexPositionTexture[6];
                vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
                vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
                vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
                vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
                vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));

                graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements);
                graphicsDevice.RenderState.AlphaBlendEnable = true;
                graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                graphicsDevice.RenderState.DepthBufferWriteEnable = false;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(0.5f);

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(actor.Radius * 0.85f) * Matrix.CreateTranslation(new Vector3(actor.Position.X, 0.03f, actor.Position.Y)));
                billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());
                billboardEffect.Parameters["Texture"].SetValue(contentManager.Load<Texture2D>(@"Sprites\shadow"));

                billboardEffect.Begin();
                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                    pass.End();
                }
                billboardEffect.End();

                graphicsDevice.RenderState.DepthBufferWriteEnable = true;
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
    }
}
