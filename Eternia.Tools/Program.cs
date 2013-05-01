using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Drawing.Design;

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
            var gameAssembly = Assembly.GetAssembly(typeof(Eternia.Game.Battle));
            foreach (var type in gameAssembly.GetTypes())
            {
                if (!type.IsEnum)
                {
                    TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
                    TypeDescriptor.AddAttributes(type, new NotifyParentPropertyAttribute(true));
                    TypeDescriptor.AddAttributes(type, new RefreshPropertiesAttribute(RefreshProperties.All));
                }
            }

            TypeDescriptor.AddAttributes(typeof(Eternia.Game.Stats.Statistics), new EditorAttribute(typeof(StatisticsEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(Eternia.Game.Items.ItemDefinition), new EditorAttribute(typeof(ItemDefinitionEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(Eternia.Game.Stats.StatDefinitionList), new EditorAttribute(typeof(StatDefinitionListEditor), typeof(UITypeEditor)));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToolsForm());
        }
    }
}
