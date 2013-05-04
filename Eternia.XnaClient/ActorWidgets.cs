using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Eternia.Game.Actors;

namespace Eternia.XnaClient
{
    public class ActorWidgets : SceneNode
    {
        private GraphicsDevice graphicsDevice;
        private Effect billboardEffect;
        private Texture2D selectionTexture;
        private Texture2D destinationTexture;
        private Texture2D orderedDestinationTexture;
        private ActorModel selectedActor;
        private VertexPositionTexture[] vertices;

        public ActorWidgets(GraphicsDevice graphicsDevice, Effect effect, Texture2D selectionTexture, Texture2D destinationTexture, Texture2D orderedDestinationTexture, ActorModel actor)
        {
            this.graphicsDevice = graphicsDevice;
            this.billboardEffect = effect;
            this.selectionTexture = selectionTexture;
            this.destinationTexture = destinationTexture;
            this.orderedDestinationTexture = orderedDestinationTexture;
            this.selectedActor = actor;

            vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 0, -1), new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(1, 0, -1), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(1, 0, 1), new Vector2(1, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(-1, 0, 1), new Vector2(0, 1));
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            if (selectedActor.IsSelected)
            {
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(1f);

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Actor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Actor.Position.X, 0.06f, selectedActor.Actor.Position.Y)));
                if (selectedActor.Actor.Faction == Factions.Friend)
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Green.ToVector4());
                else
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Red.ToVector4());
                billboardEffect.Parameters["Texture"].SetValue(selectionTexture);

                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                }

                if (selectedActor.Actor.Destination.HasValue || selectedActor.Actor.OrderedDestination.HasValue)
                {
                    if (selectedActor.Actor.Destination.HasValue)
                        billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Actor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Actor.Destination.Value.X, 0.05f, selectedActor.Actor.Destination.Value.Y)));
                    if (selectedActor.Actor.OrderedDestination.HasValue)
                        billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Actor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Actor.OrderedDestination.Value.X, 0.05f, selectedActor.Actor.OrderedDestination.Value.Y)));
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());
                    if (selectedActor.Actor.OrderedDestination.HasValue)
                        billboardEffect.Parameters["Texture"].SetValue(orderedDestinationTexture);
                    else
                        billboardEffect.Parameters["Texture"].SetValue(destinationTexture);

                    foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                    }
                }
            }
        }
    }
}
