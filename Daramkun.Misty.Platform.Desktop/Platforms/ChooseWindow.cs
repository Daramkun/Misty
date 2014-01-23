using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Platforms
{
	public class ChooseWindow
	{
		Form window;

		ILauncher [] pafs;
		Node [] mainNodes;
		IGameLooper [] gameLoopers;

		bool isClickedOK = false;
		int selectedPaf = 0;
		int selectedLooper = 0;
		int selectedNode = 0;

		private class ChooseForm : Form
		{
			public ChooseForm ( string gameName, Icon icon )
			{
				Text = gameName;
				BackColor = Color.White;
				ClientSize = new Size ( 400, 260 );
				FormBorderStyle = FormBorderStyle.FixedDialog;
				StartPosition = FormStartPosition.CenterScreen;
				MaximizeBox = false;
				MinimizeBox = true;
				Icon = icon;
			}
		}

		public ChooseWindow ( string gameName, Assembly [] pafs, Assembly [] mainNodes, Icon icon = null, Image coverImage = null, Assembly [] gameLoopers = null )
		{
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault ( false );

			window = new ChooseForm ( gameName, icon );
			window.FormClosing += ( object sender, FormClosingEventArgs e ) =>
			{
				if ( isClickedOK ) return;
			};

			PictureBox cover = new PictureBox ();
			cover.Image = coverImage ?? Image.FromStream (
				Assembly.GetExecutingAssembly ().GetManifestResourceStream ( "Daramkun.Misty.Resources.DefaultCover.png" )
			);
			cover.Bounds = new Rectangle ( 0, 0, 400, 100 );
			window.Controls.Add ( cover );

			AnalyzePAFs ( pafs );
			if ( gameLoopers == null )
				gameLoopers = new Assembly [] { Assembly.Load ( "Daramkun.Misty.Core" ) };
			AnalyzeGameLoopers ( gameLoopers );
			AnalyzeMainNodes ( mainNodes );

			Label pafLabel = new Label () { Text = "&PAF:", Bounds = new Rectangle ( 10, 120, 60, 24 ) };
			window.Controls.Add ( pafLabel );

			ComboBox pafComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 116, 310, 24 ) };
			foreach ( ILauncher launcher in this.pafs ) pafComboBox.Items.Add ( launcher.ToString () );
			pafComboBox.SelectedIndex = 0;
			window.Controls.Add ( pafComboBox );

			Label glLabel = new Label () { Text = "&Looper:", Bounds = new Rectangle ( 10, 160, 60, 24 ) };
			window.Controls.Add ( glLabel );

			ComboBox glComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 156, 310, 24 ) };
			foreach ( IGameLooper gl in this.gameLoopers ) glComboBox.Items.Add ( gl.ToString () );
			glComboBox.SelectedIndex = 0;
			window.Controls.Add ( glComboBox );

			Label nodeLabel = new Label () { Text = "&Game:", Bounds = new Rectangle ( 10, 200, 60, 24 ) };
			window.Controls.Add ( nodeLabel );

			ComboBox nodeComboBox = new ComboBox () { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle ( 70, 196, 310, 24 ) };
			foreach ( Node node in this.mainNodes ) nodeComboBox.Items.Add ( node.ToString () );
			nodeComboBox.SelectedIndex = 0;
			window.Controls.Add ( nodeComboBox );

			Button acceptButton = new Button () { Text = "&Run", Bounds = new Rectangle ( 120, 226, 80, 24 ) };
			acceptButton.Click += ( object sender, EventArgs e ) =>
			{
				if ( pafComboBox.SelectedIndex < 0 ) { MessageBox.Show ( "Please select a Platform Abstraction Framework." ); return; }
				if ( nodeComboBox.SelectedIndex < 0 ) { MessageBox.Show ( "Please select a Game." ); return; }

				isClickedOK = true;
				selectedPaf = pafComboBox.SelectedIndex;
				selectedLooper = glComboBox.SelectedIndex;
				selectedNode = nodeComboBox.SelectedIndex;
				window.Close ();
			};
			window.Controls.Add ( acceptButton );

			Button cancelButton = new Button () { Text = "E&xit", Bounds = new Rectangle ( 210, 226, 80, 24 ) };
			cancelButton.Click += ( object sender, EventArgs e ) =>
			{
				isClickedOK = false;
				window.Close (); 
			};
			window.Controls.Add ( cancelButton );

			window.AcceptButton = acceptButton;
			window.CancelButton = cancelButton;
		}

		private void AnalyzePAFs ( Assembly [] pafs )
		{
			List<ILauncher> launchers = new List<ILauncher> ();
			foreach ( Assembly asm in pafs )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( ILauncher ) ) )
						launchers.Add ( Activator.CreateInstance ( type ) as ILauncher );
				}
			}
			this.pafs = launchers.ToArray ();
		}

		private void AnalyzeGameLoopers ( Assembly [] gameLoopers )
		{
			List<IGameLooper> loopers = new List<IGameLooper> ();
			foreach ( Assembly asm in gameLoopers )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( IGameLooper ) ) && !type.IsInterface )
						loopers.Add ( Activator.CreateInstance ( type ) as IGameLooper );
				}
			}
			this.gameLoopers = loopers.ToArray ();
		}

		private void AnalyzeMainNodes ( Assembly [] mainNodes )
		{
			List<Node> nodes = new List<Node> ();
			foreach ( Assembly asm in mainNodes )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( Node ) ) )
					{
						object [] attrs = type.GetCustomAttributes ( typeof ( MainNodeAttribute ), false );
						if ( attrs != null && attrs.Length > 0 )
						{
							nodes.Add ( Activator.CreateInstance ( type ) as Node );
						}
					}
				}
			}
			this.mainNodes = nodes.ToArray ();
		}

		public void Run ( bool isInitializeAudio = true )
		{
			Application.Run ( window );

			if ( isClickedOK )
			{
				Core.Run ( pafs [ selectedPaf ], mainNodes [ selectedNode ], isInitializeAudio, gameLoopers [ selectedLooper ] );
			}
		}
	}
}
