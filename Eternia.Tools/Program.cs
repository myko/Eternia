using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace Eternia.Tools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TypeDescriptor.AddAttributes(typeof(EterniaGame.Damage), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            //TypeDescriptor.AddAttributes(typeof(EterniaGame.Cooldown), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            //TypeDescriptor.AddAttributes(typeof(EterniaGame.Range), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            //TypeDescriptor.AddAttributes(typeof(EterniaGame.ProjectileDefinition), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            //TypeDescriptor.AddAttributes(typeof(Eternia.Game.Stats.Statistics), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            //TypeDescriptor.AddAttributes(typeof(Eternia.Game.Stats.DamageReduction), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            var gameAssembly = Assembly.GetAssembly(typeof(EterniaGame.Battle));
            foreach (var type in gameAssembly.GetTypes())
            {
                TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToolsForm());
        }
    }
}
