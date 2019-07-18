using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EditorROM.FileHandlers;

namespace EditorNDS.FileHandlers
{
	public class NDSROM : BaseClass
	{
		public Stream File;
		public string WorkingFile = "";
		public string BackupFile = "";
		public List<string> Errors = new List<string>();

		// Begin Header
		public string GameTitle;                // 0x00		12 Characters; ASCII Codes 0x20-0x5F; 0x20 For Spaces; 0x00 For Padding
		public string GameCode;                 // 0x0C		Game-Specific 4-Digit Code
		public string MakerCode;                // 0x10		2-Digit License Code Assigned By Nintendo

		// Hardware/Software Information
		public byte UnitCode;                   // 0x12		Intended Hardware: 0x0 for NDS; 0x2 For NDS+DSi; 0x3 For DSi
		public byte DeviceType;                 // 0x13		Type Of Device Mounted Inside Cartridge
		public byte DeviceCapaciy;              // 0x14		0x0-0x9: Megabits [1|2|4|8|16|32|64|128|256|512]; 0xA-0xF: Gigabits [1|2|4|8|16|32]
		// Reserved [A]							// 0x15		Reserved [0x8]
		public byte RegionCode;                 // 0x1D		0x80 For China; 0x40 For Korea; 0x00 For Others
		public byte Version;                    // 0x1E		Also Referred To As "RemasterVersion"
		public byte InternalFlags;              // 0x1F		Auto-Load (?)

		// ARM9
		public uint ARM9Offset;                 // 0x20		ARM9 Source Address
		public uint ARM9Entry;                  // 0x24		ARM9 Execution Start Address
		public uint ARM9Load;                   // 0x28		ARM9 RAM Transfer Destination Address
		public uint ARM9Length;                 // 0x2C		ARM9 Size

		// ARM7
		public uint ARM7Offset;                 // 0x30		ARM7 Source Address
		public uint ARM7Entry;                  // 0x34		ARM7 Execution Start Address
		public uint ARM7Load;                   // 0x38		ARM7 RAM Transfer Destination Address
		public uint ARM7Length;                 // 0x3C		ARM7 Size

		// File System
		public uint FNTOffset;                  // 0x40		File Name Table Address
		public uint FNTLength;                  // 0x44		File Name Table Size
		public uint FATOffset;                  // 0x48		File Allocation Table Address
		public uint FATLength;                  // 0x4C		File Allocation Table Size

		// Overlay Tables
		public uint ARM9OverlayOffset;          // 0x50		ARM9 Overlay Table Address
		public uint ARM9OverlayLength;          // 0x54		ARM9 Overlay Table Size
		public uint ARM7OverlayOffset;          // 0x58		ARM7 Overlay Table Address
		public uint ARM7OverlayLength;          // 0x5C		ARM7 Overlay Table Size

		// ROM Control Parameters [A]s
		public uint PortNormal;                 // 0x60		Port 40001A4h For Normal Commands
		public uint PortKEY1;                   // 0x64		Port 40001A4h For KEY1 Commands

		// Icon/Title Offset
		public uint BannerOffset;               // 0x68		Banner File Address

		// ROM Control Parameters [B]
		public ushort SecureCRC;                // 0x6C		Secure Region CRC
		public ushort SecureTimeout;            // 0x6E		Secure Transfer Timeout

		// ARM Auto-Load List
		public uint ARM9AutoLoad;               // 0x70		ARM9 Auto-Load Hook Address
		public uint ARM7AutoLoad;               // 0x74		ARM7 Auto-Load Hook Address

		// ROM Control Parameters [C]
		public ulong SecureDisable;             // 0x78		Secure Region Disable

		// ROM Size
		public uint TotalSize;                  // 0x80		Offset At ROM's End
		public uint HeaderSize;                 // 0x84		Offset At Header's End

		// ARM Auto-Load Parameters
		public uint ARM9AutoParam;              // 0x88		ARM9 Auto-Load Parameters Address
		public uint ARM7AutoParam;              // 0x8C		ARM7 Auto-Load Parameters Address

