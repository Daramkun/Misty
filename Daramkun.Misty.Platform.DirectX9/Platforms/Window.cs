using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Platforms
{
	public class Window : StandardDispose, IWindow, IWindowDesktop
	{
		Form window;

		public object Icon { get { return window.Icon; } set { window.Icon = value as Icon; } }
		public string Title { get { return window.Text; } set { window.Text = value; } }
		public Vector2 ClientSize
		{
			get { return new Vector2 ( window.ClientSize.Width, window.ClientSize.Height ); }
			set { window.ClientSize = new Size ( ( int ) value.X, ( int ) value.Y ); }
		}
		public object Handle { get { return window; } }
		public bool IsResizable
		{
			get { return window.FormBorderStyle == FormBorderStyle.Sizable; }
			set { window.FormBorderStyle = ( value ) ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle; }
		}
		public bool IsAlive { get { return window.IsHandleCreated; } }

		public Window ()
		{
			window = new Form ();
			window.StartPosition = FormStartPosition.CenterScreen;
			window.ClientSize = new Size ( 800, 600 );
			window.MaximizeBox = false;
			window.Icon = System.Drawing.Icon.ExtractAssociatedIcon ( "C:\\Windows\\System32\\user32.dll" );
			window.Text = "Misty Framework";
			window.FormBorderStyle = FormBorderStyle.FixedSingle;
			window.Resize += ( object sender, EventArgs e ) => { if ( Resize != null ) Resize ( this, e ); };
			window.Activated += ( object sender, EventArgs e ) => { if ( Activated != null ) Activated ( this, e ); };
			window.Deactivate += ( object sender, EventArgs e ) => { if ( Deactivated != null ) Deactivated ( this, e ); };
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				window.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public void DoEvents ()
		{
			Application.DoEvents ();
		}

		public void Show ()
		{
			window.Show ();
		}

		public event EventHandler Activated;
		public event EventHandler Deactivated;
		public event EventHandler Resize;
	}
}
