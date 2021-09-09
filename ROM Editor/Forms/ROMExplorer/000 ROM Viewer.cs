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
	public partial class ROMViewer : UserControl
	{
		public ROMViewer(NDSROM rom)
		{
			ROM = rom;

			InitializeComponent();
			InitializeTree();
		}

		NDSROM ROM;

		private void InitializeTree()
		{
			if (ROM == null)
			{
				return;
			}

			romTree.BeginUpdate();

			TreeNode header = new NodeFile(romTree, propertyView, ROM.Header);
			NodeBanner banner = new NodeBanner(romTree, propertyView, ROM.Banner);

			new NodeARM(romTree, propertyView, ROM.ARM9);

			if (ROM.Header.ARM9iLength > 0)
			{
				new NodeARM(romTree, propertyView, ROM.ARM9i);
			}
			if (ROM.Header.ARM7Length > 0)
			{
				new NodeARM(romTree, propertyView, ROM.ARM7);
			}
			if (ROM.Header.ARM7iLength > 0)
			{
				new NodeARM(romTree, propertyView, ROM.ARM7i);
			}

			if (ROM.OverlayTable9.Count() > 0)
			{
				TreeNode overlay9_tree = new TreeNode("ARM9 Overlays");
				overlay9_tree.Tag = "dir";
				romTree.Nodes.Add(overlay9_tree);
				foreach (NDSOverlay overlay in ROM.OverlayTable9)
				{
					new NodeOverlay(overlay9_tree, propertyView, overlay);
				}
			}

			if (ROM.OverlayTable7.Count() > 0)
			{
				TreeNode overlay7_tree = new TreeNode("ARM7 Overlays");
				romTree.Nodes.Add(overlay7_tree);
				foreach (NDSOverlay overlay in ROM.OverlayTable7)
				{
					new NodeOverlay(overlay7_tree, propertyView, overlay);
				}
			}

			NodeDirectory node = new NodeDirectory(romTree, propertyView, ROM.directory_table[0]);

			romTree.EndUpdate();
		}

		private void romTree_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if (e.Node is NodeFile)
			{
				NodeFile node = e.Node as NodeFile;
				node.AfterCollapse();
			}
		}

		private void romTree_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			if (e.Node is NodeFile)
			{
				NodeFile node = e.Node as NodeFile;
				node.AfterCollapse();
			}
		}

		private void romTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is NodeFile)
			{
				NodeFile node = e.Node as NodeFile;
				node.DisplayNodeProperties();
			}
		}
	}
}