using EditorNDS.PokemonEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EditorNDS.FileHandlers;

namespace EditorNDS
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
			MdiClient mdi = null;
			int ExStyle;
			foreach (Control ctl in Controls)
			{
				if (ctl is MdiClient)
				{
					mdi = (MdiClient)ctl;
					mdi.BackColor = Color.FromArgb(95, 95, 95);
					ExStyle = GetWindowLong(mdi.Handle, GWL_EXSTYLE);
					ExStyle ^= WS_EX_CLIENTEDGE;
					SetWindowLong(mdi.Handle, GWL_EXSTYLE, ExStyle);
					SetWindowPos(mdi.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
					SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
				}
			}
			ToolStripSystemRenderer TSSR = new MenuRenderer();
			menuMain.Renderer = TSSR;
			statusMain.Renderer = TSSR;
			Show();
		}

		private void newPMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = CustomFormBox.Show(
				new NewProject(),
				"NewProject",
				new List<string>(),
				new List<DialogResult>());

			return;
		}

		private void statsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = CustomFormBox.Show(
				new PokemonStats(),
				"Stats Test",
				new List<string>(),
				new List<DialogResult>());

			return;
		}

		private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = "Select a ROM to edit.";
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Filter = "NDS ROM (*.nds)|*.nds";
				DialogResult result = openFileDialog.ShowDialog();

				// Testing the open file dialog result.
				if (result != DialogResult.OK)
				{
					// Presumption of cancellation.
					return;
				}
				if (!openFileDialog.CheckFileExists)
				{
					CustomMessageBox.Show("File Doesn't Exist", "Cannot find the specified file.");
				}

				NDSROM rom_file = new NDSROM(File.ReadAllBytes(openFileDialog.FileName));
				


			}
		}
	}
}

