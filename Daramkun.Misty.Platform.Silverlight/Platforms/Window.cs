using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Platforms
{
	public class Window : StandardDispose, IWindow
	{
		System.Windows.Window window;

		public string Title
		{
			get { return window.Title; }
			set { window.Title = value; }
		}

		public Vector2 ClientSize { get { return new Vector2 ( ( float ) window.Width, ( float ) window.Height ); } }

		public object Handle { get { return window; } }

		public bool IsAlive { get { throw new NotImplementedException (); } }

		public Window ( System.Windows.Window window )
		{
			this.window = window;
		}

		public void DoEvents ()
		{
			throw new NotImplementedException ();
		}

		public void Show ()
		{
			throw new NotImplementedException ();
		}

		public event EventHandler Activated;

		public event EventHandler Deactivated;

		public event EventHandler Resize;
	}
}
