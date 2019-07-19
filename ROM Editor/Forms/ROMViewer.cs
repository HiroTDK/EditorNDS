using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.FileHandlers
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

			TreeNode header = new TreeNode();
			header.Name = "ROM Header";
			header.Text = "ROM Header";
			header.ImageIndex = 6;
			header.SelectedImageIndex = 6;
			romTree.Nodes.Add(header);

			TreeNode arm9_node = new TreeNode();
			arm9_node.Text = ROM.ARM9.Name + ROM.ARM9.Extension;
			arm9_node.Name = ROM.ARM9.Name;
			arm9_node.Tag = ROM.ARM9;
			arm9_node.ImageIndex = 10;
			arm9_node.SelectedImageIndex = 10;
			romTree.Nodes.Add(arm9_node);

			if (ROM.ARM7Length > 0)
			{
				TreeNode arm7_node = new TreeNode();
				arm7_node.Text = ROM.ARM7.Name + ROM.ARM7.Extension;
				arm7_node.Name = ROM.ARM7.Name;
				arm7_node.Tag = ROM.ARM7;
				arm7_node.ImageIndex = 10;
				arm7_node.SelectedImageIndex = 10;
				romTree.Nodes.Add(arm7_node);
			}

			if ( ROM.OverlayTable9.Count() > 0 )
			{
				TreeNode overlay9_tree = new TreeNode("ARM9 Overlay Table");
				romTree.Nodes.Add(overlay9_tree);
				foreach (NDSOverlay overlay in ROM.OverlayTable9)
				{
					TreeNode overlay9_node = new TreeNode();
					overlay9_node.Text = overlay.File.Name + overlay.File.Extension;
					overlay9_node.Name = overlay.File.Name;
					overlay9_node.Tag = overlay;
					overlay9_node.ImageIndex = 10;
					overlay9_node.SelectedImageIndex = 10;

					overlay9_tree.Nodes.Add(overlay9_node);
				}
			}

			if (ROM.OverlayTable7.Count() > 0)
			{
				TreeNode overlay7_tree = new TreeNode("ARM7 Overlay Table");
				romTree.Nodes.Add(overlay7_tree);
				foreach (NDSOverlay overlay in ROM.OverlayTable7)
				{
					TreeNode overlay7_node = new TreeNode();
					overlay7_node.Text = overlay.File.Name + overlay.File.Extension;
					overlay7_node.Name = overlay.File.Name;
					overlay7_node.Tag = overlay;
					overlay7_node.ImageIndex = 10;
					overlay7_node.SelectedImageIndex = 10;

					overlay7_tree.Nodes.Add(overlay7_node);
				}
			}

			romTree.Nodes.Add(InitializeNode(ROM.DirectoryTable[0]));

			romTree.EndUpdate();
		}

		private TreeNode InitializeNode(NDSDirectory dir)
		{
			TreeNode node = new TreeNode();
			node.Name = dir.Name;
			node.Text = dir.Name;
			node.Tag = dir;

			if (dir.Children.Count > 0)
			{
				foreach (NDSDirectory child in dir.Children)
				{
					node.Nodes.Add(InitializeNode(child));
					romTree.Update();
				}
			}

			if (dir.Contents.Count > 0)
			{
				foreach (NDSFile file in dir.Contents)
				{
					node.Nodes.Add(InitializeNode(file));
					romTree.Update();
				}
			}

			return node;
		}

		private TreeNode InitializeNode(NDSFile file)
		{
			TreeNode node = new TreeNode();
			
			if (file.NARCTables.IsValid)
			{
				node = new TreeNode();
				node.Name = file.Name;
				node.Text = file.Name + file.Extension;
				node.Tag = file;
				node.ImageIndex = 3;
				node.SelectedImageIndex = 3;

				NARC narc = file.NARCTables;

				foreach (NDSDirectory child in narc.DirectoryTable[0].Children)
				{
					node.Nodes.Add(InitializeNode(child));
					romTree.Update();
				}
				foreach (NDSFile sub_file in narc.DirectoryTable[0].Contents)
				{
					node.Nodes.Add(InitializeNode(sub_file));
					romTree.Update();
				}
			}
			else
			{
				node = new FileNode(file);
			}
			

			return node;
		}

		public class FileNode : TreeNode
		{
			NDSFile NodeFile;

			public FileNode(NDSFile node_file)
			{
				NodeFile = node_file;

				Name = NodeFile.Name;
				Text = NodeFile.Name + NodeFile.Extension;
				Tag = NodeFile.ID;
				ImageIndex = 9;
				SelectedImageIndex = 9;

				switch (NodeFile.Extension)
				{
					case ".txt":
						ImageIndex = 7;
						SelectedImageIndex = 7;
						break;
					case ".dat":
						ImageIndex = 9;
						SelectedImageIndex = 9;
						break;
					case ".bin":
						ImageIndex = 9;
						SelectedImageIndex = 9;
						break;
					case ".sdat":
						ImageIndex = 13;
						SelectedImageIndex = 13;
						break;
					case ".narc":
						ImageIndex = 3;
						SelectedImageIndex = 3;
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
					case ".mesg":
						ImageIndex = 15;
						SelectedImageIndex = 15;
						break;
					case ".spa":
						ImageIndex = 14;
						SelectedImageIndex = 14;
						break;
				}
			}
		}

		private void romTree_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is NDSFile)
			{
				NDSFile file = e.Node.Tag as NDSFile;
				if (file.Extension == ".narc")
				{
					e.Node.ImageIndex = 4;
					e.Node.SelectedImageIndex = 4;
				}
			}
			if (e.Node.Tag is NDSDirectory)
			{
				e.Node.ImageIndex = 1;
				e.Node.SelectedImageIndex = 1;
			}
		}

		private void romTree_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is NDSFile)
			{
				NDSFile file = e.Node.Tag as NDSFile;
				if (file.Extension == ".narc")
				{
					e.Node.ImageIndex = 3;
					e.Node.SelectedImageIndex = 3;
				}
			}
			if (e.Node.Tag is NDSDirectory)
			{
				e.Node.ImageIndex = 0;
				e.Node.SelectedImageIndex = 0;
			}
		}
	}
}
