using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EditorNDS.FileHandlers;

namespace EditorNDS.ROMExplorer
{
	/// <summary>
	/// This is the baseclass for nodes in our treeview.
	/// It has a few basic file properties, but most will
	/// be found under the nodes specific to file types.
	/// </summary>
	public class NodeFile : TreeNode
	{
		public INitroFile File;
		public DataGridView Grid;

		public string ID = "";
		public string Path = "";
		public string Offset = "";
		public string Length = "";
		public string FileType = "";
		public string Description = "";

		public int ExpandedIndex = 0;
		public int CollapsedIndex = 0;

		public NodeFile()
		{

		}

		public NodeFile(DataGridView grid, INitroFile file)
		{
			Grid = grid;
			InitializeNode(file);
		}

		public NodeFile(TreeView tree, DataGridView grid, INitroFile file)
	 	{
			Grid = grid;
			InitializeNode(file);
			DisplayNode(tree);
		}

		public NodeFile(TreeNode node, DataGridView grid, INitroFile file)
		{
			Grid = grid;
			InitializeNode(file);
			DisplayNode(node);
		}

		private void InitializeNode(INitroFile file)
		{
			File = file;

			Text = File.Name + File.Extension;

			switch (file.Extension)
			{
				case ".l":
					ImageIndex = 8;
					SelectedImageIndex = 8;
					break;

				case ".spa":
					ImageIndex = 14;
					SelectedImageIndex = 14;
					break;

				case ".bmg":
					ImageIndex = 15;
					SelectedImageIndex = 15;
					break;
				case ".txt":
					ImageIndex = 7;
					SelectedImageIndex = 7;
					break;

				case ".dat":
					ImageIndex = 9;
					SelectedImageIndex = 9;
					break;
				case ".bin":
					ImageIndex = 10;
					SelectedImageIndex = 10;
					break;

				case ".sdat":
					ImageIndex = 13;
					SelectedImageIndex = 13;
					break;
				case ".arc":
					ImageIndex = 3;
					SelectedImageIndex = 3;
					break;

				case ".nce":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nan":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nam":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".ncg":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".ncb":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".ncl":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nmc":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nen":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nsc":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;

				case ".ncer":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nanr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".namr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".ncgr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".ncbr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nclr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nmcr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nenr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nscr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;
				case ".nftr":
					ImageIndex = 11;
					SelectedImageIndex = 11;
					break;

				case ".nsbmd":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbtx":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbca":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbtp":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbma":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbva":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
				case ".nsbta":
					ImageIndex = 12;
					SelectedImageIndex = 12;
					break;
			}
		}

		public void DisplayNode(TreeView tree)
		{
			tree.Nodes.Add(this);
		}

		public void DisplayNode(TreeNode node)
		{
			node.Nodes.Add(this);
		}

		public virtual void AfterExpand()
		{

		}

		public virtual void AfterCollapse()
		{
			
		}

		public virtual void DisplayNodeProperties()
		{
			Grid.Rows.Clear();
			Grid.Columns.Clear();

			Grid.ColumnCount = 3;
			DataGridViewColumn firstColumn = Grid.Columns[0];
			firstColumn.Resizable = DataGridViewTriState.False;
			firstColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			firstColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			DataGridViewColumn secondColumn = Grid.Columns[1];
			secondColumn.MinimumWidth = 275;
			secondColumn.Resizable = DataGridViewTriState.False;
			secondColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			secondColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			secondColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

			Grid.Rows.Add(new string[] { "Property", "Value", "Description" });
			Grid.Rows[0].DefaultCellStyle.Font = new System.Drawing.Font(Grid.Font, System.Drawing.FontStyle.Bold);

			Grid.Rows.Add(new string[] { "File Path:", File.Path, "The full path of this file from root to file." });
			Grid.Rows.Add(new string[] { "File Name:", File.Name + File.Extension, "A file name made up by this program." });
			Grid.Rows.Add(new string[] { "File Table Name:", File.OriginalName, "File name according to the File Name Table." });
			Grid.Rows.Add(new string[] { "File Type:", File.FileType, File.FileDescription });
			Grid.Rows.Add(new string[] { "File ID:", "0x" + File.ID.ToString("N0"), "File ID according to the File Allocation Table." });
			Grid.Rows.Add(new string[] { "File Offset:", "0x" + File.Offset.ToString("X"), "Offset from the beginning of the ROM to the beginning of this file." });
			Grid.Rows.Add(new string[] { "File Length:", File.Length.ToString("N0") + " Bytes", "Total length of this file." });

			foreach (string text in File.Warnings)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "Read Warning:", text, "This is a warning that doesn't prevent further reading." });
				Grid.Rows.Add(row);
			}
			foreach (string text in File.Errors)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "Read Error:", text, "This is a fatal read error. File read stops here." });
				Grid.Rows.Add(row);
			}
		}
	}
}

