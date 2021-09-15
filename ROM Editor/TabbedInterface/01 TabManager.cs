using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.TabbedInterface
{
	static class TabManager
	{
		public static int minimumTabWidth = 100;
		public static int maximumTabWidth = 150;

		public static bool showIcons = true;

		public static List<TabInterface> tabInterfaces = new List<TabInterface>();
	}
}
