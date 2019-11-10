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
		public string About = "";
		public string Type = "";
		public int ID = 0;
		public uint Offset = 0;
		public uint Length = 0;
		public NDSDirectory Parent;
		public NARC NARCTables;

		public void GetExtension(Stream stream)
		{
			if ( Length < 4)
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
					Type = "NITRO Archive";
					About = "NDS native archive format for general file storage.";
					break;
				case "SDAT":    // Sound Data Archive
					Extension = ".sdat";
					Type = "NITRO Sound Data Archive";
					About = "NDS native archive format for general sound file storage.";
					break;

				// 2D Graphics
				case "RECN":    // Cell Definition Information
					Extension = ".ncer";
					Type = "NITRO Cell For Runtime";
					break;
				case "RCMN":    // MultiCell Information
					Extension = ".nmcr";
					Type = "NITRO MultiCell For Runtime";
					break;
				case "RNAN":    // Animation Definition Information
					Extension = ".nanr";
					Type = "NITRO Animation For Runtime";
					break;
				case "RMAN":    // Mutlicell  Animation Definition Information
					Extension = ".namr";
					Type = "NITRO MultiCell Animation For Runtime";
					break;
				case "RGCN":    // Character Information
					Extension = ".ncgr";
					Type = "NITRO Character Graphic For Runtime";
					break;
				case "RBCN":    // Bitmap Character Information
					Extension = ".ncbr";
					Type = "NITRO Character Bitmap For Runtime";
					break;
				case "RLCN":    // Color Palette Information
					Extension = ".nclr";
					Type = "NITRO Color Pallete For Runtime";
					break;
				case "RNEN":    // Entity Information 
					Extension = ".nenr";
					Type = "NITRO Entity For Runtime";
					break;
				case "RCSN":    // Screen Information
					Extension = ".nscr";
					Type = "NITRO Screen For Runtime";
					break;
				case "RTFN":    // Font Information
					Extension = ".nftr";
					Type = "NITRO Font For Runtime";
					About = "NDS native font data file.";
					break;

				// 3D Graphics
				case "BMD0":    // Model Data File
					Extension = ".nsbmd";
					Type = "NITRO Model Data";
					About = "NDS native 3D model data file; textures optional.";
					break;
				case "BTX0":    // Texture Data File
					Extension = ".nsbtx";
					Type = "NITRO Texture Data";
					About = "NDS native texture data file, with palette information.";
					break;
				case "BCA0":    // Joint Animation Data File
					Extension = ".nsbca";
					Type = "NITRO Character Animation";
					About = "NDS native joint/node/skeletal animation data file.";
					break;
				case "BTP0":    // Texture Pattern Animation Data File
					Extension = ".nsbtp";
					Type = "NITRO Texture Pattern Animation";
					About = "NDS native texture pattern animation data file.";
					break;
				case "BMA0":    // Material Color Animation Data File
					Extension = ".nsbma";
					Type = "NITRO Material Animation";
					About = "NDS native material color animation data file.";
					break;
				case "BVA0":    // Visibility Animation Data File
					Extension = ".nsbva";
					Type = "NITRO Visibilty Animation";
					About = "NDS native visibility animation data file.";
					break;
				case "BTA0":    // Texture SRT Animation Data File
					Extension = ".nsbta";
					Type = "NITRO Texture SRT Animation";
					About = "NDS native texture scale, rotation, and translation animation data.";
					break;

				// Others
				case "MESG":
					Extension = ".bmg";
					Type = "Binary Message Data";
					CustomMessageBox.Show("Found MESG!", Path);
					break;
				case " APS":    // Particle File
					Extension = ".spa";
					Type = "Particle Data";
					break;

				// Malformed Header in HG/SS
				case "RPCN":
					Extension = ".nclr";
					Type = "NITRO Color Palette Data";
					break;
			}

			if ( Extension == "")
			{
				if (Length > 8) // Test for text files by searching for the "EOF" string near the end of the file.
				{
					byte[] EOF = new byte[8];
					stream.Position = Offset + Length - 8;
					stream.Read(EOF, 0, 8);
					if (System.Text.Encoding.UTF8.GetString(EOF).Contains("EOF"))
					{
						Extension = ".txt";
						Type = "Text File";
						About = "Generic text file; no special encoding.";
					}
				}

				if (Name == "utility.bin")
				{
					Name = "utility";
					Extension = ".arc";
					Type = "WiFi Utility Archive";
					About = "Archive included for WiFi uses. Contained files are compressed.";
				}
				else if (Name.Contains("."))
				{
					Extension = Name.Remove(0, Name.LastIndexOf("."));
					switch (Extension)
					{
						case ".dat":
							Type = "Generic Data File";
							About = "Generally, a structred data file with unspecified file structure.";
							break;
						case ".bin":
							Type = "Generic Binary File";
							About = "Generally, a binary code file in an unspecified language.";
							break;
						case ".l":
							Type = "LZ Compressed File";
							About = "File compressed using some variation of the LZ algorithm.";
							break;
						case ".srl":
							Type = "NITRO ROM File";
							About = "Compiled ROM file that hasn't been packaged for distribution.";
							break;

						// 2D Graphics For Runtime
						case ".nce":    // Cell Definition Information
							Type = "NITRO Cell";
							break;
						case ".nmc":    // MultiCell Information
							Type = "NITRO MultiCell";
							break;
						case ".nan":    // Animation Definition Information
							Type = "NITRO Animation";
							break;
						case ".nam":    // Mutlicell  Animation Definition Information
							Type = "NITRO MultiCell Animation";
							break;
						case ".ncg":    // Character Information
							Type = "NITRO Character Graphic";
							break;
						case ".char":    // Character Information
							Type = "NITRO Character Image";
							break;
						case ".ncb":    // Bitmap Character Information
							Type = "NITRO Character Bitmap";
							break;
						case ".ncl":    // Color Palette Information
							Type = "NITRO Color Pallete";
							break;
						case ".plt":    // Color Palette Information
							Type = "NITRO Color Pallete";
							break;
						case ".nen":    // Entity Information 
							Type = "NITRO Entity";
							break;
						case ".nsc":    // Screen Information
							Type = "NITRO Screen";
							break;
					}
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
			   Binaries        
	\*--------------------------*/

	public class NDSBinary : NDSFile
	{
		public NDSBinary()
		{

		}

		public uint Load;
		public uint AutoLoad;
		public uint AutoParams;
		public NDSFile File;
	}

	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
		   Binary  Overlays        
	\*--------------------------*/

	public class NDSOverlay : NDSFile
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

	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
	         Header File           
	\*--------------------------*/
	
	/// <summary>
	/// The is the file tree representation of the ROM header.
	/// </summary>
	public class NDSHeader : NDSFile
	{
		NDSHeader()
		{

		}
	}


	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
	         Banner File           
	\*--------------------------*/

	/// <summary>
	/// Banner files contain the icon and title data display on the DS home screen.
	/// The title is provided in many languages, depending on the game and version.
	/// DSi games can have an animated icon.
	/// </summary>
	public class NDSBanner : NDSFile
	{
		public NDSBanner()
		{

		}

		public ushort Version;

		// Version 1
		/// <summary>
		/// CRC16 of bytes 0x20 through 0x83F.
		/// </summary>
		public ushort CRC16_V1;
		public bool Verify_V1;
		
		public string TitleJapanese;
		public string TitleEnglish;
		public string TitleFrench;
		public string TitleGerman;
		public string TitleItalian;
		public string TitleSpanish;

		public Bitmap Icon;

		// Version 2
		/// <summary>
		/// CRC16 of bytes 0x20 through 0x93F.
		/// </summary>
		public ushort CRC16_V2;
		public bool Verify_V2;
		public string TitleChinese;

		// Version 3
		/// <summary>
		/// CRC16 of bytes 0x20 through 0xA3F.
		/// </summary>
		public ushort CRC16_V3;
		public bool Verify_V3;
		public string TitleKorean;

		// Version 103 (DSi)
		/// <summary>
		/// CRC16 of bytes 0x20 through 0x23BF.
		/// </summary>
		public ushort CRC16_V103;
		public bool Verify_V103;

		public Bitmap[] Icons;
		public byte[] Animaiton;


		/// <summary>
		/// Reads the information from the banner file to the NDSBanner class.
		/// </summary>
		/// <param name="stream">The ROM file being read from.</param>
		public void ReadBanner(Stream stream)
		{
			using (BinaryReader reader = new BinaryReader(stream, new UTF8Encoding(), true))
			{
				reader.BaseStream.Position = Offset;
				
				// Start by reading version data and version-specific checksums.
				Version = reader.ReadUInt16();
				CRC16_V1 = reader.ReadUInt16();
				if (Version >= 0x0002)
				{
					CRC16_V2 = reader.ReadUInt16();
				}
				if (Version >= 0x0003)
				{
					CRC16_V3 = reader.ReadUInt16();
				}
				if (Version >= 0x0103)
				{
					CRC16_V103 = reader.ReadUInt16();
				}

				reader.BaseStream.Position = Offset + 32;
				Icon = ReadIcon(reader.ReadBytes(512), reader.ReadBytes(32));

				// Now we read the language-specific title data.
				reader.BaseStream.Position = 240;
				TitleJapanese = ReadTitle(reader.ReadBytes(256));
				TitleEnglish = ReadTitle(reader.ReadBytes(256));
				TitleFrench = ReadTitle(reader.ReadBytes(256));
				TitleGerman = ReadTitle(reader.ReadBytes(256));
				TitleItalian = ReadTitle(reader.ReadBytes(256));
				TitleSpanish = ReadTitle(reader.ReadBytes(256));
				if (Version >= 0x0002)
				{
					TitleChinese = ReadTitle(reader.ReadBytes(256));
				}
				if (Version >= 0x0003)
				{
					TitleKorean = ReadTitle(reader.ReadBytes(256));
				}
			}
		}

		/// <summary>
		/// Reads the title data to a usable string.
		/// </summary>
		/// <param name="raw">The raw title data from the banner file.</param>
		/// <returns>A readable string of the title data.</returns>
		public string ReadTitle(byte[] raw)
		{
			string title = "";
			title = new String(Encoding.Unicode.GetChars(raw));
			title = title.Replace("\n", "\r\n");
			return title;
		}
		
		public Bitmap ReadIcon(byte[] raw_bitmap, byte[] raw_palette)
		{
			Bitmap icon = new Bitmap(32, 32);
			Color[] palette = ImageHandler.ReadPaletteBGR555(raw_palette);
			raw_bitmap = DataHandler.BytesToNibbles(raw_bitmap);
			
			int pixel = 0;
			for ( int tile_y = 0; tile_y < 4; tile_y++ )
			{
				for ( int tile_x = 0; tile_x < 4; tile_x++ )
				{
					for ( int pixel_y = 0; pixel_y < 8; pixel_y++ )
					{
						for ( int pixel_x = 0; pixel_x < 8; pixel_x++ )
						{
							icon.SetPixel(((tile_x * 8) + pixel_x), ((tile_y * 8) + pixel_y), palette[raw_bitmap[pixel]]);
							pixel++;
						}
					} 
				}
			}

			return icon;
		}
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