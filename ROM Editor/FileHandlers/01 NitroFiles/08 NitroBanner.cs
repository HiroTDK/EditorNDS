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
	/// <summary>
	/// Banner files contain the icon and title data display on the DS home screen.
	/// The title is provided in many languages, depending on the game and version.
	/// DSi games can have an animated icon.
	/// </summary>
	public class NDSBanner : NitroFile
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
			for (int tile_y = 0; tile_y < 4; tile_y++)
			{
				for (int tile_x = 0; tile_x < 4; tile_x++)
				{
					for (int pixel_y = 0; pixel_y < 8; pixel_y++)
					{
						for (int pixel_x = 0; pixel_x < 8; pixel_x++)
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
}