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
    public partial class ItemDefinitionEditorForm : Form
    {
        public Statistics Statistics
        {
            get { return statisticsControl1.Statistics; }
            set { statisticsControl1.Statistics = value; }
        }

        public ItemDefinitionEditorForm()
        {
            InitializeComponent();
        }

        public ItemDefinitionEditorForm(Statistics statistics)
        {
            InitializeComponent();

            this.Statistics = statistics;
        }
    }
    
    public class ItemDefinitionEditor : UITypeEditor
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
