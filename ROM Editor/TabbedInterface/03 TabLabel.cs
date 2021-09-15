using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.TabbedInterface
{
	public class TabLabel : Control
	{
		public Tab tabParent;
		public string text = "";

		public TabLabel()
		{
			BackColor = ColorSpace.DarkerGray;
			ForeColor = System.Drawing.Color.White;
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (tabParent.tabInterface.tabActivity.First() == tabParent)
			{
				using (System.Drawing.SolidBrush brush = new SolidBrush(ColorSpace.DarkGray))
				{
					pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
				}
			}
			else if (tabParent.mouseHover && !tabParent.mouseCloseHover && !tabParent.mouseCloseDown)
			{
				using (System.Drawing.SolidBrush brush = new SolidBrush(ColorSpace.Gray))
				{
					pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
				}
			}
			else
			{
				base.OnPaintBackground(pevent);
			}

			if (tabParent.mouseCloseHover || tabParent.mouseCloseDown)
			{
				using (System.Drawing.SolidBrush brush = new SolidBrush(ColorSpace.Gray))
				{
					pevent.Graphics.FillRectangle(brush, Width - 17, 3, 14, 14);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			using (System.Drawing.SolidBrush brush = new SolidBrush(ColorSpace.White))
			{
				RectangleF rectangle = new RectangleF(0, 0, this.Width - 14, this.Height);
				StringFormat string_format = new StringFormat()
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center,
					Trimming = StringTrimming.EllipsisCharacter,
					FormatFlags = StringFormatFlags.NoWrap,
				};

				e.Graphics.DrawString(text, DefaultFont, brush, rectangle, string_format);
			}

			using (System.Drawing.Pen pen = new Pen(ColorSpace.White))
			{
				if (tabParent.mouseCloseDown)
				{
					Point[] points = new Point[]
					{
						new Point(Width - 13, 7),
						new Point(Width - 7, 13),
						new Point(Width - 8, 13),
						new Point(Width - 14, 7),
					};
					e.Graphics.DrawLines(pen, points);

					points = new Point[]
					{
						new Point(Width - 8, 7),
						new Point(Width - 14, 13),
						new Point(Width - 13, 13),
						new Point(Width - 7, 7),
					};
					e.Graphics.DrawLines(pen, points);
				}
				else
				{
					Point[] points = new Point[]
					{
						new Point(Width - 13, 6),
						new Point(Width - 7, 12),
						new Point(Width - 8, 12),
						new Point(Width - 14, 6),
					};
					e.Graphics.DrawLines(pen, points);

					points = new Point[]
					{
						new Point(Width - 8, 6),
						new Point(Width - 14, 12),
						new Point(Width - 13, 12),
						new Point(Width - 7, 6),
					};
					e.Graphics.DrawLines(pen, points);
				}
			}
		}
	}
}
