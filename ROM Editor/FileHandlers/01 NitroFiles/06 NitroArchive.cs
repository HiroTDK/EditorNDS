/*----------------------------------------------------------------------------------------------------*\
                                          Nitro Archive (NARC)
\*----------------------------------------------------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------*\
	NARCs, or Nitro Archives, are the native archiving format used for the Nintendo DS. These archives
  have no support for compression, though you will occasionally find some that are themselves later
  compressed after archiving. The format is pretty easy to read, thanks to the block format it uses,
  along with signatures for each block. NARCs have four sections: Header, File Allocation Table, File
  Name Table, and File Images.

	The Header is 16 bytes, and contains 6 variables, shown here:

  * File Signature:		4 Bytes
  *		The signature for this file type is N,A,R,C, as ASCII characters.
  * Endianness:			2 Bytes
  *		0xFEFF for Little Endian, and 0xFFFF for Big Endian
  * 	Literally nothing I've seen on the NDS is Big Endian, so I have no idea why this exists.
  * Version:			2 Bytes
  * 	It seems the first byte refers to a major version, and the second byte to minor revisions.
  * 	For example, 0x0102 would be Version 1.02.
  * 	I've never seen any version other than Version 1.00.
  * FileSize:			4 Bytes
  * 	This is the total size of the archive.
  * HeaderSize:			2 Bytes
  * 	This is the size of the Header block. It's always 16 bytes, but apparently could be more.
  * BlockCount:			2 Bytes
  * 	This is the number of data blocks in the archive. Always 3, but they left room for expansion.

	The next block is the File Allocation Table. The File Allocation Table has a 12-byte header with a
  signature, 2 variables, an unused secton, and an array of offsets.

  * Block Signature:	4 Bytes
  *		The signature for this block is B,T,A,F, as ASCII characters.
  *	Block Size:			4 Bytes
  *		The size of the File Allocation Table block, including this header.
  *	File Count:			2 Bytes
  *		The number of files in the File Allocation Table, and, by extension, the NARC.
  *	Unused:				2 Bytes
  *		These two bytes seem always to be unused. They're pobably just to pad the header.
  *	Allocation Table:	File Count * 8 Bytes
  *		File indices are determined by their order in this table.
  *		For each file count, the table contains 8 bytes:
  *		Start Offset:	4 Bytes
  *			Offset from the start of the image block to the start of the file.
  *		End Offset:		4 Bytes
  *			Offset from the start of the image block to the start of the file.

	The next block is the File Name Table. The File Name Table has an 8-byte header with a signature,
  a variable, and 2 arrays.

  * Block Signature:	4 Bytes
  *		The signature for this block is B,T,N,F, as ASCII characters.
  *	Block Size:			4 Bytes
  *		This size of the File Name Table block, including this header.
  *	Directory Table:	

	ARCs are another type of archive format occasionally seen in Nintendo DS games. They are, in
  essence, the same as NARCs, except that they have no block signatures or extra header data.
\*----------------------------------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------------------------------*\
                                      Nitro Archive Binary Format                                   
\*----------------------------------------------------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------*\
	//--- Header ---\\
	const uint Signature = 1129464142;		// 0x0		File Signature (N,A,R,C)
	const ushort ByteOrder = 65534;			// 0x4		Byte Order Marks (0xFEFF)
	const ushort Version = 256;				// 0x6		Archive Format Version (0x0100)
	uint FileSize = 0;						// 0x8		Archive File Size
	const ushort HeaderSize = 16;			// 0xC		Archive Header Size (0x10)
	const ushort DataBlocks = 3;			// 0xE		Number Of Data Blocks (0x3)

	//--- FATB Data Block ---\\				// ?		[HeaderSize]
	const uint SignatureFAT = 1178686530;	// 0x0		Data Block Type (FATB)
	uint SizeFAT = 0;						// 0x4		Allocation Table Size
	ushort FilesFAT = 0;					// 0x8		Number Of Files In Allocation Table
	Reserved [FAT]							// 0xA		Reserved (0x0000)
	byte[] FATB								// 0xC		File Allocation Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FNTB Data Block ---\\				// ?		[HeaderSize + SizeFAT]
	const uint SignatureFNT = 1179538498;	// 0x0		Data Block Type (FNTB)
	uint SizeFNT = 0;						// 0x4		Name Table Size
	byte[] FNTB;							// 0x8		File Name Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FIMG Data Block ---\\				// ?		[HeaderSize + SizeFAT + SizeFNT + Padding]
	const uint SignatureIMG = 1179209031;	// 0x0		Data Block Type (FIMG)
	uint SizeIMG = 0;						// 0x4		Image File Size
	byte[] FIMG;							// 0x8		Image File
	// Padding (Required)					// ?		[Required 4-, 8-, 16-, or 32-Byte Alignment]
\*----------------------------------------------------------------------------------------------------*/


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
	/// <summary>
	/// The file handler for the native Nitro Archive format.
	/// <br></br>
	/// Implements both INitroFile and INitroDirectory.
	/// </summary>
	public class NitroArchive : INitroFile, INitroDirectory
	{
		public uint ID { get; set; }
		public uint Offset { get; set; }
		public uint Length { get; set; }

		public string OriginalName { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }

		public string Extension
		{
			get { return ".narc"; }
			set { return; }
		}
		public string FileType
		{
			get { return "Nitro Archive"; }
			set { return; }
		}
		public string FileDescription
		{
			get { return "Nintendo's native DS archive format."; }
			set { return; }
		}

		public List<String> Warnings { get; set; }
		public List<string> Errors { get; set; }

		public INitroDirectory Parent { get; set; }
		public List<INitroDirectory> AllDirectories { get; set; }
		public List<INitroFile> AllFiles { get; set; }
		public List<INitroDirectory> Children { get; set; }
		public List<INitroFile> Contents { get; set; }


		public uint[] TopOffsets;
		public uint[] BottomOffsets;
		public byte[] FileNameTable;

		/// <summary>
		/// Nitro Archive Header
		/// <br></br>
		/// The Header is 16 bytes long, consisting of a file
		/// signature, 3 constants, and 2 variables.
		/// </summary>
		public struct Header
		{
			/// <summary>
			/// Nitro Archive file signature: always "NARC".
			/// </summary>
			public string Signature;
			/// <summary>
			/// Endian indicator: always little endian (0xFEFF).
			/// </summary>
			public ushort Endianness;
			/// <summary>
			/// Version indicator: this program will always write them as version 3.14.
			/// </summary>
			public ushort Version;
			/// <summary>
			/// Total length, including any padding.
			/// </summary>
			public uint Length;
			/// <summary>
			/// Nitro Archive header size: always 16 bytes.
			/// </summary>
			public ushort HeaderSize;
			/// <summary>
			/// Block count; always 3 blocks.
			/// </summary>
			public ushort Blocks;

			public Header(
				string	signature,
				ushort	endianness,
				ushort	version,
				uint	length,
				ushort	size,
				ushort	blocks)
			{
				this.Signature = signature;
				this.Endianness = endianness;
				this.Version = version;
				this.Length = length;
				this.HeaderSize = size;
				this.Blocks = blocks;
			}
		}

		/// <summary>
		/// File Alloctation Table Headear
		/// <br></br>
		/// The header is 12 bytes long, consisting of a block
		/// signature, 2 variables, and padding.
		/// </summary>
		public struct HeaderFAT
		{
			/// <summary>
			/// File Allocation Table block signature: always "BTAF".
			/// </summary>
			public string	Signature;
			/// <summary>
			/// File Allocation Table block size, including it's header.
			/// </summary>
			public uint		Size;
			/// <summary>
			/// Nitro Archive file count.
			/// </summary>
			public ushort	FileCount;

			public HeaderFAT(
				string signature,
				uint size,
				ushort fileCount)
			{
				this.Signature = signature;
				this.Size = size;
				this.FileCount = fileCount;
			}
		}

		/// <summary>
		/// File Name Table Headear
		/// <br></br>
		/// The header is 8 bytes long, consisting of a block
		/// signature, and a variable.
		/// </summary>
		public struct HeaderFNT
		{
			/// <summary>
			/// File Name Table block signature: always "BTNF".
			/// </summary>
			public string	Signature;
			/// <summary>
			/// File Name Table block size, including it's header.
			/// </summary>
			public uint		Size;

			public HeaderFNT(
				string signature,
				uint size)
			{
				this.Signature = signature;
				this.Size = size;
			}
		}

		/// <summary>
		/// Image Headear
		/// <br></br>
		/// The header is 8 bytes long, consisting of a block
		/// signature and a variable.
		/// </summary>
		public struct HeaderIMG
		{
			/// <summary>
			/// File Name Table block signature: always "BTNF".
			/// </summary>
			public string Signature;
			/// <summary>
			/// File Name Table block size, including it's header.
			/// </summary>
			public uint Size;

			public HeaderIMG(
				string signature,
				uint size)
			{
				this.Signature = signature;
				this.Size = size;
			}
		}

		public NitroArchive(NitroFile nitroFile, Stream stream)
		{
			this.ID = nitroFile.ID;
			this.Name = nitroFile.Name;
			this.OriginalName = nitroFile.OriginalName;
			this.Parent = nitroFile.Parent;
			this.Path = nitroFile.Path;
			this.Offset = nitroFile.Offset;
			this.Length = nitroFile.Length;

			this.Warnings = new List<string>();
			this.Errors = new List<string>();

			this.AllDirectories = new List<INitroDirectory>();
			this.AllFiles = new List<INitroFile>();
			this.Children = new List<INitroDirectory>();
			this.Contents = new List<INitroFile>();

			// 44 bytes is theoretically the smallest properly
			// formated NARC file. That is enough to contain all
			// of the headers' information, assuming 0 files.
			//
			if (this.Length < 44)
			{
				this.Errors.Add("This file is too small to be a NARC file.");
			}

			if (stream.Length < 0)
			{
				this.Errors.Add("The stream is empty.");
			}

			if ( stream.Length < this.Length)
			{
				this.Errors.Add("Stream length is less than the length of the file.\n" +
					"Stream Length: " + stream.Length +"\n" +
					"File Length: " + this.Length);
				return;
			}

			if ( stream.Length < this.Offset + this.Length)
			{
				this.Errors.Add("File runs out of bounds of the stream.\n" +
					"Stream End: " + stream.Length + "\n" +
					"File End: " + this.Offset + this.Length);
				return;
			}

			using ( BinaryReader reader = new BinaryReader( stream, new UTF8Encoding(), true))
			{
				uint fat_offset = this.Offset + 16;
				uint fnt_offset;
				uint img_offset;

				//
				// Reading Nitro Archive Header
				//
				reader.BaseStream.Position = this.Offset;

				Header header = new Header(
					System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4)),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt32(),
					reader.ReadUInt16(),
					reader.ReadUInt16());

				if ( header.Signature != "NARC")
				{
					this.Warnings.Add("The file signature doesn't match.\n" + 
						"Expected Signature: 'NARC'\n" +
						"Header Signature: '" + header.Signature + "'");
				}				
				if ( header.Endianness != 65534)
				{
					if ( header.Endianness == 65535 )
					{
						this.Warnings.Add("The endianness variable in the header is incorrect.\n" +
							"Expected Endianness: Little Endian (0xFEFF)\n" +
							"Header Endianness: Big Endian (0xFFFF)");
					}
					else
					{
						this.Warnings.Add("The endianness variable in the header is unrecognized.\n" +
							"Expected Endianness: Little Endian (0xFEFF)\n" +
							"Header Endianness: " + header.Endianness.ToString("X"));
					}
				}
				if ( header.Length != this.Length )
				{
					this.Warnings.Add("The length variable in the header indicates a different size than the specified file.\n" +
						"Expected Length: " + this.Length + " Bytes\n" +
						"Header Length: " + header.Length + " Bytes");
				}
				if ( header.HeaderSize != 16)
				{
					this.Warnings.Add("The size variable in the header is incorrect.\n" +
						"Expected Size: 16 Bytes\n" +
						"Header Size: " + header.HeaderSize + " Bytes");
				}
				if ( header.Blocks != 3 )
				{
					this.Warnings.Add("The size variable in the header is incorrect.\n" +
						"Expected Blocks: 3\n" +
						"Header Blocks: " + header.Blocks);
				}

				//
				// Reading File Allocation Table Block Header
				//
				reader.BaseStream.Position = fat_offset;
				HeaderFAT fat = new HeaderFAT(
					System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4)),
					reader.ReadUInt32(),
					reader.ReadUInt16());
				if ( fat.Signature != "BTAF" )
				{
					this.Warnings.Add("The block signature doesn't match.\n" +
						"Expected Signature: 'BTAF'\n" +
						"Header Signature: '" + fat.Signature + "'");
				}
				if ( fat.FileCount != (fat.Size - 12) / 8)
				{
					this.Errors.Add("File allocation table size doesn't match file count.\n" +
						"Expected Size: " + (fat.FileCount * 8) + " Bytes\n" +
						"Table Size: " + (fat.Size - 12) + " Bytes");
					return;
				}

				fnt_offset = fat_offset + fat.Size;
				if ( stream.Length < fnt_offset )
				{
					this.Errors.Add("File allocation table runs out of bounds of the stream.\n" +
						"Stream End: " + stream.Length + "\n" +
						"Table End: " + fnt_offset);
					return;
				}

				//
				// Reading File Name Table Block Header
				//
				reader.BaseStream.Position = fnt_offset;
				HeaderFNT fnt = new HeaderFNT(
					System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4)),
					reader.ReadUInt32());
				if ( fnt.Signature != "BTNF" )
				{
					this.Warnings.Add("The block signature doesn't match.\n" +
						"Expected Signature: 'BTNF'\n" +
						"Header Signature: '" + fat.Signature + "'");
				}

				img_offset = fnt_offset + fnt.Size;
				if ( stream.Length < img_offset)
				{
					this.Errors.Add("File name table runs out of bounds of the stream.\n" +
						"Stream End: " + stream.Length + "\n" +
						"Table End: " + fnt_offset);
					return;
				}

				//
				// Reading Image Block Header
				//
				reader.BaseStream.Position = img_offset;
				HeaderIMG img = new HeaderIMG(
					System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4)),
					reader.ReadUInt32());
				if (img.Signature != "GMIF")
				{
					this.Warnings.Add("The block signature doesn't match.\n" +
						"Expected Signature: 'GMIF'\n" +
						"Header Signature: '" + img.Signature + "'");
				}
				if (stream.Length < img_offset)
				{
					this.Errors.Add("File name table runs out of bounds of the stream.\n" +
						"Stream End: " + stream.Length + "\n" +
						"Table End: " + fnt_offset);
					return;
				}
				if (stream.Length < img_offset + img.Size)
				{
					this.Errors.Add("File image runs out of bounds of the stream.\n" +
						"Stream End: " + stream.Length + "\n" +
						"Table End: " + fnt_offset);
					return;
				}
				if (this.Length < img_offset + img.Size - this.Offset)
				{
					this.Errors.Add("File image runs out of bounds of the file.\n" +
						"File End: " + stream.Length + "\n" +
						"Image End: " + fnt_offset);
					return;
				}

				//
				// Reading File Allocation Table
				//
				// The File Allocation Table contain eight bytes per file entry;
				// a four byte file offset followed by a four byte file length.
				// We start by determining the file count and then dividing the
				// offsets and lenghts into separate indexed arrays.
				//
				reader.BaseStream.Position = fat_offset + 12;
				INitroFile[] file_table = new INitroFile[fat.FileCount];
				ushort index;
				for ( index = 0; index < fat.FileCount; index++)
				{
					uint topOffset = reader.ReadUInt32();
					uint bottomOffset = reader.ReadUInt32();
					NitroFile newFile = new NitroFile()
					{
						ID = index,
						Offset = topOffset + img_offset + 8,
						Name = "File " + index.ToString("D" + fat.FileCount.ToString().Length),
						Path = this.Path + "\\" + this.Name,
						Parent = this
					};
					
					this.AllFiles.Add(newFile);
					file_table[index] = newFile;

					if ( topOffset > bottomOffset)
					{
						this.Warnings.Add("File " + index + " size is negative.\n" +
							"File Start: " + topOffset + "\n" +
							"File End: " + bottomOffset);
						newFile.Errors.Add("File runs out of bounds of the image block.\n" +
							"File Start: " + topOffset + "\n" +
							"File End: " + bottomOffset);
					}
					else
					{
						newFile.Length = bottomOffset - topOffset;
					}

					if (topOffset > img.Size)
					{
						this.Warnings.Add("File " + index + " runs out of bounds of the image block.\n" +
							"File Start: " + topOffset + "\n" +
							"Image End: " + img.Size);
						newFile.Errors.Add("File runs out of bounds of the image block.\n" +
							"File Start: " + topOffset + "\n" +
							"Image End: " + img.Size);
					}
					else if (bottomOffset > img.Size)
					{
						this.Warnings.Add("File " + index + " runs out of bounds of the image block.\n" +
							"File End: " + bottomOffset + "\n" +
							"Image End: " + img.Size);
						newFile.Errors.Add("File runs out of bounds of the image block.\n" +
							"File End: " + bottomOffset + "\n" +
							"Image End: " + img.Size);
					}
				}

				//
				// Reading File Name Table
				//
				// The File Name Table contains two sections; the first is the
				// main directory table. It's length is eight bytes per entry.
				// The first four bytes represent an offset to the entry in the
				// second section; the sub-directory table. This is followed by
				// two byte index corresponding to the first file entry in the
				// directory. Finally we have a two byte index corresponding to
				// the parent directory.
				//
				// The first entry in the table is slightly different though.
				// The last two bytes in the first entry actually denote the
				// number of directories in the first section. Let's read that,
				// then use it to iterate through the main directory table and
				// split the table into three seperate indexed arrays.
				//
				reader.BaseStream.Position = fnt_offset + 14;
				ushort directory_count = reader.ReadUInt16();
				reader.BaseStream.Position = fnt_offset + 8;

				uint[] entry_offset = new uint[directory_count];
				ushort[] first_file = new ushort[directory_count];
				ushort[] parent_index = new ushort[directory_count];

				int directory_tableSize = directory_count * 8;
				if (directory_tableSize > fnt.Size - 8)
				{
					foreach (NitroFile file in file_table)
					{
						file.GetExtension(stream);
						if (file.Extension == ".narc")
						{
							NitroArchive narc = new NitroArchive(file, stream);
							file_table[Array.IndexOf(file_table, file)] = narc;
						}
					}

					this.Warnings.Add("Directory " + index + " runs out of bounds of the name table.\n" +
						"Name Table Start: " + directory_tableSize + "\n" +
						"Directory Name Start: " + entry_offset[index]);
					this.AllFiles = file_table.ToList();
					this.Contents = this.AllFiles.ToList();
					return;
				}

				for (index = 0; index < directory_count; index++)
				{
					entry_offset[index] = reader.ReadUInt32();
					if (entry_offset[index] < directory_tableSize)
					{
						foreach (NitroFile file in file_table)
						{
							file.GetExtension(stream);
							if (file.Extension == ".narc")
							{
								NitroArchive narc = new NitroArchive(file, stream);
								file_table[Array.IndexOf(file_table, file)] = narc;
							}
						}

						this.Warnings.Add("This archive doesn't have a File Name Table\n" +
							"This is common in the Pokemon series of games.");
						this.AllFiles = file_table.ToList();
						this.Contents = this.AllFiles.ToList();
						return;
					}
					else if (entry_offset[index] > fnt.Size - 8)
					{
						foreach (NitroFile file in file_table)
						{
							file.GetExtension(stream);
							if (file.Extension == ".narc")
							{
								NitroArchive narc = new NitroArchive(file, stream);
								file_table[Array.IndexOf(file_table, file)] = narc;
							}
						}

						this.Warnings.Add("Directory " + index + " runs out of bounds of the name table.\n" +
							"Name Table End: " + fnt.Size + "\n" +
							"Directory Name Start: " + entry_offset[index]);
						this.AllFiles = file_table.ToList();
						this.Contents = this.AllFiles.ToList();
						return;
					}

					first_file[index] = reader.ReadUInt16();
					if (first_file[index] > fat.FileCount)
					{
						this.Warnings.Add("Directory " + index + " references a file outside the bounds of the File Allocation Table.\n" +
							"Number Of Files: " + fat.FileCount + "\n" +
							"File Index: " + first_file[index]);
						this.Contents = this.AllFiles.ToList();
					}

					parent_index[index] = reader.ReadUInt16();
					if (index != 0)
					{
						if (index > 0 && parent_index[index] < 61440)
						{
							this.Warnings.Add("Directory " + index + " references a directory outside the bounds of directory table.\n" +
								"Directory Index: " + (int)(parent_index[index] - 61440));
							this.Contents = this.AllFiles.ToList();
						}

						parent_index[index] -= 61440;
						if (parent_index[index] > directory_count)
						{
							this.Warnings.Add("Directory " + index + " references a directory outside the bounds of directory table.\n" +
								"Number Of Directories: " + directory_count + "\n" +
								"Directory Index: " + parent_index[index]);
							this.Contents = this.AllFiles.ToList();
						}
					}
					else
					{
						parent_index[index] = 0;
					}
				}

				INitroDirectory[] directory_table = new INitroDirectory[directory_count];
				for (int i = 1; i < directory_count; i++)
				{
					NitroDirectory n = new NitroDirectory();
					n.Name = "Fuck You!";
					directory_table[i] = n;
				}
				directory_table[0] = this;

				int file_index = first_file[0];

				for (index = 0; index < directory_count; index++)
				{
					reader.BaseStream.Position = entry_offset[index] + fnt_offset + 8;
					INitroDirectory parent_directory = directory_table[index];
					parent_directory.Children = new List<INitroDirectory>();
					parent_directory.Contents = new List<INitroFile>();

					while ( true )
					{
						// A small sanity check to make sure we havent overrun the table.
						if ( reader.BaseStream.Position > img_offset )
						{
							break;
						}

						// The first byte in the sub-directory entry indicates how to continue.
						byte entry_byte = reader.ReadByte();

						// 0 indicates the end of a directory.
						if ( entry_byte == 0 )
						{
							break;
						}

						// 128 is actually invalid, and shouldn't be encountered. It would
						// indicate a directory with no name, which isn't actually valid.
						else if ( entry_byte == 128 )
						{
							continue;
						}

						// The first bit indicates a directory entry, so anything over 128
						// is a sub-directory. The other seven bits indicate the length of
						// the sub-directory name. We simply need to subtract 128, and the
						// next so many bytes are the sub-directory name. The following
						// two bytes are the sub-directory's ID in the main directory table.
						else if ( entry_byte > 128 )
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte - 128));
							NitroDirectory childDirectory = new NitroDirectory()
							{
								Name = name,
								Parent = parent_directory,
								Path = Parent.Path + "\\" + name,
								ID = (ushort)(reader.ReadUInt16() - 61440)
							};
							directory_table[childDirectory.ID] = childDirectory;
							parent_directory.Children.Add(childDirectory);
							parent_directory.AllDirectories.Add(childDirectory);
						}

						// Anything under 128 indicates a file. Same as the previous, the
						// other seven bits indicate the length of the file name. Unlike
						// sub-directories, there is no index proceeding the file name.
						else
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte));
							NitroFile file = file_table[file_index++] as NitroFile;
							file.Name = name;
							file.OriginalName = name;
							file.Parent = parent_directory;
							file.Path = Parent.Path + "\\" + name;

							long temp = reader.BaseStream.Position;
							
							file.GetExtension(stream);
							if (file.Extension == ".narc")
							{
								NitroArchive narc = new NitroArchive(file, stream);
								file_table[Array.IndexOf(file_table, file)] = narc;

								parent_directory.Contents.Add(narc);
								parent_directory.AllFiles.Add(narc);
							}
							else
							{
								parent_directory.Contents.Add(file);
								parent_directory.AllFiles.Add(file);
							}

							reader.BaseStream.Position = temp;
						}
					}
				}
			}
		}
	}






































	/*----------------------------- ARC File Format -----------------------------------*\

	//--- Header ---\\
	const uint fnt_offset					// 0x0		File Name Table Offset
	const uint FNTLength					// 0x4		File Name Table Length
	const uint fat_offset					// 0x8		File Allocation Table Offset
	const uint FATLength					// 0xC		File Allocation Table Length

	//--- FATB Data Block ---\\				// ?		[fat_offset]
	byte[] FATB								// 0xC		File Allocation Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FNTB Data Block ---\\				// ?		[fnt_offset]
	byte[] FNTB;							// 0x8		File Name Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FIMG Data Block ---\\				// ?		[?]
	byte[] FIMG;							// 0x8		Image File
	// Padding (Required)					// ?		[Required 4-, 8-, 16-, or 32-Byte Boundary Alignment]

	\*---------------------------------------------------------------------------------*/

	/*
	public struct NARC
	{
		public NARC(NitroFile[] file_table, NitroDirectory[] directory_table, bool is_valid)
		{
			IsValid = is_valid;
			file_table = file_table;
			directory_table = directory_table;
		}

		public bool IsValid;
		public NitroFile[] file_table;
		public NitroDirectory[] directory_table;
	}

	public static class FileHandler
	{
		public static NARC NARC(Stream stream, NitroFile narc, bool is_nitro)
		{
			NitroFile[] file_table;
			NitroDirectory[] directory_table;

			using (BinaryReader reader = new BinaryReader(stream, new UTF8Encoding(), true))
			{
				reader.BaseStream.Position = narc.Offset;

				uint fnt_offset = 0;
				uint fnt_length = 0;
				uint fat_offset = 0;
				uint fat_length = 0;
				ushort file_count = 0;

				if (is_nitro)
				{
					// Here we read the header of the file. These are constants,
					// so if any of this is wrong, it's probably a malformed NARC
					// or not a NARC at all.
					if (reader.ReadUInt32() != 1129464142
						|| reader.ReadUInt16() != 65534
						|| reader.ReadUInt16() != 256
						|| reader.ReadUInt32() != narc.Length
						|| reader.ReadUInt16() != 16
						|| reader.ReadUInt16() != 3)
					{
						return new NARC(new NitroFile[0], new NitroDirectory[0], false);
					}

					// FAT Signature Check
					if (reader.ReadUInt32() != 1178686530)
					{
						return new NARC(new NitroFile[0], new NitroDirectory[0], false);
					}

					fat_length = reader.ReadUInt32();
					file_count = reader.ReadUInt16();
					reader.BaseStream.Position += 2;

					// Another validity check ...
					if (fat_length != (file_count * 8) + 12)
					{
						return new NARC(new NitroFile[0], new NitroDirectory[0], false);
					}
				}
				else
				{
					fnt_offset = narc.Offset + reader.ReadUInt32();
					fnt_length = reader.ReadUInt32();
					fat_offset = narc.Offset + reader.ReadUInt32();
					fat_length = reader.ReadUInt32();
					file_count = Convert.ToUInt16(fat_length / 8);


					reader.BaseStream.Position = fat_offset;
				}


				file_table = new NitroFile[file_count];
				for (int i = 0; i < file_count; i++)
				{
					file_table[i] = new NitroFile();
					file_table[i].Name = "File " + i.ToString("D" + file_count.ToString().Length);
					file_table[i].ID = Convert.ToUInt32(i);
					file_table[i].Offset = reader.ReadUInt32();
					file_table[i].Length = reader.ReadUInt32() - file_table[i].Offset;
				}

				if (is_nitro)
				{
					// FNT Signature check
					if (reader.ReadUInt32() != 1179538498)
					{
						return new NARC(new NitroFile[0], new NitroDirectory[0], false);
					}

					fnt_length = reader.ReadUInt32();
					fnt_offset = narc.Offset + 16 + fat_length + 8;
				}

				
				reader.BaseStream.Position = fnt_offset + 6;
				int directory_count = reader.ReadUInt16();
				reader.BaseStream.Position = fnt_offset;


				// Setting up the root directory.
				directory_table = new NitroDirectory[directory_count];
				directory_table[0] = new NitroDirectory();
				directory_table[0].Name = narc.Name;
				directory_table[0].Path = narc.Path;

				int[] entry_offset = new int[directory_count];
				int[] first_file = new int[directory_count];
				int[] parent_index = new int[directory_count];

				reader.BaseStream.Position = fnt_offset;
				for (int i = 0; i < directory_count; i++)
				{
					entry_offset[i] = Convert.ToInt32(reader.ReadUInt32() + fnt_offset);
					first_file[i] = reader.ReadUInt16();
					parent_index[i] = reader.ReadUInt16() - 61440;
				}

				// The second section is the sub-directory table. This table is
				// a bit more complex. We start by iterating through the main
				// directory table and using the entry offset to locate its
				// position in the sub-directory table.
				int file_index = first_file[0];

				for (int i = 0; i < directory_count; i++)
				{
					// Initialize the directory arrays.
					reader.BaseStream.Position = entry_offset[i];
					NitroDirectory parent_directory = directory_table[i];
					parent_directory.Children = new List<INitroDirectory>();
					parent_directory.Contents = new List<INitroFile>();

					while (true)
					{
						// A small sanity check to make sure we havent overrun the table.
						if (reader.BaseStream.Position > fnt_offset + fnt_length)
						{
							break;
						}

						// The first byte in the sub-directory entry indicates how to continue.
						byte entry_byte = reader.ReadByte();

						// 0 indicates the end of a directory.
						if (entry_byte == 0)
						{
							break;
						}

						// 128 is actually invalid, and shouldn't be encountered. It would
						// indicate a directory with no name, which isn't actually valid.
						else if (entry_byte == 128)
						{
							continue;
						}

						// The first bit indicates a directory entry, so anything over 128
						// is a sub-directory. The other seven bits indicate the length of
						// the sub-directory name. We simply need to subtract 128, and the
						// next so many bytes are the sub-directory name. The following
						// two bytes are the sub-directory's ID in the main directory table.
						else if (entry_byte > 128)
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte - 128));
							NitroDirectory child_directory = new NitroDirectory();
							child_directory.Name = name;
							child_directory.Parent = parent_directory;
							child_directory.Path = parent_directory.Path + "\\" + child_directory.Name;
							child_directory.ID = Convert.ToUInt32(reader.ReadUInt16() - 61440);

							directory_table[child_directory.ID] = child_directory;
							parent_directory.Children.Add(child_directory);
						}

						// Anything under 128 indicates a file. Same as the previous, the
						// other seven bits indicate the length of the file name. Unlike
						// sub-directories, there is no index proceeding the file name.
						else
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte));
							NitroFile file = file_table[file_index++];
							file.Name = name;
							file.Parent = parent_directory;
							file.Path = parent_directory.Path + "\\" + file.Name;

							parent_directory.Contents.Add(file);
						}
					}
				}

				// We run this now so that we don't have to jump the memory stream a ton.
				foreach (NitroFile file in file_table)
				{
					if (file.Parent == null)
					{
						file.Parent = directory_table[0];
						file.Path = file.Parent.Path + "\\" + file.Name;
						file.Parent.Contents.Add(file);
					}
					file.Offset += narc.Offset + 16 + fat_length + fnt_length + 8;
					file.GetExtension(stream);

					if (file.Extension == ".narc")
					{
						file.NARCTables = FileHandler.NARC(stream, file, true);
					}
					else if (file.Extension == ".arc")
					{
						file.NARCTables = FileHandler.NARC(stream, file, false);
					}
				}
			}

			return new NARC(file_table, directory_table, true);
		}
	}
	*/
}