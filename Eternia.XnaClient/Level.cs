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
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            foreach (ModelMesh mesh in levelModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.5f, -1f, -0.2f));
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-0.3f, 1f, 0.1f));
                    effect.DirectionalLight1.DiffuseColor = new Vector3(1f, 1f, 1f);
                    effect.LightingEnabled = true;
                    effect.World = Matrix.Identity;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.FogEnabled = true;
                    effect.FogColor = new Vector3(0.03f, 0.03f, 0.10f);
                    effect.FogEnd = 60f;
                    effect.FogStart = 40f;
                }
                
                foreach (var part in mesh.MeshParts)
                {
                    graphicsDevice.Indices = part.IndexBuffer;
                    graphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);

                    foreach (var p in part.Effect.Techniques.SelectMany(x => x.Passes))
                    {
                        p.Apply();

                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }

            base.Draw(view, projection);
        }
    }
}
