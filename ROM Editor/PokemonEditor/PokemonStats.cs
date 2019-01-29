using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS.PokemonEditor
{
	public partial class PokemonStats : UserControl
	{
		public PokemonStats()
		{
			InitializeComponent();
		}

		private void nudBaseHP_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderHP.Width = 142 + ( (int)this.nudBaseHP.Value / 2 );
			this.labelFillHP.Width = (int)this.nudBaseHP.Value / 2;
			this.labelMinHP.Text = "" + ( ( (int)this.nudBaseHP.Value * 2 ) + 100 + 10 );
			this.labelMaxHP.Text = "" + ( ( ( (int)this.nudBaseHP.Value * 2 ) + 31 + ( 255 / 4 ) ) + 100 + 10 );
			updateBaseTotals();
		}

		private void nudBaseAttack_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderAttack.Width = 142 + ( (int)this.nudBaseAttack.Value / 2 );
			this.labelFillAttack.Width = (int)this.nudBaseAttack.Value / 2;
			this.labelMinAttack.Text = "" + Math.Floor(( ( (int)this.nudBaseAttack.Value * 2 ) + 5 ) * 0.9);
			this.labelMaxAttack.Text = "" + Math.Floor(( ( ( (int)this.nudBaseAttack.Value * 2 ) + 31 + 255 / 4 ) + +5 ) * 1.1);
			updateBaseTotals();
		}

		private void nudBaseDefense_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderDefense.Width = 142 + ( (int)this.nudBaseDefense.Value / 2 );
			this.labelFillDefense.Width = (int)this.nudBaseDefense.Value / 2;
			this.labelMinDefense.Text = "" + Math.Floor(( ( (int)this.nudBaseDefense.Value * 2 ) + 5 ) * 0.9);
			this.labelMaxDefense.Text = "" + Math.Floor(( ( ( (int)this.nudBaseDefense.Value * 2 ) + 31 + 255 / 4 ) + +5 ) * 1.1);
			updateBaseTotals();
		}

		private void nudBaseSpeed_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderSpeed.Width = 142 + ( (int)this.nudBaseSpeed.Value / 2 );
			this.labelFillSpeed.Width = (int)this.nudBaseSpeed.Value / 2;
			this.labelMinSpeed.Text = "" + Math.Floor(( ( (int)this.nudBaseSpeed.Value * 2 ) + 5 ) * 0.9);
			this.labelMaxSpeed.Text = "" + Math.Floor(( ( ( (int)this.nudBaseSpeed.Value * 2 ) + 31 + 255 / 4 ) + +5 ) * 1.1);
			updateBaseTotals();
		}

		private void nudBaseSpecialAttack_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderSpecialAttack.Width = 142 + ( (int)this.nudBaseSpecialAttack.Value / 2 );
			this.labelFillSpecialAttack.Width = (int)this.nudBaseSpecialAttack.Value / 2;
			this.labelMinSpecialAttack.Text = "" + Math.Floor(( ( (int)this.nudBaseSpecialAttack.Value * 2 ) + 5 ) * 0.9);
			this.labelMaxSpecialAttack.Text = "" + Math.Floor(( ( ( (int)this.nudBaseSpecialAttack.Value * 2 ) + 31 + 255 / 4 ) + +5 ) * 1.1);
			updateBaseTotals();
		}

		private void nudBaseSpecialDefense_ValueChanged( object sender, EventArgs e )
		{
			this.labelBorderSpecialDefense.Width = 142 + ( (int)this.nudBaseSpecialDefense.Value / 2 );
			this.labelFillSpecialDefense.Width = (int)this.nudBaseSpecialDefense.Value / 2;
			this.labelMinSpecialDefense.Text = "" + Math.Floor(( ( (int)this.nudBaseSpecialDefense.Value * 2 ) + 5 ) * 0.9);
			this.labelMaxSpecialDefense.Text = "" + Math.Floor(( ( ( (int)this.nudBaseSpecialDefense.Value * 2 ) + 31 + 255 / 4 ) + +5 ) * 1.1);
			updateBaseTotals();
		}

		private void updateBaseTotals()
		{
			int total = (int)this.nudBaseHP.Value
				+ (int)this.nudBaseAttack.Value
				+ (int)this.nudBaseDefense.Value
				+ (int)this.nudBaseSpeed.Value
				+ (int)this.nudBaseSpecialAttack.Value
				+ (int)this.nudBaseSpecialDefense.Value;

			this.labelBaseTotal.Text = total.ToString();

			if ( total > 0 )
			{
				this.labelBorderTotal.Width = 142 + ( total / 12 );
				this.labelFillTotal.Width = total / 12;
			}

			this.labelMinTotal.Text = "" + ( Convert.ToInt32(this.labelMinHP.Text)
				+ Convert.ToInt32(this.labelMinAttack.Text)
				+ Convert.ToInt32(this.labelMinDefense.Text)
				+ Convert.ToInt32(this.labelMinSpeed.Text)
				+ Convert.ToInt32(this.labelMinSpecialAttack.Text)
				+ Convert.ToInt32(this.labelMinSpecialDefense.Text) );

			this.labelMaxTotal.Text = "" + ( Convert.ToInt32(this.labelMaxHP.Text)
				+ Convert.ToInt32(this.labelMaxAttack.Text)
				+ Convert.ToInt32(this.labelMaxDefense.Text)
				+ Convert.ToInt32(this.labelMaxSpeed.Text)
				+ Convert.ToInt32(this.labelMaxSpecialAttack.Text)
				+ Convert.ToInt32(this.labelMaxSpecialDefense.Text) );
		}

		private void nudEffort_ValueChanged( object sender, EventArgs e )
		{
			updateEffortTotals();
		}

		private void updateEffortTotals()
		{
			int total = (int)this.nudEffortHP.Value
				+ (int)this.nudEffortAttack.Value
				+ (int)this.nudEffortDefense.Value
				+ (int)this.nudEffortSpeed.Value
				+ (int)this.nudEffortSpecialAttack.Value
				+ (int)this.nudEffortSpecialDefense.Value;

			this.labelEffortTotal.Text = total.ToString();
		}

		bool selectByMouse = false;

		private void nudStats_Enter( object sender, EventArgs e )
		{
			NumericUpDown nudStats = sender as NumericUpDown;
			nudStats.Select();
			nudStats.Select(0, nudStats.Text.Length);
			if ( MouseButtons == MouseButtons.Left )
			{
				selectByMouse = true;
			}
		}

		private void nudStats_MouseDown( object sender, MouseEventArgs e )
		{
			NumericUpDown nudStats = sender as NumericUpDown;
			if ( selectByMouse )
			{
				nudStats.Select(0, nudStats.Text.Length);
				selectByMouse = false;
			}
		}

		protected override bool ProcessDialogKey( Keys keyData )
		{
			if ( keyData.HasFlag(Keys.Enter) )
			{
				SendKeys.Send("{TAB}");
				return true;
			}
			else
			{
				return base.ProcessDialogKey(keyData);
			}
		}

		private void label26_Click( object sender, EventArgs e )
		{

		}
	}
}
