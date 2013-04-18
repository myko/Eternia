using System;
using System.Windows.Forms;
using System.Xml;
using EterniaGame.Actors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using EterniaGame;
using Eternia.Tools.Properties;

namespace Eternia.Tools
{
    public partial class EncounterForm : Form
    {
        private readonly string fileName;
        private readonly EncounterDefinition encounter;

        public EncounterForm()
        {
            InitializeComponent();
        }

        public EncounterForm(string fileName)
        {
            InitializeComponent();

            this.fileName = fileName;

            using (var reader = XmlReader.Create(fileName))
            {
                encounter = IntermediateSerializer.Deserialize<EncounterDefinition>(reader, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Encounters\");
            }
        }

        private void ActorForm_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = encounter;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true }))
            {
                IntermediateSerializer.Serialize<EncounterDefinition>(writer, encounter, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Encounters\");
            }
        }
    }
}
