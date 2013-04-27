using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game;
using Myko.Xna.SkinnedModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eternia.XnaClient
{
    public class ProjectileModel: SceneNode
    {
        public Projectile Projectile { get; set; }
        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public AnimationPlayer AnimationPlayer { get; set; }

        public ProjectileModel()
        {
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
            {
                if (AnimationPlayer != null)
                    AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

                foreach (var system in Nodes.OfType<ParticleSystem>())
                {
                    system.Position = new Vector3(Projectile.Position.X, Projectile.Position.Z, Projectile.Position.Y);
                    system.IsAlive = Projectile.IsAlive;
                }
            }

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            //var projectileModel = Model;
            //if (AnimationPlayer == null)
            //{
            //    var skinningData = projectileModel.Tag as SkinningData;

            //    AnimationPlayer = new AnimationPlayer(skinningData);
            //    AnimationPlayer.StartClip(skinningData.AnimationClips["Fly"], true);
            //}

            //var bones = AnimationPlayer.GetSkinTransformDualQuaternions();

            //var direction = -Vector3.Normalize(new Vector3(Projectile.Target.Position.X, 1.5f, Projectile.Target.Position.Y) - new Vector3(Projectile.Position.X, Projectile.Position.Z, Projectile.Position.Y));
            //var world = Matrix.CreateScale(0.5f) * Matrix.CreateWorld(new Vector3(Projectile.Position.X, Projectile.Position.Z, Projectile.Position.Y), new Vector3(1, 0, 0), direction);

            //foreach (ModelMesh mesh in projectileModel.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        effect.Parameters["Texture"].SetValue(Texture);
            //        effect.Parameters["Bones"].SetValue(bones);
            //        //effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
            //        effect.Parameters["World"].SetValue(world);
            //        effect.Parameters["View"].SetValue(view);
            //        effect.Parameters["Projection"].SetValue(projection);
            //    }

            //    mesh.Draw();
            //}

            base.Draw(view, projection);
        }
    }
}
