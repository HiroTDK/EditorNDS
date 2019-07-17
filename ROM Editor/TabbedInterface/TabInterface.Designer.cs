namespace EditorNDS.TabbedInterface
{
    partial class TabInterface
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
			this.TabStrip = new System.Windows.Forms.Panel();
			this.TabPanel = new System.Windows.Forms.Panel();
			this.TabListButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TabStrip
			// 
			this.TabStrip.AllowDrop = true;
			this.TabStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TabStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TabStrip.Location = new System.Drawing.Point(0, 0);
			this.TabStrip.Margin = new System.Windows.Forms.Padding(0);
			this.TabStrip.Name = "TabStrip";
			this.TabStrip.Size = new System.Drawing.Size(384, 20);
			this.TabStrip.TabIndex = 0;
			this.TabStrip.Resize += new System.EventHandler(this.TabStrip_Resize);
			this.TabStrip.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabStrip_DragDrop);
			this.TabStrip.DragOver += new System.Windows.Forms.DragEventHandler(this.TabStrip_DragOver);
			// 
			// TabPanel
			// 
			this.TabPanel.AllowDrop = true;
			this.TabPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.TabPanel.Location = new System.Drawing.Point(0, 22);
			this.TabPanel.Margin = new System.Windows.Forms.Padding(0);
			this.TabPanel.Name = "TabPanel";
			this.TabPanel.Size = new System.Drawing.Size(400, 398);
			this.TabPanel.TabIndex = 1;
			// 
			// TabListButton
			// 
			this.TabListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TabListButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TabListButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TabListButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
			this.TabListButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.TabListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.TabListButton.Image = global::ROM_Editor.Properties.Resources.ChevronDownWhite;
			this.TabListButton.Location = new System.Drawing.Point(384, 0);
			this.TabListButton.Margin = new System.Windows.Forms.Padding(0);
			this.TabListButton.Name = "TabListButton";
			this.TabListButton.Size = new System.Drawing.Size(16, 20);
			this.TabListButton.TabIndex = 2;
			this.TabListButton.UseVisualStyleBackColor = false;
			this.TabListButton.Click += new System.EventHandler(this.TabListButton_Click);
			// 
			// TabInterface
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.Controls.Add(this.TabListButton);
			this.Controls.Add(this.TabPanel);
			this.Controls.Add(this.TabStrip);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TabInterface";
			this.Size = new System.Drawing.Size(400, 420);
			this.Load += new System.EventHandler(this.TabInterface_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TabStrip;
        private System.Windows.Forms.Panel TabPanel;
        private System.Windows.Forms.Button TabListButton;
    }
}
