using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Myko.Xna.SkinnedModel;
using EterniaGame.Abilities;
using EterniaGame.Actors;

namespace EterniaGame
{
    public class Projectile
    {
        public Actor Owner { get; set; }
        public Actor Target { get; set; }
        public Ability Ability { get; set; }
        public Vector3 Position { get; set; }
        public bool IsAlive { get; set; }
        public string ModelName { get; set; }
        public string TextureName { get; set; }
        public float Speed { get; set; }

        public Projectile()
        {
            IsAlive = true;
        }
    }
}
