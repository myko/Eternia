using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace EterniaGame
{
    public class Projectile
    {
        public Actor Owner { get; set; }
        public Actor Target { get; set; }
        public Ability Ability { get; set; }
        public Vector3 Position { get; set; }
        public bool IsAlive { get; set; }
        public AnimationPlayer AnimationPlayer { get; set; }
        public string ModelName { get; set; }
        public string TextureName { get; set; }

        public Projectile()
        {
            IsAlive = true;
        }
    }
}
