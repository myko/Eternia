using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eternia.Game.Stats;

namespace Eternia.Tools
{
    public partial class StatisticsViewerControl : UserControl
    {
        private Statistics statistics;
        public Statistics Statistics 
        {
            get { return statistics; }
            set { statistics = value; UpdateStatistics(); }
        }

        public StatisticsViewerControl()
        {
            InitializeComponent();
        }
        
        private void UpdateStatistics()
        {
            statsListBox.Items.Clear();

            if (Statistics != null)
            {
                var types = typeof(StatBase).Assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(StatBase)))
                    {
                        statsListBox.Items.Add(new StatViewModel(Statistics, type));
                    }
                }
            }
        }
    }
}
