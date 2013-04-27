using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game.Items
{
    public enum ItemSlots
    {
        Head,
        Chest,
        Legs,
        Hands,
        Boots,
        Weapon,
        Offhand,
    }

    public class ItemSlot
    {
        public string Name { get; set; }

        public ItemSlot()
        {
            Name = string.Empty;
        }

        public ItemSlot(string name)
        {
            this.Name = name;
        }
    }

    public static class ItemSlotHelper
    {
        public static string[,] ItemSlotNames = new string[,] 
        { 
            { "Circlet", "Headguard", "Helm" },
            { "Robes", "Chestguard", "Breastplate" },
            { "Leggings", "Legguards", "Greaves" },
            { "Gloves", "Handgrips", "Gauntlets" },
            { "Sandals", "Treads", "Sabatons" },
            { "Sceptre", "Sword", "Axe" },
            { "Tome", "Dagger", "Shield" },
        };

        public static float[] ItemSlotModifier = new float[]
        {     
            1.3f,
            1.8f,
            1.6f,
            1f,
            1.2f,
            1.6f,
            1.5f
        };
    }
}
