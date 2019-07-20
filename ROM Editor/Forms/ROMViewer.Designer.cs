namespace EditorNDS.FileHandlers
{
	partial class ROMViewer
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ROMViewer));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitROM = new System.Windows.Forms.SplitContainer();
			this.romTree = new System.Windows.Forms.TreeView();
			((System.ComponentModel.ISupportInitialize)(this.splitROM)).BeginInit();
			this.splitROM.Panel1.SuspendLayout();
			this.splitROM.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "FileDirectoryBlack.png");
			this.imageList.Images.SetKeyName(1, "FileSubmoduleBlack.png");
			this.imageList.Images.SetKeyName(2, "FileSymlinkDirectoryBlack.png");
			this.imageList.Images.SetKeyName(3, "RepoBlack.png");
			this.imageList.Images.SetKeyName(4, "RepoPullBlack.png");
			this.imageList.Images.SetKeyName(5, "ListOrderedBlack.png");
			this.imageList.Images.SetKeyName(6, "ListUnorderedBlack.png");
			this.imageList.Images.SetKeyName(7, "FileBlack.png");
			this.imageList.Images.SetKeyName(8, "FileZipBlack.png");
			this.imageList.Images.SetKeyName(9, "FileBinaryBlack.png");
			this.imageList.Images.SetKeyName(10, "FileCodeBlack.png");
			this.imageList.Images.SetKeyName(11, "FileMediaBlack.png");
			this.imageList.Images.SetKeyName(12, "PersonBlack.png");
			this.imageList.Images.SetKeyName(13, "UnmuteBlack.png");
			this.imageList.Images.SetKeyName(14, "ZapBlack.png");
			this.imageList.Images.SetKeyName(15, "NoteBlack.png");
			// 
			// splitROM
			// 
			this.splitROM.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitROM.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitROM.IsSplitterFixed = true;
			this.splitROM.Location = new System.Drawing.Point(0, 0);
			this.splitROM.Name = "splitROM";
			// 
			// splitROM.Panel1
			// 
			this.splitROM.Panel1.Controls.Add(this.romTree);
			this.splitROM.Panel1MinSize = 250;
			this.splitROM.Size = new System.Drawing.Size(640, 480);
			this.splitROM.SplitterDistance = 250;
			this.splitROM.SplitterWidth = 5;
			this.splitROM.TabIndex = 1;
			// 
			// romTree
			// 
			this.romTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.romTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.romTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.romTree.FullRowSelect = true;
			this.romTree.ImageIndex = 0;
			this.romTree.ImageList = this.imageList;
			this.romTree.Indent = 10;
			this.romTree.ItemHeight = 18;
			this.romTree.Location = new System.Drawing.Point(0, 0);
			this.romTree.Margin = new System.Windows.Forms.Padding(0);
			this.romTree.Name = "romTree";
			this.romTree.SelectedImageIndex = 0;
			this.romTree.Size = new System.Drawing.Size(250, 480);
			this.romTree.TabIndex = 1;
			// 
			// ROMViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
			this.Controls.Add(this.splitROM);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ROMViewer";
			this.Size = new System.Drawing.Size(640, 480);
			this.splitROM.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitROM)).EndInit();
			this.splitROM.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.SplitContainer splitROM;
		private System.Windows.Forms.TreeView romTree;
	}
}
