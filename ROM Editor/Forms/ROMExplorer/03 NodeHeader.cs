using EditorNDS.FileHandlers;
using System;
using System.Windows.Forms;

namespace EditorNDS.ROMExplorer
{
    class NodeHeader : NodeFile
	{
		new NitroHeader File;
		public NodeHeader(DataGridView grid, NitroHeader file)
		{
			Grid = grid;
			InitializeNode(file);
		}

		public NodeHeader(TreeView tree, DataGridView grid, NitroHeader file)
		{
			Grid = grid;
			InitializeNode(file);
			DisplayNode(tree);
		}

		public NodeHeader(TreeNode node, DataGridView grid, NitroHeader file)
		{
			Grid = grid;
			InitializeNode(file);
			DisplayNode(node);
		}

		private void InitializeNode(NitroHeader file)
		{
			File = file;
			Text = "Header Information";
		}
		public override void DisplayNodeProperties()
		{
			Grid.Rows.Clear();
			Grid.Columns.Clear();

			Grid.ColumnCount = 5;
			DataGridViewColumn firstColumn = Grid.Columns[0];
			firstColumn.Resizable = DataGridViewTriState.False;
			firstColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			firstColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			DataGridViewColumn secondColumn = Grid.Columns[1];
			secondColumn.Resizable = DataGridViewTriState.False;
			secondColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			secondColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			DataGridViewColumn thirdColumn = Grid.Columns[2];
			thirdColumn.Resizable = DataGridViewTriState.False;
			thirdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			thirdColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			DataGridViewColumn fourthColumn = Grid.Columns[3];
			fourthColumn.MinimumWidth = 150;
			fourthColumn.Resizable = DataGridViewTriState.False;
			fourthColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			fourthColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			fourthColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

			Grid.Rows.Add(new string[] { "Offset", "Length", "Property", "Value", "Description" });
			Grid.Rows[0].DefaultCellStyle.Font = new System.Drawing.Font(Grid.Font, System.Drawing.FontStyle.Bold);

			Grid.Rows.Add(new string[] { "0x00", "10 bytes", "Game Title:", File.GameTitle, "The internal title of the game." });
			Grid.Rows.Add(new string[] { "0x0C", "4 bytes", "Game Code", File.GameCode, "Game-Specific 4-Digit Code" });
			Grid.Rows.Add(new string[] { "0x10", "2 bytes", "Maker Code", File.MakerCode, "2-Digit License Code Assigned By Nintendo" });

			Grid.Rows.Add(new string[] { "0x12", "1 byte", "Poduct Code", UnitCode(File.UnitCode), "The product ID of the intended hardware." });
 			Grid.Rows.Add(new string[] { "0x13", "1 byte", "Device Type", File.DeviceType.ToString("X2"), "Internal backup device." });
			Grid.Rows.Add(new string[] { "0x14", "1 byte", "Cartridge Capacity", DeviceCapacity(File.DeviceCapacity), "Internal device capacity." });

			string reserved = "";
			foreach ( byte b in File.ReserveredA )
            {
				reserved += b.ToString("X2") + " ";
            }
			Grid.Rows.Add(new string[] { "0x15", "8 bytes", "Reserved A", reserved, "Unknown reserved area."});

			Grid.Rows.Add(new string[] { "0x1D", "1 byte", "Region Code", File.RegionCode.ToString("X2"), "Code indicating the intended region for release." });
			Grid.Rows.Add(new string[] { "0x1E", "1 byte", "Version", File.Version.ToString("X2"), "Revision number of this release." });
			Grid.Rows.Add(new string[] { "0x1F", "1 byte", "Internal Flags", File.InternalFlags.ToString("X2"), "Description" });

			Grid.Rows.Add(new string[] { "0x80", "4 bytes", "ROM Size", File.TotalSize.ToString("N0") + " bytes", "Description" });
			Grid.Rows.Add(new string[] { "0x84", "4 bytes", "Header Size", File.HeaderSize.ToString("N0") + " bytes", "Description" });

			Grid.Rows.Add(new string[] { "0x6C", "2 bytes", "Secure CRC", File.SecureCRC.ToString("X2"), "Description" });
			Grid.Rows.Add(new string[] { "0x6E", "2 bytes", "Secure Timeout", File.SecureTimeout.ToString("X2"), "Description" });
			Grid.Rows.Add(new string[] { "0x78", "8 bytes", "Secure Disable", File.SecureDisable.ToString("X2"), "Description" });

			foreach (string text in File.Warnings)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "N/A", "N/A", "Read Warning:", text, "This is a warning that doesn't prevent further reading." });
				Grid.Rows.Add(row);
			}
			foreach (string text in File.Errors)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "N/A", "N/A", "Read Error:", text, "This is a fatal read error. File read stops here." });
				Grid.Rows.Add(row);
			}
		}
		public static string UnitCode(byte unit_code)
		{
			string code = "NDS";
			switch (unit_code)
			{
				case 1:
					code = "EEPROM";
					break;
				case 2:
					code = "DSi Enhanced";
					break;
				case 3:
					code = "DSi Exclusive";
					break;
			}
			return code;
		}

		public static string DeviceCapacity(byte device_capacity)
		{
			double capacity = Math.Pow(2 , 20 + device_capacity);
			string return_capacity = "";
			if (capacity < 8388608)
			{
				capacity /= 8192;
				return_capacity = capacity.ToString("N0") + " Kilobytes";
			}
			else
			{
				capacity /= 8388608;
				return_capacity = capacity.ToString("N0") + " Megabytes";
			}
			return return_capacity;
		}

		public static string ToBinary(byte input)
		{
			string output = "|";
			for (int i = 0; i < 8; i++)
			{
				if ( (input & (1 << i)) != 0)
				{
					output += " 1 |";
				}
				else
				{
					output += " 0 |";
				}
				
			}
			return output;
		}
	}
}
