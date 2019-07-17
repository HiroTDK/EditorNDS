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
			this.romTree = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// romTree
			// 
			this.romTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.romTree.Dock = System.Windows.Forms.DockStyle.Left;
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
			this.romTree.Size = new System.Drawing.Size(200, 400);
			this.romTree.TabIndex = 0;
			this.romTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.romTree_AfterExpand);
			this.romTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.romTree_AfterCollapse);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "FileDirectoryBlack.png");
			this.imageList.Images.SetKeyName(1, "FileSubmoduleBlack.png");
			this.imageList.Images.SetKeyName(2, "FileSymlinkDirectoryBlack.png");
			this.imageList.Images.SetKeyName(3, "ArchiveBlack.png");
			this.imageList.Images.SetKeyName(4, "FileBlack.png");
			this.imageList.Images.SetKeyName(5, "FileBinaryBlack.png");
			this.imageList.Images.SetKeyName(6, "FileCodeBlack.png");
			this.imageList.Images.SetKeyName(7, "FileMediaBlack.png");
			this.imageList.Images.SetKeyName(8, "FilePdfBlack.png");
			this.imageList.Images.SetKeyName(9, "FileSymlinkFileBlack.png");
			this.imageList.Images.SetKeyName(10, "FileZipBlack.png");
			// 
			// ROMViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
			this.Controls.Add(this.romTree);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ROMViewer";
			this.Size = new System.Drawing.Size(400, 400);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView romTree;
		private System.Windows.Forms.ImageList imageList;
	}
}
