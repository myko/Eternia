using System;
using System.IO;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.SkinnedModel;
using EterniaGame.Actors;

namespace EterniaXna
{
    public class ActorModel
    {
        public Actor Actor { get; private set; }
        private Model Model { get; set; }
        //private AnimationPlayer AnimationPlayer { get; set; }

        public ActorModel(Actor actor, Model model)
        {
            this.Actor = actor;
            this.Model = model;

            //var skinningData = model.Tag as SkinningData;
            //AnimationPlayer = new AnimationPlayer(skinningData);
            //AnimationPlayer.StartClip(skinningData.AnimationClips["Idle"], true);
            //AnimationPlayer.Update(TimeSpan.FromSeconds(0), true, Matrix.Identity);
        }

        public void Update(GameTime gameTime, Turn turn)
        {
            //AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            //var skinningData = Model.Tag as SkinningData;
            //var idleClip = skinningData.AnimationClips["Idle"];
            //var walkClip = skinningData.AnimationClips["Walk"];

            if (Actor.CastingAbility != null)
            {
                //var abilityClip = skinningData.AnimationClips["Ability1"];
                //if (skinningData.AnimationClips.ContainsKey(Actor.CastingAbility.AnimationName))
                //    abilityClip = skinningData.AnimationClips[Actor.CastingAbility.AnimationName];
                
                //if (Actor.BaseAnimationState == BaseAnimationState.Casting && AnimationPlayer.CurrentClip != abilityClip)
                //    AnimationPlayer.StartClip(abilityClip, true, TimeSpan.FromSeconds(Actor.CastingAbility.Duration));
            }

            //if (Actor.BaseAnimationState == BaseAnimationState.Walking && AnimationPlayer.CurrentClip != walkClip)
            //    AnimationPlayer.StartClip(walkClip, true);

            //if (Actor.BaseAnimationState == BaseAnimationState.Idle && AnimationPlayer.CurrentClip != idleClip)
            //    AnimationPlayer.StartClip(idleClip, true);

            //if (!Actor.IsAlive)
            //    AnimationPlayer.StartClip(idleClip, false);
        }

        public void Draw(Matrix view, Matrix projection, Color color, ContentManager contentManager, GraphicsDevice device, bool drawOutline, Color outlineColor)
        {
            //var bones = AnimationPlayer.GetSkinTransforms();
            //var bones2 = AnimationPlayer.GetWorldTransforms();

            //Matrix world = Matrix.CreateScale(Actor.Diameter) * Matrix.CreateWorld(new Vector3(Actor.Position, 0), new Vector3(0, 0, -1), new Vector3(-Actor.Direction, 0));

            Matrix world = Matrix.CreateScale(Actor.Diameter) * Matrix.CreateWorld(new Vector3(Actor.Position, 0), new Vector3(0, 0, -1), new Vector3(0, 1, 0));

            if (!Actor.IsAlive)
                world = Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90f)) * world * Matrix.CreateTranslation(0, 0, 1);

            // Draw outline
            //if (drawOutline)
            //{
            //    device.RenderState.DepthBufferWriteEnable = false;
            //    foreach (ModelMesh mesh in Model.Meshes)
            //    {
            //        if (mesh.Name.StartsWith("armor") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Chest))
            //            continue;

            //        if (mesh.Name.StartsWith("boots") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Boots))
            //            continue;

            //        if (mesh.Name.StartsWith("gloves") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Hands))
            //            continue;

            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.Parameters["Texture"].SetValue((Texture)null);
            //            //effect.Parameters["Bones"].SetValue(bones);
            //            effect.Parameters["Outline"].SetValue(0.05f);
            //            effect.Parameters["AmbientColor"].SetValue(outlineColor.ToVector3());
            //            effect.Parameters["World"].SetValue(world);
            //            effect.Parameters["View"].SetValue(view);
            //            effect.Parameters["Projection"].SetValue(projection);
            //        }

