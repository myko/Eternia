namespace Eternia.Tools
{
    partial class StatisticsEditorForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.statsListBox = new System.Windows.Forms.CheckedListBox();
            this.statPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(413, 345);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // statsListBox
            // 
            this.statsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.statsListBox.FormattingEnabled = true;
            this.statsListBox.IntegralHeight = false;
            this.statsListBox.Location = new System.Drawing.Point(12, 12);
            this.statsListBox.Name = "statsListBox";
            this.statsListBox.Size = new System.Drawing.Size(215, 327);
            this.statsListBox.TabIndex = 12;
            this.statsListBox.SelectedIndexChanged += new System.EventHandler(this.statsListBox_SelectedIndexChanged);
            this.statsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.statsListBox_ItemCheck);
            // 
            // statPropertyGrid
            // 
            this.statPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statPropertyGrid.Location = new System.Drawing.Point(233, 12);
            this.statPropertyGrid.Name = "statPropertyGrid";
            this.statPropertyGrid.Size = new System.Drawing.Size(336, 327);
            this.statPropertyGrid.TabIndex = 13;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(494, 345);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // StatisticsEditorForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 380);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.statPropertyGrid);
            this.Controls.Add(this.statsListBox);
            this.Controls.Add(this.okButton);
            this.Name = "StatisticsEditorForm";
            this.Text = "StatisticsEditorForm";
            this.Load += new System.EventHandler(this.StatisticsEditorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckedListBox statsListBox;
        private System.Windows.Forms.PropertyGrid statPropertyGrid;
        private System.Windows.Forms.Button cancelButton;
    }
}