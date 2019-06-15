using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EditorROM.FileHandlers;

namespace EditorNDS.FileHandlers
{
	class NDSROM : BaseClass
	{
		public List<string> Errors;

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

		// ROM Control Parameters [A]
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

		public NDSROM(Stream file)
		{
			Errors = new List<string>();

			// Primary validity check.
			// This is the smallest possible size that
			// required for a vaild NDS ROM header.
			if (file.Length < 352)
			{
				Errors.Add(
					"This file isn't long enough to define a proper header."
					+ " The smallest possible header is 352 bytes."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);

				CustomMessageBox.Show("Error", Errors.Last());
				return;
			}

			if (file.Length < 16384)
			{
				Errors.Add(
					"This file isn't long enough to define a proper header."
					+ " All known headers are 16384 (0x4000) bytes long,"
					+ " There's either some hijinx or the ROM is corrupted."
					);

				CustomMessageBox.Show("Error", Errors.Last());

				Header = new NDSHeader(file);
			}
			else
			{
				Header = new NDSHeader(file);
			}

			// Secondary validity checks.
			// These are other areas that would suggest
			// an improperly formatted ROM or a file
			// that simply isn't a ROM at all.
			
			if (Header.HeaderSize != 16384)
			{
				Errors.Add(
					"The 4 bytes at 132 (0x84) indicate that the header size is "
					+ Header.HeaderSize + " (0x" + Header.HeaderSize.ToString("X") + ") bytes long."
					+ " All known headers are 16384 (0x4000) bytes long,"
					+ " The header size is either incorrect or the ROM is corrupted."
					);

				CustomMessageBox.Show("Error", Errors.Last());
			}

			if (file.Length < Header.TotalSize)
			{
				Errors.Add(
					"The ROM size defined at 128 (0x80) indicates a total size of "
					+ Header.TotalSize + "(0x" + Header.TotalSize.ToString("X") + ") bytes."
					+ "This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);

				CustomMessageBox.Show("Error", Errors.Last());
			}

			if (file.Length < Header.ARM9Offset + Header.ARM9Length)
			{
				Errors.Add(
					"The ARM9 is supposedly located outside the length of the actual file."
					+ " The ARM9 is supposedly located at bytes "
					+ Header.ARM9Offset + "-" + (Header.ARM9Offset + Header.ARM9Length)
					+ "(0x" + Header.ARM9Offset.ToString("X") + "-0x" + (Header.ARM9Offset + Header.ARM9Length).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			if (file.Length < Header.ARM7Offset + Header.ARM7Length)
			{
				Errors.Add(
					"The ARM7 is supposedly located outside the length of the actual file."
					+ " The ARM7 is supposedly located at bytes "
					+ Header.ARM7Offset + "-" + (Header.ARM7Offset + Header.ARM7Length)
					+ "(0x" + Header.ARM7Offset.ToString("X") + "-0x" + (Header.ARM7Offset + Header.ARM7Length).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			if (file.Length < Header.FNTOffset + Header.FNTLength)
			{
				Errors.Add(
					"The FNT is supposedly located outside the length of the actual file."
					+ " The FNT is supposedly located at bytes "
					+ Header.FNTOffset + "-" + (Header.FNTOffset + Header.FNTLength)
					+ "(0x" + Header.FNTOffset.ToString("X") + "-0x" + (Header.FNTOffset + Header.FNTLength).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			if (file.Length < Header.FATOffset + Header.FATLength)
			{
				Errors.Add(
					"The FAT is supposedly located outside the length of the actual file."
					+ " The FAT is supposedly located at bytes "
					+ Header.FATOffset + "-" + (Header.FATOffset + Header.FATLength)
					+ "(0x" + Header.FATOffset.ToString("X") + "-0x" + (Header.FATOffset + Header.FATLength).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			if (file.Length < Header.ARM9OverlayOffset + Header.ARM9OverlayLength)
			{
				Errors.Add(
					"The ARM9Overlay is supposedly located outside the length of the actual file."
					+ " The ARM9Overlay is supposedly located at bytes "
					+ Header.ARM9OverlayOffset + "-" + (Header.ARM9OverlayOffset + Header.ARM9OverlayLength)
					+ "(0x" + Header.ARM9OverlayOffset.ToString("X") + "-0x" + (Header.ARM9OverlayOffset + Header.ARM9OverlayLength).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			if (file.Length < Header.ARM7OverlayOffset + Header.ARM7OverlayLength)
			{
				Errors.Add(
					"The ARM7Overlay is supposedly located outside the length of the actual file."
					+ " The ARM7Overlay is supposedly located at bytes "
					+ Header.ARM7OverlayOffset + "-" + (Header.ARM7OverlayOffset + Header.ARM7OverlayLength)
					+ "(0x" + Header.ARM7OverlayOffset.ToString("X") + "-0x" + (Header.ARM7OverlayOffset + Header.ARM7OverlayLength).ToString("X") + "."
					+ " This file is only " + file.Length + " (0x" + file.Length.ToString("X") + ") bytes long."
					);
			}

			// Finally done with all that tedious error checking.
		}


	}
}
