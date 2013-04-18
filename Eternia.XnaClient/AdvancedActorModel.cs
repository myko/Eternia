using System;
using System.IO;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.SkinnedModel;
using EterniaGame.Actors;
using System.Collections.Generic;

namespace Eternia.XnaClient
{
    public class AdvancedActorModel: SceneNode
    {
        private readonly ContentManager contentManager;

        public Actor Actor { get; private set; }
        private Model Model { get; set; }
        private AnimationPlayer AnimationPlayer { get; set; }

        public bool IsMouseOver { get; set; }
        public bool IsSelected { get; set; }
        public bool IsTargeted { get; set; }
        public bool HasSelection { get; set; }

        public AdvancedActorModel(Actor actor, Model model, ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.Actor = actor;
            this.Model = model;

            var skinningData = model.Tag as SkinningData;
            AnimationPlayer = new AnimationPlayer(skinningData);
            if (skinningData.AnimationClips.ContainsKey("Stand"))
                AnimationPlayer.StartClip(skinningData.AnimationClips["Stand"], true);
            else
                AnimationPlayer.StartClip(skinningData.AnimationClips.First().Value, true);
            AnimationPlayer.Update(TimeSpan.FromSeconds(0), true, Matrix.Identity);
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
            {
                AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

                var skinningData = Model.Tag as SkinningData;
                var idleClip = skinningData.AnimationClips["Stand"];
                var walkClip = skinningData.AnimationClips["Walk"];
                var deathClip = skinningData.AnimationClips["Death"];

                if (Actor.CastingAbility != null)
                {
                    var abilityClip = skinningData.AnimationClips["AttackUnarmed"];
                    if (skinningData.AnimationClips.ContainsKey(Actor.CastingAbility.AnimationName))
                        abilityClip = skinningData.AnimationClips[Actor.CastingAbility.AnimationName];

                    if (Actor.BaseAnimationState == BaseAnimationState.Casting && AnimationPlayer.CurrentClip != abilityClip)
                        AnimationPlayer.StartClip(abilityClip, true, TimeSpan.FromSeconds(Actor.CastingAbility.Duration));
                }

                if (Actor.BaseAnimationState == BaseAnimationState.Walking && AnimationPlayer.CurrentClip != walkClip)
                    AnimationPlayer.StartClip(walkClip, true);

                if (Actor.BaseAnimationState == BaseAnimationState.Idle && AnimationPlayer.CurrentClip != idleClip)
                    AnimationPlayer.StartClip(idleClip, true);

                if (!Actor.IsAlive && AnimationPlayer.CurrentClip != deathClip)
                    AnimationPlayer.StartClip(deathClip, false);
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

            var bones = AnimationPlayer.GetSkinTransformDualQuaternions();
            var worldTransforms = AnimationPlayer.GetWorldTransforms();

            Matrix world = Matrix.CreateScale(Actor.Diameter * 1f / Model.Meshes.First().BoundingSphere.Radius) * Matrix.CreateWorld(new Vector3(Actor.Position.X, 0, Actor.Position.Y), new Vector3(Actor.Direction.Y, 0, -Actor.Direction.X), new Vector3(0, 1, 0));

            //if (!Actor.IsAlive)
            //    world = Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90f)) * world * Matrix.CreateTranslation(0, 0, 1);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                //var textureFileName = @"Models\Actors\" + Actor.TextureName + "_diffuse";
                //var fullPath = Path.Combine(contentManager.RootDirectory, textureFileName + ".xnb");
                //Texture2D actorTexture = null;
                //if (File.Exists(fullPath))
                //    actorTexture = contentManager.Load<Texture2D>(textureFileName);
                
                foreach (Effect effect in mesh.Effects)
                {                    
                    //effect.Parameters["Texture"].SetValue(actorTexture);
                    effect.Parameters["Bones"].SetValue(bones);
                    effect.Parameters["AmbientColor"].SetValue(new Vector3(0.17f, 0.13f, 0.1f));
                    effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();
            }

            var weapon = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Weapon);
            if (weapon != null)
            {
                var rightGripBone = Model.Bones.SingleOrDefault(x => x.Name.EndsWith("ArmR"));
                if (rightGripBone != null)
                {
                    var swordModel = contentManager.Load<Model>(@"Models\Objects\sword1");

                    foreach (ModelMesh mesh in swordModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.DirectionalLight0.Enabled = true;
                            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
                            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                            effect.DirectionalLight1.Enabled = true;
                            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
                            effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                            effect.LightingEnabled = true;
                            effect.View = view;
                            effect.Projection = projection;
                            effect.World = worldTransforms[rightGripBone.Index - (Model.Bones.Count - worldTransforms.Length)] * world;
                            effect.DiffuseColor = Color.Gray.ToVector3();
                        }

                        mesh.Draw();
                    }
                }
            }

            var offhand = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Offhand);
            if (offhand != null)
            {
                var leftGripBone = Model.Bones.SingleOrDefault(x => x.Name.EndsWith("ArmL"));
                if (leftGripBone != null)
                {
                    var offhandModel = contentManager.Load<Model>(@"Models\Objects\shield1");
                    if (offhand.ArmorClass == ItemArmorClasses.Leather)
                        offhandModel = contentManager.Load<Model>(@"Models\Objects\dagger1");

                    foreach (ModelMesh mesh in offhandModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.DirectionalLight0.Enabled = true;
                            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
                            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                            effect.DirectionalLight1.Enabled = true;
                            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
                            effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                            effect.LightingEnabled = true;
                            effect.View = view;
                            effect.Projection = projection;
                            effect.World = worldTransforms[leftGripBone.Index - (Model.Bones.Count - worldTransforms.Length)] * world;
                            effect.DiffuseColor = Color.Gray.ToVector3();
                        }

                        mesh.Draw();
                    }
                }
            }
        }
    }
}
