using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Daramkun.Misty.Utils.StringTableEditor
{
	public partial class AboutWindow : Form
	{
		public AboutWindow ()
		{
			InitializeComponent ();
		}

		private void AboutWindow_Load ( object sender, EventArgs e )
		{
			labelToolVersion.Text = Assembly.Load ( "Daramkun.Misty.Utils.StringTableEditor" ).GetName ().Version.ToString ();
			labelBlockarVersion.Text = Assembly.Load ( "Daramkun.Blockar" ).GetName ().Version.ToString ();
			labelDotNet.Text = Environment.Version.ToString ();
		}

		private void button1_Click ( object sender, EventArgs e )
		{
			Close ();
		}
	}
}
