using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.XnaClient
{
    public class Level: SceneNode
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly Model levelModel;

        public Level(GraphicsDevice graphicsDevice, Model levelModel)
        {
            this.graphicsDevice = graphicsDevice;
            this.levelModel = levelModel;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            graphicsDevice.RenderState.DepthBufferEnable = true;
            graphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[0].AddressW = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[0].MinFilter = TextureFilter.GaussianQuad;
            graphicsDevice.SamplerStates[0].MagFilter = TextureFilter.GaussianQuad;
            graphicsDevice.SamplerStates[1].AddressU = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[1].AddressW = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[1].AddressV = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[1].MinFilter = TextureFilter.Anisotropic;

            foreach (ModelMesh mesh in levelModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
                    effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 0.9f, 0.7f);
                    effect.LightingEnabled = true;
                    effect.World = Matrix.Identity;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.FogEnabled = true;
                    effect.FogColor = new Vector3(0.03f, 0.03f, 0.10f);
                    effect.FogEnd = 60f;
                    effect.FogStart = 40f;
                }

                mesh.Draw();
            }

            base.Draw(view, projection);
        }
    }
}
