using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Eternia.Tools.Properties;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Eternia.Game.Actors;

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
                    using (var reader = XmlReader.Create(fileName))
                    {
                        var actor = IntermediateSerializer.Deserialize<Actor>(reader, Resources.SourcePath + @"Eternia.XnaClient\GameContent\Actors\");
                        new ActorForm(fileName) { Text = fileName, MdiParent = this }.Show();
                    }
                };
            }

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
