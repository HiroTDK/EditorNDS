using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EditorNDS.FileHandlers;

namespace EditorNDS.ROMExplorer
{

	/// <summary>
	/// This is the node for displaying ARM binary information.
	/// </summary>
	public class NodeBanner : NodeFile
	{
		new NDSBanner File;

		public NodeBanner(DataGridView grid, NDSBanner banner)
		{
			Grid = grid;
			InitializeNode(banner);
		}

		public NodeBanner(TreeView tree, DataGridView grid, NDSBanner banner)
		{
			Grid = grid;
			InitializeNode(banner);
			DisplayNode(tree);
		}

		public NodeBanner(TreeNode node, DataGridView grid, NDSBanner banner)
		{
			Grid = grid;
			InitializeNode(banner);
			DisplayNode(node);
		}

		private void InitializeNode(NDSBanner banner)
		{
			File = banner;
			Text = "Banner";

			ImageIndex = 12;
			SelectedImageIndex = 12;
		}

		public override void DisplayNodeProperties()
		{
			
		}
	}
}