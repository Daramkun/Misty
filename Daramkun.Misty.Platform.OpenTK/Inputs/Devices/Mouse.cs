using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Inputs.Devices
{
	public class Mouse : BaseMouse
	{
		OpenTK.GameWindow w;

		MouseButton MouseButton = MouseButton.None;
		Daramkun.Misty.Mathematics.Vector2 Position = new Daramkun.Misty.Mathematics.Vector2 ();
		float Wheel = 0;

		public override bool IsSupport { get { return true; } }
		public override bool IsConnected { get { return true; } }
		public override bool IsMultiPlayerable { get { return false; } }

		public override bool IsCursorVisible
		{
			get { return w.CursorVisible; }
			set { w.CursorVisible = value; }
		}

		public override bool IsCursorBlocked { get; set; }

		public Mouse ( IWindow window )
		{
			w = window.Handle as OpenTK.GameWindow;
			w.Mouse.ButtonDown += Mouse_ButtonDown;
			w.Mouse.ButtonUp += Mouse_ButtonUp;
			w.Mouse.Move += Mouse_Move;
			w.Mouse.WheelChanged += Mouse_WheelChanged;
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				w.Mouse.ButtonDown -= Mouse_ButtonDown;
				w.Mouse.ButtonUp -= Mouse_ButtonUp;
				w.Mouse.Move -= Mouse_Move;
				w.Mouse.WheelChanged -= Mouse_WheelChanged;
			}
			base.Dispose ( isDisposing );
		}

		void Mouse_ButtonDown ( object sender, OpenTK.Input.MouseButtonEventArgs e )
		{
			if ( e.Button == OpenTK.Input.MouseButton.Left )
				MouseButton |= MouseButton.Left;
			if ( e.Button == OpenTK.Input.MouseButton.Right )
				MouseButton |= MouseButton.Right;
			if ( e.Button == OpenTK.Input.MouseButton.Middle )
				MouseButton |= MouseButton.Middle;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
		}

		void Mouse_ButtonUp ( object sender, OpenTK.Input.MouseButtonEventArgs e )
		{
			if ( e.Button == OpenTK.Input.MouseButton.Left )
				MouseButton &= MouseButton.Left;
			if ( e.Button == OpenTK.Input.MouseButton.Right )
				MouseButton &= MouseButton.Right;
			if ( e.Button == OpenTK.Input.MouseButton.Middle )
				MouseButton &= MouseButton.Middle;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
		}

		void Mouse_Move ( object sender, OpenTK.Input.MouseMoveEventArgs e )
		{
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
			if ( IsCursorBlocked )
			{
				Position -= new Mathematics.Vector2 ( w.Bounds.Left + ( w.Bounds.Width / 2 ), w.Bounds.Top + ( w.Bounds.Height / 2 ) );
				Cursor.Position = new Point ( w.Bounds.Left + ( w.Bounds.Width / 2 ), w.Bounds.Top + ( w.Bounds.Height / 2 ) );
			}
		}

		void Mouse_WheelChanged ( object sender, OpenTK.Input.MouseWheelEventArgs e )
		{
			Wheel = e.DeltaPrecise;
			Position = new Daramkun.Misty.Mathematics.Vector2 ( e.X, e.Y );
		}

		public override States.MouseState GetState ( PlayerIndex index = PlayerIndex.Player1 )
		{
			return new MouseState ( Position, Wheel, MouseButton );
		}
	}
}
