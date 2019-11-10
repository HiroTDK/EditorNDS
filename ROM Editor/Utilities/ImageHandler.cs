using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorNDS.Utilities
{
	public static class ImageHandler
	{
		/// <summary>
		/// Reads BGR555 data from byte array to a color palette.
		/// </summary>
		/// <param name="raw">Bytes encoded with BGR555</param>
		/// <returns>Color Palette</returns>
		public static Color[] ReadPaletteBGR555(byte[] raw)
		{
			Color[] palette = new Color[raw.Length / 2];

			for (int i = 0; i < raw.Length / 2; i++)
			{
				short bgr = BitConverter.ToInt16(new Byte[] { raw[i * 2], raw[i * 2 + 1] }, 0);
				palette[i] = ReadSwatchBGR555( bgr );
			}

			return palette;
		}

		/// <summary>
		/// Reads BGR555 data from two bytes to a color.
		/// </summary>
		/// <param name="bgr">Raw BGR555 color data.</param>
		/// <returns>Color Swatch</returns>
		public static Color ReadSwatchBGR555(short bgr)
		{
			int blue = ((bgr & 0x7C00) >> 10) * 8;
			int green = ((bgr & 0x03E0) >> 5) * 8;
			int red = (bgr & 0x001F) * 8;

			return Color.FromArgb(red, green, blue);
		}
	}



}
