using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EditorNDS.FileHandlers;

namespace EditorNDS.ROMExplorer
{
	public class NodeDirectory : NodeFile
	{
		public NodeDirectory(DataGridView grid, INitroDirectory dir)
		{
			Grid = grid;
			InitializeNode(dir);
		}

		public NodeDirectory(TreeView tree, DataGridView grid, INitroDirectory dir)
		{
			Grid = grid;
			InitializeNode(dir);
			DisplayNode(tree);
		}

		public NodeDirectory(TreeNode node, DataGridView grid, INitroDirectory dir)
		{
			Grid = grid;
			InitializeNode(dir);
			DisplayNode(node);
		}

		private void InitializeNode(INitroDirectory dir)
		{
			Name = dir.Name;
			Text = dir.Name;
			Path = dir.Path;

			ImageIndex = 0;
			SelectedImageIndex = 0;

			if (dir.Children.Count > 0)
			{
				foreach (INitroDirectory child in dir.Children)
				{
					new NodeDirectory(this, Grid, child);
				}
			}

			if (dir.Contents.Count > 0)
			{
				foreach (INitroFile file in dir.Contents)
				{
					if (file is NitroArchive)
					{
						new NodeNARC(this, Grid, file as NitroArchive);
					}
					else
					{
						new NodeFile(this, Grid, file);
					}
				}
			}
		}

		public override void DisplayNodeProperties()
		{
			{
				Grid.Rows.Clear();
				string[] row = new string[] { "Name:", Text };
				Grid.Rows.Add(row);
				row = new string[] { "Path:", "Root" };
				Grid.Rows.Add(row);
			}
		}
	}
}