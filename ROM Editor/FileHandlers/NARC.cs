using EditorNDS.FileHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorNDS.FileHandlers
{
	/*----------------------------- NARC File Format ----------------------------------*\

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
	// Padding (Required)					// ?		[Required 4-, 8-, 16-, or 32-Byte Boundary Alignment]

	\*---------------------------------------------------------------------------------*/

	/*----------------------------- ARC File Format -----------------------------------*\

	//--- Header ---\\
	const uint FNTOffset					// 0x0		File Name Table Offset
	const uint FNTLength					// 0x4		File Name Table Length
	const uint FATOffset					// 0x8		File Allocation Table Offset
	const uint FATLength					// 0xC		File Allocation Table Length

	//--- FATB Data Block ---\\				// ?		[FATOffset]
	byte[] FATB								// 0xC		File Allocation Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FNTB Data Block ---\\				// ?		[FNTOffset]
	byte[] FNTB;							// 0x8		File Name Table
	// Padding (Optional)					// ?		[Optional Boundary Alignment]

	//--- FIMG Data Block ---\\				// ?		[?]
	byte[] FIMG;							// 0x8		Image File
	// Padding (Required)					// ?		[Required 4-, 8-, 16-, or 32-Byte Boundary Alignment]

	\*---------------------------------------------------------------------------------*/

	public struct NARC
	{
		public NARC(NDSFile[] file_table, NDSDirectory[] directory_table, bool is_valid)
		{
			IsValid = is_valid;
			FileTable = file_table;
			DirectoryTable = directory_table;
		}

		public bool IsValid;
		public NDSFile[] FileTable;
		public NDSDirectory[] DirectoryTable;
	}

	public static class FileHandler
	{
		public static NARC NARC(Stream stream, NDSFile narc, bool is_nitro)
		{
			NDSFile[] file_table;
			NDSDirectory[] directory_table;

			using (BinaryReader reader = new BinaryReader(stream, new UTF8Encoding(), true))
			{
				reader.BaseStream.Position = narc.Offset;

				uint fnt_offset = 0;
				uint fnt_length = 0;
				uint fat_offset = 0;
				uint fat_length = 0;
				ushort file_count = 0;

				if ( is_nitro )
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
						return new NARC(new NDSFile[0], new NDSDirectory[0], false);
					}

					// FAT Signature Check
					if (reader.ReadUInt32() != 1178686530)
					{
						return new NARC(new NDSFile[0], new NDSDirectory[0], false);
					}

					fat_length = reader.ReadUInt32();
					file_count = reader.ReadUInt16();
					reader.BaseStream.Position += 2;

					// Another validity check ...
					if (fat_length != (file_count * 8) + 12)
					{
						return new NARC(new NDSFile[0], new NDSDirectory[0], false);
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


				// The File Allocation Table contain eight bytes per file entry;
				// a four byte file offset followed by a four byte file length.
				// We start by determining the file count and then dividing the
				// offsets and lenghts into separate indexed arrays.
				file_table = new NDSFile[file_count];
				for (int i = 0; i < file_count; i++)
				{
					file_table[i] = new NDSFile();
					file_table[i].Name = "File " + i.ToString("D" + file_count.ToString().Length);
					file_table[i].ID = i;
					file_table[i].Offset = reader.ReadUInt32();
					file_table[i].Length = reader.ReadUInt32() - file_table[i].Offset;
				}

				if ( is_nitro )
				{
					// FNT Signature check
					if (reader.ReadUInt32() != 1179538498)
					{
						return new NARC(new NDSFile[0], new NDSDirectory[0], false);
					}

					fnt_length = reader.ReadUInt32();
					fnt_offset = narc.Offset + 16 + fat_length + 8;
				}

				// The File Name Table contains two sections; the first is the
				// main directory table. It's length is eight bytes per entry.
				// The first four bytes represent an offset to the entry in the
				// second section; the sub-directory table. This is followed by
				// two byte index corresponding to the first file entry in the
				// directory. Finally we have a two byte index corresponding to
				// the parent directory.

				// The first entry in the table is slightly different though.
				// The last two bytes in the first entry actually denote the
				// number of directories in the first section. Let's read that,
				// then use it to iterate through the main directory table and
				// split the table into three seperate indexed arrays.
				reader.BaseStream.Position = fnt_offset + 6;
				int directory_count = reader.ReadUInt16();
				reader.BaseStream.Position = fnt_offset;


				// Setting up the root directory.
				directory_table = new NDSDirectory[directory_count];
				directory_table[0] = new NDSDirectory();
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
					NDSDirectory parent_directory = directory_table[i];
					parent_directory.Children = new List<NDSDirectory>();
					parent_directory.Contents = new List<NDSFile>();

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
							NDSDirectory child_directory = new NDSDirectory();
							child_directory.Name = name;
							child_directory.Parent = parent_directory;
							child_directory.Path = parent_directory.Path + "\\" + child_directory.Name;
							child_directory.ID = reader.ReadUInt16() - 61440;

							directory_table[child_directory.ID] = child_directory;
							parent_directory.Children.Add(child_directory);
						}

						// Anything under 128 indicates a file. Same as the previous, the
						// other seven bits indicate the length of the file name. Unlike
						// sub-directories, there is no index proceeding the file name.
						else
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte));
							NDSFile file = file_table[file_index++];
							file.Name = name;
							file.Parent = parent_directory;
							file.Path = parent_directory.Path + "\\" + file.Name;

							parent_directory.Contents.Add(file);
						}
					}
				}

				// We run this now so that we don't have to jump the memory stream a ton.
				foreach (NDSFile file in file_table)
				{
					if ( file.Parent == null)
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
}
 