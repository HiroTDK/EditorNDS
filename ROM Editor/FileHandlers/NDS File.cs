using EditorNDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EditorNDS.FileHandlers
{
	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
			    Files        
	\*--------------------------*/

	public class NDSFile
	{
		public NDSFile()
		{

		}

		public NDSFile(string name, string path, string extension, uint offset, uint length, NDSDirectory parent)
		{
			Name = name;
			Path = path;
			Extension = extension;
			Offset = offset;
			Length = length;
			Parent = parent;
		}

		public NDSFile(uint offset, uint length)
		{
			Offset = offset;
			Length = length;
		}

		public string Name = "";
		public string Path = "";
		public string Extension = "";
		public int ID = 0;
		public uint Offset = 0;
		public uint Length = 0;
		public NDSDirectory Parent;
		public NARC NARCTables;

		public void GetExtension(Stream stream)
		{
			byte[] firstFour = new byte[4];
			stream.Position = Offset;
			stream.Read(firstFour, 0, 4);

			switch (System.Text.Encoding.UTF8.GetString(firstFour))
			{
				// Archives
				case "NARC":    // NITRO Archive
					Extension = ".narc";
					break;
				case "SDAT":    // Sound Data Archive
					Extension = ".sdat";
					break;

				// 2D Graphics
				case "RECN":    // Cell Definition Information
					Extension = ".ncer";
					break;
				case "RNAN":    // Animation Definition Information
					Extension = ".nanr";
					break;
				case "RMAN":    // Mutlicell  Animation Definition Information
					Extension = ".namr";
					break;
				case "RGCN":    // Character Information
					Extension = ".ncgr";
					break;
				case "RBCN":    // Bitmap Character Information
					Extension = ".ncbr";
					break;
				case "RLCN":    // Color Palette Information
					Extension = ".nclr";
					break;
				case "RCMN":    // MultiCell Information
					Extension = ".nmcr";
					break;
				case "RNEN":    // Entity Information 
					Extension = ".nenr";
					break;
				case "RCSN":    // Screen Information
					Extension = ".nscr";
					break;
				case "RTFN":    // Font Information
					Extension = ".nftr";
					break;

				// 3D Graphics
				case "BMD0":    // Model Data File
					Extension = ".nsbmd";
					break;
				case "BTX0":    // Texture Data File
					Extension = ".nsbtx";
					break;
				case "BCA0":    // Joint Animation Data File
					Extension = ".nsbca";
					break;
				case "BTP0":    // Texture Pattern Animation Data File
					Extension = ".nsbtp";
					break;
				case "BMA0":    // Material Color Animation Data File
					Extension = ".nsbma";
					break;
				case "BVA0":    // Visibility Animation Data File
					Extension = ".nsbva";
					break;
				case "BTA0":    // Texture SRT Animation Data File
					Extension = ".nsbta";
					break;

				// Others
				case "MESG":
					Extension = ".mesg";
					break;
				case " APS":    // Particle File
					Extension = ".spa";
					break;

				// Malformed Header in HG/SS
				case "RPCN":
					Extension = ".nclr";
					break;
			}

			if ( Extension == "" && Length > 8)
			{
				// Test for text files by searching for the "EOF" string near the end of the file.
				byte[] EOF = new byte[8];
				stream.Position = Offset + Length - 8;
				stream.Read(EOF, 0, 8);
				if (System.Text.Encoding.UTF8.GetString(EOF).Contains("EOF"))
				{
					Extension = ".txt";
				}
				else if (Name.Contains("."))
				{
					Extension = Name.Remove(0, Name.LastIndexOf("."));
				}
			}

			if (Extension.Length > 0 && Name.Length > Extension.Length)
			{
				int nLength = Name.Length - Extension.Length;
				string nSub = Name.Substring(nLength);

				if (nSub == Extension || nSub == Extension.ToUpper())
				{
					Name = Name.Remove(nLength);
				}
			}
		}
	}

	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
			   Overlays        
	\*--------------------------*/

	public class NDSOverlay
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
		public NDSFile File;
	}

	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
			 Directories        
	\*--------------------------*/

	public class NDSDirectory
	{
		public NDSDirectory()
		{
			Children = new List<NDSDirectory>();
			Contents = new List<NDSFile>();
		}

		public string Name;
		public string Path;
		public int ID;
		public NDSDirectory Parent;
		public List<NDSDirectory> Children;
		public List<NDSFile> Contents;
	}
}