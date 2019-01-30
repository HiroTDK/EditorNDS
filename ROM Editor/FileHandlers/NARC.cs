using EditorNDS.FileHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorNDS.FileHandlers
{
	public class NARC : EditorNDS.BaseClass
	{
		public NARC(Stream stream, NDSFile narc)
		{
			stream.Position = narc.Offset;
			byte[] bytes = new byte[narc.Length];
			stream.Read(bytes, 0, narc.Length);

			try
			{
				if ( BitConverter.ToUInt32(bytes, 0) != Signature
					|| BitConverter.ToUInt16(bytes, 4) != ByteOrder
					|| BitConverter.ToUInt16(bytes, 6) != Version
					|| BitConverter.ToUInt32(bytes, 8) != bytes.Count()
					|| BitConverter.ToUInt16(bytes, 12) != HeaderSize
					|| BitConverter.ToUInt16(bytes, 14) != DataBlocks )
				{
					Console.WriteLine(narc.Path + narc.Name + narc.Extension + " has a malformed header.");
				}

				SizeFAT = BitConverter.ToUInt32(bytes, HeaderSize + 4);
				FilesFAT = BitConverter.ToUInt16(bytes, HeaderSize + 8);

				if ( BitConverter.ToUInt32(bytes, 16) != SignatureFAT
					|| SizeFAT != ( FilesFAT * 8 ) + 12 )
				{
					Console.WriteLine(narc.Path + narc.Name + narc.Extension + " has a malformed file allocation table data block.");
				}

				SizeFNT = BitConverter.ToUInt32(bytes, HeaderSize + Convert.ToInt32(SizeFAT) + 4);

				if ( BitConverter.ToUInt32(bytes, HeaderSize + Convert.ToInt32(SizeFAT)) != SignatureFNT )
				{
					Console.WriteLine(narc.Path + narc.Name + narc.Extension + " has a malformed file name table data block.");
				}

				SizeIMG = BitConverter.ToUInt32(bytes, HeaderSize + Convert.ToInt32(SizeFAT + SizeFNT) + 4);

				if ( BitConverter.ToInt32(bytes, HeaderSize + Convert.ToInt32(SizeFAT + SizeFNT)) != SignatureIMG )
				{
					Console.WriteLine(narc.Path + narc.Name + narc.Extension + " has a malformed file image data block.");
				}
			}
			catch
			{
				Console.WriteLine(narc.Path + narc.Name + narc.Extension + " is not a valid NARC file.");
				isValid = false;
				return;
			}

			byte[] fatArray = new byte[SizeFAT - 12];
			byte[] fntArray = new byte[SizeFNT - 8];

			Array.Copy(bytes, HeaderSize + 12, fatArray, 0, SizeFAT - 12);
			Array.Copy(bytes, HeaderSize + Convert.ToInt32(SizeFAT) + 8, fntArray, 0, SizeFNT - 8);

			int directoryCount = BitConverter.ToUInt16(fntArray, 6);
			string[] directories = new string[directoryCount];
			directories[0] = narc.Path + "\\" + narc.Name + ".narc";

			NDSFile[] files = new NDSFile[FilesFAT];

			for ( int i = 0; i < FilesFAT; i++ )
			{
				NDSFile file = new NDSFile();
				NDSFile currentFile = file;
				file.Name = "File "+ i.ToString("D" + FilesFAT.ToString().Length);
				file.Path = directories[0];
				file.Offset = Convert.ToInt32(BitConverter.ToUInt32(fatArray, i * 8));
				file.Length = Convert.ToInt32(BitConverter.ToUInt32(fatArray, i * 8 + 4)) - file.Offset;
				file.Offset += narc.Offset + Convert.ToInt32(HeaderSize + SizeFAT + SizeFNT) + 8;
				files[i] = file;
			}

			int unnamedCount = 0;
			for ( int i = 0; i < directoryCount; i++ )
			{
				int entryPos = Convert.ToInt32(BitConverter.ToUInt32(fntArray, i * 8));
				int fileIndex = Convert.ToInt32(BitConverter.ToUInt16(fntArray, ( i * 8 ) + 4));

				while ( true )
				{
					byte entryByte = fntArray[entryPos++];

					if ( entryByte == 0 )
					{
						break;
					}

					else if ( entryByte == 128 )
					{
						int index = BitConverter.ToUInt16(fntArray, entryPos) - 61440;
						directories[index] = directories[i] + "\\Unnamed " + unnamedCount++;
						entryPos += 2;
					}

					else if ( entryByte > 128 )
					{
						int index = BitConverter.ToUInt16(fntArray, ( entryPos ) + ( entryByte - 128 )) - 61440;
						directories[index] = directories[i] + "\\" + System.Text.Encoding.UTF8.GetString(fntArray, entryPos, entryByte - 128);
						entryPos += ( entryByte - 128 ) + 2;
					}

					else
					{
						files[fileIndex].Name = System.Text.Encoding.UTF8.GetString(fntArray, entryPos, entryByte);
						files[fileIndex].Path = narc.Path + directories[i];
						fileIndex++;
						entryPos += entryByte;
					}
				}
			}

			dirList = directories.ToList();
			fileList = files.ToList();
			foreach (NDSFile file in fileList )
			{
				file.GetExtension(stream);
			}

			bytes = null;
			fatArray = null;
			fntArray = null;
			directories = null;
			files = null;
		}
		

		// Validity Flag
		public bool isValid = true;
		public List<string> dirList;
		public List<NDSFile> fileList;


		// Header Data
		const uint Signature = 1129464142;		// 0x0		File Signature (N,A,R,C)
		const ushort ByteOrder = 65534;			// 0x4		Byte Order Marks (0xFEFF)
		const ushort Version = 256;             // 0x6		Archive Format Version (0x0100)
		//uint FileSize = 0;					// 0x8		Archive File Size
		const ushort HeaderSize = 16;			// 0xC		Archive Header Size (0x10)
		const ushort DataBlocks = 3;			// 0xE		Number Of Data Blocks (0x3)

		// FATB Data Block						// ?		[HeaderSize]
		const uint SignatureFAT = 1178686530;	// 0x0		Data Block Type (FATB)
		uint SizeFAT = 0;                       // 0x4		Allocation Table Size
		ushort FilesFAT = 0;                    // 0x8		Number Of Files In Allocation Table
		// Reserved [FAT]						// 0xA		Reserved (0x0000)
		// FATB									// 0xC		File Allocation Table

		// FNTB Data Block						// ?		[HeaderSize + SizeFAT]
		const uint SignatureFNT = 1179538498;	// 0x0		Data Block Type (FNTB)
		uint SizeFNT = 0;                       // 0x4		Name Table Size
		// FNTB;								// 0x8		File Name Table
		// Padding (Optional)					// ?		[Optional Boundary Alignment]

		// FIMG Data Block						// ?		[HeaderSize + SizeFAT + SizeFNT + Padding]
		const uint SignatureIMG = 1179209031;	// 0x0		Data Block Type (FIMG)
		uint SizeIMG = 0;                       // 0x4		Image File Size
		//byte[] FIMG;                          // 0x8		Image File
		// Padding (Required)					// ?		[Required 4-, 8-, 16-, or 32-Byte Boundary Alignment]
	}
}