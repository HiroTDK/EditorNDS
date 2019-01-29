using EditorNDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EditorNDS.FileHandlers
{
	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
			    Files        
	\*--------------------------*/

	public class NDSFile
	{
		public NDSFile()
		{

		}

		public NDSFile(string name, string path, string extension, int offset, int length)
		{
			Name = name;
			Path = path;
			Extension = extension;
			Offset = offset;
			Length = length;
		}

		public string Name;
		public string Path;
		public string Extension;
		public int Offset;
		public int Length;

		public void GetExtension(Stream rom)
		{
			byte[] firstFour = new byte[4];
			rom.Position = Offset;
			rom.Read(firstFour, 0, 4);

			Extension = "";

			switch ( System.Text.Encoding.UTF8.GetString(firstFour) )
			{
				// Archives
				case "NARC":
					Extension = ".narc";
					break;
				case "SDAT":
					Extension = ".sdat";
					break;

				// Graphics
				case "RLCN":
					Extension = ".nclr";
					break;
				case "RGCN":
					Extension = ".ncgr";
					break;
				case "RCSN":
					Extension = ".nscr";
					break;
				case "RNAN":
					Extension = ".nanr";
					break;
				case "RECN":
					Extension = ".ncer";
					break;
				case "RCMN":
					Extension = ".nmcr";
					break;
				case " APS":
					Extension = ".spa";
					break;

				// Models
				case "BMD0":
					Extension = ".nsbmd";
					break;
				case "BTX0":
					Extension = ".nsbtx";
					break;
				case "BCA0":
					Extension = ".nsbca";
					break;
				case "BVA0":
					Extension = ".nsbva";
					break;
				case "BMA0":
					Extension = ".nsbma";
					break;
				case "BTP0":
					Extension = ".nsbtp";
					break;
				case "BTA0":
					Extension = ".nsbta";
					break;

				// Others
				case "MESG":
					Extension = ".mesg";
					break;

				// Malformed Header in HG/SS
				case "RPCN":
					Extension = ".nclr";
					break;
			}
			// Test for text files by searching for the "EOF" string near the end of the file.
			byte[] EOF = new byte[7];
			rom.Position = Offset + Length - 7;
			rom.Read(EOF, 0, 7);
			if ( System.Text.Encoding.UTF8.GetString(EOF).Contains("EOF") )
			{
				Extension = ".txt";
			}

			if ( Extension.Length > 0 && Name.Length > Extension.Length )
			{
				int nLength = Name.Length - Extension.Length;
				string nSub = Name.Substring(nLength);

				if ( nSub == Extension || nSub == Extension.ToUpper() )
				{
					Name = Name.Remove(nLength);
				}
			}
		}
	}
}
