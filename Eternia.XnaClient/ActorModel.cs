using System;
using System.IO;
using System.Linq;
using Eternia.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.SkinnedModel;
using Eternia.Game.Actors;
using System.Collections.Generic;

namespace Eternia.XnaClient
{
    public class ActorModel: SceneNode
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly Effect effect;

        private readonly Texture2D texture;

        public Actor Actor { get; private set; }

        public bool IsMouseOver { get; set; }
        public bool IsSelected { get; set; }
        public bool IsTargeted { get; set; }
        public bool HasSelection { get; set; }

        public ActorModel(Actor actor, Texture2D texture, ContentManager contentManager, GraphicsDevice graphicsDevice, Effect effect)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
            this.texture = texture;

            this.Actor = actor;
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
            {
            }

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            var color = Color.LightGray;
            var outlineColor = Color.White;

            if (IsMouseOver)
            {
                color = Color.White;
                if (Actor.Faction == Factions.Friend)
                    outlineColor = Color.Green;
                else
                    outlineColor = Color.Red;
            }

            if (HasSelection)
            {
                color = Color.Gray;
                if (IsMouseOver)
                    color = Color.LightGray;
                if (IsTargeted)
                    color = Color.Salmon;
                if (IsSelected)
                    color = Color.White;
            }

            Matrix world = Matrix.CreateScale(Actor.Diameter) * Matrix.CreateTranslation(new Vector3(Actor.Position.X, 0, Actor.Position.Y));

            //if (!Actor.IsAlive)
            //    world = Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90f)) * world * Matrix.CreateTranslation(0, 0, 1);

            var vertices = new VertexPositionColorTexture[6];

            vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 0.8f, -1),color, new Vector2(0, 0));
            vertices[1] = new VertexPositionColorTexture(new Vector3(1, 0.8f, -1), color, new Vector2(1, 0));
            vertices[2] = new VertexPositionColorTexture(new Vector3(-1, 0.2f, 1), color, new Vector2(0, 1));
            vertices[3] = new VertexPositionColorTexture(new Vector3(1, 0.8f, -1), color, new Vector2(1, 0));
            vertices[4] = new VertexPositionColorTexture(new Vector3(1, 0.2f, 1), color, new Vector2(1, 1));
            vertices[5] = new VertexPositionColorTexture(new Vector3(-1, 0.2f, 1), color, new Vector2(0, 1));

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["Texture"].SetValue(texture);
            effect.Parameters["Alpha"].SetValue(1);
            effect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColorTexture.VertexElements);
            graphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            graphicsDevice.SamplerStates[0].MaxMipLevel = 0;
            graphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            graphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphicsDevice.RenderState.DepthBufferWriteEnable = true;
            //graphicsDevice.RenderState.CullMode = CullMode.None;


            effect.Begin();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                pass.End();
            }
            effect.End();

            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphicsDevice.RenderState.DepthBufferWriteEnable = true;

            //foreach (ModelMesh mesh in Model.Meshes)
            //{
            //    //var textureFileName = @"Models\Actors\" + Actor.TextureName + "_diffuse";
            //    //var fullPath = Path.Combine(contentManager.RootDirectory, textureFileName + ".xnb");
            //    //Texture2D actorTexture = null;
            //    //if (File.Exists(fullPath))
            //    //    actorTexture = contentManager.Load<Texture2D>(textureFileName);
                
            //    foreach (Effect effect in mesh.Effects)
            //    {                    
            //        //effect.Parameters["Texture"].SetValue(actorTexture);
            //        effect.Parameters["Bones"].SetValue(bones);
            //        effect.Parameters["AmbientColor"].SetValue(new Vector3(0.17f, 0.13f, 0.1f));
            //        effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
            //        effect.Parameters["World"].SetValue(world);
            //        effect.Parameters["View"].SetValue(view);
            //        effect.Parameters["Projection"].SetValue(projection);
            //    }

            //    mesh.Draw();
            //}

            //var weapon = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Weapon);
            //if (weapon != null)
            //{
            //    var rightGripBone = Model.Bones.SingleOrDefault(x => x.Name.EndsWith("ArmR"));
            //    if (rightGripBone != null)
            //    {
            //        var swordModel = contentManager.Load<Model>(@"Models\Objects\sword1");

            //        foreach (ModelMesh mesh in swordModel.Meshes)
            //        {
            //            foreach (BasicEffect effect in mesh.Effects)
            //            {
            //                effect.DirectionalLight0.Enabled = true;
            //                effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
            //                effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //                effect.DirectionalLight1.Enabled = true;
            //                effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
            //                effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //                effect.LightingEnabled = true;
            //                effect.View = view;
            //                effect.Projection = projection;
            //                effect.World = worldTransforms[rightGripBone.Index - (Model.Bones.Count - worldTransforms.Length)] * world;
            //                effect.DiffuseColor = Color.Gray.ToVector3();
            //            }

            //            mesh.Draw();
            //        }
            //    }
            //}

            //var offhand = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Offhand);
            //if (offhand != null)
            //{
            //    var leftGripBone = Model.Bones.SingleOrDefault(x => x.Name.EndsWith("ArmL"));
            //    if (leftGripBone != null)
            //    {
            //        var offhandModel = contentManager.Load<Model>(@"Models\Objects\shield1");
            //        if (offhand.ArmorClass == ItemArmorClasses.Leather)
            //            offhandModel = contentManager.Load<Model>(@"Models\Objects\dagger1");

            //        foreach (ModelMesh mesh in offhandModel.Meshes)
            //        {
            //            foreach (BasicEffect effect in mesh.Effects)
            //            {
            //                effect.DirectionalLight0.Enabled = true;
            //                effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
            //                effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //                effect.DirectionalLight1.Enabled = true;
            //                effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
            //                effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //                effect.LightingEnabled = true;
            //                effect.View = view;
            //                effect.Projection = projection;
            //                effect.World = worldTransforms[leftGripBone.Index - (Model.Bones.Count - worldTransforms.Length)] * world;
            //                effect.DiffuseColor = Color.Gray.ToVector3();
            //            }

            //            mesh.Draw();
            //        }
            //    }
            //}
        }
    }
}
