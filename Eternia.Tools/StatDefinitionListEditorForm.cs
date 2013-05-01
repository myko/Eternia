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
    public partial class StatDefinitionListEditorForm : Form
    {
        public StatDefinitionList StatDefinitionList { get; set; }

        public StatDefinitionListEditorForm()
        {
            InitializeComponent();
        }

        public StatDefinitionListEditorForm(StatDefinitionList statDefinitionList)
        {
            InitializeComponent();

            this.StatDefinitionList = statDefinitionList;
        }

        private void StatDefinitionListEditorForm_Load(object sender, EventArgs e)
        {
            var types = typeof(StatBase).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(StatBase)))
                {
                    statsListBox.Items.Add(type, StatDefinitionList.Any(x => x.StatType == type));
                }
            }
        }

        private void statsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var statType = (Type)statsListBox.Items[e.Index];

            if (e.NewValue == CheckState.Checked)
            {
                if (!StatDefinitionList.Any(x => x.StatType == statType))
                    StatDefinitionList.Add(new StatDefinition(statType));
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                StatDefinitionList.RemoveAll(x => x.StatType == statType);
            }
        }
    }

    public class StatDefinitionListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            StatDefinitionList foo = value as StatDefinitionList;
            if (svc != null && foo != null)
            {
                using (var form = new StatDefinitionListEditorForm(foo))
                {
                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        value = form.StatDefinitionList;
                    }
                }
            }
            return value;
        }
    }
}
