using EditorNDS.FileHandlers;
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
		public DocumentROM(TabInterface tabInterface)
		{
			documentTab = new Tab(tabInterface, "lololol");
			documentTab.tabControl = new ROMViewer();
			documentTab.tabDocument = this;
			tabInterface.OpenTab(documentTab);
			tabInterface.ActivateTab(documentTab);
		}
	}
}
