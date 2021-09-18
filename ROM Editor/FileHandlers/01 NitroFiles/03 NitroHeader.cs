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
	/*¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯*\
	         Header File           
	\*--------------------------*/

	/// <summary>
	/// The is the file tree representation of the ROM header.
	/// </summary>
	public class NitroHeader : NitroFile
	{
		// Begin Header
		public string GameTitle;                // 0x00		12 Characters; ASCII Codes 0x20-0x5F; 0x20 For Spaces; 0x00 For Padding
		public string GameCode;                 // 0x0C		Game-Specific 4-Digit Code
		public string MakerCode;                // 0x10		2-Digit License Code Assigned By Nintendo

		// Hardware/Software Information
		public byte UnitCode;                   // 0x12		Intended Hardware: 0x0 for NDS; 0x2 For NDS+DSi; 0x3 For DSi
		public byte DeviceType;                 // 0x13		Type Of Device Mounted Inside Cartridge
		public byte DeviceCapacity;              // 0x14		0x0-0x9: Megabits [1|2|4|8|16|32|64|128|256|512]; 0xA-0xF: Gigabits [1|2|4|8|16|32]
		public byte[] ReserveredA;
		public byte RegionCode;                 // 0x1D		0x80 For China; 0x40 For Korea; 0x00 For Others
		public byte Version;                    // 0x1E		Also Referred To As "RemasterVersion"
		public byte InternalFlags;              // 0x1F		Auto-Load (?)

		// ARM9
		public uint ARM9Offset;                 // 0x20		ARM9 Source Address
		public uint ARM9Load;                   // 0x28		ARM9 RAM Transfer Destination Address
		public uint ARM9Length;                 // 0x2C		ARM9 Size

		// ARM7
		public uint ARM7Offset;                 // 0x30		ARM7 Source Address
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
		public uint ARM9AutoParam;              // 0x88		ARM9 Auto-Load Hook Address
		public uint ARM7AutoParam;              // 0x8C		ARM7 Auto-Load Hook Address

		// Nintendo Logo						// 0xC0		Nintendo Logo Image [0x9C]
		public ushort NintendoLogoCRC;       
		// 0x15C	Nintendo Logo CRC
		public ushort HeaderCRC;                // 0x15E	Header CRC

		// Begin DSi Header
		public uint AccessControl;              // 0x1B4	Hardware Access Control Settings

		// ARM9i
		public uint ARM9iOffset;                // 0x1C0	ARM9i Source Address
		public uint ARM9iLoad;                  // 0x1C8	ARM9i RAM Transfer Destination Address
		public uint ARM9iLength;                // 0x1CC	ARM9i Size

		// ARM7i
		public uint ARM7iOffset;                // 0x1D0	ARM7i Source Address
		public uint ARM7iLoad;                  // 0x1D8	ARM7i RAM Transfer Destination Address
		public uint ARM7iLength;                // 0x1DC	ARM7i Size

		// Digest Tables
		public uint DigestNitroOffset;
		public uint DigestNitroLength;
		public uint DigestTwilightOffset;
		public uint DigestTwilightLength;
		public uint DigestSectorOffset;
		public uint DigestSectorLength;
		public uint DigestBlockOffset;
		public uint DisestBlockLength;
		public uint DigestSectorSize;
		public uint DigestBlockSectorCount;

		public byte[] DigestARM9Secure;
		public byte[] DigestARM7;
		public byte[] DigestMaster;
		public byte[] DigestBanner;
		public byte[] DigestARM9i;
		public byte[] DigestARM7i;
		public byte[] DigestARM9Insecure;

		public byte[] Signature;
	}
}