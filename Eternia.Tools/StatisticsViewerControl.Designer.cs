namespace Eternia.Tools
{
    partial class StatisticsViewerControl
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
            this.statsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // statsListBox
            // 
            this.statsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsListBox.FormattingEnabled = true;
            this.statsListBox.IntegralHeight = false;
            this.statsListBox.Location = new System.Drawing.Point(0, 0);
            this.statsListBox.Name = "statsListBox";
            this.statsListBox.Size = new System.Drawing.Size(676, 401);
            this.statsListBox.Sorted = true;
            this.statsListBox.TabIndex = 15;
            // 
            // StatisticsViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statsListBox);
            this.Name = "StatisticsViewerControl";
            this.Size = new System.Drawing.Size(676, 401);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox statsListBox;


    }
}
