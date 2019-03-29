using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EditorROM.FileHandlers;

namespace EditorNDS.FileHandlers
{
	class NDSROM
	{
		public List<string> Errors;
		public NDSHeader Header;
	
		public NDSROM(byte[] file)
		{
			Errors = new List<string>();
			byte[] temp;

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
				temp = new byte[16384];
				Array.Copy(file, 0, temp, 0, 16384);
				Header = new NDSHeader(temp);
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
