namespace Eternia.Tools
{
    partial class ItemDefinitionEditorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.statsListBox = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statPropertyGrid
            // 
            this.statPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.statPropertyGrid.Name = "statPropertyGrid";
            this.statPropertyGrid.Size = new System.Drawing.Size(447, 401);
            this.statPropertyGrid.TabIndex = 15;
            // 
            // statsListBox
            // 
            this.statsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsListBox.FormattingEnabled = true;
            this.statsListBox.IntegralHeight = false;
            this.statsListBox.Location = new System.Drawing.Point(0, 0);
            this.statsListBox.Name = "statsListBox";
            this.statsListBox.Size = new System.Drawing.Size(225, 401);
            this.statsListBox.Sorted = true;
            this.statsListBox.TabIndex = 14;
            this.statsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.statsListBox_ItemCheck);
            this.statsListBox.SelectedIndexChanged += new System.EventHandler(this.statsListBox_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statsListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.statPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(676, 401);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 16;
            // 
            // StatisticsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "StatisticsControl";
            this.Size = new System.Drawing.Size(676, 401);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid statPropertyGrid;
        private System.Windows.Forms.CheckedListBox statsListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;

    }
}
