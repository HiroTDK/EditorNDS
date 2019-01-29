using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWindow());
		}


		public static bool IsValidROM(string title, string code)
		{
			List<string> validNames = new List<string>() { "POKEMON HG", "POKEMON SS" };
			List<string> validCodes = new List<string>()
			{
				"IPKD", "IPKE", "IPKF", "IPKI", "IPKJ", "IPKK", "IPKS",
				"IPGD", "IPGE", "IPGF", "IPGI", "IPGJ", "IPGK", "IPGS"
			};

			if (validNames.Contains(title) && validCodes.Contains(code))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
