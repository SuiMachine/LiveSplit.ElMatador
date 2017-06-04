namespace LiveSplit.ElMatador
{
    partial class ElMatadorSettings
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
            this.gbStartSplits = new System.Windows.Forms.GroupBox();
            this.B_BrowsePath = new System.Windows.Forms.Button();
            this.TB_Path = new System.Windows.Forms.TextBox();
            this.B_ReinstallWrapper = new System.Windows.Forms.Button();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.gbStartSplits.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbStartSplits
            // 
            this.gbStartSplits.AutoSize = true;
            this.gbStartSplits.Controls.Add(this.B_BrowsePath);
            this.gbStartSplits.Controls.Add(this.TB_Path);
            this.gbStartSplits.Controls.Add(this.B_ReinstallWrapper);
            this.gbStartSplits.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbStartSplits.Location = new System.Drawing.Point(3, 3);
            this.gbStartSplits.Name = "gbStartSplits";
            this.gbStartSplits.Size = new System.Drawing.Size(470, 90);
            this.gbStartSplits.TabIndex = 5;
            this.gbStartSplits.TabStop = false;
            this.gbStartSplits.Text = "Options";
            // 
            // B_BrowsePath
            // 
            this.B_BrowsePath.Location = new System.Drawing.Point(440, 19);
            this.B_BrowsePath.Name = "B_BrowsePath";
            this.B_BrowsePath.Size = new System.Drawing.Size(24, 23);
            this.B_BrowsePath.TabIndex = 2;
            this.B_BrowsePath.Text = "...";
            this.B_BrowsePath.UseVisualStyleBackColor = true;
            // 
            // TB_Path
            // 
            this.TB_Path.Location = new System.Drawing.Point(8, 21);
            this.TB_Path.Name = "TB_Path";
            this.TB_Path.Size = new System.Drawing.Size(426, 20);
            this.TB_Path.TabIndex = 1;
            // 
            // B_ReinstallWrapper
            // 
            this.B_ReinstallWrapper.Location = new System.Drawing.Point(371, 48);
            this.B_ReinstallWrapper.Name = "B_ReinstallWrapper";
            this.B_ReinstallWrapper.Size = new System.Drawing.Size(93, 23);
            this.B_ReinstallWrapper.TabIndex = 0;
            this.B_ReinstallWrapper.Text = "Install Wrapper";
            this.B_ReinstallWrapper.UseVisualStyleBackColor = true;
            this.B_ReinstallWrapper.Click += new System.EventHandler(this.B_ReinstallWrapper_Click);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.gbStartSplits, 0, 0);
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(476, 102);
            this.tlpMain.TabIndex = 0;
            // 
            // ElMatadorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "ElMatadorSettings";
            this.Size = new System.Drawing.Size(476, 111);
            this.gbStartSplits.ResumeLayout(false);
            this.gbStartSplits.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbStartSplits;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Button B_BrowsePath;
        private System.Windows.Forms.TextBox TB_Path;
        private System.Windows.Forms.Button B_ReinstallWrapper;
    }
}
