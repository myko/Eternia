﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EterniaGame.Actors;

namespace EterniaGame.Triggers.Actions
{
    public class SpawnActor : TriggerAction
    {
        public string ActorId { get; set; }
        public Vector2 Position { get; set; }

        public override void Execute(EncounterDefinition encounterDefinition, Battle battle)
        {
            var actorDefinition = encounterDefinition.Actors.SingleOrDefault(x => x.Id == ActorId);

            var actor = new Actor(actorDefinition)
            {
                Position = Position,
                Direction = Vector2.Normalize(new Vector2(-1, -1)),
            };

            battle.Actors.Add(actor);
        }
    }
}
