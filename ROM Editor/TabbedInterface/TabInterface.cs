using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.TabbedInterface
{
	public partial class TabInterface : UserControl
	{
		public List<Tab> tabList = new List<Tab>();
		public List<Tab> tabVisibility = new List<Tab>();
		public List<Tab> tabActivity = new List<Tab>();

		public ToolTip toolTip;
		public Panel tabStrip;

		public int tabWidth;
		public int tabCount;

		public TabInterface()
		{
			InitializeComponent();

			tabStrip = this.TabStrip;
			tabStrip.AllowDrop = true;

			TabManager.tabInterfaces.Add(this);

			tabList = new List<Tab>();
			tabActivity = new List<Tab>();

			UpdateTabStrip();
		}
		private int i = 0;

		private void TabInterface_Load(object sender, EventArgs e)
		{
			toolTip = new ToolTip();

			toolTip.AutoPopDelay = 5000;
			toolTip.InitialDelay = 1000;
			toolTip.ReshowDelay = 500;
			toolTip.ShowAlways = true;

			toolTip.SetToolTip(TabListButton, "Tab List");
		}
		private void TabListButton_Click(object sender, EventArgs e)
		{
			Tab tab_new = new Tab(this, "Tab: " + i++);
			OpenTab(tab_new);
			ActivateTab(tab_new);
		}

		private void TabStrip_Resize(object sender, EventArgs e)
		{
			UpdateTabStrip();
		}
		private void TabStrip_DragDrop(object sender, DragEventArgs e)
		{

		}
		private void TabStrip_DragOver(object sender, DragEventArgs e)
		{

		}

		private void UpdateTabList()
		{
			foreach (Tab tab in tabList)
			{
				if (!tabActivity.Contains(tab))
				{
					tabActivity.Add(tab);
				}
			}
		}
		public void UpdateTabStrip()
		{
			if (tabList.Count * TabManager.maximumTabWidth <= tabStrip.Width)
			{
				foreach (Tab tab in tabActivity)
				{
					if (!tabVisibility.Contains(tab))
					{
						tabVisibility.Add(tab);
					}
				}
				tabWidth = TabManager.maximumTabWidth;
			}
			else
			{
				if (tabList.Count * TabManager.minimumTabWidth <= tabStrip.Width)
				{
					foreach (Tab tab in tabActivity)
					{
						if (!tabVisibility.Contains(tab))
						{
							tabVisibility.Add(tab);
						}
					}
					tabWidth = tabStrip.Width / Math.Max(1, tabVisibility.Count);
				}
				else
				{
					int tab_count = Math.Max(1, (tabStrip.Width / Math.Max(1, TabManager.minimumTabWidth)));
					tabWidth = Math.Min(TabManager.maximumTabWidth, tabStrip.Width / Math.Max(1, tab_count));

					if (tab_count > tabVisibility.Count)
					{
						int difference = tab_count - tabVisibility.Count;

						foreach (Tab tab in tabActivity)
						{
							if (!tabVisibility.Contains(tab))
							{
								tabVisibility.Add(tab);
								difference--;
								if (difference <= 0)
								{
									break;
								}
							}
						}
					}
					else if (tab_count < tabVisibility.Count)
					{
						int difference = tabVisibility.Count - tab_count;
						List<Tab> reverse_visible_order = new List<Tab>(tabVisibility);
						reverse_visible_order.Reverse();

						if (tabVisibility.Count > 0)
						{
							foreach (Tab tab in reverse_visible_order)
							{
								if (tabActivity.First() != tab)
								{
									tabVisibility.Remove(tab);
									difference--;
									if (difference <= 0 || tabVisibility.Count <= 1)
									{
										break;
									}
								}
							}
						}
					}
				}
			}

			UpdateTabs();
		}
		private void UpdateTabs()
		{
			foreach (Tab tab in tabList)
			{
				if (!tabVisibility.Contains(tab) && tabStrip.Controls.Contains(tab.tabLabel))
				{
					tabStrip.Controls.Remove(tab.tabLabel);
				}
			}

			int iteration = 0;
			foreach (Tab tab in tabVisibility)
			{
				if (!tabStrip.Controls.Contains(tab.tabLabel))
				{

					tabStrip.Controls.Add(tab.tabLabel);
				}

				tab.tabLabel.Left = iteration * tabWidth;
				tab.tabLabel.Width = tabWidth;
				tab.tabLabel.Invalidate();
				toolTip.SetToolTip(tab.tabLabel, tab.tabTitle);
				iteration++;
			}
		}

		public void OpenTab(Tab tab)
		{
			if (!tabList.Contains(tab))
			{
				tabList.Add(tab);
			}

			if (!tabActivity.Contains(tab))
			{
				tabActivity.Add(tab);
			}
		}
		public void ActivateTab(Tab tab)
		{
			if (tabActivity.First() != tab)
			{
				tabActivity.First().tabLabel.Invalidate();
			}
			if (tabActivity.Contains(tab))
			{
				tabActivity.Remove(tab);
			}
			tabActivity.Insert(0, tab);

			if (!tabVisibility.Contains(tab))
			{
				if (tabVisibility.Count >= tabList.Count)
				{
					tabVisibility.Remove(tabVisibility.Last());
				}
				tabVisibility.Add(tab);
				UpdateTabStrip();
			}
		}
		public void CloseTab(Tab tab)
		{
			tabList.Remove(tab);
			tabVisibility.Remove(tab);
			tabActivity.Remove(tab);
			tabStrip.Controls.Remove(tab.tabLabel);
			tab.tabLabel.Dispose();

			UpdateTabStrip();
		}
	}

	static class TabManager
	{
		public static int minimumTabWidth = 100;
		public static int maximumTabWidth = 150;

		public static bool showIcons = true;

		public static List<TabInterface> tabInterfaces = new List<TabInterface>();
	}

	public class Tab
	{
		public TabLabel tabLabel;
		public TabInterface tabInterface;
		public string tabTitle;

		public bool mouseHover = false;
		public bool mouseCloseHover = false;
		public bool mouseCloseDown = false;
		public Point mouseDownLocation;
		public int initialLeft = 0;

		public Tab(TabInterface tab_interface, string title)
		{
			tabTitle = title;
			tabLabel = new TabLabel
			{
				text = title,
				Height = 20,
			};
			tabLabel.MouseEnter += new EventHandler(this.tabLabel_MouseEnter);
			tabLabel.MouseLeave += new EventHandler(this.tabLabel_MouseLeave);
			tabLabel.MouseCaptureChanged += new EventHandler(this.tabLabel_MouseCaptureChanged);
			tabLabel.MouseMove += new MouseEventHandler(this.tabLabel_MouseMove);
			tabLabel.MouseDown += new MouseEventHandler(this.tabLabel_MouseDown);
			tabLabel.MouseUp += new MouseEventHandler(this.tabLabel_MouseUp);

			tabLabel.tabParent = this;
			tabInterface = tab_interface;
		}

		private void tabLabel_MouseEnter(object sender, EventArgs e)
		{
			mouseHover = true;
			tabLabel.Invalidate();
		}
		private void tabLabel_MouseLeave(object sender, EventArgs e)
		{
			mouseHover = false;
			mouseCloseHover = false;
			tabLabel.Invalidate();
		}
		private void tabLabel_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left && !mouseDownLocation.IsEmpty)
			{
				tabLabel.Left = Math.Max(0, Math.Min(tabLabel.Parent.Width - tabLabel.Width, e.X - mouseDownLocation.X + tabLabel.Left));

				if (tabLabel.Left > initialLeft + (tabInterface.tabWidth / 3) && tabInterface.tabVisibility.Last() != this)
				{
					int current_index = tabInterface.tabVisibility.IndexOf(this);
					Tab next_tab = tabInterface.tabVisibility.ElementAt(current_index + 1);

					tabInterface.tabVisibility.Remove(this);
					tabInterface.tabVisibility.Insert(current_index + 1, this);

					int next_left = next_tab.tabLabel.Left;
					next_tab.tabLabel.Left = initialLeft;
					initialLeft = next_left;
				}
				if (tabLabel.Left < initialLeft - (tabInterface.tabWidth / 3) && tabInterface.tabVisibility.First() != this)
				{
					int current_index = tabInterface.tabVisibility.IndexOf(this);
					Tab previous_tab = tabInterface.tabVisibility.ElementAt(current_index - 1);

					tabInterface.tabVisibility.Remove(previous_tab);
					tabInterface.tabVisibility.Insert(current_index, previous_tab);

					int previous_left = previous_tab.tabLabel.Left;
					previous_tab.tabLabel.Left = initialLeft;
					initialLeft = previous_left;
				}
			}
			else
			{
				if (e.X >= tabLabel.Width - 18 && e.X < tabLabel.Width - 2 && e.Y <= 18 && e.Y > 2)
				{
					if (!mouseCloseHover)
					{
						mouseCloseHover = true;
						tabLabel.Invalidate();
					}
				}
				else
				{
					if (mouseCloseHover)
					{
						mouseCloseHover = false;
						tabLabel.Invalidate();
					}
				}
			}
		}
		private void tabLabel_MouseDown(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					{
						if (e.X >= tabLabel.Width - 18 && e.X < tabLabel.Width - 2 && e.Y <= 18 && e.Y > 2)
						{
							mouseCloseDown = true;
							tabLabel.Invalidate();
						}
						else
						{
							mouseDownLocation = e.Location;
							initialLeft = tabLabel.Left;
							tabInterface.ActivateTab(this);
							tabLabel.BringToFront();
						}
					}
					break;
				case MouseButtons.Right:
					{
						ContextMenu context_menu = new ContextMenu
							(new MenuItem[] {
								new MenuItem("Save"),
								new MenuItem("Save All"),
								new MenuItem(),
								new MenuItem("Close"),
								new MenuItem("Close All"),
								new MenuItem("Close All But This")
							});
						tabLabel.ContextMenu = context_menu;
					}
					break;
			}

			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				int width = tabLabel.Width;
				if (e.X >= width - 18 && e.X < width - 2 && e.Y <= 18 && e.Y > 2)
				{
					if (!mouseCloseDown)
					{
						mouseCloseDown = true;
						tabLabel.Invalidate();
					}
				}
				else
				{
					if (mouseCloseDown)
					{
						mouseCloseDown = false;
						tabLabel.Invalidate();
					}

					mouseDownLocation = e.Location;
					initialLeft = tabLabel.Left;
					tabInterface.ActivateTab(this);
				}
			}
		}
		private void tabLabel_MouseUp(object sender, MouseEventArgs e)
		{
			if (!mouseDownLocation.IsEmpty)
			{
				mouseDownLocation = Point.Empty;
				tabInterface.UpdateTabStrip();
			}
			if (mouseCloseDown)
			{
				int width = tabLabel.Width;
				if (e.X >= width - 18 && e.X < width - 3 && e.Y <= 18 && e.Y > 2)
				{
					tabInterface.CloseTab(this);
				}
			}
		}
		private void tabLabel_MouseCaptureChanged(object sender, EventArgs e)
		{
			if (!mouseDownLocation.IsEmpty)
			{
				mouseDownLocation = Point.Empty;
				tabInterface.UpdateTabStrip();
			}
			if (mouseCloseDown)
			{
				mouseCloseDown = false;
				tabLabel.Invalidate();
			}
		}
	}

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