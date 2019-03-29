namespace EditorNDS
{
    partial class MainWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.menuMainFile = new System.Windows.Forms.ToolStripMenuItem();
			this.newPMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openPMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.newFMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.encounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.closeFMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeAMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closePMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusMain = new System.Windows.Forms.StatusStrip();
			this.statusMainLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabInterface1 = new EditorNDS.TabbedInterface.TabInterface();
			this.openROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMain.SuspendLayout();
			this.statusMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuMain
			// 
			this.menuMain.AutoSize = false;
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFile,
            this.testToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(784, 22);
			this.menuMain.TabIndex = 0;
			// 
			// menuMainFile
			// 
			this.menuMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPMenuItem,
            this.openPMenuItem,
            this.menuFileSep1,
            this.newFMenuItem,
            this.openFMenuItem,
            this.menuFileSep2,
            this.closeFMenuItem,
            this.closeAMenuItem,
            this.closePMenuItem});
			this.menuMainFile.Name = "menuMainFile";
			this.menuMainFile.Size = new System.Drawing.Size(37, 18);
			this.menuMainFile.Text = "&File";
			// 
			// newPMenuItem
			// 
			this.newPMenuItem.Name = "newPMenuItem";
			this.newPMenuItem.Size = new System.Drawing.Size(155, 22);
			this.newPMenuItem.Text = "&NewProject ...";
			this.newPMenuItem.Click += new System.EventHandler(this.newPMenuItem_Click);
			// 
			// openPMenuItem
			// 
			this.openPMenuItem.Name = "openPMenuItem";
			this.openPMenuItem.Size = new System.Drawing.Size(155, 22);
			this.openPMenuItem.Text = "&Open Project ...";
			// 
			// menuFileSep1
			// 
			this.menuFileSep1.Name = "menuFileSep1";
			this.menuFileSep1.Size = new System.Drawing.Size(152, 6);
			// 
			// newFMenuItem
			// 
			this.newFMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encounterToolStripMenuItem});
			this.newFMenuItem.Name = "newFMenuItem";
			this.newFMenuItem.Size = new System.Drawing.Size(155, 22);
			this.newFMenuItem.Text = "N&ew File";
			// 
			// encounterToolStripMenuItem
			// 
			this.encounterToolStripMenuItem.Name = "encounterToolStripMenuItem";
			this.encounterToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
			this.encounterToolStripMenuItem.Text = "Encounter";
			// 
			// openFMenuItem
			// 
			this.openFMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem});
			this.openFMenuItem.Name = "openFMenuItem";
			this.openFMenuItem.Size = new System.Drawing.Size(155, 22);
			this.openFMenuItem.Text = "O&pen File ...";
			// 
			// menuFileSep2
			// 
			this.menuFileSep2.Name = "menuFileSep2";
			this.menuFileSep2.Size = new System.Drawing.Size(152, 6);
			// 
			// closeFMenuItem
			// 
			this.closeFMenuItem.Name = "closeFMenuItem";
			this.closeFMenuItem.Size = new System.Drawing.Size(155, 22);
			this.closeFMenuItem.Text = "&Close File";
			// 
			// closeAMenuItem
			// 
			this.closeAMenuItem.Name = "closeAMenuItem";
			this.closeAMenuItem.Size = new System.Drawing.Size(155, 22);
			this.closeAMenuItem.Text = "Close &All";
			// 
			// closePMenuItem
			// 
			this.closePMenuItem.Name = "closePMenuItem";
			this.closePMenuItem.Size = new System.Drawing.Size(155, 22);
			this.closePMenuItem.Text = "Close &Project";
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statsToolStripMenuItem});
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size(40, 18);
			this.testToolStripMenuItem.Text = "Test";
			// 
			// statsToolStripMenuItem
			// 
			this.statsToolStripMenuItem.Name = "statsToolStripMenuItem";
			this.statsToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.statsToolStripMenuItem.Text = "Stats";
			this.statsToolStripMenuItem.Click += new System.EventHandler(this.statsToolStripMenuItem_Click);
			// 
			// statusMain
			// 
			this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMainLabel});
			this.statusMain.Location = new System.Drawing.Point(0, 539);
			this.statusMain.Name = "statusMain";
			this.statusMain.Size = new System.Drawing.Size(784, 22);
			this.statusMain.TabIndex = 1;
			this.statusMain.Text = "statusStrip1";
			// 
			// statusMainLabel
			// 
			this.statusMainLabel.Name = "statusMainLabel";
			this.statusMainLabel.Size = new System.Drawing.Size(39, 17);
			this.statusMainLabel.Text = "Ready";
			// 
			// tabInterface1
			// 
			this.tabInterface1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabInterface1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.tabInterface1.Location = new System.Drawing.Point(0, 22);
			this.tabInterface1.Margin = new System.Windows.Forms.Padding(0);
			this.tabInterface1.Name = "tabInterface1";
			this.tabInterface1.Size = new System.Drawing.Size(784, 517);
			this.tabInterface1.TabIndex = 3;
			// 
			// openROMToolStripMenuItem
			// 
			this.openROMToolStripMenuItem.Name = "openROMToolStripMenuItem";
			this.openROMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openROMToolStripMenuItem.Text = "Open &ROM";
			this.openROMToolStripMenuItem.Click += new System.EventHandler(this.openROMToolStripMenuItem_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.tabInterface1);
			this.Controls.Add(this.statusMain);
			this.Controls.Add(this.menuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuMain;
			this.Name = "MainWindow";
			this.Text = "NDS NDS ROM Editor";
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.statusMain.ResumeLayout(false);
			this.statusMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.ToolStripMenuItem newPMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPMenuItem;
        private System.Windows.Forms.ToolStripSeparator menuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem newFMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFMenuItem;
        private System.Windows.Forms.ToolStripSeparator menuFileSep2;
        private System.Windows.Forms.ToolStripMenuItem closeFMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closePMenuItem;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel statusMainLabel;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem statsToolStripMenuItem;
        private TabbedInterface.TabInterface tabInterface1;
		private System.Windows.Forms.ToolStripMenuItem openROMToolStripMenuItem;
	}
}

