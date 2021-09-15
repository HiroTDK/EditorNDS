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
			if (tab.tabControl != null)
			{
				TabPanel.Controls.Clear();
				TabPanel.Controls.Add(tab.tabControl);
			}
		}
		public void CloseTab(Tab tab)
		{
			if (tabActivity.Count > 0)
			{
				ActivateTab(tabActivity.Last());
			}

			tabList.Remove(tab);
			tabVisibility.Remove(tab);
			tabActivity.Remove(tab);
			tabStrip.Controls.Remove(tab.tabLabel);
			tab.tabLabel.Dispose();
			if (tab.tabControl != null)
			{
				TabPanel.Controls.Remove(tab.tabControl);
				tab.tabControl.Dispose();
			}
			if (tab.tabDocument != null)
			{
				tab.tabDocument.Dispose();
			}
			UpdateTabStrip();
		}
	}
}