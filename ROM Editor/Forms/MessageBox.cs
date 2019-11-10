using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorNDS
{
	/// <summary>
	/// This is going to be our custom message box. Here we 
	/// can display messages and errors in our own style.
	/// </summary>
	internal partial class MessageBox : Form
	{
		public MessageBox( string message, string title, int width, List<string> buttons, List<DialogResult> results )
		{
			InitializeComponent();                                                 // Required for designer support.

			if ( message.Length < 1 )
			{
				message = "An error has occurred. Unfortunately, no details have been provided.";
			}
			this.labelMessage.Text = message;
			this.labelMessage.Visible = true;

			if ( title.Length < 1 )
			{
				if ( message.Length < 50 )
				{
					title = message;
				}
				else
				{
					title = "Notice!";
				}
			}
			this.Text = title;

			if ( width == 0 )
			{
				width = 400;
			}
			Size textSize = TextRenderer.MeasureText(message, new Font("Microsoft Sans Serif", 9.25F), new Size(width - 50, 0), TextFormatFlags.WordBreak);
			this.ClientSize = new Size(textSize.Width + 50, textSize.Height + 85);


			/*--------------- Buttons ---------------*\
			This section makes sure that there are no
			textless buttons, that there is at least
			one button, that there are enough buttons
			for each result, and that the buttons are
			created and organized in the space that the
			message box provides. If there isn't enough
			space, the message box is expanded in order
			to accomodate each button.
			\*---------------------------------------*/

			while ( buttons.Count < results.Count )
			{
				buttons.Add("");
			}

			for ( int i = 0; i < buttons.Count; i++ )
			{
				if ( buttons[i].Length < 1 )
				{
					buttons[i] = "Button " + i;
				}
			}

			if ( buttons.Count > 1 )
			{
				List<Button> buttonList = new List<Button>();
				int buttonWidth = 0;
				int index = 1;

				foreach ( string s in buttons )
				{
					Button currentButton = newButton("button" + index, s);
					this.Controls.Add(currentButton);
					currentButton.BringToFront();

					buttonList.Add(currentButton);
					currentButton.TabIndex = index;
					buttonWidth += currentButton.Width;
					index++;
				}

				width = Math.Max(this.ClientSize.Width, buttonWidth + ( 10 * ( buttons.Count + 1 ) ));
				
				if ( width != this.ClientSize.Width )
				{
					this.ClientSize = new Size(width, this.ClientSize.Height);
				}

				int spacer = width - buttonWidth;
				{
					spacer /= buttons.Count + 1;
				}

				index = 1;
				buttonWidth = 0;
				foreach ( Button button in buttonList )
				{
					button.Location = new System.Drawing.Point(
					    ( spacer * index ) + buttonWidth,
					    ( this.ClientSize.Height - button.Height - 20 ));

					index++;
					buttonWidth += button.Width;
				}
			}

			else
			{
				Button button;

				if ( buttons.Count > 0 )
				{
					button = newButton("button1", buttons[0]);
				}
				else
				{
					button = newButton("button1", "Close");
				}

				this.Controls.Add(button);
				button.BringToFront();
				button.TabIndex = 1;

				button.Location = new System.Drawing.Point(
				( this.ClientSize.Width - button.Width ) / 2,
				( this.ClientSize.Height - button.Height - 20 ));
			}


			/*--------------- Results ---------------*\
			This section makes sure that there are
			enough results for each button generated.
			Results generated to match extra buttons
			are filled with DialogResult.Cancel.
			\*---------------------------------------*/

			if ( results.Count < buttons.Count )
			{
				for ( int i = results.Count; i < buttons.Count; i++ )
				{
					results.Add(DialogResult.Cancel);
				}
			}
			this.Results = results;

		}

		private Button newButton( string name, string text )
		{
			System.Windows.Forms.Button b = new System.Windows.Forms.Button();

			b.ForeColor = System.Drawing.Color.White;
			b.BackColor = System.Drawing.Color.FromArgb(95, 95, 95);
			b.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(31, 31, 31);
			b.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(62, 62, 62);
			b.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(127, 127, 127);
			b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			b.UseVisualStyleBackColor = false;

			b.Font = new System.Drawing.Font(
			    "Microsoft Sans Serif",
			    9.25F, System.Drawing.FontStyle.Bold,
			    System.Drawing.GraphicsUnit.Point,
			    0);

			b.Anchor = System.Windows.Forms.AnchorStyles.None;
			b.TabIndex = 1;

			b.AutoSize = true;
			b.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			b.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
			b.Margin = new System.Windows.Forms.Padding(0);

			b.Name = name;
			b.Text = text;

			b.Invalidate();
			b.Update();
			b.Click += new System.EventHandler(this.button_click);
			return b;
		}

		private List<DialogResult> Results;

		private void button_click( object sender, EventArgs e )
		{
			Button b = (Button)sender;
			int t = b.TabIndex - 1;
			if ( t >= 0 && Results.Count >= b.TabIndex )
			{
				this.DialogResult = Results[t];
			}
			this.Close();
		}


		public MessageBox( Control control, string title, List<string> buttons, List<DialogResult> results )
		{
			InitializeComponent();                                                 // Required for designer support.
			
			if ( title.Length < 1 )
			{
				title = "Attention!";
				
			}
			this.Text = title;


			/*--------------- Buttons ---------------*\
			This section makes sure that there are no
			textless buttons, that there is at least
			one button, that there are enough buttons
			for each result, and that the buttons are
			created and organized in the space that the
			message box provides. If there isn't enough
			space, the message box is expanded in order
			to accomodate each button.
			\*---------------------------------------*/

			while ( buttons.Count < results.Count )
			{
				buttons.Add("");
			}

			for ( int i = 0; i < buttons.Count; i++ )
			{
				if ( buttons[i].Length < 1 )
				{
					buttons[i] = "Button " + i;
				}
			}

			if ( buttons.Count > 1 )
			{
				List<Button> buttonList = new List<Button>();
				int buttonWidth = 0;
				int index = 1;

				foreach ( string s in buttons )
				{
					Button currentButton = newButton("button" + index, s);
					this.Controls.Add(currentButton);
					currentButton.BringToFront();

					buttonList.Add(currentButton);
					currentButton.TabIndex = index;
					buttonWidth += currentButton.Width;
					index++;
				}

				
				this.ClientSize = new Size(Math.Min(control.Width, buttonWidth + ( 10 * ( buttons.Count + 1 ) )), control.Height + 65);
				
				int spacer = this.ClientSize.Width - buttonWidth;
				{
					spacer /= buttons.Count + 1;
				}

				index = 1;
				buttonWidth = 0;
				foreach ( Button button in buttonList )
				{
					button.Location = new System.Drawing.Point(
					    ( spacer * index ) + buttonWidth,
					    ( this.ClientSize.Height - button.Height - 20 ));

					index++;
					buttonWidth += button.Width;
				}
			}

			else if ( buttons.Count > 0 )
			{
				Button button = newButton("button1", buttons[0]);

				this.ClientSize = new Size(Math.Min(control.Width, button.Width + 50), Math.Min(control.Height, button.Height + 65));

				this.Controls.Add(button);
				button.BringToFront();
				button.TabIndex = 1;

				button.Location = new System.Drawing.Point(
				( this.ClientSize.Width - button.Width ) / 2,
				( this.ClientSize.Height - button.Height - 20 ));
			}

			else
			{
				this.ClientSize = new Size(control.Width, control.Height);
			}

			/*--------------- Results ---------------*\
			This section makes sure that there are
			enough results for each button generated.
			Results generated to match extra buttons
			are filled with DialogResult.Cancel.
			\*---------------------------------------*/

			if ( results.Count < buttons.Count )
			{
				for ( int i = results.Count; i < buttons.Count; i++ )
				{
					results.Add(DialogResult.Cancel);
				}
			}
			this.Results = results;

			this.Controls.Add(control);
			control.TabIndex = 3;
			control.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( 
				System.Windows.Forms.AnchorStyles.Top |
				System.Windows.Forms.AnchorStyles.Bottom ) |
				System.Windows.Forms.AnchorStyles.Left ) |
				System.Windows.Forms.AnchorStyles.Right ) ) );
		}
    }

    /// <summary>
    /// Your custom message box helper. This wrapper allows
    /// easy creating of the message box as well as proper
    /// destruction when usage is finished. The using
    /// construct is what allows the resources to be freed
    /// when the form is closed.
    /// </summary>
    public static class CustomMessageBox
    {
        public static DialogResult Show(string message, string title, int width, List<string> buttons, List<DialogResult> results)
        {
            using (var form = new MessageBox(message, title, width, buttons, results))
            {                                                                   // New form creating our custom message box.
                DialogResult d = form.ShowDialog();                             // Shows the customized message box to the user.
                return d;
            }
        }

        public static DialogResult Show(string message, string title, int width)
        {
            using (var form = new MessageBox(message, title, width, new List<string>(), new List<DialogResult>()))
            {                                                                   // New form creating our custom message box.
                DialogResult d = form.ShowDialog();                             // Shows the customized message box to the user.
                return d;
            }
        }

        public static DialogResult Show(string title, string message)
        {
            using (var form = new MessageBox(message, title, 0, new List<string>(), new List<DialogResult>()))
            {                                                                   // New form creating our custom message box.
                DialogResult d = form.ShowDialog();                             // Shows the customized message box to the user.
                return d;
            }
        }

        public static DialogResult Show(string message)
        {
            using (var form = new MessageBox(message, "", 0, new List<string>(), new List<DialogResult>()))
            {                                                                   // New form creating our custom message box.
                DialogResult d = form.ShowDialog();                             // Shows the customized message box to the user.
                return d;
            }
        }
    }

	public static class CustomFormBox
	{
		public static DialogResult Show(Control form, string title, List<string> buttons, List<DialogResult> results)
		{
			using (var mForm = new MessageBox(form, title, buttons, results))
			{                                                                   // New form creating our custom message box.
				DialogResult d = mForm.ShowDialog();                            // Shows the customized message box to the user.
				return d;
			}
		}

		public static DialogResult Show(Control form, string title)
		{
			using (var mForm = new MessageBox(form, title, new List<string> { }, new List<DialogResult>() { }))
			{                                                                   // New form creating our custom message box.
				DialogResult d = mForm.ShowDialog();                            // Shows the customized message box to the user.
				return d;
			}
		}
	}
}