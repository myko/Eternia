namespace Eternia.Tools
{
    partial class ActorDefinitionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorDefinitionForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.actorPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.actorStatisticsViewerControl = new Eternia.Tools.StatisticsViewerControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.removeAbilityButton = new System.Windows.Forms.Button();
            this.addAbilityButton = new System.Windows.Forms.Button();
            this.abilitiesListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(710, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveButton
            // 
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(51, 22);
            this.saveButton.Text = "Save";
            this.saveButton.ToolTipText = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // actorPropertyGrid
            // 
            this.actorPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actorPropertyGrid.Location = new System.Drawing.Point(251, 6);
            this.actorPropertyGrid.Name = "actorPropertyGrid";
            this.actorPropertyGrid.Size = new System.Drawing.Size(443, 323);
            this.actorPropertyGrid.TabIndex = 10;
            this.actorPropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.actorPropertyGrid_SelectedGridItemChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(710, 363);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.actorStatisticsViewerControl);
            this.tabPage1.Controls.Add(this.actorPropertyGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(702, 337);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // actorStatisticsViewerControl
            // 
            this.actorStatisticsViewerControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.actorStatisticsViewerControl.Location = new System.Drawing.Point(8, 6);
            this.actorStatisticsViewerControl.Name = "actorStatisticsViewerControl";
            this.actorStatisticsViewerControl.Size = new System.Drawing.Size(237, 323);
            this.actorStatisticsViewerControl.Statistics = null;
            this.actorStatisticsViewerControl.TabIndex = 11;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.removeAbilityButton);
            this.tabPage2.Controls.Add(this.addAbilityButton);
            this.tabPage2.Controls.Add(this.abilitiesListBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(702, 337);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Abilities";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // removeAbilityButton
            // 
            this.removeAbilityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeAbilityButton.Location = new System.Drawing.Point(91, 303);
            this.removeAbilityButton.Name = "removeAbilityButton";
            this.removeAbilityButton.Size = new System.Drawing.Size(75, 23);
            this.removeAbilityButton.TabIndex = 2;
            this.removeAbilityButton.Text = "Remove";
            this.removeAbilityButton.UseVisualStyleBackColor = true;
            this.removeAbilityButton.Click += new System.EventHandler(this.removeAbilityButton_Click);
            // 
            // addAbilityButton
            // 
            this.addAbilityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addAbilityButton.Location = new System.Drawing.Point(9, 303);
            this.addAbilityButton.Name = "addAbilityButton";
            this.addAbilityButton.Size = new System.Drawing.Size(75, 23);
            this.addAbilityButton.TabIndex = 1;
            this.addAbilityButton.Text = "Add";
            this.addAbilityButton.UseVisualStyleBackColor = true;
            this.addAbilityButton.Click += new System.EventHandler(this.addAbilityButton_Click);
            // 
            // abilitiesListBox
            // 
            this.abilitiesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.abilitiesListBox.FormattingEnabled = true;
            this.abilitiesListBox.IntegralHeight = false;
            this.abilitiesListBox.Location = new System.Drawing.Point(8, 6);
            this.abilitiesListBox.Name = "abilitiesListBox";
            this.abilitiesListBox.Size = new System.Drawing.Size(159, 290);
            this.abilitiesListBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(223, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // ActorDefinitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 388);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ActorDefinitionForm";
            this.Text = "ActorForm";
            this.Load += new System.EventHandler(this.ActorForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.PropertyGrid actorPropertyGrid;
        private StatisticsViewerControl actorStatisticsViewerControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button removeAbilityButton;
        private System.Windows.Forms.Button addAbilityButton;
        private System.Windows.Forms.ListBox abilitiesListBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}