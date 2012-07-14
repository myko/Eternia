using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EterniaGame;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace EterniaXna.EditorForms
{
    public partial class EncounterForm : Form
    {
        public EncounterDefinition EncounterDefinition { get; set; }

        public EncounterForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EncounterDefinition != null)
            {
                EncounterDefinition.Name = textBox1.Text;
                using (var writer = XmlWriter.Create(@"D:\Projects\Eternia\EterniaXna\Content\Encounters\" + textBox2.Text, new XmlWriterSettings { Indent = true }))
                {
                    IntermediateSerializer.Serialize(writer, EncounterDefinition, @"D:\Projects\Eternia\EterniaXna\Content\Encounters\");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (EncounterDefinition != null)
            {
                EncounterDefinition.Map = new Map(30, 20);
            }
        }
    }
}
