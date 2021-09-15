using EditorNDS.FileHandlers;
using EditorNDS.ROMExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			Grid.ColumnCount = 3;
			DataGridViewColumn firstColumn = Grid.Columns[0];
			firstColumn.Resizable = DataGridViewTriState.False;
			firstColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			firstColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			DataGridViewColumn secondColumn = Grid.Columns[1];
			secondColumn.MinimumWidth = 275;
			secondColumn.Resizable = DataGridViewTriState.False;
			secondColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			secondColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			secondColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

			Grid.Rows.Add(new string[] { "Property", "Value", "Description" });
			Grid.Rows[0].DefaultCellStyle.Font = new System.Drawing.Font(Grid.Font, System.Drawing.FontStyle.Bold);

			Grid.Rows.Add(new string[] { "Game Title:", File.GameTitle, "The internal title of the game." });
			Grid.Rows.Add(new string[] { "Game Code", File.GameCode, "Game-Specific 4-Digit Code" });
			Grid.Rows.Add(new string[] { "Maker Code", File.MakerCode, "2-Digit License Code Assigned By Nintendo" });

			Grid.Rows.Add(new string[] { "Poduct Code", UnitCode(File.UnitCode), "The product ID of the intended hardware." });
			Grid.Rows.Add(new string[] { "Device Type", File.DeviceType.ToString("X"), "Internal backup device." });
			Grid.Rows.Add(new string[] { "Device Capacity", DeviceCapacity(File.DeviceCapaciy), "Internal device capacity." });
			Grid.Rows.Add(new string[] { "Region Code", File.RegionCode.ToString("X"), "Code indicating the intended region for release." });
			Grid.Rows.Add(new string[] { "Version", File.Version.ToString("X"), "Revision number of this release." });
			Grid.Rows.Add(new string[] { "Property", File.InternalFlags.ToString("X"), "Description" });

			foreach (string text in File.Warnings)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "Read Warning:", text, "This is a warning that doesn't prevent further reading." });
				Grid.Rows.Add(row);
			}
			foreach (string text in File.Errors)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Height = 60;
				row.CreateCells(Grid, new string[] { "Read Error:", text, "This is a fatal read error. File read stops here." });
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