		// Nintendo Logo						// 0xC0		Nintendo Logo Image [0x9C]
		public ushort NintendoLogoCRC;          // 0x15C	Nintendo Logo CRC
		public ushort HeaderCRC;                // 0x15E	Header CRC
		// Reserved [C]							// 0x160	Reserved For Debugging [0x20]

		// File Tables
		public NDSFile ARM9;
		public NDSFile ARM7;
		public NDSFile[] FileTable;
		public NDSDirectory[] DirectoryTable;
		public List<NDSFile> OverlayTableARM9;
		public List<NDSFile> OverlayTableARM7;

		/// <summary>
		/// Reads the specified ROM file and breaks down all the pertinent information.
		/// </summary>
		/// <returns>Errors as a list of strings.</returns>
		public List<string> ReadROM(Stream stream)
		{
			List<string> read_errors = new List<string>();

			File = stream;
			if (File == null)
			{
				read_errors.Add(
					"No ROM file specified. There is nothing to read."
					);

				CustomMessageBox.Show("Error", Errors.Last());
				return read_errors;
			}

			// Primary validity check.
			// This is the smallest possible size that
			// required for a vaild NDS ROM header.
			if (File.Length < 352)
			{
				read_errors.Add(
					"This file isn't long enough to define a proper header."
					+ " The smallest possible header is 352 bytes."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);

				CustomMessageBox.Show("Error", Errors.Last());
				return read_errors;
			}

			if (File.Length < 16384)
			{
				read_errors.Add(
					"This file isn't long enough to define a proper header."
					+ " All known headers are 16384 (0x4000) bytes long,"
					+ " There's either some hijinx or the ROM is corrupted."
					);
			}

			// With those out of the way, we can read the header data.
			ReadHeader(stream);

			// Secondary validity checks.
			// These are other areas that would suggest
			// an improperly formatted ROM or a file
			// that simply isn't a ROM at all.

			if (HeaderSize != 16384)
			{
				read_errors.Add(
					"The 4 bytes at 132 (0x84) indicate that the header size is "
					+ HeaderSize + " (0x" + HeaderSize.ToString("X") + ") bytes long."
					+ " All known headers are 16384 (0x4000) bytes long,"
					+ " The header size is either incorrect or the ROM is corrupted."
					);
			}

			if (File.Length < TotalSize)
			{
				read_errors.Add(
					"The ROM size defined at 128 (0x80) indicates a total size of "
					+ TotalSize + "(0x" + TotalSize.ToString("X") + ") bytes."
					+ "This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < ARM9Offset + ARM9Length)
			{
				read_errors.Add(
					"The ARM9 is supposedly located outside the length of the actual File."
					+ " The ARM9 is supposedly located at bytes "
					+ ARM9Offset + "-" + (ARM9Offset + ARM9Length)
					+ "(0x" + ARM9Offset.ToString("X") + "-0x" + (ARM9Offset + ARM9Length).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < ARM7Offset + ARM7Length)
			{
				read_errors.Add(
					"The ARM7 is supposedly located outside the length of the actual File."
					+ " The ARM7 is supposedly located at bytes "
					+ ARM7Offset + "-" + (ARM7Offset + ARM7Length)
					+ "(0x" + ARM7Offset.ToString("X") + "-0x" + (ARM7Offset + ARM7Length).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < FNTOffset + FNTLength)
			{
				read_errors.Add(
					"The FNT is supposedly located outside the length of the actual File."
					+ " The FNT is supposedly located at bytes "
					+ FNTOffset + "-" + (FNTOffset + FNTLength)
					+ "(0x" + FNTOffset.ToString("X") + "-0x" + (FNTOffset + FNTLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < FATOffset + FATLength)
			{
				read_errors.Add(
					"The FAT is supposedly located outside the length of the actual File."
					+ " The FAT is supposedly located at bytes "
					+ FATOffset + "-" + (FATOffset + FATLength)
					+ "(0x" + FATOffset.ToString("X") + "-0x" + (FATOffset + FATLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < ARM9OverlayOffset + ARM9OverlayLength)
			{
				read_errors.Add(
					"The ARM9Overlay is supposedly located outside the length of the actual File."
					+ " The ARM9Overlay is supposedly located at bytes "
					+ ARM9OverlayOffset + "-" + (ARM9OverlayOffset + ARM9OverlayLength)
					+ "(0x" + ARM9OverlayOffset.ToString("X") + "-0x" + (ARM9OverlayOffset + ARM9OverlayLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < ARM7OverlayOffset + ARM7OverlayLength)
			{
				read_errors.Add(
					"The ARM7Overlay is supposedly located outside the length of the actual File."
					+ " The ARM7Overlay is supposedly located at bytes "
					+ ARM7OverlayOffset + "-" + (ARM7OverlayOffset + ARM7OverlayLength)
					+ "(0x" + ARM7OverlayOffset.ToString("X") + "-0x" + (ARM7OverlayOffset + ARM7OverlayLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			// Finally done with (almost) all of that tedious error checking.

			ReadFileTables(stream);

			File = null;
			stream.Dispose();
			GC.Collect();
			return read_errors;
		}
		
		/// <summary>
		/// Reads the ROM header from the current ROM file.
		/// </summary>
		/// 
		public void ReadHeader()
		{
			ReadHeader(File);
		}

		/// <summary>
		/// Reads the ROM header from the specified stream.
		/// </summary>
		/// <param name="stream"></param>ROM file to read the header from.
		/// 
		public void ReadHeader(Stream stream)
		{
			using (BinaryReader reader = new BinaryReader(stream, new UTF8Encoding(), true))
			{
				reader.BaseStream.Position = 0;
				GameTitle = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(10));
				reader.BaseStream.Position = 12;
				GameCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));
				MakerCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(2));
				UnitCode = reader.ReadByte();
				DeviceType = reader.ReadByte();
				DeviceCapaciy = reader.ReadByte();
				reader.BaseStream.Position = 29;
				RegionCode = reader.ReadByte();
				Version = reader.ReadByte();
				InternalFlags = reader.ReadByte();
				ARM9Offset = reader.ReadUInt32();
				ARM9Entry = reader.ReadUInt32();
				ARM9Load = reader.ReadUInt32();
				ARM9Length = reader.ReadUInt32();
				ARM7Offset = reader.ReadUInt32();
				ARM7Entry = reader.ReadUInt32();
				ARM7Load = reader.ReadUInt32();
				ARM7Length = reader.ReadUInt32();
				FNTOffset = reader.ReadUInt32();
				FNTLength = reader.ReadUInt32();
				FATOffset = reader.ReadUInt32();
				FATLength = reader.ReadUInt32();
				ARM9OverlayOffset = reader.ReadUInt32();
				ARM9OverlayLength = reader.ReadUInt32();
				ARM7OverlayOffset = reader.ReadUInt32();
				ARM7OverlayLength = reader.ReadUInt32();
				PortNormal = reader.ReadUInt32();
				PortKEY1 = reader.ReadUInt32();
				BannerOffset = reader.ReadUInt32();
				SecureCRC = reader.ReadUInt16();
				SecureTimeout = reader.ReadUInt16();
				ARM9AutoLoad = reader.ReadUInt32();
				ARM7AutoLoad = reader.ReadUInt32();
				SecureDisable = reader.ReadUInt64();
				TotalSize = reader.ReadUInt32();
				HeaderSize = reader.ReadUInt32();
				ARM9AutoParam = reader.ReadUInt32();
				ARM7AutoParam = reader.ReadUInt32();
				reader.BaseStream.Position = 348;
				NintendoLogoCRC = reader.ReadUInt16();
				HeaderCRC = reader.ReadUInt16();
			}
		}

		/// <summary>
		/// Reads the File Name Table, File Allocation Table, Overlay Tables, and NARC file tables from the current ROM file.<para />
		/// Currently Supports: File Name Table (FNT); File Allocation Table (FAT); Overlay Tables (OVT); NARC.
		/// </summary>
		/// 
		public void ReadFileTables()
		{
			ReadFileTables(File);
		}

		/// <summary>
		/// Reads and breaks down the file tables from the specified stream.<para />
		/// Currently Supports: File Name Table (FNT); File Allocation Table (FAT); Overlay Tables (OVT); NARC.
		/// </summary>
		/// <param name="stream">ROM file to read tables from.</param>
		/// 
		public void ReadFileTables(Stream stream)
		{
			// We initiate a BinaryReader with using() and set it to leave the stream open after disposal.
			using (BinaryReader reader = new BinaryReader(stream, new UTF8Encoding(), true))
			{
				// The File Allocation Table contain eight bytes per file entry;
				// a four byte file offset followed by a four byte file length.
				// We start by determining the file count and then dividing the
				// offsets and lenghts into separate indexed arrays.
				int file_count = Convert.ToInt32(FATLength) / 8;
				FileTable = new NDSFile[file_count];

				reader.BaseStream.Position = FATOffset;
				for (int i = 0; i < file_count; i++)
				{
					FileTable[i] = new NDSFile();
					FileTable[i].ID = i;
					FileTable[i].Offset = reader.ReadUInt32();
					FileTable[i].Length = reader.ReadUInt32() - FileTable[i].Offset;
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
				reader.BaseStream.Position = FNTOffset + 6;

				int directory_count = reader.ReadUInt16();

				DirectoryTable = new NDSDirectory[directory_count];
				int[] entry_offset = new int[directory_count];
				int[] first_file = new int[directory_count];
				int[] parent_index = new int[directory_count];

				reader.BaseStream.Position = FNTOffset;
				for (int i = 0; i < directory_count; i++)
				{
					entry_offset[i] = Convert.ToInt32(reader.ReadUInt32() + FNTOffset);
					first_file[i] = reader.ReadUInt16();
					parent_index[i] = reader.ReadUInt16() - 61440;
				}

				// Setting up the root directory.
				DirectoryTable[0] = new NDSDirectory();
				DirectoryTable[0].Name = "Root";
				DirectoryTable[0].Path = "Root";

				// The second section is the sub-directory table. This table is
				// a bit more complex. We start by iterating through the main
				// directory table and using the entry offset to locate its
				// position in the sub-directory table.
				int file_index = first_file[0];

				for (int i = 0; i < directory_count; i++)
				{
					// Initialize the directory arrays.
					reader.BaseStream.Position = entry_offset[i];
					NDSDirectory parent_directory = DirectoryTable[i];
					parent_directory.Children = new List<NDSDirectory>();
					parent_directory.Contents = new List<NDSFile>();

					while (true)
					{
						// A small sanity check to make sure we havent overrun the table.
						if (reader.BaseStream.Position > FNTOffset + FNTLength)
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

							DirectoryTable[child_directory.ID] = child_directory;
							parent_directory.Children.Add(child_directory);
						}

						// Anything under 128 indicates a file. Same as the previous, the
						// other seven bits indicate the length of the file name. Unlike
						// sub-directories, there is no index proceeding the file name.
						else
						{
							string name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(entry_byte));
							NDSFile file = FileTable[file_index++];
							file.Name = name;
							file.Parent = parent_directory;
							file.Path = parent_directory.Path + "\\" + file.Name;

							parent_directory.Contents.Add(file);
						}
					}
				}

				// We run this now so that we don't mess up the memory stream.
				foreach (NDSFile file in FileTable)
				{
					file.GetExtension(stream);

					if ( file.Extension == ".narc" )
					{
						file.NARCTables = FileHandler.NARC(stream, file);
					}
				}
			}

		}

		public NDSROM(Stream stream)
		{
			ReadROM(stream);
		}
	}
}
