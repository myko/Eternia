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
            graphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            graphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            graphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            graphicsDevice.SamplerStates[0].MaxMipLevel = 0;

            //graphicsDevice.SamplerStates[1].AddressU = TextureAddressMode.Wrap;
            //graphicsDevice.SamplerStates[1].AddressW = TextureAddressMode.Wrap;
            //graphicsDevice.SamplerStates[1].AddressV = TextureAddressMode.Wrap;
            //graphicsDevice.SamplerStates[1].MinFilter = TextureFilter.None;
            //graphicsDevice.SamplerStates[1].MagFilter = TextureFilter.None;
            //graphicsDevice.SamplerStates[1].MaxMipLevel = 0;

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

                graphicsDevice.Indices = mesh.IndexBuffer;

                foreach (var part in mesh.MeshParts)
                {
                    part.Effect.Begin();
                    

                    graphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                    graphicsDevice.VertexDeclaration = part.VertexDeclaration;

                    foreach (var t in part.Effect.Techniques)
                    {
                        foreach (var p in t.Passes)
                        {
                            p.Begin();

                            graphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
                            graphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
                            graphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;

                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);

                            p.End();
                        }
                    }
                    
                    part.Effect.End();
                }
            }

            base.Draw(view, projection);
        }
    }
}
