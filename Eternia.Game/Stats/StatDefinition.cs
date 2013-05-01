using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Collections;

namespace Eternia.Game.Stats
{
    public class StatDefinition
    {
        private Type statType;
        [ContentSerializerIgnore]
        public Type StatType
        {
            get { return statType; }
            set 
            { 
                statType = value;
                typeName = value.Name;
            }
        }

        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set 
            { 
                typeName = value;
                statType = Type.GetType(value);
                if (statType == null)
                    statType = Type.GetType("Eternia.Game.Stats." + value);
            }
        }

        private StatDefinition()
        {

        }

        public StatDefinition(Type type)
        {
            StatType = type;
        }
    }

    public class StatDefinitionList : List<StatDefinition>, IList
    {
    }
}
