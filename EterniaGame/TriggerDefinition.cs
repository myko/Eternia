using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Triggers;

namespace EterniaGame
{
    public class TriggerDefinition
    {
        public List<TriggerCondition> Conditions { get; set; }
        public List<TriggerAction> Actions { get; set; }

        public TriggerDefinition()
        {
            Conditions = new List<TriggerCondition>();
            Actions = new List<TriggerAction>();
        }
    }
}
