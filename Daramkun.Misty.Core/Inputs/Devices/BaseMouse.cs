using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs.Devices
{
	public class MouseButtonEventArgs : EventArgs
	{
		public MouseState MouseState { get; set; }
		public MouseButtonEventArgs ( MouseState mouseState ) { MouseState = mouseState; }
	}

	public class MouseWheelEventArgs : EventArgs
	{
		public float Wheel { get; set; }
		public MouseWheelEventArgs ( float wheel ) { Wheel = wheel; }
	}

	public class BaseMouse : StandardDispose, IInputDevice<MouseState>
	{
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected { get { return false; } }
		public virtual bool IsMultiPlayerable { get { return false; } }

		public EventHandler<MouseButtonEventArgs> MouseDown, MouseUp, MouseMove;
		public EventHandler<MouseWheelEventArgs> MouseWheel;
		public EventHandler MouseHover, MouseLeave;

		public virtual MouseState GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new MouseState (); }

		protected void RunMouseDown ( MouseButtonEventArgs e ) { if ( MouseDown != null ) MouseDown ( this, e ); }
		protected void RunMouseUp ( MouseButtonEventArgs e ) { if ( MouseUp != null ) MouseUp ( this, e ); }
		protected void RunMouseMove ( MouseButtonEventArgs e ) { if ( MouseMove != null ) MouseMove ( this, e ); }

		protected void RunMouseWheel ( MouseWheelEventArgs e ) { if ( MouseWheel != null ) MouseWheel ( this, e ); }

		protected void RunMouseHover ( EventArgs e ) { if ( MouseHover != null ) MouseHover ( this, e ); }
		protected void RunMouseLeave ( EventArgs e ) { if ( MouseLeave != null ) MouseLeave ( this, e ); }

		public virtual bool IsCursorVisible { get { return false; } set { } }
		public virtual bool IsCursorBlocked { get { return false; } set { } }
	}
}
