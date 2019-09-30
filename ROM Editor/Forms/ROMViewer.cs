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

			TreeNode header = new FileNode(ROM.Header);
			romTree.Nodes.Add(header);

			TreeNode arm9_node = new TreeNode();
			arm9_node.Text = ROM.ARM9.Name + ROM.ARM9.Extension;
			arm9_node.Name = ROM.ARM9.Name;
			arm9_node.Tag = ROM.ARM9;
			arm9_node.ImageIndex = 10;
			arm9_node.SelectedImageIndex = 10;
			romTree.Nodes.Add(arm9_node);
			
			if (ROM.ARM9iLength > 0)
			{
				TreeNode arm9i_node = new TreeNode();
				arm9i_node.Text = ROM.ARM9i.Name + ROM.ARM9i.Extension;
				arm9i_node.Name = ROM.ARM9i.Name;
				arm9i_node.Tag = ROM.ARM9i;
				arm9i_node.ImageIndex = 10;
				arm9i_node.SelectedImageIndex = 10;
				romTree.Nodes.Add(arm9i_node);
			}

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

			if (ROM.ARM7iLength > 0)
			{
				TreeNode arm7i_node = new TreeNode();
				arm7i_node.Text = ROM.ARM7i.Name + ROM.ARM7i.Extension;
				arm7i_node.Name = ROM.ARM7i.Name;
				arm7i_node.Tag = ROM.ARM7i;
				arm7i_node.ImageIndex = 10;
				arm7i_node.SelectedImageIndex = 10;
				romTree.Nodes.Add(arm7i_node);
			}

			if ( ROM.OverlayTable9.Count() > 0 )
			{
				TreeNode overlay9_tree = new TreeNode("ARM9 Overlay Table");
				overlay9_tree.Tag = "dir";
				romTree.Nodes.Add(overlay9_tree);
				foreach (NDSOverlay overlay in ROM.OverlayTable9)
				{
					TreeNode overlay9_node = new TreeNode();
					overlay9_node.Text = overlay.Name + overlay.Extension;
					overlay9_node.Name = overlay.Name;
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
					overlay7_node.Text = overlay.Name + overlay.Extension;
					overlay7_node.Name = overlay.Name;
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
				Tag = NodeFile;
				ImageIndex = 9;
				SelectedImageIndex = 9;

				switch (NodeFile.Extension)
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
					case ".narc":
						ImageIndex = 3;
						SelectedImageIndex = 3;
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
			if (e.Node.Tag is NDSDirectory || e.Node.Tag is "dir")
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
			if (e.Node.Tag is NDSDirectory || e.Node.Tag is "dir")
			{
				e.Node.ImageIndex = 0;
				e.Node.SelectedImageIndex = 0;
			}
		}

		private void romTree_AfterSelect(object sender, TreeViewEventArgs e)
		{

			if (e.Node.Tag is NDSFile)
			{
				if (e.Node.Tag is NDSBinary)
				{
					DataGridView grid = propertyView;
					NDSBinary file = e.Node.Tag as NDSBinary;

					grid.Rows.Clear();
					string[] row = new string[] { "Name:", file.Name + file.Extension };
					grid.Rows.Add(row);
					row = new string[] { "Path:", "Root" };
					grid.Rows.Add(row);

					if ( file.Name.Contains("9") )
					{
						if (!file.Name.Contains("i"))
						{
							row = new string[] { "File Type:", "ARM9i Binary", "ARMv5TE Compiled Binary, DSi Extended" };
						}
						else
						{
							row = new string[] { "File Type:", "ARM7i Binary", "ARMv5TE Compiled Binary." };
						}
					}
					else if ( file.Name.Contains("7") )
					{
						if (!file.Name.Contains("i"))
						{
							row = new string[] { "File Type:", "ARM9 Binary", "ARMv4T Compiled Binary, DSi Extended" };
						}
						else
						{
							row = new string[] { "File Type:", "ARM7 Binary", "ARMv4T Compiled Binary." };
						}
					}
					else
					{
						row = new string[] { "File Type:", "ARM Binary", "ARM Compiled Binary" };
					}
					grid.Rows.Add(row);

					row = new string[] { "Address:", file.Offset.ToString(), "Offset from beginning of ROM to start of the binary." };
					grid.Rows.Add(row);
					row = new string[] { "Length:", file.Length.ToString(), "The length of the binary." };
					grid.Rows.Add(row);
					row = new string[] { "RAM Address:", file.Load.ToString(), "The RAM address this binary is loaded to." };

					if ( !file.Name.Contains("i") )
					{
						grid.Rows.Add(row);
						row = new string[] { "Hook Address:", file.AutoLoad.ToString(), "The RAM auto-load hook address." };
						grid.Rows.Add(row);
						row = new string[] { "Parameters:", file.AutoParams.ToString(), "Offset to auto-load parameter table." };
					}
					grid.Rows.Add(row);

					grid.Refresh();
				}
				else if (e.Node.Tag is NDSOverlay)
				{
					DataGridView grid = propertyView;
					NDSOverlay file = e.Node.Tag as NDSOverlay;

					grid.Rows.Clear();
					string[] row = new string[] { "Name:", file.Name + file.Extension };
					grid.Rows.Add(row);
					row = new string[] { "Path:", file.Path };
					grid.Rows.Add(row);

					if (file.Path.Contains("9"))
					{
						row = new string[] { "File Type:", "ARM9 Binary Overlay", "ARMv5TE Compiled Binary Overlay" };
					}
					else if (file.Path.Contains("7"))
					{
						row = new string[] { "File Type:", "ARM7 Binary Overlay", "ARMv4T Compiled Binary Overlay" };
					}
					else
					{
						row = new string[] { "File Type:", "ARM Binary Overlay", "ARM Compiled Binary Overlay" };
					}
					grid.Rows.Add(row);

					row = new string[] { "File ID:", file.ID.ToString(), "The ID of the overlay in the file allocation table." };
					grid.Rows.Add(row);
					row = new string[] { "Overlay ID:", file.OverlayID.ToString(), "The ID of the overlay in the overlay table." };
					grid.Rows.Add(row);
					row = new string[] { "Offset:", "0x" + file.Offset.ToString("X"), "Offset from beginning of ROM to start of the overlay." };
					grid.Rows.Add(row);
					row = new string[] { "Size:", file.Length.ToString("N0") + " bytes", "The length of the overlay." };
					grid.Rows.Add(row);
					row = new string[] { "RAM Size:", file.SizeRAM.ToString("N0") + " bytes", "The size of the RAM section of the overlay." };
					grid.Rows.Add(row);
					row = new string[] { "BSS Size:", file.SizeBSS.ToString("N0") + " bytes", "The size of the BSS section of the overlay." };
					grid.Rows.Add(row);
					row = new string[] { "Compressed:", file.Compressed.ToString(), "Flag that indicates a compressed overlay." };
					grid.Rows.Add(row);
					if ( file.Compressed )
					{
						row = new string[] { "Compressed Size:", file.CompressedSize.ToString("N0") + " bytes", "The compressed size of the overlay." };
						grid.Rows.Add(row);
						row = new string[] { "Total Size:", (file.SizeRAM + file.SizeBSS).ToString("N0") + " bytes", "The total size of the uncompressed overlay." };
						grid.Rows.Add(row);
					}

					row = new string[] { "RAM Address:", "0x" + file.AddressRAM.ToString("X"), "The overlay transfer desitation address in RAM." };
					grid.Rows.Add(row);
					row = new string[] { "Start Address:", "0x" + file.StaticStartAddress.ToString("X"), "The static initializer start address of the overlay." };
					grid.Rows.Add(row);
					row = new string[] { "End Address:", "0x" + file.StaticEndAddress.ToString("X"), "The static initializer end address of the overlay." };
					grid.Rows.Add(row);

					grid.Refresh();
				}
				else
				{
					DataGridView grid = propertyView;
					NDSFile file = e.Node.Tag as NDSFile;

					grid.Rows.Clear();
					string[] row = new string[] { "Name:", file.Name + file.Extension };
					grid.Rows.Add(row);
					row = new string[] { "Path:", file.Path };
					grid.Rows.Add(row);
					row = new string[] { "File Type:", file.Type, file.About };
					grid.Rows.Add(row);
					row = new string[] { "Offset:", file.Offset.ToString(), "The length of the file." };
					grid.Rows.Add(row);
					row = new string[] { "Size:", file.Length.ToString(), "Offset from beginning of ROM to start of the overlay." };
					grid.Rows.Add(row);



					grid.Refresh();
				}
			}
		}
	}
}
