using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EditorNDS.FileHandlers;

namespace EditorNDS.ROMExplorer
{
	public class NodeNARC : NodeFile
	{
		public NodeNARC(DataGridView grid, NitroArchive narc)
		{
			Grid = grid;
			InitializeNode(narc);
		}

		public NodeNARC(TreeView tree, DataGridView grid, NitroArchive narc)
		{
			Grid = grid;
			InitializeNode(narc);
			DisplayNode(tree);
		}

		public NodeNARC(TreeNode node, DataGridView grid, NitroArchive narc)
		{
			Grid = grid;
			InitializeNode(narc);
			DisplayNode(node);
		}

		private void InitializeNode(NitroArchive narc)
		{
			File = narc;
			Name = narc.Name;
			Text = narc.Name + narc.Extension;
			Path = narc.Path;

			ImageIndex = 3;
			SelectedImageIndex = 3;

			if (narc.Children.Count > 0)
			{
				foreach (INitroDirectory child in narc.Children)
				{
					new NodeDirectory(this, Grid, child);
				}
			}

			if (narc.Contents.Count > 0)
			{
				foreach (INitroFile file in narc.Contents)
				{
					if (file is NitroArchive)
					{
						new NodeNARC(this, Grid, file as NitroArchive);
					}

					new NodeFile(this, Grid, file);
				}
			}
		}

		public override void AfterExpand()
		{
			ImageIndex = 4;
			SelectedImageIndex = 4;
		}

		public override void AfterCollapse()
		{
			ImageIndex = 3;
			SelectedImageIndex = 3;
		}
	}
}