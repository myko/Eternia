using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Eternia.XnaClient
{
    public class SceneNode
    {
        public List<SceneNode> Nodes { get; private set; }

        public SceneNode()
        {
            Nodes = new List<SceneNode>();
        }

        public virtual bool IsExpired()
        {
            return Nodes.Any() && Nodes.All(x => x.IsExpired());
        }

        public virtual void Update(GameTime gameTime, bool isPaused)
        {
            Nodes.RemoveAll(x => x.IsExpired());
            Nodes.ForEach(x => x.Update(gameTime, isPaused));
        }

        public virtual void Draw(Matrix view, Matrix projection)
        {
            Nodes.ForEach(x => x.Draw(view, projection));
        }
    }
}
