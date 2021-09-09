/*--------------------------------------------------*\
                           |
\*--------------------------------------------------*/
/*--------------------------------------------------*\




\*--------------------------------------------------*/


using EditorNDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using EditorNDS.Utilities;
using System.Windows.Forms;


namespace EditorNDS.FileHandlers
{
	public interface INitroDirectory
	{
		string Name { get; set; }
		string Path { get; set; }
		uint ID { get; set; }

		INitroDirectory Parent { get; set; }
		List<INitroDirectory> Children { get; set; }
		List<INitroFile> Contents { get; set; }
		List<INitroDirectory> AllDirectories { get; set; }
		List<INitroFile> AllFiles { get; set; }
	}

	public class NitroDirectory : INitroDirectory
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public uint ID { get; set; }

		public INitroDirectory Parent { get; set; }
		public List<INitroDirectory> Children { get; set; }
		public List<INitroFile> Contents { get; set; }
		public List<INitroDirectory> AllDirectories { get; set; }
		public List<INitroFile> AllFiles { get; set; }

		public NitroDirectory()
		{
			Children = new List<INitroDirectory>();
			Contents = new List<INitroFile>();
			AllDirectories = new List<INitroDirectory>();
			AllFiles = new List<INitroFile>();
		}
	}
}