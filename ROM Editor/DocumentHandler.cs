using EditorNDS.FileHandlers;
using EditorNDS.ROMExplorer;
using EditorNDS.TabbedInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS
{
	public class DocumentHandler : BaseClass
	{
		public string originalFilePath;
		public string backupFilePath;
		public string workingFilePath;

		public Tab documentTab;
		public Control documentControl;

		public DocumentHandler() { }

		public void AutoSave() { }
		public void Save() { }
		public void Export() { }
		public void Reload() { }
		public void Close() { }

		public void Undo() { }
		public void Redo() { }
		public void Revert() { }
	}

	public class DocumentROM : DocumentHandler
	{
		public DocumentROM(TabInterface tabInterface, NDSROM rom)
		{
			ROMViewer viewer = new ROMViewer(rom);
			viewer.Dock = DockStyle.Fill;
			string rom_title = "[" + rom.Header.GameCode + "] " + rom.Header.GameTitle;

			documentTab = new Tab(tabInterface, rom_title);
			documentTab.tabControl = viewer;
			documentTab.tabDocument = this;
			tabInterface.OpenTab(documentTab);
			tabInterface.ActivateTab(documentTab);
		}
	}
}
