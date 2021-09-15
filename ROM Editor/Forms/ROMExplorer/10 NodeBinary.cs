using EditorNDS.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.ROMExplorer
{
	/// <summary>
	/// This is the node for displaying information
	/// about ARM binary overlays.
	/// </summary>
	public class NodeOverlay : NodeFile
	{
		public new NDSOverlay File;
		public bool isARM9 = false;
		public bool isCompressed = false;

		public string OverlayID = "";
		public string SizeRAM = "";
		public string SizeBSS = "";
		public string CompressedSize = "";
		public string TotalSize = "";
		public string AddressRAM = "";
		public string StaticStartAddress = "";
		public string StaticEndAddress = "";

		public NodeOverlay(TreeView tree, DataGridView grid, NDSOverlay overlay)
		{
			Grid = grid;
			InitializeNode(overlay);
			DisplayNode(tree);
		}

		public NodeOverlay(TreeNode node, DataGridView grid, NDSOverlay overlay)
		{
			Grid = grid;
			InitializeNode(overlay);
			DisplayNode(node);
		}
		private void InitializeNode(NDSOverlay overlay)
		{
			File = overlay;
			Path = overlay.Path;
			Name = overlay.Name;
			Text = overlay.Name + overlay.Extension;

			ID = overlay.ID.ToString();
			Offset = File.Offset.ToString("X");
			Length = File.Length.ToString("N0") + " bytes";

			isCompressed = overlay.Compressed;

			OverlayID = overlay.OverlayID.ToString();
			SizeRAM = overlay.SizeRAM.ToString("N0") + " bytes";
			SizeBSS = overlay.SizeBSS.ToString("N0") + " bytes";
			CompressedSize = overlay.CompressedSize.ToString("N0") + " bytes";
			TotalSize = (overlay.SizeRAM + overlay.SizeBSS).ToString("N0") + " bytes";
			AddressRAM = overlay.AddressRAM.ToString("X");
			StaticStartAddress = overlay.StaticStartAddress.ToString("X");
			StaticEndAddress = overlay.StaticEndAddress.ToString("X");

			FileType = "ARM";
			Description = "ARMv";
			if (Path.Contains("9"))
			{
				isARM9 = true;
				FileType += "9";
				Description += "5TE Compiled Binary Overlay";
			}
			else
			{
				FileType += "7";
				Description += "4T Compiled Binary Overlay";
			}

			ImageIndex = 10;
			SelectedImageIndex = 10;
		}

		public override void DisplayNodeProperties()
		{
			Grid.Rows.Clear();
			string[] row = new string[] { "Name:", Text };
			Grid.Rows.Add(row);
			row = new string[] { "Path:", Path };
			Grid.Rows.Add(row);

			row = new string[] { "File Type:", FileType, Description };
			Grid.Rows.Add(row);

			row = new string[] { "File ID:", ID, "The ID of the overlay in the file allocation table." };
			Grid.Rows.Add(row);
			row = new string[] { "Overlay ID:", OverlayID, "The ID of the overlay in the overlay table." };
			Grid.Rows.Add(row);
			row = new string[] { "Offset:", "0x" + Offset, "Offset from beginning of ROM to start of the overlay." };
			Grid.Rows.Add(row);
			row = new string[] { "Size:", Length + " bytes", "The length of the overlay." };
			Grid.Rows.Add(row);
			row = new string[] { "RAM Size:", SizeRAM + " bytes", "The size of the RAM section of the overlay." };
			Grid.Rows.Add(row);
			row = new string[] { "BSS Size:", SizeBSS + " bytes", "The size of the BSS section of the overlay." };
			Grid.Rows.Add(row);
			if (isCompressed)
			{
				row = new string[] { "Compressed:", "True", "Flag that indicates a compressed overlay." };
				Grid.Rows.Add(row);
				row = new string[] { "Compressed Size:", CompressedSize + " bytes", "The compressed size of the overlay." };
				Grid.Rows.Add(row);
				row = new string[] { "Total Size:", TotalSize + " bytes", "The total size of the uncompressed overlay." };
				Grid.Rows.Add(row);
			}
			else
			{
				row = new string[] { "Compressed:", "False", "Flag that indicates a compressed overlay." };
				Grid.Rows.Add(row);
			}

			row = new string[] { "RAM Address:", "0x" + AddressRAM, "The overlay transfer desitation address in RAM." };
			Grid.Rows.Add(row);
			row = new string[] { "Start Address:", "0x" + StaticStartAddress, "The static initializer start address of the overlay." };
			Grid.Rows.Add(row);
			row = new string[] { "End Address:", "0x" + StaticEndAddress, "The static initializer end address of the overlay." };
			Grid.Rows.Add(row);

			Grid.Refresh();
		}
	}
}