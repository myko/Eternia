using System;
using System.Windows.Forms;
using System.Xml;
using EterniaGame.Actors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Eternia.Tools
{
    public partial class ActorForm : Form
    {
        private readonly string fileName;
        private readonly Actor actor;

        public ActorForm()
        {
            InitializeComponent();
        }

        public ActorForm(string fileName)
        {
            InitializeComponent();

            this.fileName = fileName;

            using (var reader = XmlReader.Create(fileName))
            {
                actor = IntermediateSerializer.Deserialize<Actor>(reader, @"F:\Projects\Eternia\Eternia.XnaClient\GameContent\Actors\");
            }
        }

        private void ActorForm_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = actor;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true }))
            {
                IntermediateSerializer.Serialize<Actor>(writer, actor, @"F:\Projects\Eternia\Eternia.XnaClient\GameContent\Actors\");
            }
        }
    }
}
