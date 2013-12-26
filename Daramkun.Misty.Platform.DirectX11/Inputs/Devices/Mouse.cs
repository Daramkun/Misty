using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Inputs.Devices
{
	public class Mouse : BaseMouse
	{
		IWindow window;

		protected MouseButton MouseButton = MouseButton.None;
		protected Daramkun.Misty.Mathematics.Vector2 Position = new Daramkun.Misty.Mathematics.Vector2 ();
		protected float Wheel = 0;

		MouseButtonEventArgs buttonEvent;
		MouseWheelEventArgs wheelEvent;

		public override bool IsSupport { get { return true; } }
		public override bool IsConnected { get { return true; } }
		public override bool IsMultiPlayerable { get { return false; } }

		public Mouse ( IWindow window )
		{
			this.window = window;

			Form w = window.Handle as Form;

			w.MouseDown += ButtonDownEvent;
			w.MouseUp += ButtonUpEvent;
			w.MouseMove += MoveEvent;
			w.MouseWheel += WheelEvent;
			w.MouseHover += HoverEvent;
			w.MouseLeave += LeaveEvent;

			buttonEvent = new MouseButtonEventArgs ( new MouseState () );
			wheelEvent = new MouseWheelEventArgs ( 0 );
		}

		private void ButtonDownEvent ( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Left )
				MouseButton |= MouseButton.Left;
			if ( e.Button == MouseButtons.Right )
				MouseButton |= MouseButton.Right;
			if ( e.Button == MouseButtons.Middle )
				MouseButton |= MouseButton.Middle;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
			buttonEvent.MouseState = GetState ();
			RunMouseDown ( buttonEvent );
		}

		private void ButtonUpEvent ( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Left )
				MouseButton &= MouseButton.Left;
			if ( e.Button == MouseButtons.Right )
				MouseButton &= MouseButton.Right;
			if ( e.Button == MouseButtons.Middle )
				MouseButton &= MouseButton.Middle;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
			buttonEvent.MouseState = GetState ();
			RunMouseUp ( buttonEvent );
		}

		private void MoveEvent ( object sender, MouseEventArgs e )
		{
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
			buttonEvent.MouseState = GetState ();
			RunMouseMove ( buttonEvent );
		}

		private void WheelEvent ( object sender, MouseEventArgs e )
		{
			Wheel = e.Delta / 120.0f;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
			wheelEvent.Wheel = Wheel;
			RunMouseWheel ( wheelEvent );
		}

		private void HoverEvent ( object sender, EventArgs e )
		{
			RunMouseHover ( null );
		}

		private void LeaveEvent ( object sender, EventArgs e )
		{
			RunMouseLeave ( null );
		}

		public override MouseState GetState ( PlayerIndex index = PlayerIndex.Player1 )
		{
			return new MouseState ( Position, Wheel, MouseButton );
		}
	}
}
