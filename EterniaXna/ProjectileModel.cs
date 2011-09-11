using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame;
using Myko.Xna.SkinnedModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EterniaXna
{
    public class ProjectileModel
    {
        public Projectile Projectile { get; set; }
        public ParticleSystem ParticleSystem { get; set; }
        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public AnimationPlayer AnimationPlayer { get; set; }

        public void Update(GameTime gameTime)
        {
            if (AnimationPlayer != null)
                AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            if (ParticleSystem != null)
            {
                ParticleSystem.Position = Projectile.Position;
                ParticleSystem.IsAlive = Projectile.IsAlive;
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var projectileModel = Model;
            if (AnimationPlayer == null)
            {
                var skinningData = projectileModel.Tag as SkinningData;

                AnimationPlayer = new AnimationPlayer(skinningData);
                AnimationPlayer.StartClip(skinningData.AnimationClips["Fly"], true);
            }

            Matrix[] bones = AnimationPlayer.GetSkinTransforms();

            var direction = -Vector3.Normalize(new Vector3(Projectile.Target.Position, 1.5f) - Projectile.Position);
            var world = Matrix.CreateScale(0.5f) * Matrix.CreateWorld(Projectile.Position, new Vector3(0, 0, -1), direction);

            foreach (ModelMesh mesh in projectileModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["Texture"].SetValue(Texture);
                    effect.Parameters["Bones"].SetValue(bones);
                    //effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();
            }
        }
    }
}
