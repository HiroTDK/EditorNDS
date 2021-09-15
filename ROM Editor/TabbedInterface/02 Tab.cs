using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.TabbedInterface
{

	public class Tab
	{
		public TabLabel tabLabel;
		public TabInterface tabInterface;
		public DocumentHandler tabDocument;
		public Control tabControl;
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
			tabLabel.MouseMove += new MouseEventHandler(this.tabLabel_MouseMove);
			tabLabel.MouseDown += new MouseEventHandler(this.tabLabel_MouseDown);
			tabLabel.MouseUp += new MouseEventHandler(this.tabLabel_MouseUp);
			tabLabel.MouseClick += new MouseEventHandler(this.tabLabel_MouseClick);
			tabLabel.MouseCaptureChanged += new EventHandler(this.tabLabel_MouseCaptureChanged);

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
			if (e.Button == MouseButtons.Left)
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
		private void tabLabel_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ContextMenu context_menu = new ContextMenu
					(new MenuItem[] {
						new MenuItem("Save"),
						new MenuItem("Save All"),
						new MenuItem(),
						new MenuItem("Close", new EventHandler(this.contextMenu_Close_MouseClick)),
						new MenuItem("Close All", new EventHandler(this.contextMenu_CloseAll_MouseClick)),
						new MenuItem("Close All But This", new EventHandler(this.contextMenu_CloseAllButThis_MouseClick))
					});
				tabLabel.ContextMenu = context_menu;
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

		private void contextMenu_Close_MouseClick(object sender, EventArgs e)
		{
			tabInterface.CloseTab(this);
		}
		private void contextMenu_CloseAll_MouseClick(object sender, EventArgs e)
		{
			foreach (Tab tab in tabInterface.tabList.ToList())
			{
				if (this != tab)
				{
					tabInterface.CloseTab(tab);
				}
			}
			tabInterface.CloseTab(this);
		}
		private void contextMenu_CloseAllButThis_MouseClick(object sender, EventArgs e)
		{
			foreach (Tab tab in tabInterface.tabList.ToList())
			{
				if (this != tab)
				{
					tabInterface.CloseTab(tab);
				}
			}
		}
	}
}