            //        mesh.Draw();
            //    }
            //    device.RenderState.DepthBufferWriteEnable = true;
            //}

            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (mesh.Name.StartsWith("armor") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Chest))
                    continue;

                if (mesh.Name.StartsWith("boots") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Boots))
                    continue;

                if (mesh.Name.StartsWith("gloves") && !Actor.Equipment.Any(x => x.Slot == ItemSlots.Hands))
                    continue;

                var textureFileName = @"Models\Actors\" + Actor.TextureName + "_" + mesh.Name + "_diffuse";
                var fullPath = Path.Combine(contentManager.RootDirectory, textureFileName + ".xnb");
                Texture2D actorTexture = null;
                if (!File.Exists(fullPath))
                {
                    textureFileName = @"Models\Actors\heman_" + mesh.Name + "_diffuse";
                    fullPath = Path.Combine(contentManager.RootDirectory, textureFileName + ".xnb");
                }
                if (File.Exists(fullPath))
                    actorTexture = contentManager.Load<Texture2D>(textureFileName);

                actorTexture = contentManager.Load<Texture2D>(@"Models\Actors\token_dwarf");
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1, 1, 1));
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0.2f, 0.2f, 0.4f);
                    effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.LightingEnabled = true;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = world;
                    effect.DiffuseColor = Color.White.ToVector3();
                    effect.Texture = actorTexture;
                    effect.TextureEnabled = true;
                }

                
                //foreach (Effect effect in mesh.Effects)
                //{
                //    effect.Parameters["Texture"].SetValue(actorTexture);
                //    //effect.Parameters["Bones"].SetValue(bones);
                //    effect.Parameters["Outline"].SetValue(0f);
                //    effect.Parameters["AmbientColor"].SetValue(Color.Black.ToVector3());
                //    effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
                //    effect.Parameters["World"].SetValue(world);
                //    effect.Parameters["View"].SetValue(view);
                //    effect.Parameters["Projection"].SetValue(projection);
                //}

                mesh.Draw();
            }

            //var weapon = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Weapon);
            //if (weapon != null)
            //{
            //    var rightGripBone = Model.Bones.Single(x => x.Name == "Grip_R");
            //    var swordModel = contentManager.Load<Model>(@"Models\Objects\sword1");

            //    foreach (ModelMesh mesh in swordModel.Meshes)
            //    {
            //        foreach (BasicEffect effect in mesh.Effects)
            //        {
            //            effect.DirectionalLight0.Enabled = true;
            //            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
            //            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //            effect.DirectionalLight1.Enabled = true;
            //            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
            //            effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //            effect.LightingEnabled = true;
            //            effect.View = view;
            //            effect.Projection = projection;
            //            //effect.World = bones2[rightGripBone.Index - (Model.Bones.Count - bones2.Length)] * world;
            //            effect.DiffuseColor = Color.Gray.ToVector3();
            //        }

            //        mesh.Draw();
            //    }
            //}

            //var offhand = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Offhand);
            //if (offhand != null)
            //{
            //    var leftGripBone = Model.Bones.Single(x => x.Name == "Grip_L");
            //    var offhandModel = contentManager.Load<Model>(@"Models\Objects\shield1");
            //    if (offhand.ArmorClass == ItemArmorClasses.Leather)
            //        offhandModel = contentManager.Load<Model>(@"Models\Objects\dagger1");

            //    foreach (ModelMesh mesh in offhandModel.Meshes)
            //    {
            //        foreach (BasicEffect effect in mesh.Effects)
            //        {
            //            effect.DirectionalLight0.Enabled = true;
            //            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
            //            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //            effect.DirectionalLight1.Enabled = true;
            //            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
            //            effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
            //            effect.LightingEnabled = true;
            //            effect.View = view;
            //            effect.Projection = projection;
            //            effect.World = bones2[leftGripBone.Index - (Model.Bones.Count - bones2.Length)] * world;
            //            effect.DiffuseColor = Color.Gray.ToVector3();
            //        }

            //        mesh.Draw();
            //    }
            //}
        }
    }
}
