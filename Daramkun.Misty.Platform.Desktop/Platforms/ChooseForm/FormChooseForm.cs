using System;
using System.Windows.Forms;
using System.Drawing;
using Daramkun.Misty.Platforms;
using Daramkun.Misty.Nodes;
using System.Reflection;

namespace Daramkun.Misty.Platforms.ChooseForm
{
	class FormChooseForm : IChooseForm
	{
		Form window;

		#region IChooseForm implementation

		public bool IsClickedOK { get; private set; }

		public int SelectedPAF { get; private set; }
		public int SelectedGameLooper { get; private set; }
		public int SelectedMainNode { get; private set; }

		public bool InitializePlatform ()
		{
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault ( false );

			return true;
		}

		public bool InitializeWindow ( string gameName, Image coverImage,
			ILauncher [] launchers, IGameLooper [] gameLoopers, Node [] mainNodes )
		{
			window = new Form ();

			window.Text = gameName;
			window.BackColor = Color.White;
			window.ClientSize = new Size ( 400, 260 );
			window.FormBorderStyle = FormBorderStyle.FixedDialog;
			window.StartPosition = FormStartPosition.CenterScreen;
			window.MaximizeBox = false;
			window.MinimizeBox = true;
			window.ShowInTaskbar = false;

			PictureBox cover = new PictureBox ();
			cover.Image = coverImage ?? Image.FromStream (
				Assembly.GetExecutingAssembly ().GetManifestResourceStream ( "Daramkun.Misty.Resources.DefaultCover.png" )
			);
			cover.Bounds = new Rectangle ( 0, 0, 400, 100 );
			window.Controls.Add ( cover );

			Label pafLabel = new Label () { Text = "&PAF:", Bounds = new Rectangle ( 10, 120, 60, 24 ) };
			window.Controls.Add ( pafLabel );

			ComboBox pafComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 116, 310, 24 ) };
			foreach ( ILauncher launcher in launchers )
				pafComboBox.Items.Add ( launcher.ToString () );
			if ( pafComboBox.Items.Count >= 1 )
				pafComboBox.SelectedIndex = 0;
			if ( pafComboBox.Items.Count <= 1 )
				pafComboBox.Enabled = false;
			window.Controls.Add ( pafComboBox );

			Label glLabel = new Label () { Text = "&Looper:", Bounds = new Rectangle ( 10, 160, 60, 24 ) };
			window.Controls.Add ( glLabel );

			ComboBox glComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 156, 310, 24 ) };
			foreach ( IGameLooper gl in gameLoopers )
				glComboBox.Items.Add ( gl.ToString () );
			if ( glComboBox.Items.Count >= 1 )
				glComboBox.SelectedIndex = 0;
			if ( glComboBox.Items.Count <= 1 )
				glComboBox.Enabled = false;
			window.Controls.Add ( glComboBox );

			Label nodeLabel = new Label () { Text = "&Game:", Bounds = new Rectangle ( 10, 200, 60, 24 ) };
			window.Controls.Add ( nodeLabel );

			ComboBox nodeComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 196, 310, 24 ) };
			foreach ( Node node in mainNodes )
				nodeComboBox.Items.Add ( node.ToString () );
			if ( nodeComboBox.Items.Count >= 1 )
				nodeComboBox.SelectedIndex = 0;
			if ( nodeComboBox.Items.Count <= 1 )
				nodeComboBox.Enabled = false;
			window.Controls.Add ( nodeComboBox );

			Button acceptButton = new Button () { Text = "&Run", Bounds = new Rectangle ( 120, 226, 80, 24 ) };
			acceptButton.Click += ( object sender, EventArgs e ) =>
			{
				if ( pafComboBox.SelectedIndex < 0 )
				{
					MessageBox.Show ( "Please select a Platform Abstraction Framework." );
					return;
				}
				if ( nodeComboBox.SelectedIndex < 0 )
				{
					MessageBox.Show ( "Please select a Game." );
					return;
				}

				IsClickedOK = true;
				SelectedPAF = pafComboBox.SelectedIndex;
				SelectedGameLooper = glComboBox.SelectedIndex;
				SelectedMainNode = nodeComboBox.SelectedIndex;
				window.Close ();
			};
			window.Controls.Add ( acceptButton );

			Button cancelButton = new Button () { Text = "E&xit", Bounds = new Rectangle ( 210, 226, 80, 24 ) };
			cancelButton.Click += ( object sender, EventArgs e ) =>
			{
				IsClickedOK = false;
				window.Close (); 
			};
			window.Controls.Add ( cancelButton );

			window.AcceptButton = acceptButton;
			window.CancelButton = cancelButton;

			return true;
		}

		public void RunWindow ()
		{
			Application.Run ( window );
		}

		#endregion
	}
}

