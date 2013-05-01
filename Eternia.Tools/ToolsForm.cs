using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Eternia.Game.Actors;
using Eternia.Tools.Properties;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Eternia.Tools
{
    public partial class ToolsForm : Form
    {
        public ToolsForm()
        {
            InitializeComponent();
        }

        private void ToolsForm_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors", "*.xml");

            foreach (var file in files)
            {
                var fileName = file;
                var actorMenuItem = actorsToolStripMenuItem.DropDown.Items.Add(Path.GetFileName(fileName));
                actorMenuItem.Click += (x, y) =>
                {
                    //using (var reader = XmlReader.Create(fileName))
                    //{
                        //var actor = IntermediateSerializer.Deserialize<ActorDefinition>(reader, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors\");
                        new ActorDefinitionForm(fileName) { Text = fileName, MdiParent = this }.Show();
                    //}
                };
            }

            var newActorMenuItem = actorsToolStripMenuItem.DropDown.Items.Add("New...");
            newActorMenuItem.Click += (x, y) =>
            {
                saveFileDialog1.InitialDirectory = Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var fileName = saveFileDialog1.FileName;
                    new ActorDefinitionForm(fileName) { Text = fileName, MdiParent = this }.Show();
                }
            };

            files = Directory.GetFiles(Resources.SourcePath + @"Eternia.XnaClient\GameContent\Encounters", "*.xml");

            foreach (var file in files)
            {
                var fileName = file;
                var encounterMenuItem = encountersToolStripMenuItem.DropDown.Items.Add(Path.GetFileName(fileName));
                encounterMenuItem.Click += (x, y) => new EncounterForm(fileName) { Text = fileName, MdiParent = this }.Show();
            }
        }
    }
}
