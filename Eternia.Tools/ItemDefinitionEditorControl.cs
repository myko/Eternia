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
    public partial class ItemDefinitionEditorControl : UserControl
    {
        private Statistics statistics;
        public Statistics Statistics 
        {
            get { return statistics; }
            set { statistics = value; UpdateStatistics(); }
        }

        public ItemDefinitionEditorControl()
        {
            InitializeComponent();
        }

        private void statsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var statViewModel = (StatViewModel)statsListBox.Items[e.Index];

            if (e.NewValue == CheckState.Checked)
            {
                if (!Statistics.Has(statViewModel.Type))
                {
                    var stat = (StatBase)Activator.CreateInstance(statViewModel.Type);
                    Statistics.Add(stat);
                    if (statsListBox.SelectedIndex == e.Index)
                        statPropertyGrid.SelectedObject = stat;
                }
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (Statistics.Has(statViewModel.Type))
                    Statistics.RemoveAll(x => x.GetType() == statViewModel.Type);
            }
        }

        private void statsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var statViewModel = (StatViewModel)statsListBox.Items[statsListBox.SelectedIndex];

            if (Statistics.Has(statViewModel.Type))
                statPropertyGrid.SelectedObject = Statistics.For(statViewModel.Type);
            else
                statPropertyGrid.SelectedObject = null;
        }
        
        private void UpdateStatistics()
        {
            if (Statistics != null)
            {
                var types = typeof(StatBase).Assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(StatBase)))
                    {
                        statsListBox.Items.Add(new StatViewModel(Statistics, type), Statistics.Has(type));
                    }
                }
            }
        }
    }
}
