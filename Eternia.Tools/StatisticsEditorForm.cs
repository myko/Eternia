using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Eternia.Game.Stats;

namespace Eternia.Tools
{
    public partial class StatisticsEditorForm : Form
    {
        public Statistics Statistics { get; private set; }

        public StatisticsEditorForm()
        {
            InitializeComponent();
        }

        public StatisticsEditorForm(Statistics statistics)
        {
            InitializeComponent();

            this.Statistics = statistics;
        }

        private void StatisticsEditorForm_Load(object sender, EventArgs e)
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
    }

    public class StatViewModel
    {
        private readonly Statistics statistics;

        public Type Type { get; set; }

        public StatViewModel(Statistics statistics, Type type)
        {
            this.statistics = statistics;
            this.Type = type;
        }

        public override string ToString()
        {
            return statistics.For(Type).Name;
        }
    }

    public class StatisticsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            Statistics foo = value as Statistics;
            if (svc != null && foo != null)
            {
                using (var form = new StatisticsEditorForm(foo))
                {
                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        value = form.Statistics;
                    }
                }
            }
            return value;
        }
    }
}
