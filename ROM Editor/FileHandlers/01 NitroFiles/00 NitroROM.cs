/*--------------------------------------------------*\
                           |
\*--------------------------------------------------*/
/*--------------------------------------------------*\




\*--------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EditorNDS.FileHandlers
{
	public class NitroROM : BaseClass
	{
		public Stream File;
		public string WorkingFile = "";
		public string BackupFile = "";
		public List<string> Errors = new List<string>();

		// File Tables
		public NitroHeader Header;
		public NDSBanner Banner;
		public NDSBinary ARM9;
		public NDSBinary ARM7;
		public NDSBinary ARM9i;
		public NDSBinary ARM7i;
		public INitroFile[] file_table;
		public INitroDirectory[] directory_table;
		public NDSOverlay[] OverlayTable9;
		public NDSOverlay[] OverlayTable7;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public NitroROM(string file_path)
		{
			MemoryStream memory_stream = new MemoryStream();
			FileStream file_stream = new FileStream(file_path, FileMode.Open);
			file_stream.CopyTo(memory_stream);
			file_stream.Close();
			file_stream.Dispose();
			ReadROM(memory_stream);

			WorkingFile = file_path;
		}

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
			ReadHeader();

			// Secondary validity checks.
			// These are other areas that would suggest
			// an improperly formatted ROM or a file
			// that simply isn't a ROM at all.

			if (Header.HeaderSize != 16384)
			{
				read_errors.Add(
					"The 4 bytes at 132 (0x84) indicate that the header size is "
					+ Header.HeaderSize + " (0x" + Header.HeaderSize.ToString("X") + ") bytes long."
					+ " All known headers are 16384 (0x4000) bytes long,"
					+ " The header size is either incorrect or the ROM is corrupted."
					);
			}

			if (File.Length < Header.TotalSize)
			{
				read_errors.Add(
					"The ROM size defined at 128 (0x80) indicates a total size of "
					+ Header.TotalSize + "(0x" + Header.TotalSize.ToString("X") + ") bytes."
					+ "This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.ARM9Offset + Header.ARM9Length)
			{
				read_errors.Add(
					"The ARM9 is supposedly located outside the length of the actual File."
					+ " The ARM9 is supposedly located at bytes "
					+ Header.ARM9Offset + "-" + (Header.ARM9Offset + Header.ARM9Length)
					+ "(0x" + Header.ARM9Offset.ToString("X") + "-0x" + (Header.ARM9Offset + Header.ARM9Length).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.ARM7Offset + Header.ARM7Length)
			{
				read_errors.Add(
					"The ARM7 is supposedly located outside the length of the actual File."
					+ " The ARM7 is supposedly located at bytes "
					+ Header.ARM7Offset + "-" + (Header.ARM7Offset + Header.ARM7Length)
					+ "(0x" + Header.ARM7Offset.ToString("X") + "-0x" + (Header.ARM7Offset + Header.ARM7Length).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.FNTOffset + Header.FNTLength)
			{
				read_errors.Add(
					"The FNT is supposedly located outside the length of the actual File."
					+ " The FNT is supposedly located at bytes "
					+ Header.FNTOffset + "-" + (Header.FNTOffset + Header.FNTLength)
					+ "(0x" + Header.FNTOffset.ToString("X") + "-0x" + (Header.FNTOffset + Header.FNTLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.FATOffset + Header.FATLength)
			{
				read_errors.Add(
					"The FAT is supposedly located outside the length of the actual File."
					+ " The FAT is supposedly located at bytes "
					+ Header.FATOffset + "-" + (Header.FATOffset + Header.FATLength)
					+ "(0x" + Header.FATOffset.ToString("X") + "-0x" + (Header.FATOffset + Header.FATLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.ARM9OverlayOffset + Header.ARM9OverlayLength)
			{
				read_errors.Add(
					"The ARM9Overlay is supposedly located outside the length of the actual File."
					+ " The ARM9Overlay is supposedly located at bytes "
					+ Header.ARM9OverlayOffset + "-" + (Header.ARM9OverlayOffset + Header.ARM9OverlayLength)
					+ "(0x" + Header.ARM9OverlayOffset.ToString("X") + "-0x" + (Header.ARM9OverlayOffset + Header.ARM9OverlayLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			if (File.Length < Header.ARM7OverlayOffset + Header.ARM7OverlayLength)
			{
				read_errors.Add(
					"The ARM7Overlay is supposedly located outside the length of the actual File."
					+ " The ARM7Overlay is supposedly located at bytes "
					+ Header.ARM7OverlayOffset + "-" + (Header.ARM7OverlayOffset + Header.ARM7OverlayLength)
					+ "(0x" + Header.ARM7OverlayOffset.ToString("X") + "-0x" + (Header.ARM7OverlayOffset + Header.ARM7OverlayLength).ToString("X") + "."
					+ " This file is only " + File.Length + " (0x" + File.Length.ToString("X") + ") bytes long."
					);
			}

			// Finally done with (almost) all of that tedious error checking.

			Readfile_tables();

			File = null;
			stream.Dispose();
			GC.Collect();
			return read_errors;
		}

		/// <summary>
		/// Reads the ROM header from the specified stream.
		/// </summary>
		/// <param name="stream"></param>ROM file to read the header from.
		/// 
		public void ReadHeader()
		{
			using (BinaryReader reader = new BinaryReader(File, new UTF8Encoding(), true))
			{
				Header = new NitroHeader();

				reader.BaseStream.Position = 0;
				Header.GameTitle = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(10));
				reader.BaseStream.Position = 12;
				Header.GameCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));
				Header.MakerCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(2));
				Header.UnitCode = reader.ReadByte();
				Header.DeviceType = reader.ReadByte();
				Header.DeviceCapaciy = reader.ReadByte();
				Header.RegionCode = reader.ReadByte();
				Header.Version = reader.ReadByte();
				Header.InternalFlags = reader.ReadByte();
				reader.BaseStream.Position = 32;

				Header.ARM9Offset = reader.ReadUInt32();
				reader.BaseStream.Position += 4;
				Header.ARM9Load = reader.ReadUInt32();
				Header.ARM9Length = reader.ReadUInt32();

				Header.ARM7Offset = reader.ReadUInt32();
				reader.BaseStream.Position += 4;
				Header.ARM7Load = reader.ReadUInt32();
				Header.ARM7Length = reader.ReadUInt32();

				Header.FNTOffset = reader.ReadUInt32();
				Header.FNTLength = reader.ReadUInt32();
				Header.FATOffset = reader.ReadUInt32();
				Header.FATLength = reader.ReadUInt32();

				Header.ARM9OverlayOffset = reader.ReadUInt32();
				Header.ARM9OverlayLength = reader.ReadUInt32();
				Header.ARM7OverlayOffset = reader.ReadUInt32();
				Header.ARM7OverlayLength = reader.ReadUInt32();

				reader.BaseStream.Position += 8;
				Header.BannerOffset = reader.ReadUInt32();
				reader.BaseStream.Position += 8;

				Header.ARM9AutoLoad = reader.ReadUInt32();
				Header.ARM7AutoLoad = reader.ReadUInt32();
				reader.BaseStream.Position += 8;

				Header.TotalSize = reader.ReadUInt32();
				Header.HeaderSize = reader.ReadUInt32();

				Header.ARM9AutoParam = reader.ReadUInt32();
				Header.ARM7AutoParam = reader.ReadUInt32();

				if (Header.UnitCode > 0)
				{
					reader.BaseStream.Position = 448;

					Header.ARM9iOffset = reader.ReadUInt32();
					reader.BaseStream.Position += 4;
					Header.ARM9iLoad = reader.ReadUInt32();
					Header.ARM9iLength = reader.ReadUInt32();
					Header.ARM7iOffset = reader.ReadUInt32();
					reader.BaseStream.Position += 4;
					Header.ARM7iLoad = reader.ReadUInt32();
					Header.ARM7iLength = reader.ReadUInt32();
				}
			}
		}

		/// <summary>
		/// Reads the File Name Table, File Allocation Table, Overlay Tables, and NARC file tables from the current ROM file.<para />
		/// Currently Supports: File Name Table (FNT); File Allocation Table (FAT); Overlay Tables (OVT); NARC.
		/// </summary>
		public void Readfile_tables()
		{
			if (Header.FATOffset == 0 || Header.FATLength == 0 ||
				Header.FNTOffset == 0 || Header.FNTLength == 0)
			{
				return;
			}

			// We initiate a BinaryReader with using() and set it to leave the stream open after disposal.
			using (BinaryReader reader = new BinaryReader(File, new UTF8Encoding(), true))
			{
				// The File Allocation Table contain eight bytes per file entry;
				// a four byte file offset followed by a four byte file length.
				// We start by determining the file count and then dividing the
				// offsets and lenghts into separate indexed arrays.
				int file_count = Convert.ToInt32(Header.FATLength) / 8;
				file_table = new INitroFile[file_count];

				reader.BaseStream.Position = Header.FATOffset;
				for (int i = 0; i < file_count; i++)
				{
					file_table[i] = new NitroFile();
					file_table[i].ID = Convert.ToUInt32(i); ;
					file_table[i].Offset = reader.ReadUInt32();
					file_table[i].Length = reader.ReadUInt32() - file_table[i].Offset;
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
				reader.BaseStream.Position = Header.FNTOffset + 6;

				int directory_count = reader.ReadUInt16();

				directory_table = new NitroDirectory[directory_count];
				int[] entry_offset = new int[directory_count];
				int[] first_file = new int[directory_count];
				int[] parent_index = new int[directory_count];

				reader.BaseStream.Position = Header.FNTOffset;
				for (int i = 0; i < directory_count; i++)
				{
					entry_offset[i] = Convert.ToInt32(reader.ReadUInt32() + Header.FNTOffset);
					first_file[i] = reader.ReadUInt16();
					parent_index[i] = reader.ReadUInt16() - 61440;
				}

				// Setting up the root directory.
				directory_table[0] = new NitroDirectory();
				directory_table[0].Name = "Root";
				directory_table[0].Path = "Root";

				// The second section is the sub-directory table. This table is
				// a bit more complex. We start by iterating through the main
				// directory table and using the entry offset to locate its
				// position in the sub-directory table.
				int file_index = first_file[0];

				for (int i = 0; i < directory_count; i++)
				{
					// Initialize the directory arrays.
					reader.BaseStream.Position = entry_offset[i];
					INitroDirectory parent_directory = directory_table[i];
					parent_directory.Children = new List<INitroDirectory>();
					parent_directory.Contents = new List<INitroFile>();

					while (true)
					{
						// A small sanity check to make sure we havent overrun the table.
						if (reader.BaseStream.Position > Header.FNTOffset + Header.FNTLength)
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
							NitroDirectory child_directory = new NitroDirectory()
							{
								Name = name,
								Parent = parent_directory,
								Path = parent_directory.Path + "\\" + name,
								ID = Convert.ToUInt32(reader.ReadUInt16() - 61440)
							};
							directory_table[child_directory.ID] = child_directory;
							parent_directory.Children.Add(child_directory);
							parent_directory.AllDirectories.Add(child_directory);

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
							file.Path = parent_directory.Path + "\\" + file.Name;

							long temp = reader.BaseStream.Position;

							file.GetExtension(File);
							if (file.Extension == ".narc")
							{
								NitroArchive narc = new NitroArchive(file, File);
								file_table[Array.IndexOf(file_table, file)] = narc;

								parent_directory.Contents.Add(narc);
							}
							else
							{
								parent_directory.Contents.Add(file);
							}

							reader.BaseStream.Position = temp;
						}
					}
				}

				// Banner Represented As A File
				Banner = new NDSBanner();
				Banner.Name = "Banner";
				Banner.Offset = Header.BannerOffset;
				Banner.Length = 9216;
				Banner.ReadBanner(File);

				// ARM9 and ARM9 Overlay Table reading.
				ARM9 = new NDSBinary();
				ARM9.Name = "ARM9";
				ARM9.Extension = ".bin";
				ARM9.Offset = Header.ARM9Offset;
				ARM9.Length = Header.ARM9Length;
				ARM9.Load = Header.ARM9Load;
				ARM9.AutoLoad = Header.ARM9AutoLoad;
				ARM9.AutoParams = Header.ARM9AutoParam;

				if (Header.ARM9iLength > 0)
				{
					ARM9i = new NDSBinary();
					ARM9i.Name = "ARM9i";
					ARM9i.Extension = ".bin";
					ARM9i.Offset = Header.ARM9iOffset;
					ARM9i.Length = Header.ARM9iLength;
				}

				int overlay9_count = Convert.ToInt32(Header.ARM9OverlayLength / 32);
				reader.BaseStream.Position = Header.ARM9OverlayOffset;
				OverlayTable9 = new NDSOverlay[overlay9_count];

				for (int i = 0; i < overlay9_count; i++)
				{
					NDSOverlay overlay = new NDSOverlay();
					overlay.OverlayID = reader.ReadUInt32();
					overlay.AddressRAM = reader.ReadUInt32();
					overlay.SizeRAM = reader.ReadUInt32();
					overlay.SizeBSS = reader.ReadUInt32();
					overlay.StaticStartAddress = reader.ReadUInt32();
					overlay.StaticEndAddress = reader.ReadUInt32();
					overlay.ID =reader.ReadUInt32();
					overlay.Offset = file_table[overlay.ID].Offset;
					overlay.Length = file_table[overlay.ID].Length;
					uint temp = reader.ReadUInt32();
					uint flag = temp >> 24;
					if ((flag & 2) != 0)
					{
						overlay.Authenticated = true;
					}
					if ((flag & 1) != 0)
					{
						overlay.Compressed = true;
						overlay.CompressedSize = (temp << 8) >> 8;
					}

					overlay.Name = "Overlay " + overlay.OverlayID;
					overlay.Path = "ARM9 Overlay Table\\" + overlay.Name;
					overlay.Extension = ".bin";

					OverlayTable9[i] = overlay;
					file_table[overlay.ID] = overlay;
				}

				// ARM7 and ARM7 Overlay Table reading.
				if (Header.ARM7Length > 0)
				{
					ARM7 = new NDSBinary();
					ARM7.Name = "ARM7";
					ARM7.Extension = ".bin";
					ARM7.Offset = Header.ARM7Offset;
					ARM7.Length = Header.ARM7Length;
					ARM7.Load = Header.ARM7Load;
					ARM7.AutoLoad = Header.ARM7AutoLoad;
					ARM7.AutoParams = Header.ARM7AutoParam;
				}
				if (Header.ARM7iLength > 0)
				{
					ARM7i = new NDSBinary();
					ARM7i.Name = "ARM7i";
					ARM7i.Extension = ".bin";
					ARM7i.Offset = Header.ARM7iOffset;
					ARM7i.Length = Header.ARM7Length;
				}

				int overlay7_count = Convert.ToInt32(Header.ARM7OverlayLength / 32);
				OverlayTable7 = new NDSOverlay[overlay7_count];
				if (overlay7_count > 0)
				{
					reader.BaseStream.Position = Header.ARM7OverlayOffset;

					for (int i = 0; i < overlay7_count; i++)
					{
						NDSOverlay overlay = new NDSOverlay();
						overlay.OverlayID = reader.ReadUInt32();
						overlay.AddressRAM = reader.ReadUInt32();
						overlay.SizeRAM = reader.ReadUInt32();
						overlay.SizeBSS = reader.ReadUInt32();
						overlay.StaticStartAddress = reader.ReadUInt32();
						overlay.StaticEndAddress = reader.ReadUInt32();
						overlay.ID = reader.ReadUInt32();
						overlay.Offset = file_table[overlay.ID].Offset;
						overlay.Length = file_table[overlay.ID].Length;
						uint temp = reader.ReadUInt32();
						uint flag = temp >> 24;
						if ((flag & 2) != 0)
						{
							overlay.Authenticated = true;
						}
						if ((flag & 1) != 0)
						{
							overlay.Compressed = true;
							overlay.CompressedSize = (temp << 8) >> 8;
						}

						overlay.Name = "Overlay " + overlay.OverlayID;
						overlay.Path = "ARM9 Overlay Table\\" + overlay.Name;
						overlay.Extension = ".bin";

						OverlayTable7[i] = overlay;
						file_table[overlay.ID] = overlay;
					}
				}
			}
		}

		void WriteHashTables()
		{

		}

		void ReadHashTables()
		{

		}
	}
}
