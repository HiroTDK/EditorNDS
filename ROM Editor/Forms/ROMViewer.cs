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

			romTree.Nodes.Add(InitializeNode(ROM.DirectoryTable[0]));

			romTree.EndUpdate();
		}

		private TreeNode InitializeNode(NDSDirectory dir)
		{
			TreeNode node = new TreeNode();
			node.Name = dir.Name;
			node.Text = dir.Name;
			node.Tag = dir.ID;

			if (dir.Children.Count > 0)
			{
				foreach (NDSDirectory child in dir.Children)
				{
					node.Nodes.Add(InitializeNode(child));
				}
			}

			if (dir.Contents.Count > 0)
			{
				foreach (NDSFile file in dir.Contents)
				{
					node.Nodes.Add(InitializeNode(file));
				}
			}

			return node;
		}

		private TreeNode InitializeNode(NDSFile file)
		{
			TreeNode node = new TreeNode();

			if (file.Extension == ".narc")
			{

				node = new TreeNode();
				node.Name = file.Name;
				node.Text = file.Name + file.Extension;
				//node.Tag = file.ID;

				NARC narc = file.NARCTables;
				foreach (NDSDirectory child in narc.DirectoryTable[0].Children)
				{
					node.Nodes.Add(InitializeNode(child));
				}
				foreach (NDSFile sub_file in narc.DirectoryTable[0].Contents)
				{
					node.Nodes.Add(InitializeNode(sub_file));
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

				switch (NodeFile.Extension)
				{
					case ".narc":
						ImageIndex = 3;
						SelectedImageIndex = 3;
						break;
					case ".txt":
						ImageIndex = 4;
						SelectedImageIndex = 4;
						break;
				}
			}
		}

		private void romTree_AfterExpand(object sender, TreeViewEventArgs e)
		{
			e.Node.ImageIndex = 1;
			e.Node.SelectedImageIndex = 1;
		}

		private void romTree_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			e.Node.ImageIndex = 0;
			e.Node.SelectedImageIndex = 0;
		}
	}
}
