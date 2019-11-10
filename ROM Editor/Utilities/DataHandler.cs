using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorNDS.Utilities
{
	public static class DataHandler
	{
		/// <summary>
		/// Converts a single byte to an array of 8 bytes representing the 8 bits that constitute the byte.
		/// </summary>
		public static byte[] ByteToBits(byte bite)
		{
			byte[] bits = new byte[8];
			for (int b = 7; b >= 0; b--)
			{
				bits[7 - b] = (byte)((bite >> b) & 1);
			}
			return bits;
		}

		/// <summary>
		/// Converts an array of bytes to an array of bytes representing the 8 bits that constitute each byte.
		/// </summary>
		public static byte[] BytesToBits(byte[] bites)
		{
			byte[] bits = new byte[8 * bites.Count()];
			for (int i = 0; i < bites.Count(); i++)
			{
				for (int b = 7; b >= 0; b--)
				{
					int bit = (7-b) + (i*8);
					bits[bit] = (byte)((bites[i] >> b) & 1);
				}
			}
			return bits;
		}

		/// <summary>
		/// Converts a single byte to an array of 4 bytes representing the 4 crumbs (2 bits) that constitute the byte.
		/// </summary>
		public static byte[] ByteToCrumbs(byte bite)
		{
			byte[] crumbs = new byte[4];
			for (int c = 3; c >= 0; c--)
			{
				crumbs[3-c] = (byte)((bite >> c*2) & 3);
			}
			return crumbs;
		}

		/// <summary>
		/// Converts an array of bytes to an array of bytes representing the 4 crumbs (2 bits) that constitute each byte.
		/// </summary>
		public static byte[] BytesToCrumbs(byte[] bites)
		{
			byte[] crumbs = new byte[4 * bites.Count()];
			for (int i = 0; i < bites.Count(); i++)
			{
				for (int c = 3; c >= 0; c--)
				{
					int crumb = (3-c) + (i*4);
					crumbs[crumb] = (byte)((bites[i] >> c*2) & 3);
				}
			}
			return crumbs;
		}

		/// <summary>
		/// Converts a single byte to an array of 2 bytes representing the 2 nibbles (4 bits) that constitute the byte.
		/// </summary>
		public static byte[] ByteToNibbles(byte bite)
		{
			byte[] nibbles = new byte[2];
			nibbles[0] = (byte)(bite & 15);
			nibbles[1] = (byte)((bite & 240) >> 4);
			return nibbles;
		}

		/// <summary>
		/// Converts an array of bytes to an array of bytes representing the 2 nibbles (4 bits) that constitute each byte.
		/// </summary>
		public static byte[] BytesToNibbles(byte[] bites)
		{
			byte[] nibbles = new byte[2 * bites.Length];
			for (int i = 0; i < bites.Length; i++)
			{
				nibbles[i * 2] = (byte)(bites[i] & 15);
				nibbles[(i * 2) + 1] = (byte)((bites[i] & 240) >> 4);
			}
			return nibbles;
		}
	}
}
