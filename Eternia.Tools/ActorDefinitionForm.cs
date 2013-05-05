using System;
using System.Windows.Forms;
using System.Xml;
using Eternia.Game.Actors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Eternia.Tools.Properties;
using Eternia.Game.Stats;
using System.IO;
using Eternia.Game.Abilities;

namespace Eternia.Tools
{
    public partial class ActorDefinitionForm : Form
    {
        private readonly ActorDefinition actor;
        private readonly string fileName;

        public ActorDefinitionForm()
        {
            InitializeComponent();
        }

        public ActorDefinitionForm(string fileName)
        {
            InitializeComponent();

            this.fileName = fileName;

            if (File.Exists(fileName))
            {
                using (var reader = XmlReader.Create(fileName))
                {
                    actor = IntermediateSerializer.Deserialize<ActorDefinition>(reader, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors\");
                }
            }
            else
            {
                actor = new ActorDefinition();
            }
        }

        private void ActorForm_Load(object sender, EventArgs e)
        {
            actorPropertyGrid.SelectedObject = actor;
            actorStatisticsViewerControl.Statistics = actor.BaseStatistics;

            foreach (var ability in actor.Abilities)
            {
                abilitiesListBox.Items.Add(ability);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true }))
            {
                IntermediateSerializer.Serialize<ActorDefinition>(writer, actor, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors\");
            }
        }

        private void actorPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            actorStatisticsViewerControl.Statistics = actor.BaseStatistics;
        }

        private void addAbilityButton_Click(object sender, EventArgs e)
        {
            var ability = new Ability();
            abilitiesListBox.Items.Add(ability);
            actor.Abilities.Add(ability);
        }

        private void removeAbilityButton_Click(object sender, EventArgs e)
        {
            var ability = (Ability)abilitiesListBox.SelectedItem;
            abilitiesListBox.Items.Remove(ability);
            actor.Abilities.Remove(ability);
        }
    }
}
