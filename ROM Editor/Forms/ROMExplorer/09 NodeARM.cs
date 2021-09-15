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
	public class NodeARM : NodeFile
	{
		public new NDSBinary File;

		public bool isARM9 = false;
		public bool isDSi = false;

		public string Load = "";
		public string AutoLoad = "";
		public string AutoParams = "";

		public NodeARM(TreeView tree, DataGridView grid, NDSBinary arm)
		{
			Grid = grid;
			InitializeNode(arm);
			DisplayNode(tree);
		}

		public NodeARM(TreeNode node, DataGridView grid, NDSBinary arm)
		{
			Grid = grid;
			InitializeNode(arm);
			DisplayNode(node);
		}
		 
		private void InitializeNode(NDSBinary arm)
		{
			File = arm;
			Path = arm.Path;
			Name = arm.Name;
			Text = arm.Name + arm.Extension;
			Offset = File.Offset.ToString("X");
			Length = File.Length.ToString("N0") + " bytes";
			Load = File.Load.ToString("X");

			FileType = "ARM";
			Description = "ARMv";
			if (Name.Contains("9"))
			{
				isARM9 = true;
				FileType += "9";
				Description += "5TE Compiled Binary";
			}
			else
			{
				FileType += "7";
				Description += "4T Compiled Binary";
			}
			if (Name.Contains("i"))
			{
				isDSi = true;
				FileType += "i Binary";
				Description += ", DSi Extended";
			}
			else
			{
				FileType += " Binary";
				AutoLoad = File.AutoLoad.ToString("X");
				AutoParams = File.AutoParams.ToString("X");
			}

			ImageIndex = 10;
			SelectedImageIndex = 10;
		}

		public override void DisplayNodeProperties()
		{
			Grid.Rows.Clear();
			string[] row = new string[] { "Name:", Text };
			Grid.Rows.Add(row);
			row = new string[] { "Path:", "Root" };
			Grid.Rows.Add(row);

			row = new string[] { "File Type:", FileType, Description };
			Grid.Rows.Add(row);

			row = new string[] { "Address:", Offset, "Offset from beginning of ROM to start of the binary." };
			Grid.Rows.Add(row);
			row = new string[] { "Length:", Length, "The length of the binary." };
			Grid.Rows.Add(row);
			row = new string[] { "RAM Address:", Load, "The RAM address this binary is loaded to." };

			if (!isDSi)
			{
				Grid.Rows.Add(row);
				row = new string[] { "Hook Address:", AutoLoad, "The RAM auto-load hook address." };
				Grid.Rows.Add(row);
				row = new string[] { "Parameters:", AutoParams, "Offset to auto-load parameter table." };
			}
			Grid.Rows.Add(row);

			Grid.Refresh();
		}
	}
}