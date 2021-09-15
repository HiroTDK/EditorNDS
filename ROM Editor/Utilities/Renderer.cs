using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS
{
    public static class ColorSpace
    {
        public static readonly Color Black = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        public static readonly Color DarkestGray = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
        public static readonly Color DarkerGray = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        public static readonly Color DarkGray = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
        public static readonly Color Gray = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
        public static readonly Color LightGray = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
        public static readonly Color LighterGray = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
        public static readonly Color LightestGray = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        public static readonly Color White = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
    }

    public partial class MainWindow : Form
    {
        [DllImport("user32")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        const int GWL_EXSTYLE = (-20);
        const int WS_EX_CLIENTEDGE = 0x00000200;
        const int SWP_FRAMECHANGED = 0x0020;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;

        public class MenuRenderer : ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                ToolStripDropDown t = e.ToolStrip as ToolStripDropDown;
                System.Drawing.SolidBrush b;

                if (t != null)
                {
                    b = new System.Drawing.SolidBrush(Color.FromArgb(31, 31, 31));
                    e.Graphics.FillRectangle(b, e.AffectedBounds);
                    b.Dispose();
                }
                else
                {
                    b = new System.Drawing.SolidBrush(Color.FromArgb(63, 63, 63));
                    e.Graphics.FillRectangle(b, e.AffectedBounds);
                    b.Dispose();
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                ToolStripDropDown t = e.ToolStrip as ToolStripDropDown;

                if (t != null)
                {
                    System.Drawing.Pen p;
                    p = new System.Drawing.Pen(Color.FromArgb(127, 127, 127));
                    e.Graphics.DrawRectangle(p, 0, 0, t.Width - 1, t.Height - 1);
                    p.Dispose();

                    if (t.OwnerItem != null)
                    {
                        if (t.OwnerItem.OwnerItem == null)
                        {
                            p = new System.Drawing.Pen(Color.FromArgb(31, 31, 31));
                            e.Graphics.DrawLine(p, 1, 0, t.OwnerItem.Width - 2, 0);
                            p.Dispose();
                        }
                        else
                        {
                            t.Show(new Point(4, 5));
                        }
                    }

                }
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                ToolStripMenuItem t = e.Item as ToolStripMenuItem;

                if (t != null)
                {
                    System.Drawing.SolidBrush b;
                    if (t.Pressed)
                    {
                        if (t.HasDropDownItems && t.OwnerItem == null)
                        {
                            b = new System.Drawing.SolidBrush(Color.FromArgb(31, 31, 31));
                            e.Graphics.FillRectangle(b, 0, 0, t.Width, t.Height);
                            b.Dispose();

                            System.Drawing.Pen p;
                            p = new System.Drawing.Pen(Color.FromArgb(127, 127, 127));
                            e.Graphics.DrawRectangle(p, 0, 0, t.Width - 1, t.Height);
                            p.Dispose();
                        }
                        else
                        {
                            b = new System.Drawing.SolidBrush(Color.FromArgb(127, 127, 127));
                            e.Graphics.FillRectangle(b, 3, 1, t.Width - 5, t.Height - 2);
                            b.Dispose();
                        }
                    }
                    else if (t.Selected)
                    {
                        b = new System.Drawing.SolidBrush(Color.FromArgb(127, 127, 127));
                        e.Graphics.FillRectangle(b, 3, 1, t.Width - 5, t.Height - 2);
                        b.Dispose();
                    }
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = Color.White;
                base.OnRenderItemText(e);
            }
        }
    }
}

