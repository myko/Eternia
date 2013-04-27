using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Triggers;

namespace Eternia.Game
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
