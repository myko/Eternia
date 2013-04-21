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
            var gameAssembly = Assembly.GetAssembly(typeof(EterniaGame.Battle));
            foreach (var type in gameAssembly.GetTypes())
            {
                if (!type.IsEnum)
                    TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            }

            TypeDescriptor.AddAttributes(typeof(Eternia.Game.Stats.Statistics), new EditorAttribute(typeof(StatisticsEditor), typeof(UITypeEditor)));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToolsForm());
        }
    }
}
