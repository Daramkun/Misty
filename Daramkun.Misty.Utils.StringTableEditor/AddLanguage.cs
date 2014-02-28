using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Daramkun.Misty.Utils.StringTableEditor
{
	public partial class AddLanguage : Form
	{
		public AddLanguage ()
		{
			InitializeComponent ();
			foreach ( CultureInfo cultureInfo in CultureInfo.GetCultures ( CultureTypes.AllCultures ) )
				if ( cultureInfo.Name != "" )
					comboBox1.Items.Add ( cultureInfo.Name );
			comboBox1.SelectedIndex = 0;
		}

		private void button1_Click ( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
		}

		private void button2_Click ( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
