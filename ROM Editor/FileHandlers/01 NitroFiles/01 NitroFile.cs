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
	public interface INitroFile
	{
		uint ID { get; set; }
		uint Offset { get; set; }
		uint Length { get; set; }

		string OriginalName { get; set; }
		string Name { get; set; }
		string Path { get; set; }

		string Extension { get; set; }
		string FileType { get; set; }
		string FileDescription { get; set; }

		List<String> Warnings { get; set; }
		List<string> Errors { get; set; }

		INitroDirectory Parent { get; set; }
			}

	public class NitroFile : INitroFile
	{
		public uint ID { get; set; }
		public uint Offset { get; set; }
		public uint Length { get; set; }

		public string OriginalName { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }

		public string Extension { get; set; }
		public string FileType { get; set; }
		public string FileDescription { get; set; }

		public List<String> Warnings { get; set; }
		public List<string> Errors { get; set; }

		public INitroDirectory Parent { get; set; }

		public NitroFile()
		{
			ID = 0;
			Offset = 0;
			Length = 0;

			OriginalName = "";
			Name = "";
			Path = "";

			Extension = "";
			FileType = "";
			FileDescription = "";

			Warnings = new List<string>();
			Errors = new List<string>();
		}

		public void GetExtension(Stream stream)
		{
			if (Length < 4)
			{
				return;
			}
			stream.Position = Offset;

			byte[] firstFour = new byte[4];
			stream.Read(firstFour, 0, 4);

			switch (System.Text.Encoding.UTF8.GetString(firstFour))
			{
				// Archives
				case "NARC":    // NITRO Archive
					Extension = ".narc";
					FileType = "NITRO Archive";
					FileDescription = "NDS native archive format for general file storage.";
					break;
				case "SDAT":    // Sound Data Archive
					Extension = ".sdat";
					FileType = "NITRO Sound Data Archive";
					FileDescription = "NDS native archive format for general sound file storage.";
					break;

				// 2D Graphics
				case "RECN":    // Cell Definition Information
					Extension = ".ncer";
					FileType = "NITRO Cell For Runtime";
					break;
				case "RCMN":    // MultiCell Information
					Extension = ".nmcr";
					FileType = "NITRO MultiCell For Runtime";
					break;
				case "RNAN":    // Animation Definition Information
					Extension = ".nanr";
					FileType = "NITRO Animation For Runtime";
					break;
				case "RMAN":    // Mutlicell  Animation Definition Information
					Extension = ".namr";
					FileType = "NITRO MultiCell Animation For Runtime";
					break;
				case "RGCN":    // Character Information
					Extension = ".ncgr";
					FileType = "NITRO Character Graphic For Runtime";
					break;
				case "RBCN":    // Bitmap Character Information
					Extension = ".ncbr";
					FileType = "NITRO Character Bitmap For Runtime";
					break;
				case "RLCN":    // Color Palette Information
					Extension = ".nclr";
					FileType = "NITRO Color Pallete For Runtime";
					break;
				case "RNEN":    // Entity Information 
					Extension = ".nenr";
					FileType = "NITRO Entity For Runtime";
					break;
				case "RCSN":    // Screen Information
					Extension = ".nscr";
					FileType = "NITRO Screen For Runtime";
					break;
				case "RTFN":    // Font Information
					Extension = ".nftr";
					FileType = "NITRO Font For Runtime";
					FileDescription = "NDS native font data file.";
					break;

				// 3D Graphics
				case "BMD0":    // Model Data File
					Extension = ".nsbmd";
					FileType = "NITRO Model Data";
					FileDescription = "NDS native 3D model data file; textures optional.";
					break;
				case "BTX0":    // Texture Data File
					Extension = ".nsbtx";
					FileType = "NITRO Texture Data";
					FileDescription = "NDS native texture data file, with palette information.";
					break;
				case "BCA0":    // Joint Animation Data File
					Extension = ".nsbca";
					FileType = "NITRO Character Animation";
					FileDescription = "NDS native joint/node/skeletal animation data file.";
					break;
				case "BTP0":    // Texture Pattern Animation Data File
					Extension = ".nsbtp";
					FileType = "NITRO Texture Pattern Animation";
					FileDescription = "NDS native texture pattern animation data file.";
					break;
				case "BMA0":    // Material Color Animation Data File
					Extension = ".nsbma";
					FileType = "NITRO Material Animation";
					FileDescription = "NDS native material color animation data file.";
					break;
				case "BVA0":    // Visibility Animation Data File
					Extension = ".nsbva";
					FileType = "NITRO Visibilty Animation";
					FileDescription = "NDS native visibility animation data file.";
					break;
				case "BTA0":    // Texture SRT Animation Data File
					Extension = ".nsbta";
					FileType = "NITRO Texture SRT Animation";
					FileDescription = "NDS native texture scale, rotation, and translation animation data.";
					break;

				// Others
				case "MESG":
					Extension = ".bmg";
					FileType = "Binary Message Data";
					//CustomMessageBox.Show("Found MESG!", Path);
					break;
				case " APS":    // Particle File
					Extension = ".spa";
					FileType = "Particle Data";
					break;

				// Malformed Header in HG/SS
				case "RPCN":
					Extension = ".nclr";
					FileType = "NITRO Color Palette Data";
					break;
			}

			if (Extension == "")
			{
				if (Length > 8) // Test for text files by searching for the "EOF" string near the end of the file.
				{
					byte[] EOF = new byte[8];
					stream.Position = Offset + Length - 8;
					stream.Read(EOF, 0, 8);
					if (System.Text.Encoding.UTF8.GetString(EOF).Contains("EOF"))
					{
						Extension = ".txt";
						FileType = "Text File";
						FileDescription = "Generic text file; no special encoding.";
					}
				}

				if (Name == "utility.bin")
				{
					Name = "utility";
					Extension = ".arc";
					FileType = "WiFi Utility Archive";
					FileDescription = "Archive included for WiFi uses. Contained files are compressed.";
				}
				else if (Name.Contains("."))
				{
					Extension = Name.Remove(0, Name.LastIndexOf("."));
					switch (Extension)
					{
						case ".dat":
							FileType = "Generic Data File";
							FileDescription = "Generally, a structred data file with unspecified file structure.";
							break;
						case ".bin":
							FileType = "Generic Binary File";
							FileDescription = "Generally, a binary code file in an unspecified language.";
							break;
						case ".l":
							FileType = "LZ Compressed File";
							FileDescription = "File compressed using some variation of the LZ algorithm.";
							break;
						case ".srl":
							FileType = "NITRO ROM File";
							FileDescription = "Compiled ROM file that hasn't been packaged for distribution.";
							break;

						// 2D Graphics For Runtime
						case ".nce":    // Cell Definition Information
							FileType = "NITRO Cell";
							break;
						case ".nmc":    // MultiCell Information
							FileType = "NITRO MultiCell";
							break;
						case ".nan":    // Animation Definition Information
							FileType = "NITRO Animation";
							break;
						case ".nam":    // Mutlicell  Animation Definition Information
							FileType = "NITRO MultiCell Animation";
							break;
						case ".ncg":    // Character Information
							FileType = "NITRO Character Graphic";
							break;
						case ".char":    // Character Information
							FileType = "NITRO Character Image";
							break;
						case ".ncb":    // Bitmap Character Information
							FileType = "NITRO Character Bitmap";
							break;
						case ".ncl":    // Color Palette Information
							FileType = "NITRO Color Pallete";
							break;
						case ".plt":    // Color Palette Information
							FileType = "NITRO Color Pallete";
							break;
						case ".nen":    // Entity Information 
							FileType = "NITRO Entity";
							break;
						case ".nsc":    // Screen Information
							FileType = "NITRO Screen";
							break;
					}
				}
			}

			if (Extension != null && Extension.Length > 0 && Name.Length > Extension.Length)
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
}