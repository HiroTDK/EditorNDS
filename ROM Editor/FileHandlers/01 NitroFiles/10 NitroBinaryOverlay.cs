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
	public class NDSOverlay : NitroFile
	{
		public NDSOverlay()
		{

		}

		public uint OverlayID;
		public uint AddressRAM;
		public uint SizeRAM;
		public uint SizeBSS;
		public uint StaticStartAddress;
		public uint StaticEndAddress;
		public bool Authenticated;
		public bool Compressed;
		public uint CompressedSize;
	}
}