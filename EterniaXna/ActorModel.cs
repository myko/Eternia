using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EterniaXna
{
    public class ActorModel
    {
        public Actor Actor { get; private set; }
        private Model Model { get; set; }
        private AnimationPlayer AnimationPlayer { get; set; }

        public ActorModel(Actor actor, Model model)
        {
            this.Actor = actor;
            this.Model = model;

            var skinningData = model.Tag as SkinningData;
            AnimationPlayer = new AnimationPlayer(skinningData);
            AnimationPlayer.StartClip(skinningData.AnimationClips["Idle"], true);
            AnimationPlayer.Update(TimeSpan.FromSeconds(0), true, Matrix.Identity);
        }

        public void Update(GameTime gameTime, Turn turn)
        {
            AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            var skinningData = Model.Tag as SkinningData;
            var idleClip = skinningData.AnimationClips["Idle"];
            var walkClip = skinningData.AnimationClips["Walk"];
            var swingClip = skinningData.AnimationClips["Swing"];
            var castClip = skinningData.AnimationClips["Casting"];

            if (AnimationPlayer.CurrentClip != swingClip || (AnimationPlayer.CurrentClip == swingClip && AnimationPlayer.CurrentTime >= swingClip.Duration))
            {
                if (Actor.BaseAnimationState == BaseAnimationState.Casting && AnimationPlayer.CurrentClip != castClip)
                    AnimationPlayer.StartClip(castClip, true);

                if (Actor.BaseAnimationState == BaseAnimationState.Walking && AnimationPlayer.CurrentClip != walkClip)
                    AnimationPlayer.StartClip(walkClip, true);

                if (Actor.BaseAnimationState == BaseAnimationState.Idle && AnimationPlayer.CurrentClip != idleClip)
                    AnimationPlayer.StartClip(idleClip, true);
            }

            if (turn.Events.Any(x => x.Actor == Actor && x.Type == EventTypes.Swing) && AnimationPlayer.CurrentClip != swingClip)
                AnimationPlayer.StartClip(swingClip, false);

            if (!Actor.IsAlive)
                AnimationPlayer.StartClip(idleClip, false);
        }

        public void Draw(Matrix view, Matrix projection, Color color, ContentManager contentManager)
        {
            var bones = AnimationPlayer.GetSkinTransforms();
            var bones2 = AnimationPlayer.GetWorldTransforms();

            Matrix world = Matrix.CreateScale(Actor.Diameter) * Matrix.CreateWorld(new Vector3(Actor.Position, 0), new Vector3(0, 0, -1), new Vector3(-Actor.Direction, 0));

            if (!Actor.IsAlive)
                world = Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90f)) * world * Matrix.CreateTranslation(0, 0, 1);

            var actorTexture = contentManager.Load<Texture2D>(@"Models\Actors\" + Actor.TextureName + "_diffuse");

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["Texture"].SetValue(actorTexture);
                    effect.Parameters["Bones"].SetValue(bones);
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
                var rightGripBone = Model.Bones.Single(x => x.Name == "Grip_R");
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
                        effect.World = bones2[rightGripBone.Index - 2] * world;
                        effect.DiffuseColor = Color.Gray.ToVector3();
                    }

                    mesh.Draw();
                }
            }

            var offhand = Actor.Equipment.SingleOrDefault(x => x.Slot == ItemSlots.Offhand);
            if (offhand != null)
            {
                var leftGripBone = Model.Bones.Single(x => x.Name == "Grip_L");
                var offhandModel = contentManager.Load<Model>(@"Models\Objects\test");
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
                        effect.World = bones2[leftGripBone.Index - 2] * world;
                        effect.DiffuseColor = Color.Gray.ToVector3();
                    }

                    mesh.Draw();
                }
            }
        }
    }
}
