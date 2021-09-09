namespace EditorNDS.ROMExplorer
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitROM = new System.Windows.Forms.SplitContainer();
			this.romTree = new System.Windows.Forms.TreeView();
			this.propertyView = new System.Windows.Forms.DataGridView();
			this.Field = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.About = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.splitROM)).BeginInit();
			this.splitROM.Panel1.SuspendLayout();
			this.splitROM.Panel2.SuspendLayout();
			this.splitROM.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.propertyView)).BeginInit();
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
			// 
			// splitROM.Panel2
			// 
			this.splitROM.Panel2.Controls.Add(this.propertyView);
			this.splitROM.Size = new System.Drawing.Size(640, 480);
			this.splitROM.SplitterDistance = 250;
			this.splitROM.SplitterWidth = 1;
			this.splitROM.TabIndex = 1;
			// 
			// romTree
			// 
			this.romTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.romTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.romTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.romTree.FullRowSelect = true;
			this.romTree.HideSelection = false;
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
			this.romTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.romTree_AfterCollapse);
			this.romTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.romTree_AfterExpand);
			this.romTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.romTree_AfterSelect);
			// 
			// propertyView
			// 
			this.propertyView.AllowUserToAddRows = false;
			this.propertyView.AllowUserToDeleteRows = false;
			this.propertyView.AllowUserToResizeColumns = false;
			this.propertyView.AllowUserToResizeRows = false;
			this.propertyView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.propertyView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.propertyView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			this.propertyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.propertyView.ColumnHeadersVisible = false;
			this.propertyView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Field,
            this.Value,
            this.About});
			this.propertyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.propertyView.Location = new System.Drawing.Point(0, 0);
			this.propertyView.Margin = new System.Windows.Forms.Padding(0);
			this.propertyView.MultiSelect = false;
			this.propertyView.Name = "propertyView";
			this.propertyView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.propertyView.RowHeadersVisible = false;
			this.propertyView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.propertyView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.propertyView.ShowEditingIcon = false;
			this.propertyView.Size = new System.Drawing.Size(389, 480);
			this.propertyView.TabIndex = 0;
			// 
			// Field
			// 
			this.Field.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.Field.DefaultCellStyle = dataGridViewCellStyle1;
			this.Field.HeaderText = "Field";
			this.Field.Name = "Field";
			this.Field.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Value
			// 
			this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.Value.DefaultCellStyle = dataGridViewCellStyle1;
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Value.Width = 200;
			// 
			// About
			// 
			this.About.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.About.DefaultCellStyle = dataGridViewCellStyle2;
			this.About.HeaderText = "About";
			this.About.Name = "About";
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
			this.splitROM.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitROM)).EndInit();
			this.splitROM.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.propertyView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.SplitContainer splitROM;
		private System.Windows.Forms.TreeView romTree;
		private System.Windows.Forms.DataGridView propertyView;
		private System.Windows.Forms.DataGridViewTextBoxColumn Field;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.DataGridViewTextBoxColumn About;
	}
}
