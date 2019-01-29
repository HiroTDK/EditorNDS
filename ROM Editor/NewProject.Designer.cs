namespace EditorNDS
{
	partial class NewProject
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProject));
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.labelName = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.labelAuthor = new System.Windows.Forms.Label();
			this.textBoxAuthor = new System.Windows.Forms.TextBox();
			this.labelFolder = new System.Windows.Forms.Label();
			this.labelFolderInput = new System.Windows.Forms.Label();
			this.buttonFolder = new System.Windows.Forms.Button();
			this.labelROM = new System.Windows.Forms.Label();
			this.labelROMInput = new System.Windows.Forms.Label();
			this.buttonROM = new System.Windows.Forms.Button();
			this.buttonCreate = new System.Windows.Forms.Button();
			this.progressBar1 = new EditorNDS.ProgressBarText();
			this.progressBar2 = new EditorNDS.ProgressBarText();
			this.SuspendLayout();
			// 
			// labelName
			// 
			resources.ApplyResources(this.labelName, "labelName");
			this.labelName.ForeColor = System.Drawing.Color.White;
			this.labelName.Name = "labelName";
			// 
			// textBoxName
			// 
			this.textBoxName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			resources.ApplyResources(this.textBoxName, "textBoxName");
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.ShortcutsEnabled = false;
			this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
			this.textBoxName.GotFocus += new System.EventHandler(this.textBoxName_GotFocus);
			this.textBoxName.LostFocus += new System.EventHandler(this.textBoxName_LostFocus);
			// 
			// labelAuthor
			// 
			resources.ApplyResources(this.labelAuthor, "labelAuthor");
			this.labelAuthor.ForeColor = System.Drawing.Color.White;
			this.labelAuthor.Name = "labelAuthor";
			// 
			// textBoxAuthor
			// 
			resources.ApplyResources(this.textBoxAuthor, "textBoxAuthor");
			this.textBoxAuthor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.textBoxAuthor.Name = "textBoxAuthor";
			this.textBoxAuthor.ShortcutsEnabled = false;
			this.textBoxAuthor.TextChanged += new System.EventHandler(this.textBoxAuthor_TextChanged);
			this.textBoxAuthor.GotFocus += new System.EventHandler(this.textBoxAuthor_GotFocus);
			this.textBoxAuthor.LostFocus += new System.EventHandler(this.textBoxAuthor_LostFocus);
			// 
			// labelFolder
			// 
			resources.ApplyResources(this.labelFolder, "labelFolder");
			this.labelFolder.ForeColor = System.Drawing.Color.White;
			this.labelFolder.Name = "labelFolder";
			// 
			// labelFolderInput
			// 
			resources.ApplyResources(this.labelFolderInput, "labelFolderInput");
			this.labelFolderInput.AutoEllipsis = true;
			this.labelFolderInput.BackColor = System.Drawing.Color.White;
			this.labelFolderInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelFolderInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.labelFolderInput.Name = "labelFolderInput";
			this.labelFolderInput.Click += new System.EventHandler(this.buttonFolder_Click);
			// 
			// buttonFolder
			// 
			resources.ApplyResources(this.buttonFolder, "buttonFolder");
			this.buttonFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
			this.buttonFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.buttonFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.buttonFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.buttonFolder.ForeColor = System.Drawing.Color.White;
			this.buttonFolder.Name = "buttonFolder";
			this.buttonFolder.UseVisualStyleBackColor = false;
			this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
			// 
			// labelROM
			// 
			resources.ApplyResources(this.labelROM, "labelROM");
			this.labelROM.ForeColor = System.Drawing.Color.White;
			this.labelROM.Name = "labelROM";
			// 
			// labelROMInput
			// 
			resources.ApplyResources(this.labelROMInput, "labelROMInput");
			this.labelROMInput.AutoEllipsis = true;
			this.labelROMInput.BackColor = System.Drawing.Color.White;
			this.labelROMInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelROMInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.labelROMInput.Name = "labelROMInput";
			this.labelROMInput.Click += new System.EventHandler(this.buttonROM_Click);
			// 
			// buttonROM
			// 
			resources.ApplyResources(this.buttonROM, "buttonROM");
			this.buttonROM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
			this.buttonROM.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.buttonROM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.buttonROM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.buttonROM.ForeColor = System.Drawing.Color.White;
			this.buttonROM.Name = "buttonROM";
			this.buttonROM.UseVisualStyleBackColor = false;
			this.buttonROM.Click += new System.EventHandler(this.buttonROM_Click);
			// 
			// buttonCreate
			// 
			resources.ApplyResources(this.buttonCreate, "buttonCreate");
			this.buttonCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
			this.buttonCreate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.buttonCreate.ForeColor = System.Drawing.Color.White;
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.UseVisualStyleBackColor = false;
			this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
			// 
			// progressBar1
			// 
			resources.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
			this.progressBar1.ForeColor = System.Drawing.Color.Black;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Step = 1;
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// progressBar2
			// 
			resources.ApplyResources(this.progressBar2, "progressBar2");
			this.progressBar2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
			this.progressBar2.ForeColor = System.Drawing.Color.Black;
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Step = 1;
			this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// NewProject
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.labelAuthor);
			this.Controls.Add(this.textBoxAuthor);
			this.Controls.Add(this.labelFolder);
			this.Controls.Add(this.labelFolderInput);
			this.Controls.Add(this.buttonFolder);
			this.Controls.Add(this.labelROM);
			this.Controls.Add(this.labelROMInput);
			this.Controls.Add(this.buttonROM);
			this.Controls.Add(this.buttonCreate);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.progressBar2);
			this.Name = "NewProject";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Label labelAuthor;
		private System.Windows.Forms.TextBox textBoxAuthor;
		private System.Windows.Forms.Label labelFolder;
		private System.Windows.Forms.Label labelFolderInput;
		private System.Windows.Forms.Button buttonFolder;
		private System.Windows.Forms.Label labelROM;
		private System.Windows.Forms.Label labelROMInput;
		private System.Windows.Forms.Button buttonROM;
		private System.Windows.Forms.Button buttonCreate;
		private EditorNDS.ProgressBarText progressBar1;
		private EditorNDS.ProgressBarText progressBar2;
	}
}
