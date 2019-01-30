using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROM_Editor.NDS_Handlers
{
	public class SDAT : ROM_Editor.BaseClass
	{
		
		s8 type[4];			// 'SDAT'
		u32 magic;			// 0x0100feff
		u32 nFileSize;
		u16 nSize;
		u16 nBlock;			// usually 4, but some have 3 only ( Symbol Block omitted )

		file;
		u32 nSymbOffset;	// offset of Symbol Block = 0x40
		u32 nSymbSize;		// size of Symbol Block
		u32 nInfoOffset;	// offset of Info Block
		u32 nInfoSize;		// size of Info Block
		u32 nFatOffset;		// offset of FAT
		u32 nFatSize;		// size of FAT
		u32 nFileOffset;	// offset of File Block
		u32 nFileSize;		// size of File Block
		u8 reserved[16];	// unused, 0
	}
}
