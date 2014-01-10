using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Platforms
{
	public class Window : StandardDispose, IWindow, IWindowDesktop
	{
		OpenTK.GameWindow window;

		public string Title { get { return window.Title; } set { window.Title = value; } }
		public Vector2 ClientSize
		{
			get { return new Vector2 ( window.ClientSize.Width, window.ClientSize.Height ); }
			set { window.ClientSize = new System.Drawing.Size ( ( int ) value.X, ( int ) value.Y ); }
		}
		public object Handle { get { return window; } }

		public bool IsResizable
		{
			get { return window.WindowBorder == OpenTK.WindowBorder.Resizable; }
			set { window.WindowBorder = value ? OpenTK.WindowBorder.Resizable : OpenTK.WindowBorder.Fixed; }
		}

		public object Icon
		{
			get { return window.Icon; }
			set { window.Icon = value as System.Drawing.Icon; }
		}

		public bool IsAlive { get { return !window.IsExiting; } }

		public Window ()
		{
			window = new OpenTK.GameWindow ( 800, 600,
				new OpenTK.Graphics.GraphicsMode ( new OpenTK.Graphics.ColorFormat ( 32 ), 16, 8 ) );
			window.ClientSize = new System.Drawing.Size ( 800, 600 );
			window.WindowBorder = OpenTK.WindowBorder.Fixed;
			window.Load += (object sender, EventArgs e ) => { window.Title = "Misty Framework"; };
			window.Resize += ( object sender, EventArgs e ) => { if ( Resize != null ) Resize ( this, e ); };
			window.FocusedChanged += ( object sender, EventArgs e ) =>
			{
				if ( window.Focused && Activated != null ) Activated ( this, e );
				else if ( !window.Focused && Deactivated != null ) Deactivated ( this, e );
			};
			window.Context.SwapInterval = 0;

			window.Context.Update ( window.WindowInfo );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				window.Dispose ();
				window = null;
			}
			base.Dispose ( isDisposing );
		}

		public void DoEvents ()
		{
			window.ProcessEvents ();
		}

		public void Show ()
		{
			window.Visible = true;
		}

		public event EventHandler Activated;
		public event EventHandler Deactivated;
		public event EventHandler Resize;
	}
}
