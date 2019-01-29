using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS
{
     public class BaseClass : IDisposable                                        // Creates an inheritable class that can be cleaned up.
     {
          bool disposed = false;                                                  // Flag: Has Dispose already been called?
          SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);              // Instantiate a SafeHandle instance.

          public void Dispose()                                                   // Public implementation of Dispose pattern.
          {
               Dispose(true);                                                      // Calls the protected implementation.
               GC.SuppressFinalize(this);                                          // Suppress finalizing to prevent redundant cleanup.
          }

          protected virtual void Dispose(bool disposing)                          // Protected implementation of Dispose pattern.
          {
               if (disposed) { return; }                                           // If already called, ignore future calls.

               if (disposing)                                                      // If we are disposing, do cleanup.
               {
               handle.Dispose();                                               // Cleanup stuff here.
               // Possible future cleanup.
               }
                
               disposed = true;                                                    // Set disposed boolean to prevent redundant calls.
          }

     }

	public class ProgressBarText : ProgressBar
	{
		const int WmPaint = 15;

		protected override void WndProc( ref Message m )
		{
			base.WndProc(ref m);

			switch ( m.Msg )
			{
				case WmPaint:
					using ( var graphics = Graphics.FromHwnd(Handle) )
					{
						SizeF textSize = graphics.MeasureString(Text, Font);
						Point textPos = new System.Drawing.Point(( Width / 2 ) - (int)( textSize.Width / 2 ), ( Height / 2 ) - (int)( textSize.Height / 2 ));

						TextRenderer.DrawText(graphics, Text, Font, textPos, ForeColor);
							
					}
					break;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams result = base.CreateParams;
				if ( Environment.OSVersion.Platform == PlatformID.Win32NT
					&& Environment.OSVersion.Version.Major >= 6 )
				{
					result.ExStyle |= 0x02000000; // WS_EX_COMPOSITED 
				}

				return result;
			}
		}
	}
}
