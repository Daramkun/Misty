using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Platforms
{
	public interface IWindow : IDisposable
	{
		string Title { get; set; }
		Vector2 ClientSize { get; }
		object Handle { get; }
		bool IsAlive { get; }

		void DoEvents ();

		void Show ();

		event EventHandler Activated;
		event EventHandler Deactivated;
		event EventHandler Resize;
	}

	public interface IWindowDesktop
	{
		object Icon { get; set; }
		bool IsResizable { get; set; }
		Vector2 ClientSize { set; }
	}
}
