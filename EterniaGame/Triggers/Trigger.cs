using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EterniaGame.Triggers
{
    public class Trigger
    {
        public List<TriggerCondition> Conditions { get; set; }
        public List<TriggerAction> Actions { get; set; }

        public Trigger()
        {
            Conditions = new List<TriggerCondition>();
            Actions = new List<TriggerAction>();
        }

        public Trigger(TriggerDefinition triggerDefinition)
            : this()
        {
            Conditions.AddRange(triggerDefinition.Conditions);
            Actions.AddRange(triggerDefinition.Actions);
        }
    }

    public abstract class TriggerCondition
    {
        public abstract bool IsTrue(Battle battle);
    }

    public abstract class TriggerAction
    {
        public abstract void Execute(EncounterDefinition encounterDefinition, Battle battle);
    }
}
